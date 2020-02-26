using JomMalaysia.Framework.Constant;
using JomMalaysia.Presentation.Models.AppUsers;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace JomMalaysia.Presentation.Manager
{
    public class AuthorizationManagers : IAuthorizationManagers
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
     
        public AuthorizationManagers(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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