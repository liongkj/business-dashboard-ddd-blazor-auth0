using System.Collections.Generic;
using JomMalaysia.Presentation.Models.Common;

namespace JomMalaysia.Presentation.ViewModels.Merchants
{
    public class CreateMerchant
    {
        public string SsmId { get; set; }
        public string CompanyRegistrationName { get; set; }
        public Address Address { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}