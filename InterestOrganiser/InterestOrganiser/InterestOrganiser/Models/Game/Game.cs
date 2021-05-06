using System.Collections.Generic;

namespace InterestOrganiser.Models.Game
{
    public class Game
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<Result> results { get; set; }
        public bool user_platforms { get; set; }
    }
}