using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Library.Data.Entities;
using Library.Data.Interfaces.Repositories;
using Library.Data.Interfaces.Services;
using Library.Data.Models.Requests;
using Library.Identity.Entities;
using Library.Resources.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Library.Data.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _repo;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AppUserService(IAppUserRepository repo, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            _repo = repo;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<RequestResult> GetAllUsersAsync()
        {
            var users = await _repo.GetAllUsersAsync();
            return new RequestResult()
            {
                Success = true,
                Result = users
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RequestResult> GetUserByIdAsync(int id)
        {
            var user = await _repo.GetUserByIdAsync(id);
            return new RequestResult()
            {
                Success = true,
                Result = user
            };
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="user"></param>
       /// <returns></returns>
        public async Task<RequestResult> CreateUserAsync(RegisterModel user)
        {
            var userEntity = new AppUser()
            {
                UserName = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedOn = DateTime.UtcNow,
                Profile = !string.IsNullOrEmpty(user.Profile) ? Convert.FromBase64String(user.Profile) : Array.Empty<byte>(),
            };

            var result = await _repo.CreateUserAsync(userEntity, user.Password, user?.Roles);
            return new RequestResult()
            {
                Success = true,
                Result = result
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<RequestResult> UpdateUserAsync(int id, UpdateUserModel user)
        {
            var existingUser = await _repo.GetUserByIdAsync(id) ?? throw new InvalidOperationException($"User with Id {id} not found");

            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Profile = !string.IsNullOrEmpty(user.Profile) ? Convert.FromBase64String(user.Profile) : Array.Empty<byte>();

            var result = await _repo.UpdateUserAsync(id, existingUser, roles: user?.Roles, newPassword: user.Password);

            return new RequestResult
            {
                Success = true,
                Result = result
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RequestResult> DeleteUserAsync(int id)
        {
            await _repo.DeleteUserAsync(id);
            return new RequestResult()
            {
                Success = true,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<RequestResult> ChangePasswordAsync(int id, string oldPassword, string newPassword)
        {
            var existingUser = await _repo.GetUserByIdAsync(id) ?? throw new InvalidOperationException($"User with Id {id} not found");
            var isCorrectPass = await _userManager.CheckPasswordAsync(existingUser, oldPassword);
            if (!isCorrectPass)
                throw new InvalidOperationException("Old Password is incorrect.");


            var result = await _repo.ChangePasswordAsync(existingUser, oldPassword, newPassword);
            return new RequestResult()
            {
                Success = result.Succeeded,
                Result = result.Errors
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<RequestResult> SearchUsersAsync(string searchTerm)
        {
            var result = await _repo.SearchUsersAsync(searchTerm);
            return new RequestResult()
            {
                Success = true,
                Result = result
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RequestResult> GetUserRolesAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new RequestResult { Message = "User not found.", Success = false };

            var result = await _repo.GetUserRolesAsync(user);
            return new RequestResult()
            {
                Success = true,
                Result = result
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<RequestResult> AddUserRolesAsync(int id, string roleName)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new RequestResult { Message = "User not found.", Success = false };

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                return new RequestResult { Message = "Role does not exist.", Success = false };

            var userHaveRole = await _userManager.IsInRoleAsync(user, roleName);
            if (userHaveRole)
                return new RequestResult { Message = "User already have role.", Success = false };

            var result = await _repo.AddUserRolesAsync(user, roleName);
            return new RequestResult
            {
                Success = result.Succeeded,
                Message = "Role added successfully."
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<RequestResult> RemoveUserRolesAsync(int id, string roleName)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new RequestResult { Message = "User not found.", Success = false };

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                return new RequestResult { Message = "Role does not exist.", Success = false };

            var userHasRole = await _userManager.IsInRoleAsync(user, roleName);
            if (!userHasRole)
                return new RequestResult { Message = "User does not have this role.", Success = false };

            var result = await _repo.RemoveUserRoleAsync(user, roleName);
            return new RequestResult
            {
                Success = result.Succeeded,
                Message = "Role removed successfully."
            };
        }
    }
}
