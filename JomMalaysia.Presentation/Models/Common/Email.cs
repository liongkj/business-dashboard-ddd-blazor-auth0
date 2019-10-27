namespace JomMalaysia.Presentation.Models.Common
{
    public class Email
    {
        public string User { get; set; }
        public string Domain { get; set; }


        public override string ToString()
        {
            return User + "@" + Domain;
        }
    }
}