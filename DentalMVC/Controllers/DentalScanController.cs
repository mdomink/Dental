using DentalBusiness.Interfaces;
using DentalDomain.Models;
using DentalWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DentalWeb.Controllers
{
    [Authorize]
    public class DentalScanController : Controller
    {
        private readonly IDentalBusinessRepository _dentalBusiness;
        private readonly UserManager<UserModel> _userManager;

        public DentalScanController(UserManager<UserModel> userManager, IDentalBusinessRepository dentalRepository)
        {
            _dentalBusiness = dentalRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int dentalId)
        {
            UserModel user;
            DentalScanModel dentalScan;

            if (User.IsInRole("Admin"))
            {
                dentalScan = await _dentalBusiness.GetDentalScanById(dentalId);
            }
            else
            {
                user = await _dentalBusiness.GetUserById(User.GetUserID());

                if (user == null)
                {
                    return RedirectToAction("Error", "Home", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                dentalScan = await _dentalBusiness.GetDentalScanById(dentalId, user.OutUserId);
            }            

            if (dentalScan == null)
            {
                return RedirectToAction("Error", "Home", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });                
            }

            DentalScanViewModel dentalScanVM = new DentalScanViewModel
            {
                Id = dentalScan.Id,
                LastUpdate = dentalScan.ModifiedDate,
                CreationDate = dentalScan.CreationDate,
                Status = dentalScan.Status,
                PatientId = dentalScan.PatientId
            };

            return View(dentalScanVM);
        }

        [HttpPost]
        public IActionResult Create(PatientViewModel patientVM)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Index", "Patient", new { patientVM.Id });
            }

            if (!User.IsInRole("Admin"))
            {
                var user = _userManager.GetUserAsync(User).Result;

                if (patientVM.OutUserId != user.OutUserId)
                {
                    TempData["Error"] = "Failed to crete new scan";

                    return RedirectToAction("Index", "Patient", new { patientVM.Id });
                }
            }

            DateTime dtNow = DateTime.Now;

            DentalScanModel dentalScan = new DentalScanModel
            {
                CreationDate = dtNow,
                Status = '0',
                PatientId = patientVM.Id
            };

            if (!_dentalBusiness.Add(dentalScan, (int)patientVM.OutUserId))
            {
                TempData["Error"] = "Failed to crete new scan";

                return RedirectToAction("Index", new { patientVM.Id });
            }

            return RedirectToAction("Index", "DentalScan", new { dentalId = dentalScan.Id, patientId = dentalScan.PatientId });
        }
    }
}
