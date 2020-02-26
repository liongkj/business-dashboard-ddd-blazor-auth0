using System.Threading.Tasks;
using RestSharp;

namespace  JomMalaysia.Framework.Interfaces
{
    public interface IWebServiceExecutor
    {
     
        Task<IWebServiceResponse<T>> ExecuteRequestAsync<T>(string url, Method method, 
            params object[] objects) where T : new();
    }

    public enum HttpParameterType
    {
        RequestBody,
        QueryString,
        HttpHeader,
        GetOrPost,
        UrlSegment,
        Cookie
    };

    //public enum Method
    //{
    //    GET,
    //    POST,
    //    PUT,
    //    DELETE
    //};
}