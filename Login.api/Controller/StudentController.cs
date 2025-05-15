using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Login.api.Common;
using Login.api.dtos.Require;
using Login.api.dtos.Response;
using Login.api.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Login.api.Controller
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [Authorize(Roles = "STUDENT")]
        public async Task<IActionResult> GetAllStudent()
        {
            var students = await _studentService.GetAllStudents();
            if (students == null)
            {
                return NotFound(new ApiResponse<string>(404, "No students found"));
            }
            return Ok(new ApiResponse<List<StudentResDto>>(200, students));
        }

        [HttpGet("{username}")]
        [Authorize(Roles = "STUDENT")]
        public async Task<IActionResult> GetStudentByUsername(string username)
        {
            var student = await _studentService.GetStudentByUsername(username);
            if (student == null)
            {
                return NotFound(new ApiResponse<string>(404, "No students found"));
            }
            return Ok(new ApiResponse<StudentResDto>(200, student));
        }

        [HttpPut]
        [Authorize(Roles = "STUDENT")]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentUpdate dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _studentService.UpdateStudent(userId, dto);
            if (!result)
            {
                return BadRequest(new ApiResponse<string>(400, "Lỗi không cập nhật được"));
            }
            return Ok(new ApiResponse<string>(200, "Cập nhật thành công"));
        }
    }
}
