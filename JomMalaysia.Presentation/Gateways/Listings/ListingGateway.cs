using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models;
using JomMalaysia.Presentation.Models.Listings;
using JomMalaysia.Presentation.ViewModels.Listings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Listings
{
    public class ListingGateway : IListingGateway
    {
        private readonly IWebServiceExecutor _webServiceExecutor;
        private readonly IAuthorizationManagers _authorizationManagers;
        private readonly IApiBuilder _apiBuilder;
        private readonly string auth;

        public ListingGateway(IWebServiceExecutor webServiceExecutor, IAuthorizationManagers authorizationManagers, IApiBuilder apiBuilder)
        {
            _webServiceExecutor = webServiceExecutor;
            _authorizationManagers = authorizationManagers;
            _apiBuilder = apiBuilder;
            if (_authorizationManagers != null) auth = _authorizationManagers.accessToken;
        }

        public async Task<IWebServiceResponse> Add(RegisterListingViewModel vm)
        {
            IWebServiceResponse<Listing> response;
            try
            {
                var req = _apiBuilder.GetApi((APIConstant.API.Path.Listing));

                var method = Method.POST;
                response = await _webServiceExecutor.ExecuteRequestAsync<Listing>(req, method, auth).ConfigureAwait(false);
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
                var method = Method.GET;
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
    }
}
