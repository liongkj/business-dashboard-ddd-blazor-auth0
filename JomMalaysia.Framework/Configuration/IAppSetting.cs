﻿namespace JomMalaysia.Framework.Configuration
{
    public interface IAppSetting
    {
        void Initialize();

        string WebApiUrl { get; }

        string Auth0Domain { get; }

        string Auth0ClientId { get; }

        string Auth0ClientSecret { get; }

        string DbConnection { get; }

        string Scope { get; }
        string Audience { get; }
        string AdditionalClaimsRoles { get; }
    }
}