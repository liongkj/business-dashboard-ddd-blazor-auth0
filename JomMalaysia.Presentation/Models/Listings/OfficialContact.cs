using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JomMalaysia.Presentation.Models.Listings
{
    public class OfficialContact
    {
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^((\+?60(\s|-)?)|0)(((?!0)(?!2)(?!80)(?!81)\d{1,2})[1]?)(\s|-)?\d{2,4}(\s|-)?\d{4}$",
            ErrorMessage = "Invalid Phone Number.")]
        [Display(Name = "Mobile Phone")]
        public string MobileNumber { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Office / Hotline")]
        public string OfficeNumber { get; set; }

        [RegularExpression(@"^[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$")]
        [Display(Name = "Web / Facebook / Insta", Prompt = "Invalid Website Url (https://example.com)")]
        public string Website { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^((\+?60(\s|-)?)|0)(((?!0)(?!2)(?!80)(?!81)\d{1,2})[1]?)(\s|-)?\d{2,4}(\s|-)?\d{4}$",
            ErrorMessage = "Invalid Phone Number.")]
        public string Fax { get; set; }

        [EmailAddress] public string Email { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(MobileNumber)) sb.Append(MobileNumber).Append("; ");
            if (!string.IsNullOrEmpty(OfficeNumber)) sb.Append(OfficeNumber).Append("; ");
            if (!string.IsNullOrEmpty(Email)) sb.Append(Email).Append("; ");
            if (!string.IsNullOrEmpty(Website)) sb.Append(Website).Append("; ");
            if (!string.IsNullOrEmpty(Fax)) sb.Append(Fax).Append("; ");
            
            return sb.ToString();
        }
    }
}