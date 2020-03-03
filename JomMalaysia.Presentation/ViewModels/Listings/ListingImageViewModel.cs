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
            CoverPhoto = new Image("https://res.cloudinary.com/jomn9-com/image/upload/w_1000,ar_16:9,c_fill,g_auto,e_sharpen/v1575257964/placeholder_xtcpy8.jpg");
            Ads = new List<Image>(5) ;
        }
    }
}