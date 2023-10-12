using Dental.Data;
using Dental.Interfaces;
using Dental.Models;
using Dental.Reposiotry;
using Dental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dental.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;

        public PatientController(IUserRepository userRpository, IPatientRepository patientRepository) 
        {
            _patientRepository = patientRepository;
            _userRepository = userRpository;
        }

        public async Task<IActionResult> Index(int id)
        {
            PatientModel patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                return RedirectToAction("Error", "Home", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            IEnumerable<DentalScanModel> dentalScans = await _patientRepository.GetAllDentalScans(id);

            bool addNewDentalScan = _userRepository.EnableAddingNewDentalScan();

            PatientViewModel patientVM = new PatientViewModel { Id = patient.Id, 
                                                                Name1 = patient.Name1, 
                                                                Name2 = patient.Name2, 
                                                                UserId = patient.UserId, 
                                                                DentalScans = dentalScans, 
                                                                LastUpdate = DateTime.Now - patient.UpdateDate,
                                                                DisableNewDentalScans = !addNewDentalScan};

            return View(patientVM);
        }

        [Authorize]
        public async Task<IActionResult> Detail(DentalScanViewModel dentalScanVW)
        {
            var dentalScan = await _patientRepository.GetDentalScanById((int)dentalScanVW.PatientId, dentalScanVW.Id);

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

        public IActionResult Create(int? id)
        {
            var createPatient = new PatientViewModel { };
            return View(createPatient); 
        }

        [HttpPost]
        public IActionResult Create(PatientViewModel patientVM)
        {
            if (!ModelState.IsValid)
            {
                return View(patientVM);
            }

            PatientModel patientModel = new PatientModel
            {
                Name1 = patientVM.Name1,
                Name2 = patientVM.Name2,
                UpdateDate = DateTime.Now
            };

            _patientRepository.Add(patientModel);   
            
            return RedirectToAction("Index", new { patientModel.Id });
        }

        [HttpPost]
        public IActionResult CreateDentalScan(int id, PatientViewModel patientVM)
        {
            if (!ModelState.IsValid)
            {
                View("Index", patientVM.Id);
            }

            DateTime dtNow = DateTime.Now;

            DentalScanModel dentalScan = new DentalScanModel { CreationDate = dtNow, Status = '0', PatientId = id };          

            if (!_patientRepository.AddDentalScan(dentalScan))
            {
                TempData["Error"] = "Failed to crete new scan";

                return RedirectToAction("Index", new { patientVM.Id });
            }            

            return RedirectToAction("Index", "DentalScan", new { dentalId = dentalScan.Id, patientId = dentalScan.PatientId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _patientRepository.GetByIdAsyncNoTracking(id);

            if (patient == null)
            {
                return RedirectToAction("Error", "Home", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            var patientVM = new PatientViewModel
            {
                Id = id,
                Name1 = patient.Name1,
                Name2 = patient.Name2,                
            };

            return View(patientVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PatientViewModel patientVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", patientVM);
            }

            var userPatient = await _patientRepository.GetByIdAsyncNoTracking(id);

            if(userPatient != null)
            {
                var patient = new PatientModel
                {
                    Id = id,
                    Name1 = patientVM.Name1,
                    Name2 = patientVM.Name2,
                    UserId = userPatient.UserId
                };

                if (!_patientRepository.Update(patient))
                {
                    TempData["Error"] = "Failed to save changes";
                    return View("Edit", patientVM);
                }

                return RedirectToAction("Patients", "Home");
            }
            else
            {
                return View(patientVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
                return View("Error");

            return View(patient);
        }
    }
}
