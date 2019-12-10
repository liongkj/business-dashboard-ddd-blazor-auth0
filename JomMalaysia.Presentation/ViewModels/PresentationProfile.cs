using AutoMapper;
using JomMalaysia.Presentation.Models.Common;
using JomMalaysia.Presentation.Models.Listings;
using JomMalaysia.Presentation.ViewModels.Common;
using JomMalaysia.Presentation.ViewModels.Listings;

namespace JomMalaysia.Presentation.ViewModels
{
    public class PresentationProfile : Profile
    {
        public PresentationProfile()
        {
            CreateMap<Listing, RegisterListingViewModel>();
            
            CreateMap<OperatingHour, OperatingHourViewModel>();
            
            CreateMap<Address, AddressViewModel>();
            CreateMap<ListingImages, ListingImageViewModel>();

        }
        
    }
}