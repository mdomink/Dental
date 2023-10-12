using Dental.Data;
using Dental.Interfaces;
using Dental.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Dental.Reposiotry
{
    public class DentalScanRepository : IDentalScanRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DentalScanRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Add(DentalScanModel dentalScanModel, bool bSave = true)
        {
            _context.Add(dentalScanModel);

            if (bSave)
                return Save();

            return true;
        }

        public async Task<IEnumerable<DentalScanModel>> GetAllDentalScan(int? patientId)
        {
            if (_httpContextAccessor.HttpContext?.User.IsInRole("user") ?? false)
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserID();

                var patient = await _context.Patients.AsNoTracking().FirstOrDefaultAsync(p => p.Id == patientId && p.UserId == currentUserId);

                if (patient == null)
                {
                    return null;
                }

                return await _context.DentalScans.Where(ds => ds.PatientId == patientId).ToListAsync();
            }

            return await _context.DentalScans.Where(ds => (patientId == null ||ds.PatientId == patientId)).ToListAsync();
        }

        public async Task<DentalScanModel> GetDentalScanById(int id, int? patientId)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserID(); 

            if (_httpContextAccessor.HttpContext?.User.IsInRole("user") ?? false)
            {              

                if(_context.Patients.FirstOrDefault(p => p.Id == patientId && p.UserId == currentUserId) == null)
                {
                    return null;
                }

                return await _context.DentalScans.FirstOrDefaultAsync(ds => ds.Id == id && ds.PatientId == patientId);
            }

            return await _context.DentalScans.FirstOrDefaultAsync(ds => ds.Id == id && (patientId == null || ds.PatientId == patientId));
        }

        public async Task<DentalScanModel> GetDentalScanByIdNoTracking(int id, int? patientId)
        {
            if (_httpContextAccessor.HttpContext?.User.IsInRole("admin") ?? false)
            {
                return await _context.DentalScans.AsNoTracking().FirstOrDefaultAsync(ds => ds.Id == id);
            }

            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserID();

            if (_context.Patients.FirstOrDefault(p => p.Id == patientId && p.UserId == currentUserId) == null)
            {
                return null;
            }

            return await _context.DentalScans.AsNoTracking().FirstOrDefaultAsync(ds => ds.Id == id && ds.PatientId == patientId);
            
        }

        public bool Save()
        {
            int ret = _context.SaveChanges();
            return ret > 0;
        }

        public bool Update(DentalScanModel dentalScanModel, bool bSave = true)
        {
            throw new NotImplementedException();
        }
    }
}
