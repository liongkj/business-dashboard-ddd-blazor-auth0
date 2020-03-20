using Microsoft.Extensions.Configuration;

namespace JomMalaysia.Framework.Configuration
{
    public class AppSetting : IAppSetting
    {
        IConfiguration _IConfiguration;
        private static string _auth0Domain;
        private static string _auth0ClientId;
        private static string _auth0ClientSecret;
        private static string _webApiUrl;
        private static string _dbConnection;
        private static string _scope;
        private static string _audience;
        private static string _additionalClaimsRoles;

        public AppSetting(IConfiguration configuration)
        {
            _IConfiguration = configuration;
            Initialize();
        }

        public string AdditionalClaimsRoles => _additionalClaimsRoles;
        public string Auth0Domain => _auth0Domain;

        public string Auth0ClientId => _auth0ClientId;

        public string Auth0ClientSecret => _auth0ClientSecret;

        public string WebApiUrl => _webApiUrl;

        public string DbConnection => _dbConnection;

        public string Scope => _scope;
        public string Audience => _audience;

        public void Initialize()
        {
            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:Domain")))
            {
                _auth0Domain = _IConfiguration.GetValue<string>("Auth0:Domain");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:ClientId")))
            {
                _auth0ClientId = _IConfiguration.GetValue<string>("Auth0:ClientId");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:ClientSecret")))
            {
                _auth0ClientSecret = _IConfiguration.GetValue<string>("Auth0:ClientSecret");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:DBConnection")))
            {
                _dbConnection = _IConfiguration.GetValue<string>("Auth0:DBConnection");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("WebApiURl")))
            {
                _webApiUrl = _IConfiguration.GetValue<string>("WebApiURl");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:Audience")))
            {
                _audience = _IConfiguration.GetValue<string>("Auth0:Audience");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:Scope")))
            {
                _scope = _IConfiguration.GetValue<string>("Auth0:Scope");
            }

            if (!string.IsNullOrEmpty(_IConfiguration.GetValue<string>("Auth0:AdditionalClaims:Roles")))
            {
                _additionalClaimsRoles = _IConfiguration.GetValue<string>("Auth0:AdditionalClaims:Roles");
            }
        }
    }
}