using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JomMalaysia.Presentation.ViewModels.Listings
{
    public class OperatingHourViewModel
    {
        public DayOfWeek Day { get; set; }
        [Required]
        [DisplayName("Opening hour")]
        public string StartTime { get; set; }
        [Required]
        [DisplayName("Closing hour")]
        public string CloseTime { get; set; }
        public bool Enabled { get; set; }
    }
}
