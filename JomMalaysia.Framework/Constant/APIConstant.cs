namespace JomMalaysia.Framework.Constant
{
    public class ApiConstant
    {
        public class Api
        {
            public class Path
            {
                //category
                public const string CATEGORY = "api/Categories";

                public const string CATEGORY_WITH_ID = "api/Categories/{0}";

                public const string NEW_SUBCATEGORY = "api/Categories/{0}/Subcategories";

                //user
                public const string USER = "api/Users";
                public const string USER_WITH_ID = "api/Users/{0}";

                //merchant
                public const string MERCHANT = "api/Merchants";
                public const string MERCHANT_DETAIL = "api/Merchants/{0}";

                //workflow
                public const string WORKFLOW = "api/Workflows";
                public const string WORKFLOW_DETAIL = "api/Workflows/{0}";

                //listing
                public const string PUBLISH = "api/Listings/{0}/Publish/{1}";
                public const string LISTING = "api/Listings";
                public const string LISTING_DETAIL = "api/Listings/{0}";
                
                //index
                internal const string INDEX = "api/algolia";
                public const string PLACE_INDEX = INDEX +"/places/batch";
            }
        }
    }
}