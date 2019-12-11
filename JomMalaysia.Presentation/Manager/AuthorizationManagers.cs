using JomMalaysia.Framework.Constant;
using JomMalaysia.Presentation.Models;
using JomMalaysia.Presentation.Models.AppUsers;
using JomMalaysia.Presentation.Models.Auth0;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace JomMalaysia.Presentation.Manager
{
    public class AuthorizationManagers : IAuthorizationManagers
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationManagers(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string accessToken
        {
            get
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims;
                return claims.Where(c => c.Type == ConstantHelper.Claims.accessToken).Select(c => c.Value)
                    .FirstOrDefault();
            }
        }

        public string refreshToken
        {
            get
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims;
                return claims.Where(c => c.Type == ConstantHelper.Claims.refreshToken).Select(c => c.Value)
                    .FirstOrDefault();
            }
        }

        public AppUser LoginInfo
        {
            get
            {
                AppUser userInfo = new AppUser();

                var claims = _httpContextAccessor.HttpContext.User.Claims;
                userInfo.UserId = claims.Where(c => c.Type == ConstantHelper.Claims.userId).Select(c => c.Value)
                    .FirstOrDefault();
                userInfo.Role = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();

                userInfo.Name = claims.Where(c => c.Type == ConstantHelper.Claims.name).Select(c => c.Value)
                    .FirstOrDefault();
                return userInfo;
            }
        }
    }
}