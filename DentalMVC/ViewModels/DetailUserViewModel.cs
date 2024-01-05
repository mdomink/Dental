namespace DentalWeb.ViewModels
{
    public class DetailUserViewModel : UserViewModel
    {
        public IEnumerable<PatientViewModel>? Patients { get; set; }
    }
}
