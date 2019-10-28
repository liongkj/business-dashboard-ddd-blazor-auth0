using System;

namespace JomMalaysia.Presentation.Models.Listings
{
    public class PublishStatus
    {

        public Status Status { get; set; }
        public DateTime ValidityStart { get; set; }
        public DateTime ValidityEnd { get; set; }

    }

    public class Status
    {
        public string Name { get; set; }
    }
}