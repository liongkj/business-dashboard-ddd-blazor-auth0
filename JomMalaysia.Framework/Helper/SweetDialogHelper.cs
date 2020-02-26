using System;
using System.Net;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Interfaces;

namespace JomMalaysia.Framework.Helper
{
    public static class SweetDialogHelper
    {
        public static Tuple<int, string> HandleResponse(IWebServiceResponse response)
        {
            if (response == null)
                return Tuple.Create(GlobalConstant.StatusCode.RESPONSE_ERR_VALIDATION_FAILED,
                    GlobalConstant.Message.VALIDATION);
            return HandleStatusCode(response.StatusCode, response.StatusDescription);
        }

        public static Tuple<int, string> HandleStatusCode(HttpStatusCode StatusCode, string msg = null)
        {
            switch (StatusCode)
            {
                case HttpStatusCode.OK:
                    return Tuple.Create(GlobalConstant.StatusCode.RESPONSE_OK, msg ?? GlobalConstant.Message.COMPLETED);
                case HttpStatusCode.BadRequest:
                    return Tuple.Create(GlobalConstant.StatusCode.RESPONSE_ERR_DEPENDENCY,
                        msg ?? GlobalConstant.Message.INCOMPLETE);
                case HttpStatusCode.Conflict:
                    return Tuple.Create(GlobalConstant.StatusCode.RESPONSE_ERR_DUPLICATED,
                        msg ?? GlobalConstant.Message.CONFLICT);
                default:
                    return Tuple.Create(GlobalConstant.StatusCode.RESPONSE_ERR_UNKNOWN,
                        msg ?? GlobalConstant.Message.UNKNOWN);
            }
        }

        public static Tuple<int, string> HandleNotFound()
        {
            return Tuple.Create(GlobalConstant.StatusCode.RESPONSE_ERR_NOT_FOUND, GlobalConstant.Message.NOTFOUND);
        }
    }
}