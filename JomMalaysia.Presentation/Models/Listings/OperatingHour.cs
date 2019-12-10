using System;

namespace JomMalaysia.Presentation.Models.Listings
{
    public class OperatingHour
    {
        public DayOfWeek DayOfWeek { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
    }
}