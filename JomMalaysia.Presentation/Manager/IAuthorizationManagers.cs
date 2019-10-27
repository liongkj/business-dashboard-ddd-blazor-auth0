
using JomMalaysia.Presentation.Models.Auth0;

namespace JomMalaysia.Presentation.Manager
{
    public interface IAuthorizationManagers
    {
        string accessToken { get; }

        string refreshToken { get; }

        UserInfoViewModel LoginInfo { get; }
    }
}
