using System.Collections.Generic;
using System.ComponentModel;

namespace JomMalaysia.Presentation.Models.Common
{
    public class Address
    {
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string City { get; set; }
        public StateEnum State { get; set; }
        public string PostalCode { get; set; }
        public CountryEnum Country { get; set; }
        public Location Location { get; set; }
    }
    
    public class Location{
        public List<Coordinates> Coordinates { get; set; }
    }
    
    public enum CountryEnum
    {
        [Description("Malaysia")]
        MY
    }

    public enum StateEnum
    {
        [Description("Negeri Sembilan")]
        NSN
    }
}