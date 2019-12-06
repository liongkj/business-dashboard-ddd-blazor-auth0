using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models.Listings;
using JomMalaysia.Presentation.ViewModels.Listings;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Listings
{
    public class ListingGateway : IListingGateway
    {
        private readonly IWebServiceExecutor _webServiceExecutor;
        private readonly IApiBuilder _apiBuilder;
        private readonly string auth;

        public ListingGateway(IWebServiceExecutor webServiceExecutor, IAuthorizationManagers authorizationManagers, IApiBuilder apiBuilder)
        {
            _webServiceExecutor = webServiceExecutor;
            var authorizationManagers1 = authorizationManagers;
            _apiBuilder = apiBuilder;
            if (authorizationManagers1 != null) auth = authorizationManagers1.accessToken;
        }

        public async Task<IWebServiceResponse> Add(RegisterListingViewModel vm)
        {
            IWebServiceResponse<Listing> response;
            try
            {
                var req = _apiBuilder.GetApi((APIConstant.API.Path.Listing));

                const Method method = Method.POST;
                response = await _webServiceExecutor.ExecuteRequestAsync<Listing>(req, method, auth,vm).ConfigureAwait(false);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }
            return response;


            //handle exception

        }

        public async Task<List<Listing>> GetAll()
        {
            List<Listing> result = new List<Listing>();
            IWebServiceResponse<ListViewModel<Listing>> response;
            try
            {
                var req = _apiBuilder.GetApi((APIConstant.API.Path.Listing));
                const Method method = Method.GET;
                var queries = new Dictionary<string, string>
                {
                    {"status","all"}
                };
                response = await _webServiceExecutor.ExecuteRequestAsync<ListViewModel<Listing>>(req, method, auth).ConfigureAwait(false);

            }
            catch (GatewayException ex)
            {
                throw;
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listings = response.Data.Data;
                foreach (var list in listings)
                {
                    result.Add(list);
                }


            }
            //handle exception
            return result;
        }

        public async Task<IWebServiceResponse> Publish(string ListingId, int months)
        {
            IWebServiceResponse<ListViewModel<Listing>> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.Publish, ListingId, months.ToString());
                const Method method = Method.POST;

                response = await _webServiceExecutor.ExecuteRequestAsync<ListViewModel<Listing>>(req, method, auth).ConfigureAwait(false);

            }
            catch (GatewayException ex)
            {
                throw;
            }

            return response;
        }
    }
}
