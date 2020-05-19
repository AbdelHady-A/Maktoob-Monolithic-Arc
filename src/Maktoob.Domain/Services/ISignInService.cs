using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Domain.Services
{
    public interface ISignInService : ICrudService<UserLogin>
    {
        IUserService UserService { get; }
        Task<GResult<TokenModel>> SignInAsync(string credentials, string password, UserLogin userLogin, bool emailCredentials = false);
        Task<GResult<TokenModel>> RefreshTokenAsync(string expiredToken, string refreshToken);
        Task<GResult> SignOutAsync(Guid userId, string jwtId, string refreshToken, bool logOutAll = false);
    }
}