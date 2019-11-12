using System.Collections.Generic;
using JomMalaysia.Presentation.Models.Common;

namespace JomMalaysia.Presentation.Models.Merchants
{
    public class Merchant
    {
        public string MerchantId { get; set; }
        public string CompanyName { get; set; }
        public MerchantSummary CompanyRegistration { get; set; }
        public Address Address { get; set; }
        public List<string> Listings { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
