using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalDomain.Models
{
    public class PatientModel
    {
        [Key]
        public int Id { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        [DisplayName("Last update")]
        [DisplayFormat(DataFormatString = "{d/M/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdate { get; set; } = DateTime.Now;

        [ForeignKey("UserModel")]
        public string? UserId { get; set; }

        public UserModel? User { get; set; }

        public virtual ICollection<DentalScanModel> DentalScans { get; set; }

    }
}
