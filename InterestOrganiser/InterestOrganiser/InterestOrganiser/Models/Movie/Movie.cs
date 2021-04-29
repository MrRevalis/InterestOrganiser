using System;
using System.Collections.Generic;
using System.Text;

namespace InterestOrganiser.Models.Movie
{
    public class Movie
    {
        public int page { get; set; }
        public List<Result> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }
}
