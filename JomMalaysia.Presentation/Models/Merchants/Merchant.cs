using System.Collections.Generic;
using JomMalaysia.Presentation.Models.Common;
using JomMalaysia.Presentation.Models.Listings;

namespace JomMalaysia.Presentation.Models.Merchants
{
    public class Merchant
    {
        private List<string> _listings;
        private List<Contact> _contacts;
        public string MerchantId { get; set; }
        public string CompanyName { get; set; }
        public MerchantSummary CompanyRegistration { get; set; }
        public Address Address { get; set; }

        public List<string> Listings
        {
            get => _listings;
            set => _listings = value;
        }

        public List<Listing> Listing { get; set; }

        public List<Contact> Contacts
        {
            get => _contacts;
            set => _contacts = value;
        }
    }
}