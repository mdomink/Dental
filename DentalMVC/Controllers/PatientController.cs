using DentalBusiness.Interfaces;
using DentalDomain.Models;
using DentalBusiness.Logic;
using DentalWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DentalWeb.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private readonly IDentalBL _dentalBusiness;
        private readonly UserManager<UserModel> _userManager;

        public PatientController(UserManager<UserModel> userManager, IDentalBL dentalBusiness) 
        {
            _dentalBusiness = dentalBusiness;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            PatientModel patient;
            UserModel user;

            try
            {
                patient = await _dentalBusiness.GetPatientByIdAsyncNoTracking(id);

                user = await _dentalBusiness.GetUserById(patient.UserId);

                if (patient == null)
                {
                    return RedirectToAction("Error", "Home", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                int outUserId = user.OutUserId;

                bool addNewDentalScan = await _dentalBusiness.EnableAddingNewDentalScan(outUserId);

                IEnumerable<DentalScanModel> dentalScans = await _dentalBusiness.GetAllDentalScansAsync(id);

                PatientViewModel patientVM = new PatientViewModel
                {
                    Id = patient.Id,
                    Name1 = patient.Name1,
                    Name2 = patient.Name2,
                    OutUserId = outUserId,
                    DentalScans = dentalScans.Select(d => new DentalScanViewModel()
                    {
                        Id = d.Id,
                        PatientId = d.PatientId,
                        CreationDate = d.CreationDate,
                        LastUpdate = d.ModifiedDate,
                        Status = d.Status
                    }).ToList(),
                    LastUpdate = patient.LastUpdate,
                    DisableNewDentalScans = !addNewDentalScan
                };

                return View(patientVM);
            }
            catch
            {
                return RedirectToAction("Error", "Home", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }                      
        }

        public async Task<IActionResult> Detail(DentalScanViewModel dentalScanVW)
        {
            try
            {
                var dentalScan = await _dentalBusiness.GetDentalScanById(dentalScanVW.Id);

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
            catch
            {
                return RedirectToAction("Error", "Home", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        }

        public IActionResult Create()
        {
            var createPatient = new PatientViewModel();

            return View(createPatient); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientViewModel patientVM)
        {
            if (!ModelState.IsValid)
            {
                return View(patientVM);
            }

            UserModel user;

            try
            {
                if(patientVM.OutUserId != null)
                {
                    user = await _dentalBusiness.GetUserByOutId((int)patientVM.OutUserId);                    
                }                
                else
                {
                    user = await _dentalBusiness.GetUserById(User.GetUserID());
                }

                string userId = user.Id;

                PatientModel patientModel = new PatientModel
                {
                    Name1 = patientVM.Name1,
                    Name2 = patientVM.Name2,
                    LastUpdate = DateTime.Now,
                    UserId = userId
                };

                var bRes = await _dentalBusiness.Add(patientModel);

                if (!bRes)
                {
                    TempData["Error"] = "Failed to crete new patient";

                    return RedirectToAction("Create");
                }

                return RedirectToAction("Index", new { patientModel.Id });
            }
            catch
            {
                TempData["Error"] = "Failed to crete new patient";

                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDentalScan(PatientViewModel patientVM)
        {
            if (!ModelState.IsValid)
            {
                View("Index", patientVM.Id);
            }

            DateTime dtNow = DateTime.Now;

            DentalScanModel dentalScan = new DentalScanModel 
            { 
                CreationDate = dtNow, 
                Status = '0', 
                PatientId = patientVM.Id
            };

            var bRes = await _dentalBusiness.Add(dentalScan, (int)patientVM.OutUserId);

            if (!bRes)
            {
                TempData["Error"] = "Failed to crete new scan";

                return RedirectToAction("Index", new { patientVM.Id });
            }            

            return RedirectToAction("Index", "DentalScan", new { dentalId = dentalScan.Id, patientId = dentalScan.PatientId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _dentalBusiness.GetPatientByIdAsyncNoTracking(id);

            if (patient == null)
            {
                return RedirectToAction("Error", "Home", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            var patientVM = new PatientViewModel
            {
                Id = patient.Id,
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

            var userPatient = await _dentalBusiness.GetPatientByIdAsyncNoTracking(id);

            if(userPatient != null)
            {
                if (!User.IsInRole("Admin"))
                {
                    if (userPatient.UserId != User.GetUserID())
                    {
                        TempData["Error"] = "Failed to save changes";
                        return View("Edit", patientVM);
                    }
                }  
                
                var patient = new PatientModel
                {
                    Id = id,
                    Name1 = patientVM.Name1,
                    Name2 = patientVM.Name2,
                    UserId = userPatient.UserId,
                    LastUpdate = DateTime.Now
                };

                var bRes = await _dentalBusiness.Update(patient);

                if (!bRes)
                {
                    TempData["Error"] = "Failed to save changes";
                    return View("Edit", patientVM);
                }

                return RedirectToAction("Patients", "Home");
            }
            else
            {
                TempData["Error"] = "Failed to save changes";
                return View(patientVM);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _dentalBusiness.GetPatientByIdAsync(id);

            if (patient == null)
                return View("Error");

            return View(patient);
        }
    }
}
