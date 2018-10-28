using System;
using System.Collections.Generic;
using System.Text;

namespace NFBB.Core.Data
{
    public class Review
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
    }
}
