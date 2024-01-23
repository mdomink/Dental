using DentalDomain.Data;
using DentalBusiness.Interfaces;
using DentalDomain.Models;
using DentalWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace DentalWeb.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IDentalBL _dentalRepository;

        public UserController(IDentalBL dentalRepository)
        {
            _dentalRepository = dentalRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index(int? id)
        {
            UserModel user;            

            if (User.IsInRole("Admin") && id != null)
            {
                user = await _dentalRepository.GetUserByOutId((int)id);
            }
            else
            {
                user = await _dentalRepository.GetUserById(User.GetUserID());
            }           

            DetailUserViewModel userViewModel = new DetailUserViewModel
            {
                OutUserId = user.OutUserId,
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
        public async Task<IActionResult> Index(DetailUserViewModel userVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userVM);
            }

            UserModel user = await _dentalRepository.GetUserByOutId(userVM.OutUserId);

            if(user != null)
            {
                user.Name1 = userVM.Name;
                user.Name2 = userVM.Surname;
                user.Street = userVM.Street;
                user.City = userVM.City;
                user.PostalCode = userVM.PostalCode;

                var bRes = await _dentalRepository.Update(user);

                if (bRes)
                {
                    return RedirectToAction("Index", new { user.OutUserId });
                }
            }            

            TempData["Error"] = "Failed to update user data";

            return View(userVM);
        }
    }
}
