using DentalBusiness.ExtensionMethods;
using DentalBusiness.Interfaces;
using DentalDomain.Models;
using DentalDomain.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DentalWeb;
using System.Data;
using DentalRepository.Interface;
using DentalRepository.Repository;
using Microsoft.VisualBasic;

namespace DentalBusiness.Logic
{
    public partial class DentalBL : IDentalBL
    {
        private readonly ClaimsPrincipal _user;
        private readonly IDentalRep _dentalRepository;

        public DentalBL(IDentalRep dentalRepository, ClaimsPrincipal user)
        {
            _dentalRepository = dentalRepository;
            _user = user;
        }

        public async Task<DentalScanModel> GetDentalScanById(int dentalId)
        {
            string[] roles = new string[] { "Admin" };

            bool bIsAuthorized = _user.IsInRole(roles);

            return await _dentalRepository.GetDentalScan(ds => ds.Id == dentalId && (ds.Patient.UserId == _user.GetUserID() || bIsAuthorized)) ??
                        (bIsAuthorized ? throw new NullReferenceException() : throw new UnauthorizedAccessException());
        }

        public async Task<DentalScanModel> GetDentalScanByIdNoTracking(int dentalId)
        {
            string[] roles = new string[] { "Admin" };

            bool bIsAuthorized = _user.IsInRole(roles);

            return await _dentalRepository.GetDentalScan(ds => ds.Id == dentalId && (ds.Patient.UserId == _user.GetUserID() || bIsAuthorized)) ??
                        (bIsAuthorized ? throw new NullReferenceException() : throw new UnauthorizedAccessException());

        }

        public async Task<bool> Add(DentalScanModel dentalScanModel, int outUserId)
        {
            try
            {
                var patient = GetPatientByIdAsyncNoTracking(dentalScanModel.PatientId).Result;
                if (patient == null)
                {
                    return false;
                }

                bool bAllowNewScan = EnableAddingNewDentalScan(outUserId).Result;
                if (!bAllowNewScan)
                {
                    return false;
                }

                if (!_dentalRepository.Add(dentalScanModel))
                    return false;

                patient.LastUpdate = dentalScanModel.CreationDate;

                return await Update(patient);
            }
            catch
            {
                return false;
            }
            
        }

        public bool Update(DentalScanModel dentalScanMode)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Add(PatientModel patient)
        {
            try
            {
                string[] roles = new string[] { "Admin" };

                bool bIsAuthorized = _user.IsInRole(roles);

                if(!bIsAuthorized && patient.UserId != _user.GetUserID())
                {
                    return false;
                }

                if (!_dentalRepository.Add(patient))
                    return false;

                return Save();
            }
            catch
            {
                return false;
            }
        }                       
        public async Task<PatientModel> GetPatientByIdAsync(int Id)
        {
            string[] roles = new string[] { "Admin" };

            bool bIsAuthorized = _user.IsInRole(roles);

            return await _dentalRepository.GetPatient(p => p.Id == Id && (p.UserId == _user.GetUserID() || bIsAuthorized));
        }
        public async Task<PatientModel> GetPatientByIdAsyncNoTracking(int Id)
        {
            string[] roles = new string[] { "Admin" };

            bool bIsAuthorized = _user.IsInRole(roles);

            try
            {
                return await _dentalRepository.GetPatient(p => p.Id == Id && (p.UserId == _user.GetUserID() || bIsAuthorized));
            }
            catch
            {
                return bIsAuthorized ? throw new NullReferenceException() : throw new UnauthorizedAccessException();
            }
                                   
        }
        public async Task<IEnumerable<PatientModel>> GetAllPatients(int outUserId)
        {
            try
            {
                //checks if user iz authorized to access
                var user = await GetUserByOutId(outUserId);

                return await _dentalRepository.GetPatientsNoTracking(p => (p.UserId == user.Id));

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get all dental scans of patient
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <returns>
        /// List of dental scans
        /// 
        /// Exceptions:
        /// NullReferenceException if given patient does not exisits:
        /// 
        /// UnauthorizedAccessException if access to this data is permited
        /// </returns>
        public async Task<IEnumerable<DentalScanModel>> GetAllDentalScansAsync(int patientId)
        {
            try
            {
                //checks if user iz authorized to access this patient
                var patient = await GetPatientByIdAsyncNoTracking(patientId);

                return await _dentalRepository.GetDentalScans(ds => ds.PatientId == patientId);
            }
            catch(Exception)
            {
                throw;
            }                                  
        }

        /// <summary>
        /// Get list of all users
        /// </summary>
        /// <returns>
        /// List of users
        /// 
        /// Exceptions:
        /// 
        /// UnauthorizedAccessException if access to this data is permited
        /// </returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            if (_user.IsInRole("Admin"))
                return await _dentalRepository.GetUsers(u => true);

            else
               throw new UnauthorizedAccessException();
        }

        public async Task<UserModel> GetUserById(string userId)
        {
            string[] roles = new string[] { "Admin" };

            bool bIsAuthorized = _user.IsInRole(roles);

            return await _dentalRepository.GetUser(u => bIsAuthorized ? 
                                                    u.Id == userId : 
                                                    (u.Id == userId && userId == _user.GetUserID())) ??
                (bIsAuthorized ? throw new NullReferenceException() : throw new UnauthorizedAccessException());
        }

        public async Task<UserModel> GetUserByOutId(int outUserId)
        {
            string[] roles = new string[] { "Admin" };

            bool bIsAuthorized = _user.IsInRole(roles);

            return await _dentalRepository.GetUser(u => bIsAuthorized ? 
                                                    u.OutUserId == outUserId : 
                                                    (u.OutUserId == outUserId && u.Id == _user.GetUserID())) ??
                (bIsAuthorized ? throw new NullReferenceException() : throw new UnauthorizedAccessException());
        }        

        //check wether user can add more dental scans
        public async Task<bool> EnableAddingNewDentalScan(int outUserId)
        {
            try
            {
                int iDentalScanCount = 0;
                var lsPatients = await GetAllPatients(outUserId);

                foreach (var patient in lsPatients)
                {
                    iDentalScanCount += GetAllDentalScansAsync(patient.Id).Result.Where(d => d.IsActive()).Count();
                }

                return iDentalScanCount < 7;
            }
            catch
            {
                return false;
            }            
        }

        public async Task<bool> Update(UserModel user)
        {
            string[] roles = new string[] { "Admin" };

            if (!_user.IsInRole(roles) && user.Id != _user.GetUserID())
            {
                return false;
            }

            var exisitngUser = await _dentalRepository.GetUser(u => u.Id == user.Id);

            if (!_dentalRepository.Update(user))
                return false;

            return Save();
        }

        public async Task<bool> Update(PatientModel patient)
        {
            try
            {
                var exisitngPatient = await _dentalRepository.GetPatient(p => p.Id == patient.Id);

                if (exisitngPatient == null)
                {
                    return false;
                }

                if (!_dentalRepository.Update(patient))
                    return false;

                return Save();
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(PatientModel patient)
        {
            if (GetPatientByIdAsyncNoTracking(patient.Id) == null)
            {
                return false;
            }

            _dentalRepository.Delete(patient);

            return Save();
        }

        private bool Save()
        {
            return _dentalRepository.Save();
        }
    }
}
