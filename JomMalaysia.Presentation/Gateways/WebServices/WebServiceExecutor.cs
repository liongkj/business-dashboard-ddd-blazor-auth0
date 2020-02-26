using System;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.WebServices
{
    public class WebServiceExecutor : IWebServiceExecutor
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public WebServiceExecutor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IWebServiceResponse<T>> ExecuteRequestAsync<T>(string url, Method method, 
            params object[] objects) where T : new()
        {
            //Create Client
            IRestClient client =  RestSharpFactory.ConstructClient(NetHelper.GetBaseUrl(url), await
                _httpContextAccessor.HttpContext.GetTokenAsync("access_token"));
            //Create Request
            IRestRequest request = RestSharpFactory.ConstructRequest(NetHelper.GetUrlPath(url), method, objects);

            //wait response
            IRestResponse<T> response = await client.ExecuteAsync<T>(request);
            ThrowExceptionIfError(response);
            return new WebServiceResponse<T>()
            {
                RawContent = response.Content,
                StatusCode = response.StatusCode,
                StatusDescription = response.StatusDescription,
                Data = response.Data
            };
        }


        /// <summary>
        /// Throws a WebServiceException if the request is not successful. The current implementation is coupled
        /// with Web API. So it may NOT work in other APIs.
        /// </summary>
        /// <param name="response">Response from RestSharp</param>
        /// <exception cref="WebServiceException">Thrown if request is not successful.</exception>
        protected virtual void ThrowExceptionIfError(IRestResponse response)
        {
            if (response.ErrorException != null && String.IsNullOrEmpty(response.StatusDescription))
            {
                throw new GatewayException("Failed to connect to web service.", response.ErrorException);
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return;
                // Business Exceptions
                case HttpStatusCode.Unauthorized:
                    dynamic content = null;
                    
                    throw new GatewayException(response.StatusCode,response.ErrorMessage);
                case HttpStatusCode.Conflict:
                {
                    // Parse the content manually to get the error details (if any).
                    dynamic conflictContent = null;
                    try
                    {
                        conflictContent = JObject.Parse(response.Content);
                    }
                    catch (Exception)
                    {
                        // Do nothing.
                    }

                    if (conflictContent != null && conflictContent.message != null)
                    {
                        string msg = conflictContent.message;
                        throw new GatewayException(response.StatusCode, msg);
                    }

                    break;
                }
            }
            //others

            // Business Exceptions

            // Parse the content manually to get the error details (if any).
            dynamic badRequest = null;
            try
            {
                badRequest = JObject.Parse(response.Content);
            }
            catch (Exception)
            {
                // Do nothing.
            }

            if (badRequest != null && badRequest.message != null)
            {
                string msg = badRequest.message;
                throw new GatewayException(response.StatusCode, msg);
            }

            if (badRequest != null && badRequest.errors != null)
            {
                string msg = badRequest.errors[0];
                throw new GatewayException(response.StatusCode, msg);
            }
        }
    }
}