using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models;
using JomMalaysia.Presentation.Models.Categories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Categories
{
    public class CategoryGateway : ICategoryGateway
    {
        private readonly IWebServiceExecutor _webServiceExecutor;
        private readonly IAuthorizationManagers _authorizationManagers;
        private readonly IApiBuilder _apiBuilder;
        private readonly IMapper _mapper;

        public CategoryGateway(IWebServiceExecutor webServiceExecutor, IAuthorizationManagers authorizationManagers, IApiBuilder apiBuilder, IMapper mapper)
        {
            _webServiceExecutor = webServiceExecutor;
            _authorizationManagers = authorizationManagers;
            _apiBuilder = apiBuilder;
            _mapper = mapper;

        }

        public async Task<IWebServiceResponse> CreateCategory(Category vm)
        {
            IWebServiceResponse<Category> response;
            try
            {
                var req = _apiBuilder.GetApi((APIConstant.API.Path.Category));

                var method = Method.POST;
                response = await _webServiceExecutor.ExecuteRequestAsync<Category>(req, method, _authorizationManagers.accessToken, vm);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }
            return response;


            //handle exception

        }

        public async Task<IWebServiceResponse> EditCategory(Category vm)
        {
            IWebServiceResponse<Category> response;
            try
            {
                var req = $"{_apiBuilder.GetApi((APIConstant.API.Path.Category))}/{vm.CategoryId}";

                var method = Method.PUT;
                response = await _webServiceExecutor.ExecuteRequestAsync<Category>(req, method, _authorizationManagers.accessToken, vm);
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
            List<Category> result = new List<Category>();
            IWebServiceResponse<ListViewModel<Category>> response = default;

            try
            {
                var req = _apiBuilder.GetApi((APIConstant.API.Path.Category));
                var method = Method.GET;
                response = await _webServiceExecutor.ExecuteRequestAsync<ListViewModel<Category>>(req, method, _authorizationManagers.accessToken);

            }
            catch (GatewayException ex)
            {
                throw ex;
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var categories = response.Data.Data;
                foreach (var cat in categories)
                {
                    result.Add(cat);
                }


            }
            //handle exception
            return result;
        }

        public async Task<IWebServiceResponse> Delete(string categoryId)
        {
            IWebServiceResponse<Category> response;
            try
            {
                var req = $"{_apiBuilder.GetApi((APIConstant.API.Path.Category))}/{categoryId}";

                var method = Method.DELETE;
                response = await _webServiceExecutor.ExecuteRequestAsync<Category>(req, method, _authorizationManagers.accessToken);
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
