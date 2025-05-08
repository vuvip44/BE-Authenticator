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
        private readonly IStudentRepository _studenRepo;
        private readonly ITeacherRepository _teacherRepo;
        private readonly IRoleRepository _roleRepository;
        private readonly IStudentTeacherRepository _studentTeacherRepo;

        public UserService(IUserRepository userRepository, ITokenService tokenService, IConfiguration configuration, IRoleRepository roleRepository, IStudentRepository studentRepo, ITeacherRepository teacherRepo, IStudentTeacherRepository studentTeacherRepo)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _config = configuration;
            _roleRepository = roleRepository;
            _studenRepo = studentRepo;
            _teacherRepo = teacherRepo;
            _studentTeacherRepo = studentTeacherRepo;
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
                FullName = user.FullName
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


            Role? role = null;

            if (dto.Role == "student")
            {
                role = await _roleRepository.GetByNameAsync("STUDENT");
                if (role == null)
                {
                    throw new AppException("Role 'Student' not found");
                }
            }
            else if (dto.Role == "teacher")
            {
                role = await _roleRepository.GetByNameAsync("TEACHER");
                if (role == null)
                {
                    throw new AppException("Role 'Teacher' not found");
                }
            }
            else
            {
                throw new AppException("Must select either Student or Teacher.");
            }
            if (role == null)
            {
                throw new AppException("Role not found.");
            }

            var user = new User
            {
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = role.Id,
                FullName = dto.FullName
            };


            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            if (dto.Role == "student")
            {
                var student = new Student { UserId = user.Id };
                await _studenRepo.AddAsync(student);
                await _userRepository.SaveChangesAsync();
                if (!string.IsNullOrEmpty(dto.TeacherId))
                {
                    int teacherId = int.Parse(dto.TeacherId); // Chuyển TeacherId thành kiểu int
                    var teacher = await _teacherRepo.GetByIdAsync(teacherId); // Kiểm tra TeacherId hợp lệ
                    if (teacher == null)
                    {
                        throw new AppException("Teacher not found.");
                    }

                    var rel = new StudentTeacher
                    {
                        StudentId = student.Id,
                        TeacherId = teacherId
                    };
                    await _studentTeacherRepo.AddAsync(rel);
                    await _studentTeacherRepo.SaveChangesAsync();
                }
            }
            else if (dto.Role == "teacher")
            {
                var teacher = new Teacher { UserId = user.Id };
                await _teacherRepo.AddAsync(teacher);
                await _userRepository.SaveChangesAsync();
            }



            var userDto = new UserResDto()
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role.Name,
                FullName = user.FullName
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
                FullName = user.FullName
            };
            return userDto;
        }
    }
}