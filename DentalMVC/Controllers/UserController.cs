using Dental.Data;
using Dental.Interfaces;
using Dental.Models;
using Dental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace Dental.Controllers
{    
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index(string id)
        {
            UserModel user = await _userRepository.GetUserById(id);

            DetailUserViewModel userViewModel = new DetailUserViewModel
            {
                Id = user.Id,
                EmailAddress = user.Email,
                Name = user.Name1,
                Surname = user.Name2,
                Street = user.Street,
                City = user.City,
                PostalCode = user.PostalCode
            };

            return View(userViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(string Id, DetailUserViewModel userVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userVM);
            }

            UserModel user = await _userRepository.GetUserById(Id);

            if(user != null)
            {
                user.Name1 = userVM.Name;
                user.Name2 = userVM.Surname;
                user.Street = userVM.Street;
                user.City = userVM.City;
                user.PostalCode = userVM.PostalCode;

                if (_userRepository.Update(user))
                {
                    return RedirectToAction("Index", new { user.Id });
                }
            }            

            TempData["Error"] = "Failed to update user data";

            return View(userVM);
        }
    }
}
