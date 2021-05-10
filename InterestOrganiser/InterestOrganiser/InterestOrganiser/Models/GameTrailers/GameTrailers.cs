using System;
using System.Collections.Generic;
using System.Text;

namespace InterestOrganiser.Models.GameTrailers
{
    public class GameTrailers
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<Result> results { get; set; }
    }
}
