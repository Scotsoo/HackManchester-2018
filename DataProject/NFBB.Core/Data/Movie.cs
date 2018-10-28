using System;
using System.Collections.Generic;
using System.Text;

namespace NFBB.Core.Data
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }

        public string IMDBId { get; set; }

        public string PosterUrl { get; set; }

        public int MaxRating { get; set; }
        public int AverageRating { get; set; }
        public int NoOfRatings { get; set; }
        public bool Available { get; set; }


    }
}
