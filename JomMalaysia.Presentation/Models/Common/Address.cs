using System;
using System.Collections.Generic;
using System.ComponentModel;
using JomMalaysia.Framework.Helper;

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
        public Coordinates Coordinates { get; set; }

        public override string ToString()
        {
            var formatted = String.Format("{0} {1} \n{2} {3} {4} {5}",
                Add1, (!string.IsNullOrEmpty(Add2) ? Add2 : ""),
                PostalCode, City, EnumHelper.GetDescription(State), EnumHelper.GetDescription(Country)
            );
            return formatted;
        }
    }

    public class Location
    {
        public List<Coordinates> Coordinates { get; set; }
    }


    public enum CountryEnum
    {
        [Description("Malaysia")] MY
    }

    public enum StateEnum
    {
        [Description("Negeri Sembilan")] NSN,
        [Description("Johor")] JHR,
        [Description("Kedah")] KDH,
        [Description("Kelantan")] KTN,
        [Description("Malacca")] MLK,
        [Description("Pahang")] PHG,
        [Description("Penang")] PNG,
        [Description("Perak")] PRK,
        [Description("Perlis")] PLS,
        [Description("Sabah")] SBH,
        [Description("Selangor")] SGR,
        [Description("Terengganu")] TRG,
        [Description("Kuala Lumpur")] KUL,
        [Description("Labuan")] LBN,
        [Description("Putrajaya")] PJY,


    }
}