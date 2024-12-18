using Library.Data.Models.Requests;
using Library.Identity.Entities;
using Library.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Interfaces.Services
{
    public interface IAppUserService
    {
        public Task<RequestResult> GetAllUsersAsync();
        public Task<RequestResult> GetUserByIdAsync(int id);
        public Task<RequestResult> CreateUserAsync(RegisterModel user);
        public Task<RequestResult> UpdateUserAsync(int id, UpdateUserModel user);
        public Task<RequestResult> DeleteUserAsync(int id);
        public Task<RequestResult> ChangePasswordAsync(int id, string oldPassword, string newPassword);
        public Task<RequestResult> SearchUsersAsync(string searchTerm);
        public Task<RequestResult> GetUserRolesAsync(int id);
        public Task<RequestResult> AddUserRolesAsync(int id, string roleName);
        public Task<RequestResult> RemoveUserRolesAsync(int id, string roleName);
    }
}
