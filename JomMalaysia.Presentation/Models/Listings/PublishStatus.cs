using System;
using static JomMalaysia.Framework.Constant.EnumConstant;

namespace JomMalaysia.Presentation.Models.Listings
{
    public class PublishStatus
    {
        public PublishStatusEnum Status { get; set; }
        public DateTime ValidityStart { get; set; }
        public DateTime ValidityEnd { get; set; }
    }
}