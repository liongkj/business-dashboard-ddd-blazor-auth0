using Microsoft.Extensions.Configuration;

namespace JomMalaysia.Framework.Configuration
{
    public class AppSetting : IAppSetting
    {
        IConfiguration _IConfiguration;
        private static string _Auth0Domain;
        private static string _Auth0ClientId;
        private static string _Auth0ClientSecret;
        private static string _WebApiUrl;
        private static string _DbConnection;
        private static string _Scope;
        private static string _Audience;
        private static string _AdditionalClaimsRoles;

        public AppSetting(IConfiguration IConfiguration)
        {
            _IConfiguration = IConfiguration;
            Initialize();
        }

        public string AdditionalClaimsRoles => _AdditionalClaimsRoles;
        public string Auth0Domain => _Auth0Domain;

        public string Auth0ClientId => _Auth0ClientId;

        public string Auth0ClientSecret => _Auth0ClientSecret;

        public string WebApiUrl => _WebApiUrl;

        public string DBConnection => _DbConnection;

        public string Scope => _Scope;
        public string Audience => _Audience;

        public void Initialize()
        {
            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:Domain")))
            {
                _Auth0Domain = _IConfiguration.GetValue<string>("Auth0:Domain");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:ClientId")))
            {
                _Auth0ClientId = _IConfiguration.GetValue<string>("Auth0:ClientId");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:ClientSecret")))
            {
                _Auth0ClientSecret = _IConfiguration.GetValue<string>("Auth0:ClientSecret");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:DBConnection")))
            {
                _DbConnection = _IConfiguration.GetValue<string>("Auth0:DBConnection");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("WebApiURl")))
            {
                _WebApiUrl = _IConfiguration.GetValue<string>("WebApiURl");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:Audience")))
            {
                _Audience = _IConfiguration.GetValue<string>("Auth0:Audience");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:Scope")))
            {
                _Scope = _IConfiguration.GetValue<string>("Auth0:Scope");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:AdditionalClaims:Roles")))
            {
                _AdditionalClaimsRoles = _IConfiguration.GetValue<string>("Auth0:AdditionalClaims:Roles");
            }
        }
    }
}