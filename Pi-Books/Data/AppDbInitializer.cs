using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Pi_Books.Data.Models;
using Pi_Books.Data.ViewModels.Authentication;

namespace Pi_Books.Data
{
    public class AppDbInitializer
    {
        public static void InitialDbSeed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                if (!context.Publishers.Any())
                {
                    context.Publishers.AddRange(
                        new Publisher() { Name = "Manuals Lib" },
                        new Publisher() { Name = "Open World" }
                        );
                    context.SaveChanges();
                }
                if (!context.Books.Any())
                {
                    context.Books.AddRange(new Book()
                    {
                        Title = "User Manual of Midea AC",
                        Description = "A brief literature for end user to operate a Midea AC",
                        IsRead = true,
                        DateRead = DateTime.Now.AddDays(-30),
                        Rating = 2,
                        Genre = "Operation Manual",
                        //Author = "Midea Group",
                        CoverUrl = "https://www.arafat.pro/...",
                        DateAdded = DateTime.Now,
                        PublisherId = 1

                    },
                    new Book()
                    {
                        Title = "Vivo Smart Phone Y1 User Guide",
                        Description = "Complete handbook for a user to operate a Vivo Smart-phone",
                        IsRead = false,
                        Genre = "Handbook",
                        //Author = "Vivo Ltd.",
                        CoverUrl = "https://www.arafat.pro/...",
                        DateAdded = DateTime.Now,
                        PublisherId = 2
                    }
                    );
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedRoles(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(UserRoles.Publisher))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Publisher));

                if (!await roleManager.RoleExistsAsync(UserRoles.Author))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Author));

                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }
        }
    }
}
