using System;
using System.Threading.Tasks;
using JomMalaysia.Framework.CacheKeys;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Indexes
{
    public class IndexGateway : IIndexGateway
    {
        private readonly IWebServiceExecutor _webServiceExecutor;
        private readonly IApiBuilder _apiBuilder;


        public IndexGateway(IWebServiceExecutor webServiceExecutor,IApiBuilder apiBuilder)
        {
            _webServiceExecutor = webServiceExecutor;
            _apiBuilder = apiBuilder;
        }
   

        public async Task<IWebServiceResponse> BatchIndex()
        {
            IWebServiceResponse<Index> response;
            try
            {
                var req = _apiBuilder.GetApi(ApiConstant.Api.Path.PLACE_INDEX);
                    

                const Method method = Method.POST;
                response = await _webServiceExecutor.ExecuteRequestAsync<Index>(req, method);
                
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;
        }

    }
}