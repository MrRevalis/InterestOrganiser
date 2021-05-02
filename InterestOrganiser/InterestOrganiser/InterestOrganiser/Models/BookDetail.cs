using System;
using System.Collections.Generic;
using System.Text;
using static Google.Apis.Books.v1.Data.Volume;

namespace InterestOrganiser.Models
{
    public class BookDetail
    {
        public string ID { get; set; }
        public VolumeInfoData Book { get; set; }
    }
}
