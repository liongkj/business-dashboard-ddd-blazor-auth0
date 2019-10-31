using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.WebServices;

namespace JomMalaysia.Framework.Helper
{
    public static class SweetDialogHelper
    {
        public static Tuple<int, string> HandleResponse(IWebServiceResponse response)
        {
            if (response == null) return
                       Tuple.Create(GlobalConstant.StatusCode.RESPONSE_ERR_VALIDATION_FAILED, GlobalConstant.Message.VALIDATION);
            return HandleStatusCode(response.StatusCode);
        }

        public static Tuple<int, string> HandleStatusCode(HttpStatusCode StatusCode,string msg = null)
        {

            if (StatusCode == HttpStatusCode.OK)
            {
                return Tuple.Create(GlobalConstant.StatusCode.RESPONSE_OK, msg ?? GlobalConstant.Message.COMPLETED);
            }
            else if (StatusCode == HttpStatusCode.BadRequest)
            {
                return Tuple.Create(GlobalConstant.StatusCode.RESPONSE_ERR_DEPENDENCY, msg ?? GlobalConstant.Message.INCOMPLETE);
            }
            else if(StatusCode == HttpStatusCode.Conflict)
            {
                return Tuple.Create(GlobalConstant.StatusCode.RESPONSE_ERR_DUPLICATED, msg ?? GlobalConstant.Message.CONFLICT);
            }
            else return Tuple.Create(GlobalConstant.StatusCode.RESPONSE_ERR_UNKNOWN, msg ?? GlobalConstant.Message.UNKNOWN);            
}

        public static Tuple<int, string> HandleNotFound()
        {
            return Tuple.Create(GlobalConstant.StatusCode.RESPONSE_ERR_NOT_FOUND, GlobalConstant.Message.NOTFOUND);
        }

    }
}
