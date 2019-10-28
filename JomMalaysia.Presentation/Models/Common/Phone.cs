namespace JomMalaysia.Presentation.Models.Common
{
    public class Phone
    {
        public string Number { get; set; }

        public override string ToString()
        {
            return Number.Substring(3, Number.Length - 3);
        }
    }
}