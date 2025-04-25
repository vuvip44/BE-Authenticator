using System.Security.Claims;
using Login.api.Common;
using Login.api.dtos.Require;
using Login.api.dtos.Response;
using Login.api.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Login.api.Controller
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                var error = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return BadRequest(new ApiResponse<string>(400, error));
            }

            var result = await _userService.LoginAsync(dto);

            // Set refreshToken vào cookie HttpOnly
            SetRefreshTokenCookie(result.refreshToken);
            // Set accessToken vào cookie HttpOnly
            SetAccessTokenCookie(result.accessToken);
            return Ok(new ApiResponse<object>(200, new
            {
                accessToken = result.accessToken,
                refreshToken = result.refreshToken,
                user = new UserResDto()
                {
                    Id = result.userRes.Id,
                    Username = result.userRes.Username,
                    Role = result.userRes.Role
                }
            }));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest(new ApiResponse<string>(400, "Refresh token is required."));
            }

            var result = await _userService.RefreshTokenAsync(refreshToken);
            // Set refreshToken vào cookie HttpOnly
            SetRefreshTokenCookie(result.refreshToken);

            // Set accessToken vào cookie HttpOnly
            SetAccessTokenCookie(result.accessToken);
            return Ok(new ApiResponse<object>(200, new
            {
                accessToken = result.accessToken,
                refreshToken = result.refreshToken

            }));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                var error = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return BadRequest(new ApiResponse<string>(400, error));
            }

            var user = await _userService.RegisterAsync(dto);

            return Ok(new ApiResponse<UserResDto>(200, user, "Register successfully."));
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Xóa refreshToken cookie
            Response.Cookies.Delete("refreshToken");
            // Xóa accessToken cookie
            Response.Cookies.Delete("accessToken");

            return Ok(new ApiResponse<string>(200, null, "Logout successfully."));
        }


        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        private void SetAccessTokenCookie(string accessToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };

            Response.Cookies.Append("accessToken", accessToken, cookieOptions);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new ApiResponse<string>(401, "Unauthorized"));

            var user = await _userService.GetUserByIdAsync(int.Parse(userId));

            if (user == null)
                return NotFound(new ApiResponse<string>(404, "User not found"));

            return Ok(new ApiResponse<UserResDto>(200, user));
        }

    }
}
