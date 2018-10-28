using NFBB.Core.Data;
using NFBB.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace NFBB.Core
{
    public static class Importer
    {
        public static bool StartImport(string filepath, MovieRepository repo, UserRepository uRepo)
        {
            Console.WriteLine("importing... " + filepath);
            //ImportTitles(filepath, repo);

            ImportReviews(filepath, repo);

            GenerateUsers(uRepo);
            return true;

        }

        private static void GenerateUsers(UserRepository repo)
        {
            //repo.DeleteAll();
            var missingUsers = repo.GetMissingUsers();
            var newUsers = repo.CreateRandomUsers(missingUsers);
            repo.SaveUsers(newUsers);

            Console.WriteLine(newUsers.Count + " missing users generated");

        }

        private static void ImportReviews(string filepath, MovieRepository repo)
        {
            var reviewsimported = 0;
            //repo.DeleteAllReviews();

            for(int i = 1; i <= 4; i++)
            {
                var reader = File.OpenText(filepath + "combined_data_" + i + ".txt");
                Console.WriteLine("Importing file..." + "combined_data_" + i + ".txt");
                reviewsimported+= ExtractReviewsFromFile(repo, reader);
            }
            Console.WriteLine(reviewsimported + " reviews imported");
        }

        private static int ExtractReviewsFromFile(MovieRepository repo, StreamReader reader)
        {
           
            var c = 0;
            int movieid = 0;
            var reviewsimported = 0;

            var itemsPerCompany = 0;

            while (c < Int32.MaxValue && !reader.EndOfStream)
            {

                var line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.EndsWith(":"))
                    {
                        movieid = Int32.Parse(line.Replace(":", ""));
                        itemsPerCompany = 0;
                        Console.WriteLine(movieid);
                    }
                    else
                    {
                        if (itemsPerCompany < 5)
                        {
                            var items = line.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                            var r = new Review
                            {
                                MovieId = movieid,
                                UserId = Int32.Parse(items[0]),
                                Rating = Int32.Parse(items[1]),
                                Date = DateTime.Parse(items[2])

                            };
                            itemsPerCompany++;

                            var existingReviews = repo.GetReviewsByUserAndMovie(r.UserId, movieid);

                            if (existingReviews.Count() == 0)
                            {
                                repo.AddReview(r);

                            }
                            reviewsimported++;
                        }

                    }

                    c++;
                }
            }

            return reviewsimported;
        }

        private static void ImportTitles(string filepath, MovieRepository repo)
        {
            var reader = File.OpenText(filepath + "movie_titles.csv");

            repo.DeleteGenres();
            //repo.DeleteAll();

            var titlesimported = 0;

            int testlimit = 0;
            while (!reader.EndOfStream && testlimit < Int32.MaxValue)
            {
                string line = reader.ReadLine();

                var items = line.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                int year, id;

                if (Int32.TryParse(items[0], out id) && Int32.TryParse(items[1], out year) && !string.IsNullOrEmpty(items[2]))
                {
                    Movie m = new Movie()
                    {
                        Id = Int32.Parse(items[0]),
                        Year = Int32.Parse(items[1]),
                        Title = items[2]

                    };

                    OMDBItem o = repo.GetOMDBData(m.Title);

                    if (o != null && o.imdbID!=null)
                    {
                        m.IMDBId = o.imdbID;
                        m.PosterUrl = "http://img.omdbapi.com/?i=" + m.IMDBId + "&h=200&apikey=7e6ca2d5";

                        repo.DeleteById(m);
                        repo.Add(m);

                        repo.AddGenresForMovie(m.Id, o.Genre);
                        titlesimported++;
                    }

                    
                    
                    testlimit++;
                }


            }

            Console.WriteLine(titlesimported + " titles imported");
        }
    }
}
