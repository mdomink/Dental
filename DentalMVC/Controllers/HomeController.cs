using DentalBusiness.Interfaces;
using DentalDomain.Models;
using DentalBusiness.Repository;
using DentalWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DentalWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IDentalBusinessRepository _dentalReposiotry;

        public HomeController(UserManager<UserModel> userManager, ILogger<HomeController> logger, IDentalBusinessRepository dentalRepository)
        {
            _logger = logger;
            _dentalReposiotry = dentalRepository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Login", "Account");            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Patients(string? filterPatients)
        {
            UserModel user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            IEnumerable<PatientModel> lsPatients;

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                lsPatients = await _dentalReposiotry.GetAllPatients();
            }
            else
            {
                lsPatients = await _dentalReposiotry.GetAllPatients(user.OutUserId);
            }

            if (!string.IsNullOrEmpty(filterPatients))
            {
                lsPatients = lsPatients.Where(p => filterPatients == null ||
                                                p.Id.ToString().Contains(filterPatients) ||
                                                p.Name1.Contains(filterPatients, StringComparison.OrdinalIgnoreCase) ||
                                                p.Name2.Contains(filterPatients, StringComparison.OrdinalIgnoreCase)).
                                                OrderBy(p => p.Id);
            }

            IEnumerable<PatientViewModel> lsPatientsVM = lsPatients.Select(p => new PatientViewModel()
            {
                Id = p.Id,
                Name1 = p.Name1,
                Name2 = p.Name2,
                LastUpdate = p.LastUpdate
            });

            return View(lsPatientsVM);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            IEnumerable<UserModel> users = await _dentalReposiotry.GetAllUsers();

            return View(users.Select(u => new DetailUserViewModel()
                                    {
                                       Name = u.Name1,
                                       Surname = u.Name2,
                                       EmailAddress = u.Email,
                                       Street = u.Street,
                                       City = u.City,
                                       PostalCode = u.PostalCode,
                                       OutUserId = u.OutUserId                                        
                                    }));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}