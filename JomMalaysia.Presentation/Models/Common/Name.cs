using JomMalaysia.Framework.Helper;

namespace JomMalaysia.Presentation.Models.Common
{
    public class Name
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return StringHelper.CapitalizeOrConvertNullToEmptyString(FirstName) + " " +
                   StringHelper.CapitalizeOrConvertNullToEmptyString(LastName);
        }
    }
}