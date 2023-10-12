using Dental.Models;

namespace Dental.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel>> GetAllUsers();

        Task<UserModel> GetUserById(string id);

        Task<IEnumerable<PatientModel>> GetAllPatients(string id);

        bool EnableAddingNewDentalScan();

        bool Update(UserModel user, bool bSave = true);
    }
}
