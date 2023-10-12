using Dental.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dental.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
	{
        public DbSet<PatientModel> Patients { get; set; }

        public DbSet<DentalScanModel> DentalScans { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		
        }		


    }
}
