using System.ComponentModel.DataAnnotations;

namespace Dental.ViewModels
{
    public class DentalScanViewModel
    {        
        public int Id { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Display(Name ="Last update")]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdate { get; set; }

        public char? Status { get; set; }       

        public int? PatientId { get; set; }
      
    }
}
