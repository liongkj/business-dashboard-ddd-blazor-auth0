using System.ComponentModel;
using System.Text;

namespace JomMalaysia.Presentation.Models.Listings
{
    public class ListingDescription
    {
        [DisplayName("English")] public string En { get; set; }

        [DisplayName("Chinese")] public string Zh { get; set; }

        [DisplayName("Malay")] public string Ms { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (En !="") sb.Append(En).Append("\n");
            if (Zh!="") sb.Append(Zh).Append("\n");
            
            if (Ms!="") sb.Append(Ms).Append("\n");
            
            return sb.ToString();
        }
    }
}