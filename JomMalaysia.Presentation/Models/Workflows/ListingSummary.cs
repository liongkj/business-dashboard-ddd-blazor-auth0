using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.Models.Merchants;
using static JomMalaysia.Framework.Constant.EnumConstant;

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