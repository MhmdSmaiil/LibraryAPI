using Library.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Models.Requests
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceName = "UserNameRequired")]
        public string Username { get; set; }
        [Required(ErrorMessageResourceName = "PasswordRequired")]
        public string Password { get; set; }
    }
}
