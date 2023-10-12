using Dental.Models;

namespace Dental.Interfaces
{
    public partial interface IPatientRepository
    {
        Task<IEnumerable<PatientModel>> GetAllPatients();

        Task<PatientModel> GetByIdAsync(int id);

        Task<PatientModel> GetByIdAsyncNoTracking(int id);

        Task<IEnumerable<DentalScanModel>> GetAllDentalScans(int Id);

        Task<DentalScanModel> GetDentalScanById(int id, int dentalID);

        bool AddDentalScan(DentalScanModel dentalScan);

        bool Add(PatientModel patient, bool bSave = true);

        bool Update(PatientModel patient, bool bSave = true);

        bool Delete(PatientModel patient, bool bSave = true);
    }
}
