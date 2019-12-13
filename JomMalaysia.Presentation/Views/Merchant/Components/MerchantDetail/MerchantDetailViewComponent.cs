using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Gateways.Listings;
using JomMalaysia.Presentation.Gateways.Merchants;
using Microsoft.AspNetCore.Mvc;

namespace JomMalaysia.Presentation.Views.Merchant.Components.MerchantDetail
{
    public class MerchantDetailViewComponent : ViewComponent
    {
        private readonly IMerchantGateway _merchantGateway;

        private readonly IListingGateway _listingGateway;
        public MerchantDetailViewComponent(IMerchantGateway merchantGateway,IListingGateway listingGateway)
        {
            _merchantGateway = merchantGateway;
            _listingGateway = listingGateway;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string id)
        {
           
            var merchant = await _merchantGateway.Detail(id).ConfigureAwait(false);
            var listings = await _listingGateway.GetAll().ConfigureAwait(false);
            var result = listings.Where(l => merchant.Listings.Any(x => x == l.ListingId)).ToList();
            merchant.Listing = result;
            return View(merchant);
        }
    }
}