using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.Models.Workflows;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Workflows
{
    public class WorkflowGateway : IWorkflowGateway
    {
        private readonly IWebServiceExecutor _webServiceExecutor;
        private readonly IAuthorizationManagers _authorizationManagers;
        private readonly IApiBuilder _apiBuilder;

        public WorkflowGateway(IWebServiceExecutor webServiceExecutor, IAuthorizationManagers authorizationManagers, IApiBuilder apiBuilder)
        {
            _webServiceExecutor = webServiceExecutor;
            _authorizationManagers = authorizationManagers;
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
                response = await _webServiceExecutor.ExecuteRequestAsync<ViewModel<WorkflowModel>>(req, method, _authorizationManagers.accessToken).ConfigureAwait(false);

            }
            catch (GatewayException ex)
            {
                throw ex;
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {

               result= response.Data.Data;
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
                var req = _apiBuilder.GetApi((APIConstant.API.Path.Workflow));
                var method = Method.GET;
                response = await _webServiceExecutor.ExecuteRequestAsync<ListViewModel<WorkflowModel>>(req, method, _authorizationManagers.accessToken).ConfigureAwait(false);

            }
            catch (GatewayException ex)
            {
                throw ex;
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Console.Error.WriteLine("called workflow api");
                var workflows = response.Data.Data;
                foreach (var work in workflows)
                {
                    result.Add(work);
                }


            }
            //handle exception
            return result;
        }

        //public async Task<IWebServiceResponse> Delete(string categoryId)
        //{
        //    IWebServiceResponse<Category> response;
        //    try
        //    {
        //        var req = $"{_apiBuilder.GetApi((APIConstant.API.Path.Category))}/{categoryId}";

        //        var method = Method.DELETE;
        //        response = await _webServiceExecutor.ExecuteRequestAsync<Category>(req, method, _authorizationManagers.accessToken).ConfigureAwait(false);
        //    }
        //    catch (GatewayException ex)
        //    {
        //        throw ex;
        //    }
        //    return response;


        //    //handle exception

        //}
    }
}
