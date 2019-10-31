using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JomMalaysia.Framework.Constant
{
    public class GlobalConstant
    {
        public class StatusCode {
            public const int RESPONSE_OK = 1;
            public const int RESPONSE_ERR_UNKNOWN = -99;
            public const int RESPONSE_ERR_DUPLICATED = -1;
            public const int RESPONSE_ERR_DEPENDENCY = -2;
            public const int RESPONSE_ERR_NOT_FOUND = -3;
            public const int RESPONSE_ERR_VALIDATION_FAILED = -4;
        }

        public class Message
        {
            public const string VALIDATION = "Validation error. Please fix the following errors.";
            public const string COMPLETED = "Operation completed successfully.";
            public const string INCOMPLETE = "Some issue happen to your request. Please double check you submission again.";
            public const string CONFLICT = "Please choose another username/email.";
            public const string UNKNOWN = "Unknown error occur. Please contact your administrator.";
            public const string NOTFOUND = "The record is not found. Are you selecting the correct record?";
        }
      
    }

    
}
