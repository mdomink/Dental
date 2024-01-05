using DentalDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBusiness.Interfaces
{
    public interface IDentalBusinessRepository
    {
        Task<IEnumerable<UserModel>> GetAllUsers();

        Task<UserModel> GetUserByOutId(int? id);

        Task<UserModel> GetUserById(string? id);

        Task<IEnumerable<PatientModel>> GetAllPatients(int? outUserId = null);

        Task<PatientModel> GetPatientByIdAsync(int Id, int? outUserId = null);

        Task<PatientModel> GetPatientByIdAsyncNoTracking(int Id, int? outUserId = null);

        Task<IEnumerable<DentalScanModel>> GetAllDentalScans(int patientId, int? outUserId = null);

        Task<DentalScanModel?> GetDentalScanById(int dentalId, int? outUserId = null);

        Task<DentalScanModel?> GetDentalScanByIdNoTracking(int dentalId, int? outUserId = null);

        bool Add(DentalScanModel dentalScanModel, int outUserId);

        bool Update(DentalScanModel dentalScanModel);        

        bool Add(PatientModel patient);

        bool Update(PatientModel patient);

        bool Delete(PatientModel patient);

        Task<bool> EnableAddingNewDentalScan(int outUserID);

        bool Update(UserModel user);
    }
}
