using System.ComponentModel.DataAnnotations;
using DentalBusiness.Models;

namespace Dental.ViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        [Display(Name = "Last update")]
        [DisplayFormat(DataFormatString = "{d/M/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdate  { get; set; }

        public int? OutUserId { get; set; }

        public bool DisableNewDentalScans { get; set; }

        public IEnumerable<DentalScanViewModel>? DentalScans { get; set; }

    }
}
