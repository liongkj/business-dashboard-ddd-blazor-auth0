namespace JomMalaysia.Framework.Constant
{
    public class APIConstant
    {
        public class API
        {
            public class Path
            {
                //category
                public const string Category = "api/Categories";

                public const string CategoryWithId = "api/Categories/{0}";

                public const string NewSubcategory = "api/Categories/{0}/Subcategories";

                //user
                public const string User = "api/Users";
                public const string UserWithId = "api/Users/{0}";

                //merchant
                public const string Merchant = "api/Merchants";
                public const string MerchantDetail = "api/Merchants/{0}";

                //workflow
                public const string Workflow = "api/Workflows";
                public const string WorkflowDetail = "api/Workflows/{0}";

                //listing
                public const string Publish = "api/Listings/{0}/Publish/{1}";
                public const string Listing = "api/Listings";
                public const string ListingDetail = "api/Listings/{0}";
            }
        }
    }
}