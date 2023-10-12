using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental.Models
{
    public class AddressModel
    {
        [Key]
        public int Id { get; set; }

        public string Street { get; set; }

        [ForeignKey("CityModel")]
        public int CityId { get; set; }

        public CityModel City { get; set; }

        public string State { get; set; }

    }
}
