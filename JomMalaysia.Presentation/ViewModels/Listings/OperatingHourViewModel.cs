using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JomMalaysia.Presentation.ViewModels.Listings
{
    public class OperatingHourViewModel
    {
        public DayOfWeek Day { get; set; }

        [Required]
        [DisplayName("Opening hour")]
        public string OpenTime { get; set; }

        [Required]
        [DisplayName("Closing hour")]
        public string CloseTime { get; set; }

        public bool Enabled { get; set; }
    }
}