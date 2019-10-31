using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models;
using JomMalaysia.Presentation.Models.AppUsers;
using JomMalaysia.Presentation.Models.Auth0;
using JomMalaysia.Presentation.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Users
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

        public async Task<IWebServiceResponse> Add(RegisterUserViewModel vm)
        {
            IWebServiceResponse<RegisterUserViewModel> response;
            try
            {
                var req = _apiBuilder.GetApi((APIConstant.API.Path.User));
                

                var method = Method.POST;
                response = await _webServiceExecutor.ExecuteRequestAsync<RegisterUserViewModel>(req, method, _authorizationManagers.accessToken,vm);
            }


            catch (GatewayException ex)
            {
                throw ex;
            }
            //if (response.StatusCode == HttpStatusCode.OK)
            //{
            //    result = response.Data;
            //}
            //else
            //{
            //    throw new ProcessException(response.StatusCode, ConstantHelper.Error.Common.WebServiceError);
            //}
            return response;


            //handle exception

        }

        public async Task<List<AppUser>> GetAll()
        {
            List<AppUser> result = new List<AppUser>();
            IWebServiceResponse<UserListResponse> response;
            try
            {
                var req = _apiBuilder.GetApi((APIConstant.API.Path.User));
                var method = Method.GET;
                response = await _webServiceExecutor.ExecuteRequestAsync<UserListResponse>(req, method, _authorizationManagers.accessToken);

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
            IWebServiceResponse response;
            try
            {
                var req = $"{_apiBuilder.GetApi((APIConstant.API.Path.User))}/{userId}";

                var method = Method.DELETE;
                response = await _webServiceExecutor.ExecuteRequestAsync<User>(req, method, _authorizationManagers.accessToken);
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
