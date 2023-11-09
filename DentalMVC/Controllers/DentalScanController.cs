using DentalBusiness.Interfaces;
using DentalBusiness.Models;
using Dental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dental.Controllers
{
    [Authorize]
    public class DentalScanController : Controller
    {
        private readonly IDentalBusinessRepository _dentalRepository;

        public DentalScanController(IDentalBusinessRepository dentalRepository)
        {
            _dentalRepository = dentalRepository;
        }

        public async Task<IActionResult> Index(int dentalId, int? patientId = null)
        {
            var dentalScan =  await _dentalRepository.GetDentalScanById(dentalId, patientId);

            if(dentalScan == null)
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
        public IActionResult Create(int id, PatientViewModel patientVM)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Detail", "Patient", patientVM);
            }

            var dentalScan = new DentalScanModel { CreationDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = '0', PatientId = id };

            if (_dentalRepository.Add(dentalScan,(int)patientVM.OutUserId))
            {
                //RedirectToAction("Detail", patientVM, dentalScan);
            }

            return RedirectToAction("Detail", "Patient", patientVM);
        }
    }
}
