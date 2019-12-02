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
        public List<Image> ListingDetails { get; set; }

        public ListingImageViewModel()
        {
            ListingLogo = new Image();
            CoverPhoto = new Image();
        }
    }
}
