using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Dental.ViewModels
{
    public class RegisterViewModel : UserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Password do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
