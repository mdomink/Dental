using DentalDomain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection.Emit;

namespace DentalDomain.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
	{
        public DbSet<PatientModel> Patients { get; set; }

        public DbSet<DentalScanModel> DentalScans { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		    
        }		

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserModel>().Property(u => u.OutUserId).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            base.OnModelCreating(builder);
        }
    }
}
