using Dental.Interfaces;
using Dental.Models;
using Dental.Reposiotry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dental.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, IPatientRepository patientRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _patientRepository = patientRepository;
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

        public async Task<IActionResult> Patients()
        {
            IEnumerable<PatientModel> lsPatients = await _patientRepository.GetAllPatients();
            return View(lsPatients);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Users()
        {
            IEnumerable<UserModel> users = await _userRepository.GetAllUsers();
            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}