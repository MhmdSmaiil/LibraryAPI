using Microsoft.AspNetCore.Identity;
using Library.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Interfaces.Repositories
{
    public interface IAppUserRepository : IRepositoryBase<AppUser>
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> CreateUserAsync(AppUser user, string password, string[] roles);
        Task<AppUser> UpdateUserAsync(int id, AppUser user, string[] roles, string oldPassword = "", string newPassword = "");
        Task<AppUser> DeleteUserAsync(int id);
        Task<IdentityResult> ChangePasswordAsync(AppUser existingUser, string oldPassword, string newPassword);
        Task<IEnumerable<AppUser>> SearchUsersAsync(string searchTerm);
        Task<IList<string>> GetUserRolesAsync(AppUser user);
        Task<IdentityResult> AddUserRolesAsync(AppUser user, string roleName);
        Task<IdentityResult> RemoveUserRoleAsync(AppUser user, string roleName);
    }
}
