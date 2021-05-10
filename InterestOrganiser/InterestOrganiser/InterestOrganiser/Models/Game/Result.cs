using System;
using System.Collections.Generic;

namespace InterestOrganiser.Models.Game
{
    public class Result
    {
        public string slug { get; set; }
        public string name { get; set; }
        //public int playtime { get; set; }
        public List<Platform> platforms { get; set; }
        public List<Store> stores { get; set; }
        public string released { get; set; }
        public bool tba { get; set; }
        public string background_image { get; set; }
        public double rating { get; set; }
        public int rating_top { get; set; }
        //public List<Rating> ratings { get; set; }
        //public int ratings_count { get; set; }
        public int reviews_text_count { get; set; }
        //public int added { get; set; }
        //public AddedByStatus added_by_status { get; set; }
        //public object metacritic { get; set; }
        public int suggestions_count { get; set; }
        //public DateTime updated { get; set; }
        public int id { get; set; }
        public string score { get; set; }
        //public object clip { get; set; }
        //public List<Tag> tags { get; set; }
        //public EsrbRating esrb_rating { get; set; }
        //public object user_game { get; set; }
        public int reviews_count { get; set; }
        public string saturated_color { get; set; }
        public string dominant_color { get; set; }
        public List<ShortScreenshot> short_screenshots { get; set; }
        public List<ParentPlatform> parent_platforms { get; set; }
        public List<Genre> genres { get; set; }
        public int? community_rating { get; set; }
    }
}