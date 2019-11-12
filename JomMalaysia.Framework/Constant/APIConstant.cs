using System;
using System.Collections.Generic;
using System.Text;

namespace JomMalaysia.Framework.Constant
{
    public partial class APIConstant
    {
        public class API
        {
            public class Path
            {
                public const string Category = "api/Categories";

                

              

                public const string User = "api/Users";

                public const string Merchant = "api/Merchants";

                public const string Workflow = "api/Workflows";
                public const string WorkflowDetail = "api/Workflows/{0}";

                //listing
                public const string Publish = "api/Listings/{0}/Publish/{1}";
                public const string Listing = "api/Listings";
            }
        }
    }
}
