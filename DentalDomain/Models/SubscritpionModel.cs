using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalDomain.Models
{
    public class SubscritpionModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = "Basic";

        public int MaxDentalScans { get; set; }

    }
}
