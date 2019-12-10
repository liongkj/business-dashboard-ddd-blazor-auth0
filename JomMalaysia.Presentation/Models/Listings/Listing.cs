using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.Models.Common;
using JomMalaysia.Presentation.Models.Merchants;
using JomMalaysia.Presentation.ViewModels.Listings;
using Newtonsoft.Json;
using static JomMalaysia.Framework.Constant.EnumConstant;

namespace JomMalaysia.Presentation.Models.Listings
{
    public class Listing
    {
        public string ListingId { get; set; }
        public CategoryPath Category { get; set; }
        public MerchantSummary Merchant { get; set; }
        public string ListingName { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
        public Address Address { get; set; }
        public List<OperatingHour> OperatingHours { get; set; }
        public OfficialContact Contact { get; set; }
        public ListingImages ListingImages{get;set;}
        public PublishStatus PublishStatus { get; set; }
        public CategoryType? CategoryType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }


        

        
    }

    public class ListingImages
    {
        public Image ListingLogo { get; set; }
        public Image CoverPhoto { get; set; }
        public List<Image> ListingDetails { get; set; }
    }
}
