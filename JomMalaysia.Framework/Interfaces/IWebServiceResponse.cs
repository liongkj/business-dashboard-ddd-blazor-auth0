using System.Net;

namespace  JomMalaysia.Framework.Interfaces
{
    public interface IWebServiceResponse
    {
        HttpStatusCode StatusCode { get; set; }
        string StatusDescription { get; set; }
        string RawContent { get; set; }
    }

    public interface IWebServiceResponse<T> : IWebServiceResponse
    {
        T Data { get; set; }
    }
}