using AutoMapper;
using JomMalaysia.Presentation.Models.Common;
using JomMalaysia.Presentation.Models.Listings;
using JomMalaysia.Presentation.Models.Merchants;
using JomMalaysia.Presentation.ViewModels.Common;
using JomMalaysia.Presentation.ViewModels.Listings;
using JomMalaysia.Presentation.ViewModels.Merchants;

namespace JomMalaysia.Presentation.ViewModels
{
    public class PresentationProfile : Profile
    {
        public PresentationProfile()
        {
            CreateMap<Listing, RegisterListingViewModel>()
                .ForMember(vm => vm.MerchantId, opt => opt.MapFrom(m => m.Merchant.MerchantId))
                .ForMember(vm => vm.FullAddress, opt => opt.MapFrom(m => m.Address.ToString()))
                .ForMember(vm => vm.CategoryId, opt => opt.MapFrom(m => m.Category.CategoryId))
                ;


            CreateMap<OperatingHour, OperatingHourViewModel>();

            CreateMap<Address, AddressViewModel>();

            CreateMap<ListingImages, ListingImageViewModel>();

            CreateMap<Merchant, RegisterMerchantViewModel>()
                .ForPath(vm=>vm.CompanyRegistrationName,opt=>opt.MapFrom(m=>m.CompanyRegistration.RegistrationName))
                .ForPath(vm=>vm.OldSsmId,opt=>opt.MapFrom(m=>m.CompanyRegistration.OldSsmId))
                .ForPath(vm=>vm.SsmId,opt=>opt.MapFrom(m=>m.CompanyRegistration.SsmId));

            CreateMap<Contact, ContactViewModel>();
        }
    }
}