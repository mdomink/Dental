using Dental.Models;

namespace Dental.ViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public TimeSpan? LastUpdate  { get; set; }

        public string? UserId { get; set; }

        public IEnumerable<DentalScanModel>? DentalScans { get; set; }

        public bool DisableNewDentalScans { get; set; }
    }
}
