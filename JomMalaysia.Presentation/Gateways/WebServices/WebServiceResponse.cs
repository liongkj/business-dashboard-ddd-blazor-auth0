using System.Net;
using JomMalaysia.Framework.Interfaces;

namespace JomMalaysia.Presentation.Gateways.WebServices
{
    public class WebServiceResponse : IWebServiceResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string RawContent { get; set; }
    }

    public class WebServiceResponse<T> : WebServiceResponse, IWebServiceResponse<T>
    {
        public T Data { get; set; }
    }
}