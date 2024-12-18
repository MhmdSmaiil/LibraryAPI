using Library.Resources;
using Library.Resources.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library.Data.Models.Requests
{
    public class UpdateUserModel
    {

        [Required(ErrorMessageResourceName = "FirstNameRequired")]
        [StringLength(255, ErrorMessage = "FirstNameValidation")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "LastNameRequired")]
        [StringLength(255, ErrorMessage = "LastNameValidation")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired")]
        //[EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", ErrorMessageResourceName = "InvalidEmail")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLengthIfNotEmpty(20, ErrorMessageResourceName = "PasswordPolicyNotMet", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessageResourceName = "PasswordsNotMatch")]
        public string? ConfirmPassword { get; set; }

        [StringLengthIfNotEmpty(20, ErrorMessageResourceName = "PasswordPolicyNotMet", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string? OldPassword { get; set; }
        public string Profile { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string[]? Roles { get; set; }
    }
}
