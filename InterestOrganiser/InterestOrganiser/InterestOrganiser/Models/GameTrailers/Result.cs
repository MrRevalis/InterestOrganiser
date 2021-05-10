using System;
using System.Collections.Generic;
using System.Text;

namespace InterestOrganiser.Models.GameTrailers
{
    public class Result
    {
        public int id { get; set; }
        public string name { get; set; }
        public string preview { get; set; }
        public Data data { get; set; }
    }
}
