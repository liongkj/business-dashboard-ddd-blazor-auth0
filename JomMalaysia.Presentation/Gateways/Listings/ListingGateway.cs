using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.CacheKeys;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Models.Listings;
using JomMalaysia.Presentation.ViewModels.Listings;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Listings
{
    public class ListingGateway : IListingGateway
    {
        
        private readonly IMemoryCache _cache;
        private readonly IWebServiceExecutor _webServiceExecutor;
        private readonly IApiBuilder _apiBuilder;

        public ListingGateway(IWebServiceExecutor webServiceExecutor,
            IApiBuilder apiBuilder, IMemoryCache cache)
        {
            _webServiceExecutor = webServiceExecutor;
            _apiBuilder = apiBuilder;
            _cache = cache;
        }
            
        public async Task<List<Listing>> GetAll()
        {
            List<Listing> result;
            IWebServiceResponse<ListViewModel<Listing>> response;
            if (!_cache.TryGetValue(CacheKeys<Listing>.Entry, out result))
            {
                
                    var req = _apiBuilder.GetApi(APIConstant.API.Path.Listing);
                    const Method method = Method.GET;
                    
                    response = await _webServiceExecutor.ExecuteRequestAsync<ListViewModel<Listing>>(req, method);
                
              

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var listings = response.Data.Data;
                    result = listings;
                }
            
                _cache.Set(CacheKeys<Listing>.Entry, result, CacheKeys<Listing>.CacheEntryOptions);
            }

            return result;
        }

        
        public async Task<IWebServiceResponse> Add(RegisterListingViewModel vm)
        {
            IWebServiceResponse<Listing> response;
           
                var req = _apiBuilder.GetApi(APIConstant.API.Path.Listing);

                const Method method = Method.POST;
                response = await _webServiceExecutor.ExecuteRequestAsync<Listing>(req, method, vm);
                
                CacheKeys<Listing>.InValidateCache(_cache);
         

            return response;


            //handle exception
        }

        
        public async Task<Listing> Detail(string id)
        {
            if (!_cache.TryGetValue(CacheKeys<Listing>.EntryItem + id, out Listing result))
            {
               
                    var req = _apiBuilder.GetApi(APIConstant.API.Path.ListingDetail, id);
                    const Method method = Method.GET;
                    var response = await _webServiceExecutor.ExecuteRequestAsync<ViewModel<Listing>>(req, method);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = response.Data.Data;
                    }
                    _cache.Set(CacheKeys<Listing>.EntryItem + id, result, CacheKeys<Listing>.CacheEntryOptions);
            }

            return result;
        }

        public async Task<IWebServiceResponse> Edit(RegisterListingViewModel vm, string listingId)
        {
            IWebServiceResponse<Listing> response;
            
                var req = _apiBuilder.GetApi(APIConstant.API.Path.ListingDetail, listingId);

                const Method method = Method.PUT;
                response = await _webServiceExecutor.ExecuteRequestAsync<Listing>(req, method, vm);
                CacheKeys<Listing>.InValidateCache(_cache,listingId);
           
            return response;
        }

        public async Task<IWebServiceResponse> Delete(string listingId)
        {
            IWebServiceResponse<Listing> response;
            
                var req = _apiBuilder.GetApi(APIConstant.API.Path.ListingDetail, listingId);

                const Method method = Method.DELETE;
                response = await _webServiceExecutor.ExecuteRequestAsync<Listing>(req, method);
                
                CacheKeys<Listing>.InValidateCache(_cache,listingId);
            
        

            return response;


            //handle exception
        }
        
        public async Task<IWebServiceResponse> Publish(string listingId, int months)
        {
            IWebServiceResponse<ListViewModel<Listing>> response;
            
                var req = _apiBuilder.GetApi(APIConstant.API.Path.Publish, listingId, months.ToString());
                const Method method = Method.POST;

                response = await _webServiceExecutor.ExecuteRequestAsync<ListViewModel<Listing>>(req, method);
                
                CacheKeys<Listing>.InValidateCache(_cache,listingId);
            

            return response;
        }
    }
}