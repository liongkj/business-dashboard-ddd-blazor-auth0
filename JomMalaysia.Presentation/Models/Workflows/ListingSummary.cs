using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.Models.Merchants;

namespace JomMalaysia.Presentation.Models.Workflows
{
    public class ListingSummary
    {
        public string ListingId { get; set; }
        public MerchantSummary Merchant { get; set; }
        public string ListingName { get; set; }
        public CategoryType? CategoryType { get; set; }
        public string Status { get; set; }
    }
}