using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace JomMalaysia.Framework.WebServices
{
    public class WebServiceExecutor : IWebServiceExecutor
    {
        public virtual IWebServiceResponse ExecuteRequest(string url, Method method, string auth,
            params object[] objects)
        {
            //Create Client
            IRestClient client = RestSharpFactory.ConstructClient(NetHelper.GetBaseUrl(url));
            client.AddDefaultHeader($"Authorization", $"Bearer {auth}");
            //Create Request
            IRestRequest request = RestSharpFactory.ConstructRequest(NetHelper.GetUrlPath(url), method, objects);
            //wait response
            IRestResponse response = client.Execute(request);
            ThrowExceptionIfError(response);
            return new WebServiceResponse()
            {
                RawContent = response.Content,
                StatusCode = response.StatusCode,
                StatusDescription = response.StatusDescription
            };
        }


        public IWebServiceResponse<T> ExecuteRequest<T>(string url, Method method, string auth, params object[] objects)
            where T : new()
        {
            //Create Client
            IRestClient client = RestSharpFactory.ConstructClient(NetHelper.GetBaseUrl(url));
            client.AddDefaultHeader($"Authorization", $"Bearer {auth}");
            //Create Request
            IRestRequest request = RestSharpFactory.ConstructRequest(NetHelper.GetUrlPath(url), method, objects);
            //wait response
            IRestResponse<T> response = client.Execute<T>(request);
            ThrowExceptionIfError(response);


            return new WebServiceResponse<T>()
            {
                RawContent = response.Content,
                StatusCode = response.StatusCode,
                StatusDescription = response.StatusDescription,
                Data = response.Data
            };
        }


        public async Task<IWebServiceResponse<T>> ExecuteRequestAsync<T>(string url, Method method, string auth,
            params object[] objects) where T : new()
        {
            //Create Client
            IRestClient client = RestSharpFactory.ConstructClient(NetHelper.GetBaseUrl(url));
            client.AddDefaultHeader($"Authorization", $"Bearer {auth}");
            //Create Request
            IRestRequest request = RestSharpFactory.ConstructRequest(NetHelper.GetUrlPath(url), method, objects);

            //wait response
            IRestResponse<T> response = await client.ExecuteTaskAsync<T>(request);
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