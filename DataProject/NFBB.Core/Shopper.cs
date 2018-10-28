using NFBB.Core.Data;
using NFBB.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NFBB.Core
{
    public class Shopper
    {
        private MovieRepository movieRepo;
        private UserRepository userRepo;

        public List<Movie> ShelfContent = new List<Movie>();

        public Shopper()
        {
            var connectionstring = "Data Source=queue.cnucaihvfwwr.eu-west-1.rds.amazonaws.com;Initial Catalog=HackMcr2018;User ID=hm2018;Password=Random@@12345";

            IDbConnection db = new SqlConnection(connectionstring);
            movieRepo = new MovieRepository(db);
            userRepo = new UserRepository(db);
        }

        public IEnumerable<Movie> PopulateShelves()
        {
            var movies = movieRepo.GetAll().OrderBy(x => x.Title);

            return movies;

        }

        public IEnumerable<Movie> PopulateShelvesForGenre(string genreName)
        {
            var movies = movieRepo.GetByGenre(genreName).OrderBy(x => x.Title);

            return movies;

        }

        public IEnumerable<Genre> getGenres()
        {
            return movieRepo.GetAllGenre();
        }

        public IEnumerable<Review> GetReviewsForMovie(int movieid)
        {
            return movieRepo.GetReviewsForMovie(movieid);
        }

        public User GetUser(int userid)
        {
            return userRepo.GetById(userid);
        }

        public IEnumerable<Movie> GetMoviesSeenByUser(int userid)
        {
            return movieRepo.GetMoviesSeenByUser(userid);
        }

        public IEnumerable<Actor> GetActorsForMovie(int movieid)
        {
            return movieRepo.GetActorsForMovie(movieid);
        }

        public IEnumerable<Movie> GetMoviesForActor(string actorname)
        {
            return movieRepo.GetMoviesForActor(actorname);
        }


    }
}
