using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Dental.Models
{
    public class DentalScanModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Valid to")]
        public DateTime? ValidTo { get; set; }

        public char Status { get; set; }

        [ForeignKey("PatientModel")]
        public int PatientId { get; set; }

        public PatientModel? Patient { get; set; }

    }
}
