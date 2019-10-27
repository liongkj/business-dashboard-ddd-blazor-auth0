﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.Models.Common;
using JomMalaysia.Presentation.Models.Merchants;
using Newtonsoft.Json;

namespace JomMalaysia.Presentation.Models.Listings
{
    public class ListingViewModel
    {
        public string ListingId { get; set; }
        public CategoryPathViewModel Category { get; set; }
        public MerchantViewModel Merchant { get; set; }
        public string ListingName { get; set; }
        public string Description { get; set; }
        public List<string> Gags { get; set; }
        public Address Address { get; set; }
        public List<OperatingHour> OperatingHours { get; set; }
        public PublishStatus PublishStatus { get; set; }
        public string ListingType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }



        public class DayOfWeek
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }

        public class OperatingHour
        {
            public DayOfWeek DayOfWeek { get; set; }
            public string OpenTime { get; set; }
            public string CloseTime { get; set; }
        }









    }
}
