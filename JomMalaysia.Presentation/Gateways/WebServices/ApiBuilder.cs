﻿using System.Text;
using JomMalaysia.Framework.Configuration;
using JomMalaysia.Framework.Interfaces;

namespace  JomMalaysia.Presentation.Gateways.WebServices
{
    public class ApiBuilder : IApiBuilder
    {
        private readonly IAppSetting _appSetting;


        public ApiBuilder(IAppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        public string WebApiUrl => _appSetting.WebApiUrl;


        public string GetApi(string path, params string[] parameters)
        {
            var apiString = new StringBuilder();
            apiString.Append(WebApiUrl);
            apiString.Append(path);

            for (var i = 0; i < parameters.Length; i++)
            {
                apiString.Replace("{" + i + "}", parameters[i]);
            }

            return apiString.ToString();
        }
    }
}