using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models.Workflows;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Workflows
{
    public class WorkflowGateway : IWorkflowGateway
    {
        private readonly IWebServiceExecutor _webServiceExecutor;
        private readonly IApiBuilder _apiBuilder;

        public WorkflowGateway(IWebServiceExecutor webServiceExecutor, 
            IApiBuilder apiBuilder)
        {
            _webServiceExecutor = webServiceExecutor;
            _apiBuilder = apiBuilder;
        }

        public async Task<WorkflowModel> Detail(string id)
        {
            WorkflowModel result = null;
            IWebServiceResponse<ViewModel<WorkflowModel>> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.WorkflowDetail, id);
                var method = Method.GET;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<ViewModel<WorkflowModel>>(req, method)
                    .ConfigureAwait(false);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = response.Data.Data;
            }

            //handle exception
            return result;
        }


        public async Task<List<WorkflowModel>> GetAll()
        {
            List<WorkflowModel> result = new List<WorkflowModel>();
            IWebServiceResponse<ListViewModel<WorkflowModel>> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.Workflow);
                const Method method = Method.GET;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<ListViewModel<WorkflowModel>>(req, method)
                    .ConfigureAwait(false);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var workflows = response.Data.Data;
                result.AddRange(workflows);
            }

            //handle exception
            return result;
        }
    }
}