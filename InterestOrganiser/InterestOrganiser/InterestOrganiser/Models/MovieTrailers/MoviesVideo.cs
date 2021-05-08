using System;
using System.Collections.Generic;
using System.Text;

namespace InterestOrganiser.Models.MovieTrailers
{
    public class MoviesVideo
    {
        public int id { get; set; }
        public List<Result> results { get; set; }
    }
}
