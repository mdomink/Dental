using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Configuration;
using DentalDomain.Data;
using DentalDomain.Data.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalWeb.ViewModels
{
    abstract public class UserViewModel
    {
        public int OutUserId {  get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MinLength(2), MaxLength(20)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [MinLength(2), MaxLength(40)]
        public string Surname { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is required")]
        [DefaultValue("")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required")]
        [DefaultValue("")]
        public string City { get; set; }

        [IntegerValidator(MinValue = 10000, MaxValue = 59999)]
        public int PostalCode { get; set; }

        public List<SelectListItem> SelectCities
        {
            get
            {
                return Cities.GetCity().ConvertAll(c =>
                {
                    return new SelectListItem()
                    {
                        Text = c.City.ToString(),
                        Value = c.City.ToString(),
                    };
                });
            }            
        }

        public List<SelectListItem> SelectPostalCode
        {
            get
            {
                return Cities.GetCity().ConvertAll(c =>
                {
                    return new SelectListItem()
                    {
                        Text = c.PostalCode.ToString(),
                        Value = c.PostalCode.ToString()                        
                    };
                });
            }
        }
    }
}
