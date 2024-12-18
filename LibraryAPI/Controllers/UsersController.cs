using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Library.Data.Interfaces.Services;
using Library.Data.Models.Requests;
using Library.Data.Services;
using Library.Identity.Entities;
using Library.Resources.Extensions;
using System.Reflection;

namespace Library.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IAppUserService _appUserService;

        public UsersController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _appUserService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _appUserService.GetUserByIdAsync(id);
            if (!result.Success)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterModel user)
        {
            var result = await _appUserService.CreateUserAsync(user);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ValidateModelState]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserModel user)
        {
            var result = await _appUserService.UpdateUserAsync(id, user);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _appUserService.DeleteUserAsync(id);
            return Ok(result);
        }


        [HttpPost("change-password")]
        [ValidateModelState]
        public async Task<IActionResult> ChangePassword([FromQuery] string oldPassword, [FromQuery]  string newPassword)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("Invalid User ID");
            }

            if (!int.TryParse(userId, out int guidId))
            {
                throw new InvalidOperationException("Invalid User ID format");
            }

            var result = await _appUserService.ChangePasswordAsync(guidId, oldPassword, newPassword);
            return Ok(result);
        }

        [HttpGet("search-users")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> SearchUsers(string searchTerm)
        {
            var result = await _appUserService.SearchUsersAsync(searchTerm);

            return Ok(result);  
        }

        [HttpGet("user-roles")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetUserRoles(int id)
        {
            var result = await _appUserService.GetUserRolesAsync(id);

            return Ok(result);
        }

        [HttpPost("user-roles")]
        [Authorize(Roles = Roles.Admin)]
        [ValidateModelState]
        public async Task<IActionResult> AddUserRoles(int id, string roleName)
        {
            var result = await _appUserService.AddUserRolesAsync(id, roleName);

            return Ok(result);
        }

        [HttpDelete("user-roles")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> RemoveUserRole(int id, string roleName)
        {
            var result = await _appUserService.RemoveUserRolesAsync(id, roleName);

            return Ok(result);
        }

        [HttpGet("all-roles")]
        public IActionResult GetAllRoles()
        {
            Dictionary<string, string> keyValuePairs = new()
            {
                { Roles.Admin, Roles.Admin },
                { Roles.User, Roles.User }
            };

            return Ok(keyValuePairs.ToArray());
        }

    }
}
