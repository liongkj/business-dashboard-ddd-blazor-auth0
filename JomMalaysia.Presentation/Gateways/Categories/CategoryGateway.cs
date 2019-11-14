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
using JomMalaysia.Presentation.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Categories
{
    public class WorkflowGateway : ICategoryGateway
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
        
        public async Task<IWebServiceResponse> CreateCategory(NewCategoryViewModel vm, string categoryID)
        {
            IWebServiceResponse<Category> response;
            try
            {
                var req = string.IsNullOrEmpty(categoryID) ? _apiBuilder.GetApi(APIConstant.API.Path.Category) : _apiBuilder.GetApi(APIConstant.API.Path.NewSubcategory,categoryID);
               
                const Method method = Method.POST;
                response = await _webServiceExecutor.ExecuteRequestAsync<Category>(req, method, _authorizationManagers.accessToken, vm).ConfigureAwait(false);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }
            return response;

        }

        public async Task<IWebServiceResponse> EditCategory(Category vm)
        {
            IWebServiceResponse<Category> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.Category,vm.CategoryId);

                const Method method = Method.PUT;
                response = await _webServiceExecutor.ExecuteRequestAsync<Category>(req, method, _authorizationManagers.accessToken, vm).ConfigureAwait(false);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }
            return response;


            //handle exception

        }

        public async Task<List<Category>> GetCategories()
        {
            var result = new List<Category>();
            IWebServiceResponse<ListViewModel<Category>> response = default;

            try
            {
                var req = _apiBuilder.GetApi((APIConstant.API.Path.Category));
                const Method method = Method.GET;
                response = await _webServiceExecutor.ExecuteRequestAsync<ListViewModel<Category>>(req, method, _authorizationManagers.accessToken).ConfigureAwait(false);

            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            if (response.StatusCode != HttpStatusCode.OK) return result;
            var categories = response.Data.Data;
            result.AddRange(categories);
            //handle exception
            return result;
        }

        public async Task<IWebServiceResponse> Delete(string categoryId)
        {
            IWebServiceResponse<Category> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.CategoryWithId,categoryId);

                const Method method = Method.DELETE;
                response = await _webServiceExecutor.ExecuteRequestAsync<Category>(req, method, _authorizationManagers.accessToken).ConfigureAwait(false);
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
