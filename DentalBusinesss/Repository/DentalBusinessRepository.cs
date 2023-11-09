using DentalBusiness.ExtensionMethods;
using DentalBusiness.Interfaces;
using DentalBusiness.Models;
using DentalBusinnes.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBusiness.Repository
{
    public class DentalBusinessRepository : IDentalBusinessRepository
    {
        private readonly ApplicationDbContext _context;

        public DentalBusinessRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DentalScanModel> GetDentalScanById(int dentalId, int? patientId)
        {
            return await _context.DentalScans.FirstOrDefaultAsync(ds => ds.Id == dentalId && (ds.PatientId == patientId || patientId == null));
        }

        public async Task<DentalScanModel> GetDentalScanByIdNoTracking(int dentalId, int? patientId)
        {
            return await _context.DentalScans.AsNoTracking().FirstOrDefaultAsync(ds => ds.Id == dentalId && (ds.PatientId == patientId || patientId == null));
        }

        public bool Add(DentalScanModel dentalScanModel, int outUserId)
        {
            var patient = GetPatientByIdAsyncNoTracking(dentalScanModel.PatientId, outUserId).Result;

            if (patient == null)
            {
                return false;
            }

            if (!EnableAddingNewDentalScan(outUserId).Result)
            {
                return false;
            }

            _context.Add(dentalScanModel);

            patient.LastUpdate = dentalScanModel.CreationDate;

            return Update(patient); ;
        }

        public bool Update(DentalScanModel dentalScanMode)
        {
            throw new NotImplementedException();
        }

        public bool Add(PatientModel patient)
        {
           // var user = Get
            _context.Add(patient);

             return Save();
        }

        public async Task<PatientModel> GetPatientByIdAsync(int Id, int? outUserId = null)
        {
            var user = await GetUserByOutId(outUserId);

            return await _context.Patients.FirstOrDefaultAsync(p => p.Id == Id && (user == null || p.UserId == user.Id));
        }

        public async Task<PatientModel> GetPatientByIdAsyncNoTracking(int Id, int? outUserId = null)
        {
            var user = await GetUserByOutId(outUserId);

            return await _context.Patients.AsNoTracking().FirstOrDefaultAsync(p => p.Id == Id && (user == null || p.UserId == user.Id));
        }

        public async Task<IEnumerable<DentalScanModel>> GetAllDentalScans(int patientId, int? outUserId = null)
        {   
            var patient = await GetPatientByIdAsyncNoTracking(patientId, outUserId);

            if (patient == null)
            {
                return Enumerable.Empty<DentalScanModel>();
            }

            return await _context.DentalScans.Where(ds => ds.PatientId == patientId).ToListAsync();
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserModel> GetUserById(string? userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<UserModel> GetUserByOutId(int? outUserId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.OutUserId == outUserId);
        }

        public async Task<IEnumerable<PatientModel>> GetAllPatients(int? outUserId)
        {
            var user = await GetUserByOutId(outUserId);

            var userPatients = _context.Patients.AsNoTracking().Where(p => (user == null ||p.UserId == user.Id));

            return await userPatients.ToListAsync();
        }

        public async Task<bool> EnableAddingNewDentalScan(int outUserId)
        {
            //return true;
            int iDentalScanCount = 0;
            var lsPatients = await GetAllPatients(outUserId);

            foreach (var patient in lsPatients)
            {
                iDentalScanCount += GetAllDentalScans(patient.Id).Result.Where(d => d.IsActive()).Count();
            }

            return iDentalScanCount < 7;
        }

        public bool Update(UserModel user)
        {
            if (GetUserByOutId(user.OutUserId).Result == null)
            {
                return false;
            }

            _context.Update(user);

            return Save();
        }

        public bool Update(PatientModel patient)
        {
            if (GetPatientByIdAsyncNoTracking(patient.Id).Result == null)
            {
                return false;
            }

            _context.Update(patient);

             return Save();
        }

        public bool Delete(PatientModel patient)
        {
            if (GetPatientByIdAsyncNoTracking(patient.Id) == null)
            {
                return false;
            }

            _context.Remove(patient);

            return Save();
        }

        private bool Save()
        {
            int ret = _context.SaveChanges();
            return ret > 0;
        }
    }
}
