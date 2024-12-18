using Microsoft.AspNetCore.Identity;
using Library.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library.Identity.Interfaces
{
    public interface IAppIdentityService
    {
        public Task<AppUser> FindByNameAsync(string username);
        public Task<AppUser> FindByEmailAsync(string email);
        public Task<bool> CheckPasswordAsync(AppUser user, string password);
        public Task<string> GenerateJwtToken(AppUser user);
        public Task<IdentityResult> CreateUser(AppUser user, string password);
        public Task<IdentityResult> UpdateUser(AppUser user);
        public Task<IdentityResult> AddRoleToUser(AppUser user, string role);
        string GenerateRandomCode();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateRefreshToken(int size = 32);
        string GenerateRandomPassword(PasswordOptions? opts = null);
        AppUser FindByVerficationCodeAsync(string verficationCode);
    }
}
