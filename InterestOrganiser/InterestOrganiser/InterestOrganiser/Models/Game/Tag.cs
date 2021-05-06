namespace InterestOrganiser.Models.Game
{
    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string language { get; set; }
        public int games_count { get; set; }
        public string image_background { get; set; }
    }
}