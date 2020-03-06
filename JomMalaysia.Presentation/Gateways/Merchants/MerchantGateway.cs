using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.CacheKeys;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Models.Merchants;
using JomMalaysia.Presentation.ViewModels.Merchants;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Merchants
{
    public class MerchantGateway : IMerchantGateway
    {
        private readonly IWebServiceExecutor _webServiceExecutor;
        private readonly IApiBuilder _apiBuilder;

        private readonly IMemoryCache _cache;

        public MerchantGateway(IWebServiceExecutor webServiceExecutor,
            IApiBuilder apiBuilder, IMemoryCache cache)
        {
            _webServiceExecutor = webServiceExecutor;
            _apiBuilder = apiBuilder;
            _cache = cache;
        }

        public async Task<List<Merchant>> GetMerchants()
        {
            List<Merchant> result;
            IWebServiceResponse<ListViewModel<Merchant>> response;
            if (!_cache.TryGetValue(CacheKeys<Merchant>.Entry, out result))
            {
                try
                {
                    var req = _apiBuilder.GetApi(APIConstant.API.Path.Merchant);
                    const Method method = Method.GET;
                    response = await _webServiceExecutor
                        .ExecuteRequestAsync<ListViewModel<Merchant>>(req, method)
                        .ConfigureAwait(false);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var listings = response.Data.Data;
                        result = listings;
                    }
                }
                catch (GatewayException ex)
                {
                    throw ex;
                }

                _cache.Set(CacheKeys<Merchant>.Entry, result, CacheKeys<Merchant>.CacheEntryOptions);
            }

            return result;
        }

        public async Task<IWebServiceResponse> Add(RegisterMerchantViewModel vm)
        {
            IWebServiceResponse<Merchant> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.Merchant);

                const Method method = Method.POST;
                response = await _webServiceExecutor.ExecuteRequestAsync<Merchant>(req, method, vm)
                    .ConfigureAwait(false);
                CacheKeys<Merchant>.InValidateCache(_cache);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;


            //handle exception
        }


        public async Task<Merchant> Detail(string id)
        {
            Merchant result;
            IWebServiceResponse<ViewModel<Merchant>> response;
            if (!_cache.TryGetValue(CacheKeys<Merchant>.EntryItem + id, out result))
            {
                try
                {
                    var req = _apiBuilder.GetApi(APIConstant.API.Path.MerchantDetail, id);
                    const Method method = Method.GET;
                    response = await _webServiceExecutor.ExecuteRequestAsync<ViewModel<Merchant>>(req, method)
                        .ConfigureAwait(false);
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

                _cache.Set(CacheKeys<Merchant>.EntryItem + id, result, CacheKeys<Merchant>.CacheEntryOptions);
            }

            return result;
        }


        public async Task<IWebServiceResponse> Edit(RegisterMerchantViewModel vm, string merchantId)
        {
            IWebServiceResponse<Merchant> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.MerchantDetail, merchantId);

                const Method method = Method.PUT;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<Merchant>(req, method, vm)
                    .ConfigureAwait(false);
                CacheKeys<Merchant>.InValidateCache(_cache,merchantId);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;


            //handle exception
        }
    }
}