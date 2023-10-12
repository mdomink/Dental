using Dental.Models;
using Humanizer.Localisation.TimeToClockNotation;

namespace Dental.Interfaces
{
    public interface IDentalScanRepository
    {
        Task<IEnumerable<DentalScanModel>> GetAllDentalScan(int? patinetId);

        Task<DentalScanModel> GetDentalScanById(int id, int? patinetId);

        Task<DentalScanModel> GetDentalScanByIdNoTracking(int id, int? patinetId);

        bool Add(DentalScanModel dentalScanModel, bool bSave = true);

        bool Update(DentalScanModel dentalScanModel, bool bSave = true);

    }
}
