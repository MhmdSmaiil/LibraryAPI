using Microsoft.AspNetCore.Identity;
using System;

namespace Library.Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? VerificationCode { get; set; } = string.Empty;
        public byte[] Profile { get; set; } = Array.Empty<byte>();
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string? RefreshToken { get; set; }
        public bool? RefreshTokenValidated { get; set; }

        public AppUser()
        {
            CreatedOn = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
