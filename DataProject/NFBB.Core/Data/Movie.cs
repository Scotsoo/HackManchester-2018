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

    }
}
