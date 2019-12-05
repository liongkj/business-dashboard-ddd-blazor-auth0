using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Models.Common;

namespace JomMalaysia.Presentation.ViewModels.Listings
{
    public class ListingImageViewModel
    {
        [DisplayName("Logo")]
        public Image ListingLogo { get; set; }
        [DisplayName("Cover Photo")]
        public Image CoverPhoto { get; set; }
        [DisplayName("Ads Image")]
        public List<Image> ListingDetails { get; }

        public ListingImageViewModel()
        {
            ListingLogo = new Image();
            CoverPhoto = new Image();
            ListingDetails = new List<Image>();
        }
    }
}
