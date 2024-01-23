using DentalDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBusiness.Interfaces
{
    public interface IDentalBL
    {
        Task<IEnumerable<UserModel>> GetAllUsers();

        Task<UserModel> GetUserByOutId(int id);

        Task<UserModel> GetUserById(string id);

        Task<IEnumerable<PatientModel>> GetAllPatients(int outUserID);

        Task<PatientModel> GetPatientByIdAsync(int id);

        Task<PatientModel> GetPatientByIdAsyncNoTracking(int id);

        Task<IEnumerable<DentalScanModel>> GetAllDentalScansAsync(int patientId);

        Task<DentalScanModel> GetDentalScanById(int dentalId);

        Task<DentalScanModel> GetDentalScanByIdNoTracking(int dentalId);

        Task<bool> Add(DentalScanModel dentalScanModel, int outUserId);

        //bool Update(DentalScanModel dentalScanModel);        

        Task<bool> Add(PatientModel patient);

        Task<bool> Update(PatientModel patient);

        bool Delete(PatientModel patient);

        Task<bool> EnableAddingNewDentalScan(int outUserID);

        Task<bool> Update(UserModel user);
    }
}
