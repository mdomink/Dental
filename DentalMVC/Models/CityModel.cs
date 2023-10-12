﻿using System.ComponentModel.DataAnnotations;

namespace Dental.Models
{
    public class CityModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int PostalCode { get; set; }

    }
}
