using Microsoft.AspNetCore.Identity;
using Library.Identity.Entities;
using Library.Identity.Interfaces;

namespace Library.Identity.Services
{
    internal class AppIdentityDbContextSeed : IAppIdentityDbContextSeed
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AppIdentityDbContextSeed(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // Seed roles
            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
            {
                var role = new AppRole { Name = Roles.Admin };
                await _roleManager.CreateAsync(role);
            }
            if (!await _roleManager.RoleExistsAsync(Roles.User))
            {
                var role = new AppRole { Name = Roles.User };
                await _roleManager.CreateAsync(role);
            }

            // Seed users
            if (await _userManager.FindByEmailAsync("rentalschalet@gmail.com") == null)
            {
                var user = new AppUser { UserName = "mhmd.smaiil", Email = "mhmd.smaiil2@gmail.com" };
                var result = await _userManager.CreateAsync(user, "P@ssw0rd");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Admin);
                }
            }
        }
    }

}
