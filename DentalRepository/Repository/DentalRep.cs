using DentalDomain.Data;
using DentalDomain.Models;
using DentalRepository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Linq.Expressions;

namespace DentalRepository.Repository
{
    public class DentalRep : IDentalRep
    {
        private readonly ApplicationDbContext _context;

        public DentalRep(ApplicationDbContext context)
        {
            _context = context;
        }

        #region user
        public async Task<UserModel> GetUser(Expression<Func<UserModel, bool>> expression) => await _context.Users.AsNoTracking().FirstAsync(expression);

        public async Task<IEnumerable<UserModel>> GetUsers(Expression<Func<UserModel, bool>> expression)
        {
            return await _context.Users
                        .AsNoTracking()
                        .Where(expression)
                        .ToListAsync();
        }

        #endregion
        public async Task<PatientModel> GetPatient(Expression<Func<PatientModel, bool>> expression)
        {
            return await _context.Patients
                        .Include(p => p.User)
                        .AsNoTracking()
                        .FirstAsync(expression);        
        }        
        public async Task<IEnumerable<PatientModel>> GetPatientsNoTracking()
        {
            return await _context.Patients
                        .Include(p => p.User)
                        .AsNoTracking()
                        .Where(p => p != null)
                        .ToListAsync();
        }
        public async Task<IEnumerable<PatientModel>> GetPatientsNoTracking(Expression<Func<PatientModel, bool>> expression)
        {
            return await _context.Patients
                        .Include(p => p.User)
                        .AsNoTracking()
                        .Where(expression)
                        .ToListAsync();
        }

        public async Task<DentalScanModel> GetDentalScan(Expression<Func<DentalScanModel, bool>> expression)
        {
            return await _context.DentalScans
                        .Include(d => d.Patient)
                        .ThenInclude(p => p.User)
                        .AsNoTracking()
                        .Where(expression)
                        .FirstAsync();
        }

        public async Task<IEnumerable<DentalScanModel>> GetDentalScans(Expression<Func<DentalScanModel, bool>> expression)
        {
            return await _context.DentalScans
                        .Include(d => d.Patient)
                        .ThenInclude(p => p.User)
                        .AsNoTracking()
                        .Where(expression)
                        .ToListAsync();
        }
        public bool Update<T>(T entity)
        {
            try
            {
                _context.Update(entity);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Add<T>(T entity)
        {
            try
            {
                _context.Update(entity);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<EntityState> Delete<T>(T entity)
        {
            throw new NotImplementedException();
        }
        public bool Save()
        {
            int ret = _context.SaveChanges();
            return ret > 0;
        }        
    }
}