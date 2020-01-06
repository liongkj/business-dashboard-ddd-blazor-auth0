﻿using System.ComponentModel.DataAnnotations;

namespace JomMalaysia.Presentation.Models.Listings
{
    public class OfficialContact
    {
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^((60(\s|-)?)|0)(((?!0)(?!2)(?!80)(?!81)\d{1,2})[1]?)(\s|-)?\d{2,4}(\s|-)?\d{4}$",
            ErrorMessage = "Invalid Phone Number. (Remove + sign and start with 60)")]
        [Display(Name = "Mobile Phone")]
        public string MobileNumber { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Office / Hotline")]
        public string OfficeNumber { get; set; }

        [Url(ErrorMessage = "Invalid Website Url (https://example.com)")]
        [Display(Name = "Web / Facebook / Insta", Prompt = "Invalid Website Url (https://example.com)")]
        public string Website { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^((60(\s|-)?)|0)(((?!0)(?!2)(?!80)(?!81)\d{1,2})[1]?)(\s|-)?\d{2,4}(\s|-)?\d{4}$",
            ErrorMessage = "Invalid Phone Number. (Remove + sign and start with 60)")]
        public string Fax { get; set; }

        [EmailAddress] public string Email { get; set; }
    }
}