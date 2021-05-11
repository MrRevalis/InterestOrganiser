using System;
using System.Collections.Generic;
using System.Text;

namespace InterestOrganiser.Models
{
    public class BrowseItem
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public bool ToRealise { get; set; }
        public bool Realised { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
    }
}
