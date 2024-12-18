using Library.Data.Models.Requests;
using Library.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Interfaces.Services
{
    public interface IUserService
    {
        public Task<RequestResult> Authenticate(LoginModel model);
        public Task<RequestResult> RegisterUser(RegisterModel model);
        public Task<RequestResult> RefreshToken(RefreshTokenModel model);
    }
}
