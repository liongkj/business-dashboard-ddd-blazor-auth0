using System.Collections.Generic;
using System.Threading.Tasks;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Models.Listings;
using JomMalaysia.Presentation.ViewModels.Listings;

namespace JomMalaysia.Presentation.Gateways.Listings
{
    public interface IListingGateway
    {
        Task<List<Listing>> GetAll();
        Task<IWebServiceResponse> Add(RegisterListingViewModel vm);
        Task<IWebServiceResponse> Publish(string ListingId, int months);
        Task<Listing> Detail(string id);
        Task<IWebServiceResponse> Edit(RegisterListingViewModel vm, string listingId);
        Task<IWebServiceResponse> Delete(string listingId);
    }
}