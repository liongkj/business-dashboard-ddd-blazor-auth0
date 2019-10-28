namespace JomMalaysia.Presentation.Models.Common
{

    public class Name
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return FirstName.ToUpperInvariant() + LastName.ToUpperInvariant();
        }
    }
}