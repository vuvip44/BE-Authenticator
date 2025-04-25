using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Login.api.dtos.Require;
using Login.api.dtos.Response;
using Login.api.Middleware.Exceptions;
using Login.api.models;
using Login.api.Repository.IRepository;
using Login.api.Service.IService;
using Microsoft.IdentityModel.Tokens;

namespace Login.api.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, ITokenService tokenService, IConfiguration configuration, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _config = configuration;
            _roleRepository = roleRepository;
        }

        public async Task<(string accessToken, string refreshToken, UserResDto userRes)> LoginAsync(UserLoginDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null)
            {
                throw new AppException("Invalid username.");
            }
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                throw new AppException("Invalid username or password.");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken(user);

            user.RefreshToken = refreshToken;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return (accessToken, refreshToken, new UserResDto()
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role.Name,
            });
        }

        public async Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var principal = handler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))

                }, out SecurityToken validatedToken);

                var username = principal.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                {
                    throw new AppException("Invalid refresh token: username not found.");
                }

                var user = await _userRepository.GetByUsernameAsync(username);
                if (user == null || user.RefreshToken != refreshToken)
                {
                    throw new AppException("Invalid refresh token: username not found.");
                }

                //Cap lai accessToken va refreshToken
                var newAccessToken = _tokenService.GenerateAccessToken(user);
                var newRefreshToken = _tokenService.GenerateRefreshToken(user);

                user.RefreshToken = newRefreshToken;
                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();

                return (newAccessToken, newRefreshToken);
            }
            catch (System.Exception)
            {

                throw new AppException("Invalid refresh token.");
            }
        }



        public async Task<UserResDto> RegisterAsync(UserRegisterDto dto)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
            {
                throw new AppException("Username already exists.");
            }
            var userRole = await _roleRepository.GetByNameAsync("USER");
            if (userRole == null)
            {
                throw new AppException("Role 'User' not found in system.");
            }

            var user = new User
            {
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = userRole.Id

            };


            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            var userDto = new UserResDto()
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role.Name,
            };

            return userDto;
        }

        public async Task<UserResDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetWithRole(id);
            if (user == null)
            {
                throw new AppException("User not found.");
            }
            var userDto = new UserResDto()
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role.Name,
            };
            return userDto;
        }
    }
}