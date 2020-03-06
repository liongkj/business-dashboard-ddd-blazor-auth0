using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.CacheKeys;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models.AppUsers;
using JomMalaysia.Presentation.Models.Auth0;
using JomMalaysia.Presentation.ViewModels.Users;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;

namespace JomMalaysia.Presentation.Gateways.Users
{
    public class UserGateway : IUserGateway
    {
        private readonly IApiBuilder _apiBuilder;
        private readonly IWebServiceExecutor _webServiceExecutor;

        private readonly IMemoryCache _cache;

        public UserGateway(IWebServiceExecutor webServiceExecutor, IAuthorizationManagers authorizationManagers,
            IApiBuilder apiBuilder, IMemoryCache cache)
        {
            _webServiceExecutor = webServiceExecutor;
            _apiBuilder = apiBuilder;
            _cache = cache;
        }

        public async Task<List<AppUser>> GetAll()
        {
            List<AppUser> result;
            IWebServiceResponse<UserListResponse> response;
            if (!_cache.TryGetValue(CacheKeys<AppUser>.Entry, out result))
            {
                try
                {
                    var req = _apiBuilder.GetApi(APIConstant.API.Path.User);
                    const Method method = Method.GET;
                    response = await _webServiceExecutor
                        .ExecuteRequestAsync<UserListResponse>(req, method)
                        .ConfigureAwait(false);
                    CacheKeys<AppUser>.InValidateCache(_cache);
                }
                catch (GatewayException ex)
                {
                    throw ex;
                }

                if (response.StatusCode != HttpStatusCode.OK) return result;
                var users = response.Data.Data.Results;
                result = users;
                _cache.Set(CacheKeys<AppUser>.Entry, result, CacheKeys<AppUser>.CacheEntryOptions);
            }

            return result;
        }

        public async Task<IWebServiceResponse> Add(RegisterUserViewModel vm)
        {
            IWebServiceResponse<RegisterUserViewModel> response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.User);
                const Method method = Method.POST;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<RegisterUserViewModel>(req, method, vm)
                    .ConfigureAwait(false);
                CacheKeys<AppUser>.InValidateCache(_cache);
            }


            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;
        }


        public async Task<AppUser> Detail(string id)
        {
            AppUser result;
            IWebServiceResponse<ViewModel<AppUser>> response;
            if (!_cache.TryGetValue(CacheKeys<AppUser>.EntryItem + id, out result))
            {
                try
                {
                    var req = _apiBuilder.GetApi(APIConstant.API.Path.UserWithId, id);
                    const Method method = Method.GET;
                    response = await _webServiceExecutor
                        .ExecuteRequestAsync<ViewModel<AppUser>>(req, method)
                        .ConfigureAwait(false);
                    CacheKeys<AppUser>.InValidateCache(_cache, id);
                }
                catch (GatewayException e)
                {
                    throw e;
                }

                if (response.StatusCode == HttpStatusCode.OK) result = response.Data.Data;
                _cache.Set(CacheKeys<AppUser>.EntryItem + id, result, CacheKeys<AppUser>.CacheEntryOptions);
            }

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
                CacheKeys<AppUser>.InValidateCache(_cache, userId);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<IWebServiceResponse> UpdateRole(string userId, RegisterUserViewModel vm)
        {
            IWebServiceResponse response;
            try
            {
                var req = _apiBuilder.GetApi(APIConstant.API.Path.UserWithId, userId);

                const Method method = Method.PATCH;
                response = await _webServiceExecutor
                    .ExecuteRequestAsync<AppUser>(req, method, vm)
                    .ConfigureAwait(false);
                CacheKeys<AppUser>.InValidateCache(_cache, userId);
            }
            catch (GatewayException ex)
            {
                throw ex;
            }

            return response;
        }

      
    }
}