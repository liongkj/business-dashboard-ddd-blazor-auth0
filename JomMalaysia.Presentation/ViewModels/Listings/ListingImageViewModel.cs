using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Models.Common;

namespace JomMalaysia.Presentation.ViewModels.Listings
{
    public class ListingImageViewModel
    {
        public Image ListingLogo { get; set; }
        public Image CoverPhoto { get; set; }
        public List<Image> ListingDetails { get; set; }
    }
}
