using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JomMalaysia.Presentation.Models.Workflows
{
    public class Listing
    {
        public string ListingId { get; set; }
        public Merchant Merchant { get; set; }
        public string ListingName { get; set; }
        public string ListingType { get; set; }
        public string Status { get; set; }
    }

    public class Merchant
    {
        public string MerchantId { get; set; }
        public string SsmId { get; set; }
        public string RegistrationName { get; set; }
    }
}
