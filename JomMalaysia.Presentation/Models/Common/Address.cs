using System;

namespace JomMalaysia.Presentation.Models.Common
{
    public class Address
    {

        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public Location Location { get; set; }

        public override string ToString()
        {
            var formatted = String.Format("{0} {1} \n{2} {3} {4} {5}",
                Add1,
                (!string.IsNullOrEmpty(Add2) ? Add2 : ""),
                PostalCode, City, State, Country
                );
            return formatted;
        }
    }
}