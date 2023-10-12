using Dental.Data;
using Dental.Interfaces;
using Dental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Dental.Reposiotry
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPatientRepository _patientReposiotry;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(ApplicationDbContext context, IPatientRepository patientReposiotry, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _patientReposiotry = patientReposiotry;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [Authorize(Roles = "admin")]
        public async Task<UserModel> GetUserById(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<PatientModel>> GetAllPatients(string id)
        {
            var allPatients = await _patientReposiotry.GetAllPatients();

            return allPatients.Where(p => p.UserId == id);
        }

        public bool EnableAddingNewDentalScan()
        {
            return true;
            //int iDentalScanCount = 0;
            //var patients = _patientReposiotry.GetAllPatients().Result.ToList();

            //var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserID();

            //foreach (var pat in patients)
            //{
            //    iDentalScanCount += _patientReposiotry.GetAllDentalScans(pat.Id).Result.Count();
            //}

            //if(_context.UserRoles.FirstOrDefault(us => us.UserId == currentUserId))
        }

        public bool Update(UserModel user, bool bSave = true)
        {
            if (GetUserById(user.Id).Result == null)
            {
                return false;
            }

            _context.Update(user);

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
