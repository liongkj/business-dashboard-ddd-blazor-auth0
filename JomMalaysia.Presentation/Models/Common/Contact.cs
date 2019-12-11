using System;

namespace JomMalaysia.Presentation.Models.Common
{
    public class Contact

    {
        public Name Name { get; set; }
        public string Email { get; set; }
        public Phone Phone { get; set; }
        public bool IsPrimary { get; set; }

        public override string ToString()
        {
            var formatted = String.Format("{0} {1} {2} ",
                (IsPrimary ? '*' : ' '), Name.ToString(), Phone.ToString()
            );
            return formatted;
        }
    }
}