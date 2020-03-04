using System.ComponentModel;

namespace JomMalaysia.Presentation.Models.Merchants
{
    public class MerchantSummary
    {
        [DisplayName("SSM (New)")]
        public string SsmId { get; set; }
        [DisplayName("Old")]
        public string OldSsmId { get; set; }
        [DisplayName("Name")]
        public string RegistrationName { get; set; }
        public string MerchantId { get; set; }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(OldSsmId) ? $"{SsmId}({OldSsmId})" : $"{SsmId}";
        }
    }
}