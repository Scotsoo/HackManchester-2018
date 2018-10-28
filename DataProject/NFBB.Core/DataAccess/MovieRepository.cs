using NFBB.Core.Data;
using System.Data;
using Dapper;
using DapperExtensions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NFBB.Core.DataAccess
{
    public class MovieRepository
    {

        IDbConnection connection;

        public MovieRepository(IDbConnection cnn)
        {
            connection = cnn;
        }

        public IEnumerable<Movie> GetAll()
        {
            connection.Open();
            string sql = "select Id, Title, year, imdbID, PosterUrl,MaxRating, AverageRating, NoOfRatings, Available from vwmovie";
            var movies = connection.Query<Movie>(sql);
            connection.Close();

            return movies;
        }

        public IEnumerable<Movie> GetByGenre(string name)
        {
            connection.Open();
            string sql = "select Id, Title, year, imdbID, PosterUrl,MaxRating, AverageRating, NoOfRatings, Available from vwmovie where id in (select movieid from moviegenre where genre=@genre)";
            var movies = connection.Query<Movie>(sql, new { genre = name });
            connection.Close();

            return movies;
        }

        public IEnumerable<Movie> GetMoviesSeenByUser(int userid)
        {
            connection.Open();
            string sql = "select Id, Title, year, imdbID, PosterUrl,MaxRating, AverageRating, NoOfRatings, Available from vwmovie where id in (select movieid from review where userid=@userid)";
            var movies = connection.Query<Movie>(sql, new { userid=userid });
            connection.Close();

            return movies;
        }

        public void Add(Movie movie)
        {
            connection.Open();

            string sql = "insert into Movie(Id, Title, year, imdbID, PosterUrl) Values(@Id, @Title, @Year, @imdbId, @posterUrl)";
            connection.Execute(sql, new { Id = movie.Id, Title=movie.Title, Year = movie.Year, movie.IMDBId, movie.PosterUrl});
            connection.Close();
        }

        public void DeleteAll()
        {
            connection.Open();

            string sql = "delete from Movie";
            connection.Execute(sql);
            connection.Close();
        }

        public void DeleteById(Movie movie)
        {
            connection.Open();
            connection.Delete(movie);
            connection.Close();
        }

        public void Save(Movie r)
        {
            connection.Open();
            if (r.Id < 1)
            {
                connection.Insert(r);
            }
            else
            {
                connection.Update(r);
            }
            connection.Close();
        }

        public void AddReview(Review review)
        {
            connection.Open();
            string sql = "insert into Review([MovieId],[UserId],[Rating],[Date]) Values(@movieId, @UserId, @Rating, @Date)";
            connection.Execute(sql, new { review.MovieId, review.UserId, review.Rating, review.Date });
            connection.Close();
        }

        


        public void DeleteAllReviews()
        {
            connection.Open();

            string sql = "delete from Review";
            connection.Execute(sql);
            connection.Close();
        }

        public OMDBItem GetOMDBData(string title)
        {
            string json;

            using (var client = new System.Net.Http.HttpClient())
            {
                var response = client.GetAsync("http://www.omdbapi.com/?apikey=7e6ca2d5&t=" + title).Result;
                using (HttpContent content = response.Content)
                {
                    Task<string> result = content.ReadAsStringAsync();
                    json = result.Result;
                }
            }

            var r = JsonConvert.DeserializeObject<OMDBItem>(json);

            return r;
        }

        public IEnumerable<Genre> GetAllGenre()
        {
            connection.Open();

            var c = connection.GetList<Genre>();
            connection.Close();
            return c;
        }

        public void AddAllGenre()
        {
            connection.Open();

            string sql = "insert into Genre(Name) select distinct genre from moviegenre";
            connection.Execute(sql);
            connection.Close();
        }

        public void AddGenresForMovie(int movieid, string genres)
        {
            connection.Open();

            var genreArray = genres.Split(",".ToCharArray());

            for (int i = 0; i < genreArray.Length; i++)
            {
                string sql = "insert into MovieGenre(movieid, genre) Values(@movieid, @genre)";
                connection.Execute(sql, new { movieid, genre=genreArray[i].Trim() });

            }

            connection.Close();
        }

        public void DeleteGenres()
        {
            connection.Open();

            string sql = "delete from moviegenre";
            connection.Execute(sql);
            sql = "delete from genre";
            connection.Execute(sql);
            connection.Close();
        }

        public IEnumerable<Review> GetReviewsForMovie(int movieId)
        {
            connection.Open();
            var predicate = Predicates.Field<Review>(f => f.MovieId, Operator.Eq, movieId);
            var reviews = connection.GetList<Review>(predicate);
            connection.Close();

            return reviews;
        }

        public IEnumerable<Review> GetReviewsByUserAndMovie(int userId, int movieId)
        {
            connection.Open();
            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<Review>(f => f.MovieId, Operator.Eq, movieId));
            pg.Predicates.Add(Predicates.Field<Review>(f => f.UserId, Operator.Eq, userId));
            var reviews = connection.GetList<Review>(pg);
            connection.Close();

            return reviews;
        }

    }
}
