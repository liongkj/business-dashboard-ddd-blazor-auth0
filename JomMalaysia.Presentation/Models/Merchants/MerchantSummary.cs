namespace JomMalaysia.Presentation.Models.Merchants
{
    public class MerchantSummary
    {
        public string SsmId { get; set; }
        public string OldSsmId { get; set; }
        public string RegistrationName { get; set; }
        public string MerchantId { get; set; }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(OldSsmId) ? $"{SsmId}({OldSsmId})" : $"{SsmId}";
        }
    }
}