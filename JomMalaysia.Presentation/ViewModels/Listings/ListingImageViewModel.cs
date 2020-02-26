using System.Collections.Generic;
using System.ComponentModel;
using JomMalaysia.Presentation.Models.Common;

namespace JomMalaysia.Presentation.ViewModels.Listings
{
    public class ListingImageViewModel
    {
        [DisplayName("Logo")] public Image ListingLogo { get; set; }
        [DisplayName("Cover Photo")] public Image CoverPhoto { get; set; }
        [DisplayName("Ads Image")] public List<Image> Ads { get; set; }

        public ListingImageViewModel()
        {
            ListingLogo = new Image();
            CoverPhoto = new Image();
            Ads = new List<Image>(5) ;
        }
    }
}