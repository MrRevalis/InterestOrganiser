using System;
using System.Collections.Generic;
using System.Text;

namespace MovieBase.Models.MovieDetail
{
    public class BelongsToCollection
    {
        public int id { get; set; }
        public string name { get; set; }
        public string poster_path { get; set; }
        public string backdrop_path { get; set; }
    }
}
