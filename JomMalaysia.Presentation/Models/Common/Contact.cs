using System;

namespace JomMalaysia.Presentation.Models.Common
{
    public class Contact

    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsPrimary { get; set; }

        public override string ToString()
        {
            var formatted = String.Format("{0} {1} ",
                 Name, Phone
            );
            return formatted;
        }
    }
}