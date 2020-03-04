using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Interfaces;
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

        public ListingGateway(IWebServiceExecutor webServiceExecutor, IAuthorizationManagers authorizationManagers,
            IApiBuilder apiBuilder)
        {
            _webServiceExecutor = webServiceExecutor;
            var authorizationManagers1 = authorizationManagers;
            _apiBuilder = apiBuilder;
           
        }

        public async Task<IWebServiceResponse> Add(RegisterListingViewModel vm)
        {
            IWebServiceResponse<Listing> response;
            try
            {
                var req = _apiBuilder.GetApi((APIConstant.API.Path.Listing));

                const Method method = Method.POST;
                response = await _webServiceExecutor.ExecuteRequestAsync<Listing>(req, method, vm)
                    ;
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
                    {"status", "all"}
                };
                response = await _webServiceExecutor.ExecuteRequestAsync<ListViewModel<Listing>>(req, method);
                
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var listings = response.Data.Data;
                result.AddRange(listings);
            }

            //handle exception
            return result;
        }

        public async Task<Listing> Detail(string id)
        {
            Listing result = null;
            IWebServiceResponse<ViewModel<Listing>> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.ListingDetail, id);
                const Method method = Method.GET;
                response = await _webServiceExecutor.ExecuteRequestAsync<ViewModel<Listing>>(req, method)
                    ;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = response.Data.Data;
                }
            }
            catch (GatewayException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }

            //handle exception
            return result;
        }

        public async Task<IWebServiceResponse> Edit(RegisterListingViewModel vm, string listingId)
        {
            IWebServiceResponse<Listing> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.ListingDetail, listingId);

                const Method method = Method.PUT;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<Listing>(req, method, vm)
                    ;
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;


            //handle exception
        }

        public async Task<IWebServiceResponse> Delete(string listingId)
        {
            IWebServiceResponse<Listing> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.ListingDetail, listingId);

                const Method method = Method.DELETE;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<Listing>(req, method)
                    ;
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;


            //handle exception
        }
        
        public async Task<IWebServiceResponse> Publish(string ListingId, int months)
        {
            IWebServiceResponse<ListViewModel<Listing>> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.Publish, ListingId, months.ToString());
                const Method method = Method.POST;

                response = await _webServiceExecutor.ExecuteRequestAsync<ListViewModel<Listing>>(req, method)
                    ;
            }
            catch (GatewayException ex)
            {
                throw;
            }

            return response;
        }
    }
}