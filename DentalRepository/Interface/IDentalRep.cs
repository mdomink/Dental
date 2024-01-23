using DentalDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DentalDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace DentalRepository.Interface
{
    public interface IDentalRep
    {
        Task<IEnumerable<UserModel>> GetUsers(Expression<Func<UserModel, bool>> expression);
        Task<UserModel> GetUser(Expression<Func<UserModel, bool>> expression);
        Task<IEnumerable<PatientModel>> GetPatientsNoTracking();
        Task<IEnumerable<PatientModel>> GetPatientsNoTracking(Expression<Func<PatientModel, bool>> expression);
        Task<PatientModel> GetPatient(Expression<Func<PatientModel, bool>> expression);
        Task<DentalScanModel> GetDentalScan(Expression<Func<DentalScanModel, bool>> expression);
        Task<IEnumerable<DentalScanModel>> GetDentalScans(Expression<Func<DentalScanModel, bool>> expression);
        bool Add<T>(T entity);
        bool Update<T>(T entity);
        Task<EntityState> Delete<T>(T entity);
        bool Save();
    }
}
        