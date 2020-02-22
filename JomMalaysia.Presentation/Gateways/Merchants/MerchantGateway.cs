﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models.Merchants;
using JomMalaysia.Presentation.ViewModels.Merchants;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Merchants
{
    public class MerchantGateway : IMerchantGateway
    {
        private readonly IWebServiceExecutor _webServiceExecutor;
        private readonly IAuthorizationManagers _authorizationManagers;
        private readonly IApiBuilder _apiBuilder;
        private readonly string _auth;

        public MerchantGateway(IWebServiceExecutor webServiceExecutor, IAuthorizationManagers authorizationManagers,
            IApiBuilder apiBuilder)
        {
            _webServiceExecutor = webServiceExecutor;
            _authorizationManagers = authorizationManagers;
            _apiBuilder = apiBuilder;
            _auth = _authorizationManagers.accessToken;
        }

        public async Task<IWebServiceResponse> Add(RegisterMerchantViewModel vm)
        {
            IWebServiceResponse<Merchant> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.Merchant);

                var method = Method.POST;
                response = await _webServiceExecutor.ExecuteRequestAsync<Merchant>(req, method, _auth, vm)
                    .ConfigureAwait(false);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;


            //handle exception
        }

        public async Task<List<Merchant>> GetMerchants()
        {
            List<Merchant> result = new List<Merchant>();
            IWebServiceResponse<ListViewModel<Merchant>> response;
            try
            {
                var req = _apiBuilder.GetApi((APIConstant.API.Path.Merchant));
                var method = Method.GET;
                response = await _webServiceExecutor.ExecuteRequestAsync<ListViewModel<Merchant>>(req, method, _auth)
                    .ConfigureAwait(false);
            }
            catch (GatewayException ex)
            {
                throw ex;
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
        
        public async Task<Merchant> Detail(string id)
        {
            Merchant result = null;
            IWebServiceResponse<ViewModel<Merchant>> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.MerchantDetail, id);
                const Method method = Method.GET;
                response = await _webServiceExecutor.ExecuteRequestAsync<ViewModel<Merchant>>(req, method, _auth)
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

            //handle exception
            return result;
        }
        public async Task<IWebServiceResponse> Edit(RegisterMerchantViewModel vm, string merchangId)
        {
            IWebServiceResponse<Merchant> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.MerchantDetail, merchangId);

                const Method method = Method.PUT;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<Merchant>(req, method, _auth, vm)
                    .ConfigureAwait(false);
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