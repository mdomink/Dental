using Dental.Data;
using Dental.Interfaces;
using Dental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Dental.Reposiotry
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDentalScanRepository _dentalScanRepository;

        public PatientRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IDentalScanRepository dentalScanRepository)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _dentalScanRepository = dentalScanRepository;
        }

        public bool Add(PatientModel patient, bool bSave = true)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserID();
            patient.UserId = currentUserId;
            _context.Add(patient);   

            if(bSave)
                return Save();

            return true;
        }        

        public bool AddDentalScan(DentalScanModel dentalScan)
        {
            var patient = GetByIdAsync(dentalScan.PatientId).Result;

            if (patient == null)
            {
                return false;
            }

            if(!_dentalScanRepository.Add(dentalScan, false))
            {
                return false;
            }

            patient.UpdateDate = dentalScan.CreationDate;

            return Update(patient);
        }

        public async Task<IEnumerable<PatientModel>> GetAllPatients()
        {            
            if ((bool)_httpContextAccessor.HttpContext?.User.IsInRole("Admin"))
            {
                var allPatients = _context.Patients;

                return allPatients.ToList();
                
            }

            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserID();

            if (currentUserId == null)
            {
                return new List<PatientModel>();
            }

            var userPatients = _context.Patients.Where(p => p.UserId == currentUserId);

            return userPatients.ToList();

        }

        public async Task<PatientModel> GetByIdAsync(int Id)
        {
            if ((bool)_httpContextAccessor.HttpContext?.User.IsInRole("user"))
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserID();

                return await _context.Patients.FirstOrDefaultAsync(item => item.Id == Id && item.UserId == currentUserId);
            }

            return await _context.Patients.FirstOrDefaultAsync(item => item.Id == Id);

        }

        public async Task<PatientModel> GetByIdAsyncNoTracking(int Id)
        {
            if ((bool)_httpContextAccessor.HttpContext?.User.IsInRole("user"))
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserID();

                return await _context.Patients.AsNoTracking().FirstOrDefaultAsync(item => item.Id == Id && item.UserId == currentUserId);
            }

            return await _context.Patients.AsNoTracking().FirstOrDefaultAsync(item => item.Id == Id );
        }

        public async Task<IEnumerable<DentalScanModel>> GetAllDentalScans(int Id)
        {
            return await _dentalScanRepository.GetAllDentalScan(Id);
            var userPatient = await GetByIdAsyncNoTracking(Id);

            if (userPatient == null)
            {
                return new List<DentalScanModel>();
            }

            return _context.DentalScans.Where(ds => ds.PatientId == Id).ToList();
        }

        public async Task<DentalScanModel> GetDentalScanById(int Id, int dentalId)
        {
            var userPatient = await GetByIdAsyncNoTracking(Id);

            if (userPatient == null)
            {
                return null;
            }

            return await _context.DentalScans.FirstOrDefaultAsync(ds => ds.Id == dentalId);
        }       

        public bool Update(PatientModel patient, bool bSave = true)
        {
            if(GetByIdAsyncNoTracking(patient.Id).Result == null)
            {
                return false;
            }

           _context.Update(patient);

            if (bSave)
                return Save();

            return true;
        }

        public bool Delete(PatientModel patient, bool bSave = true)
        {
            if (GetByIdAsyncNoTracking(patient.Id) == null)
            {
                return false;
            }

            _context.Remove(patient);

            if (bSave)
                return Save();

            return true;
        }

        private bool Save()
        {
            int ret = _context.SaveChanges();
            return ret > 0;
        }
    }
}
