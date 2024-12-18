using Microsoft.AspNetCore.Identity;

namespace Library.Identity.Entities
{
    public class AppRole : IdentityRole
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
        public AppRole()
        {
            CreatedOn = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
