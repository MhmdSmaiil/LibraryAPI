using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Library.Data.Interfaces.Services;
using Library.Data.Models.Requests;
using Library.Identity.Entities;
using Library.Identity.Interfaces;
using Library.Resources.Models;
using Library.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Services
{
    public class UserService : IUserService
    {
        private readonly IAppIdentityService _repo;
        private readonly ILogger<UserService> _logger;
        public UserService(IAppIdentityService repo,
            ILogger<UserService> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        public async Task<RequestResult> Authenticate(LoginModel model)
        {
            var user = await _repo.FindByNameAsync(model.Username);

            if (user != null && await _repo.CheckPasswordAsync(user, model.Password))
            {
                var tokenString = await _repo.GenerateJwtToken(user);
                var refreshToken = _repo.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenValidated = true;
                await _repo.UpdateUser(user);

                _logger.LogInformation($"User {user?.UserName} logged in.");

                return new RequestResult()
                {
                    Success = true,
                    Result = new { AccessToken = tokenString, RefreshToken = refreshToken }
                };
            }

            else if (user != null)
            {
                _logger.LogError("InvalidPassword");
                return new RequestResult()
                {
                    Success = false,
                    Message = "InvalidPassword"
                };
            }
            else
            {
                _logger.LogError("InvalidCredentails");
                return new RequestResult()
                {
                    Success = false,
                    Message = "InvalidCredentails"
                };
            }
        }

        public async Task<RequestResult> RefreshToken(RefreshTokenModel model)
        {
            var principal = _repo.GetPrincipalFromExpiredToken(model.AccessToken ?? "");

            if (principal == null)
            {
                _logger.LogError("Invalid access token");
                return new RequestResult()
                {
                    Success = false,
                    Message = "Invalid access token"
                };
            }

            var username = principal?.Identity?.Name;
            var user = await _repo.FindByNameAsync(username);

            if (user == null || user.RefreshToken != model.RefreshToken || !user.RefreshTokenValidated.Value)
            {
                _logger.LogError("Invalid refresh token");
                return new RequestResult()
                {
                    Success = false,
                    Message = "Invalid refresh token"
                };
            }

            var accessToken = await _repo.GenerateJwtToken(user);
            var newRefreshToken = _repo.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenValidated = false;
            await _repo.UpdateUser(user);

            return new RequestResult()
            {
                Success = true,
                Result = new { AccessToken = accessToken, RefreshToken = newRefreshToken }
            };
        }
        public async Task<RequestResult> RegisterUser(RegisterModel model)
        {
            var user = new AppUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedOn = DateTime.UtcNow,
                VerificationCode = _repo.GenerateRandomCode(),
            };

            if (!string.IsNullOrEmpty(model.Profile))
            {
                user.Profile = Convert.FromBase64String(model.Profile);
            }

            var creationResult = await _repo.CreateUser(user, model.Password);
            if (creationResult.Succeeded)
            {
                await _repo.AddRoleToUser(user, Roles.User);
                _logger.LogInformation("User created a new account with password.");

                return new RequestResult() { Success = true, Result = true };
            }
            else
            {
                return new RequestResult()
                {
                    Success = false,
                    Message = creationResult.Errors.FirstOrDefault()?.Description ?? string.Empty
                };
            }

        }
    }
}
