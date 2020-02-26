using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Presentation.Manager;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using RestSharp;


namespace  JomMalaysia.Presentation.Gateways.WebServices
{
    public class RestSharpFactory
    {
      
        public static IRestRequest ConstructRequest(string path, Method method, object[] objects,
            Dictionary<string, string> query = null)
        {
            IRestRequest request = new RestRequest(path, method, DataFormat.Json)
            {
                JsonSerializer = NewtonsoftJsonSerializer.Default,
                Timeout = TimeSpan.FromMinutes(60).Milliseconds
            };
            if (query != null)
            {
                foreach (var q in query)
                {
                    request.AddQueryParameter(q.Key, q.Value);
                }
            }

            foreach (var obj in objects)
            {
                request.AddJsonBody(obj);
            }

            return request;
        }

        /// <summary>
        /// Constructs a RestSharp client.
        /// </summary>
        /// <param name="baseUrl">Base URL of web service to connect. (Example: http://api.google.com)</param>
        /// <returns>A RestSharp client.</returns>
        public static  IRestClient ConstructClient(string baseUrl, string auth)
        {
            var client = new RestClient(baseUrl)
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            
            
            client.ClearHandlers();
            client.AddDefaultHeader($"Authorization", $"Bearer {auth}");
            var handler = NewtonsoftJsonSerializer.Default;
            client.AddHandler("application/json", () => handler); // Use custom deserializer.
            client.AddHandler("text/json", () => handler);
            client.AddHandler("text/x-json", () => handler);
            client.AddHandler("text/javascript", () => handler);
            client.AddHandler("*+json", () => handler);
            //bypass ssl validation check by using RestClient object
            
           
            client.Timeout = 300000; // 5 minutes
            return client;
        }
    }
}