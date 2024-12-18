using Library.Resources.Extensions;
using Library.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library.Data.Models.Requests
{
    public class ResetPasswordModel
    {
        public string? ResetToken { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired")]
        [StringLengthIfNotEmpty(20, ErrorMessageResourceName = "PasswordPolicyNotMet", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordsNotMatch")]
        public string? ConfirmPassword { get; set; }
    }
}
