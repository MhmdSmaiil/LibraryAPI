using Library.Data.Interfaces.Repositories;
using Library.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Repositories
{
    public class AppUserRepository : RepositoryBase<AppUser>, IAppUserRepository
    {
        private new readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        public AppUserRepository(AppDbContext appDbContext, UserManager<AppUser> userManager)
            : base(appDbContext)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        public async Task<AppUser> CreateUserAsync(AppUser user, string password, string[] roles)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
                await _userManager.AddToRolesAsync(user, roles);

            return result.Succeeded ? user : throw new DbUpdateException($"{result.Errors.First().Description}");
        }

        public async Task<AppUser> DeleteUserAsync(int id)
        {
            var existingUser = await GetUserByIdAsync(id) ?? throw new InvalidOperationException($"User with id {id} not found.");

            existingUser.IsDeleted = true;

            await _appDbContext.SaveChangesAsync();
            return existingUser;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(c => c.Id == id.ToString() && !c.IsDeleted) ?? throw new InvalidDataException("User not found");
            return user;
        }

        public async Task<AppUser> UpdateUserAsync(int id, AppUser user, string[] roles, string oldPassword = "", string newPassword = "")
        {

            if (!string.IsNullOrEmpty(newPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResponse = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (!resetResponse.Succeeded)
                    throw new InvalidOperationException(resetResponse.Errors.First().Description);

            }

            var response = await _userManager.UpdateAsync(user);
            if (response.Succeeded)
            {
                var existingRoles = await _userManager.GetRolesAsync(user);
                var rolesToAdd = roles.Except(existingRoles);
                var rolesToRemove = existingRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, rolesToAdd);
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            return response.Succeeded ? user : throw new DbUpdateException($"{response.Errors.First().Description}");
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            var users = await _appDbContext.Users.Where(c => !c.IsDeleted).OrderByDescending(c => c.CreatedOn).ToListAsync() ?? throw new InvalidDataException();

            return users;
        }

        public async Task<IdentityResult> ChangePasswordAsync(AppUser existingUser, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(existingUser, oldPassword, newPassword);

            return result;
        }

        public async Task<IEnumerable<AppUser>> SearchUsersAsync(string searchTerm)
        {
            var users = await _userManager.Users.ToListAsync();

            var filteredUsers = users.Where(u => u.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                || u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                || u.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                || u.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
               .ToList();

            return filteredUsers;
        }

        public async Task<IList<string>> GetUserRolesAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return roles;
        }

        public async Task<IdentityResult> AddUserRolesAsync(AppUser user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);

            return result;
        }

        public async Task<IdentityResult> RemoveUserRoleAsync(AppUser user, string roleName)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            return result;
        }
    }

}
