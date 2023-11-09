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
    public class PatientController : Controller
    {
        private readonly IDentalBusinessRepository _dentalBusiness;

        public PatientController(IDentalBusinessRepository dentalBusiness) 
        {
            _dentalBusiness = dentalBusiness;
        }

        public async Task<IActionResult> Index(int id)
        {
            PatientModel patient;
            UserModel user;

            if (!User.IsInRole("Admin"))
            {
                user = await _dentalBusiness.GetUserById(User.GetUserID());

                patient = await _dentalBusiness.GetPatientByIdAsyncNoTracking(id, user?.OutUserId);

                if (patient == null)
                {

                }
            }
            else
            {
                patient = await _dentalBusiness.GetPatientByIdAsyncNoTracking(id);
            }

            if (patient == null)
            {
                return RedirectToAction("Error", "Home", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            user = await _dentalBusiness.GetUserById(patient.UserId);

            int outUserId = user.OutUserId;

            bool addNewDentalScan = await _dentalBusiness.EnableAddingNewDentalScan(outUserId);

            IEnumerable<DentalScanModel> dentalScans = await _dentalBusiness.GetAllDentalScans(id, outUserId);

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

        public async Task<IActionResult> Detail(DentalScanViewModel dentalScanVW)
        {
            var dentalScan = await _dentalBusiness.GetDentalScanById((int)dentalScanVW.PatientId, dentalScanVW.Id);

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

        public IActionResult Create()
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

            string userId;
            if (!User.IsInRole("Admin"))
            {
                var user = _dentalBusiness.GetUserById(User.GetUserID());

                userId = user.Result.Id;
            }
            else if(patientVM.OutUserId != null)
            {
                var user = _dentalBusiness.GetUserByOutId((int)patientVM.OutUserId);

                if (user == null)
                {
                    TempData["Error"] = "No user with ID: " + patientVM.OutUserId.ToString();

                    return RedirectToAction("Create");
                }
                userId = user.Result.Id;
            }            
            else
            {
                TempData["Error"] = "Failed to crete new patient";
                return RedirectToAction("Create");
            }

            PatientModel patientModel = new PatientModel
            {
                Name1 = patientVM.Name1,
                Name2 = patientVM.Name2,
                LastUpdate = DateTime.Now,
                UserId = userId
            };

            if (!_dentalBusiness.Add(patientModel))
            {
                TempData["Error"] = "Failed to crete new patient";

                return RedirectToAction("Create");
            }

            return RedirectToAction("Index", new { patientModel.Id });
        }

        [HttpPost]
        public IActionResult CreateDentalScan(PatientViewModel patientVM)
        {
            if (!ModelState.IsValid)
            {
                View("Index", patientVM.Id);
            }

            if (!User.IsInRole("Admin"))
            {
                var user = _dentalBusiness.GetUserById(User.GetUserID()).Result;

                if(patientVM.OutUserId != user.OutUserId)
                {
                    TempData["Error"] = "Failed to crete new scan";

                    return RedirectToAction("Index", new { patientVM.Id });
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

                if (!_dentalBusiness.Update(patient))
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
