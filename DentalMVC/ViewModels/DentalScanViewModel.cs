using System.ComponentModel.DataAnnotations;

namespace Dental.ViewModels
{
    public class DentalScanViewModel
    {        
        public int Id { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ValidTo { get; set; }

        public char? Status { get; set; }       

        public int? PatientId { get; set; }
      
    }
}
