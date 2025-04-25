using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.dtos.Require;
using Login.api.dtos.Response;
using Login.api.models;

namespace Login.api.Service.IService
{
    public interface IUserService
    {
        Task<UserResDto> RegisterAsync(UserRegisterDto dto);

        Task<(string accessToken, string refreshToken, UserResDto userRes)> LoginAsync(UserLoginDto dto);

        Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken);

        Task<UserResDto> GetUserByIdAsync(int id);
    }
}