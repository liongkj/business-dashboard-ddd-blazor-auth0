using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models.AppUsers;
using JomMalaysia.Presentation.Models.Auth0;
using JomMalaysia.Presentation.ViewModels.Users;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Users
{
    public class UserGateway : IUserGateway
    {
        private readonly IApiBuilder _apiBuilder;
        private readonly IAuthorizationManagers _authorizationManagers;
        private readonly IWebServiceExecutor _webServiceExecutor;

        public UserGateway(IWebServiceExecutor webServiceExecutor, IAuthorizationManagers authorizationManagers,
            IApiBuilder apiBuilder)
        {
            _webServiceExecutor = webServiceExecutor;
            _authorizationManagers = authorizationManagers;
            _apiBuilder = apiBuilder;
        }

        public async Task<IWebServiceResponse> Add(RegisterUserViewModel vm)
        {
            IWebServiceResponse<RegisterUserViewModel> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.User);
                var method = Method.POST;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<RegisterUserViewModel>(req, method, vm)
                    .ConfigureAwait(false);
            }


            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<List<AppUser>> GetAll()
        {
            var result = new List<AppUser>();
            IWebServiceResponse<UserListResponse> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.User);
                var method = Method.GET;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<UserListResponse>(req, method)
                    .ConfigureAwait(false);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            if (response.StatusCode != HttpStatusCode.OK) return result;
            var users = response.Data.Data.Results;
            result.AddRange(users);
            //handle exception
            return result;
        }

        public async Task<AppUser> Detail(string id)
        {
            AppUser result = null;
            IWebServiceResponse<ViewModel<AppUser>> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.UserWithId, id);
                const Method method = Method.GET;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<ViewModel<AppUser>>(req, method)
                    .ConfigureAwait(false);
            }
            catch (GatewayException e)
            {
                throw e;
            }

            if (response.StatusCode == HttpStatusCode.OK) result = response.Data.Data;
            //handle exception
            return result;
        }

        public async Task<IWebServiceResponse> Delete(string userId)
        {
            IWebServiceResponse response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.UserWithId, userId);

                const Method method = Method.DELETE;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<User>(req, method).ConfigureAwait(false);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<IWebServiceResponse> UpdateRole(string UserId, RegisterUserViewModel vm)
        {
            IWebServiceResponse response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.UserWithId, UserId);

                const Method method = Method.PATCH;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<AppUser>(req, method, vm)
                    .ConfigureAwait(false);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;
        }
    }
}