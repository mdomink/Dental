using Dental.Data;
using Dental.Data.Enum;
using Dental.Models;
using Microsoft.AspNetCore.Identity;

namespace RunGroopWebApp.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Patients.Any())
                {
                    context.Patients.AddRange(new List<PatientModel>()
                    {
                        new PatientModel()
                        {
                            Name1 = "Marko",
                            Name2 = "Marković"
                         },
                        new PatientModel()
                        {
                            Name1 = "Ivan",
                            Name2 = "Ivanović"
                         },
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
                string adminUserEmail = "markoo.dominkovic@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new UserModel()
                    {
                        Name1 = "Marko",
                        Name2 = "Dominković",
                        UserName = "mdomin",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Street = "Ulica Slave Raškaj 17",
                        City = "Samobor",
                        PostalCode = 10430                                    
                    };
                    await userManager.CreateAsync(newAdminUser, "md253200Dental!");                    
                }
                else
                {
                    await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                }
                

                string appUserEmail = "user@dental.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new UserModel()
                    {
                        Name1 = "User",
                        Name2 = "Userović",
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Street = "Sveti Matej 37",
                        City = "Novi Zagreb",
                        PostalCode = 10010      
                    };
                    
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");                    
                }
                else
                {
                    await userManager.AddToRoleAsync(appUser, UserRoles.User);
                }
            }
        }
    }
}