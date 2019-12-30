using System.ComponentModel;

namespace JomMalaysia.Presentation.Models.Listings
{
    public class ListingDescription
    {
        [DisplayName("English")] public string En { get; set; }

        [DisplayName("Chinese")] public string Zh { get; set; }

        [DisplayName("Malay")] public string Ms { get; set; }
    }
}