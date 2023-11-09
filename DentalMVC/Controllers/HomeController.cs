using DentalBusiness.Interfaces;
using DentalBusiness.Models;
using DentalBusiness.Repository;
using Dental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dental.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDentalBusinessRepository _dentalReposiotry;

        public HomeController(ILogger<HomeController> logger, IDentalBusinessRepository dentalRepository)
        {
            _logger = logger;
            _dentalReposiotry = dentalRepository;
        }

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
            int? outUserID = null;
            if (!User.IsInRole("Admin"))
            {
                outUserID = _dentalReposiotry.GetUserById(User.GetUserID()).Result.OutUserId;
            }

            IEnumerable<PatientModel> lsPatients = await _dentalReposiotry.GetAllPatients(outUserID);

            if (!string.IsNullOrEmpty(filterPatients))
            {
                lsPatients = lsPatients.Where(p => filterPatients == null ||
                                                p.Id.ToString().Contains(filterPatients) ||
                                                p.Name1.Contains(filterPatients, StringComparison.OrdinalIgnoreCase) ||
                                                p.Name2.Contains(filterPatients, StringComparison.OrdinalIgnoreCase)).
                                                OrderBy(p => p.Id);
            }

            return View(lsPatients.Select(p => new PatientViewModel
                                            {
                                                Id = p.Id,
                                                Name1 = p.Name1,
                                                Name2 = p.Name2,
                                                OutUserId = _dentalReposiotry.GetUserById(p.UserId).Result.OutUserId,
                                                LastUpdate = p.LastUpdate,
                                                DentalScans = _dentalReposiotry.
                                                                GetAllDentalScans(p.Id, outUserID).
                                                                Result.
                                                                Select(d => new DentalScanViewModel()
                                                                                {
                                                                                    Id = d.Id,
                                                                                    PatientId = d.PatientId,
                                                                                    CreationDate = d.CreationDate,
                                                                                    LastUpdate = d.ModifiedDate,
                                                                                    Status = d.Status
                                                                                }).ToList()
                                            }));
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