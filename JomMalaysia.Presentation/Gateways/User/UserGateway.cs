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
using JomMalaysia.Presentation.Models.Auth0;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.User
{
    public class UserGateway : IUserGateway
    {
        private readonly IWebServiceExecutor _webServiceExecutor;
        private readonly IAuthorizationManagers _authorizationManagers;
        private readonly IApiBuilder _apiBuilder;
        private readonly IMapper _mapper;

        public UserGateway(IWebServiceExecutor webServiceExecutor, IAuthorizationManagers authorizationManagers, IApiBuilder apiBuilder, IMapper mapper)
        {
            _webServiceExecutor = webServiceExecutor;
            _authorizationManagers = authorizationManagers;
            _apiBuilder = apiBuilder;
            _mapper = mapper;

        }

        public async Task<IWebServiceResponse> Add(UserInfoViewModel vm)
        {
            IWebServiceResponse<UserInfoViewModel> response;
            try
            {
                var req = _apiBuilder.GetApi((APIConstant.API.Path.User));

                var method = Method.GET;
                response = await _webServiceExecutor.ExecuteRequestAsync<UserInfoViewModel>(req, method, _authorizationManagers.accessToken);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }
            return response;


            //handle exception

        }

        public async Task<List<UserInfoViewModel>> GetAll()
        {
            List<UserInfoViewModel> result = new List<UserInfoViewModel>();
            IWebServiceResponse<UserListViewModel> response = default;

            try
            {
                var req = _apiBuilder.GetApi((APIConstant.API.Path.User));
                var method = Method.GET;
                response = await _webServiceExecutor.ExecuteRequestAsync<UserListViewModel>(req, method, _authorizationManagers.accessToken);

            }
            catch (GatewayException ex)
            {
                throw ex;
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var users = response.Data.Data.Results;
                foreach (var u in users)
                {
                    result.Add(u);
                }
            }
            //handle exception
            return result;
        }

        public async Task<IWebServiceResponse> Delete(string userId)
        {
            IWebServiceResponse<CategoryViewModel> response;
            try
            {
                var req = $"{_apiBuilder.GetApi((APIConstant.API.Path.User))}/{userId}";

                var method = Method.DELETE;
                response = await _webServiceExecutor.ExecuteRequestAsync<CategoryViewModel>(req, method, _authorizationManagers.accessToken);
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
