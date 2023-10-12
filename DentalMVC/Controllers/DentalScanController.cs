using Dental.Interfaces;
using Dental.Models;
using Dental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dental.Controllers
{
    [Authorize]
    public class DentalScanController : Controller
    {
        private readonly IDentalScanRepository _dentalScanRepository;

        public DentalScanController(IDentalScanRepository dentalScanRepository)
        {
            _dentalScanRepository = dentalScanRepository;
        }

        public async Task<IActionResult> Index(int dentalId, int? patientId = null)
        {
            var dentalScan =  await _dentalScanRepository.GetDentalScanById(dentalId, patientId);

            if(dentalScan == null)
            {
                return RedirectToAction("Error", "Home", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });                
            }

            DentalScanViewModel dentalScanVM = new DentalScanViewModel
            {
                Id = dentalScan.Id,
                ValidTo = dentalScan.ValidTo,
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

            var dentalScan = new DentalScanModel { CreationDate = DateTime.Now, Status = '0', PatientId = id };

            if (_dentalScanRepository.Add(dentalScan))
            {
                //RedirectToAction("Detail", patientVM, dentalScan);
            }

            return RedirectToAction("Detail", "Patient", patientVM);
        }
    }
}
