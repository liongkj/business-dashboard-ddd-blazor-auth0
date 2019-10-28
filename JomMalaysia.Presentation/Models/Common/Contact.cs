using System;

namespace JomMalaysia.Presentation.Models.Common
{
    public class Contact

    {
        public Name Name { get; set; }
        public Email Email { get; set; }
        public Phone Phone { get; set; }
        public bool IsPrimary { get; set; }

        public override string ToString()
        {
            var formatted = String.Format("{0} {2} \n{3}",
                (IsPrimary ? '*' : ' '), Name, Phone.ToString(), Email
            );
            return formatted;
        }
    }

}