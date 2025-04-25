using Login.api.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Login.api.Controller
{
    [ApiController]
    [Route("api/result")]
    public class ResultController : ControllerBase
    {
        [Authorize(Roles = "USER")]
        [HttpGet("user")]
        public IActionResult GetUser()
        {
            return Ok(new ApiResponse<string>(200, "Bạn là user"));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("admin")]
        public IActionResult GetAdmin()
        {
            return Ok(new ApiResponse<string>(200, "Bạn là admin"));
        }
    }
}
