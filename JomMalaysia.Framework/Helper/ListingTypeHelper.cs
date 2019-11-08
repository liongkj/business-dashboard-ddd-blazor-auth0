using System;
using System.Collections.Generic;
using System.Text;

namespace JomMalaysia.Framework.Helper
{
    public static class ListingTypeHelper
    {
        private readonly static string Local = "Local";
        private readonly static string Civic = "Civic";
        private readonly static string Gover = "Government";
        private readonly static string Event = "Event";

        public static List<string> GetTypeList()
        {
            List<string> Types = new List<string>
            {
                Local,
                Civic,
                Gover,
                Event
            };
            return Types;
        }
    }
}
