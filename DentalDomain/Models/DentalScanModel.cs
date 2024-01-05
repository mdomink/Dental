using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalDomain.Models
{
    public class DentalScanModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Date created")]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Last modified")]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public char Status { get; set; }

        [ForeignKey("PatientModel")]
        public int PatientId { get; set; }

        public PatientModel Patient { get; set; }

    }
}
