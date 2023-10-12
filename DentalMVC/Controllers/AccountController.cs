﻿using Dental.Data;
using Dental.Interfaces;
using Dental.Models;
using Dental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dental.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        

        public AccountController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                //Password is incorrect
                TempData["Error"] = "Wrong credential. Please try again.";
                return View(loginViewModel);
            }
            //User not found 
            TempData["Error"] = "User not found";
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Register()
        {
            var register = new RegisterViewModel();
            return View(register);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);

            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }

            var newUser = new UserModel
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,
                Name1 = registerViewModel.Name,
                Name2 = registerViewModel.Surname,
                Street = registerViewModel.Street,
                City = registerViewModel.City,
                PostalCode = registerViewModel.PostalCode
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (!newUserResponse.Succeeded)
            {
                TempData["Error"] = newUserResponse;
                return View(registerViewModel);
            }

            await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return RedirectToAction("Index", "Home");
        }      
    }
}