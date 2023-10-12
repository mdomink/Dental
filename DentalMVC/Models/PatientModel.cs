using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental.Models
{
    public class PatientModel
    {
        [Key]
        public int Id { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        [DefaultValue("2023-10-01")]
        public DateTime UpdateDate { get; set; }

        [ForeignKey("UserModel")]
        public string? UserId { get; set; }

        public UserModel? User { get; set; }

    }
}
