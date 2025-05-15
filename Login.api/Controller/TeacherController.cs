using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Login.api.Common;
using Login.api.dtos.Require;
using Login.api.dtos.Response;
using Login.api.Repository.IRepository;
using Login.api.Service;
using Login.api.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Login.api.Controller
{
    [ApiController]
    [Route("api/teachers")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly IStudentTeacherService _studentTeacherService;

        public TeacherController(ITeacherService teacherService, IStudentTeacherService studentTeacherService)
        {
            _teacherService = teacherService;
            _studentTeacherService = studentTeacherService;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllTeacher()
        {
            var teachers = await _teacherService.getAllTeachersAsync();

            if (teachers == null || teachers.Count == 0)
            {
                return NotFound(new ApiResponse<string>(404, "Not teachers found"));
            }
            return Ok(new ApiResponse<List<TeacherResDto>>(200, teachers));
        }

        [HttpGet("{teacherId}/students")]
        [Authorize(Roles = "TEACHER")]
        public async Task<IActionResult> GetStudentsByTeacherId(int teacherId)
        {
            var studentsDtos = await _studentTeacherService.GetStudentsByTeacherId(teacherId);
            if (studentsDtos == null)
            {
                return NotFound(new ApiResponse<string>(404, "No students found"));
            }
            return Ok(new ApiResponse<List<StudentResDto>>(200, studentsDtos));
        }

        [HttpGet("my-students")]
        [Authorize(Roles = "TEACHER")]
        public async Task<IActionResult> GetStudentsOfMy()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var teacher = await _teacherService.getTeacherByUserIdAsync(userId);
            if (teacher == null)
                return Forbid("Không tìm thấy giáo viên tương ứng");

            int teacherId = teacher.Id;
            var studentsDtos = await _studentTeacherService.GetStudentsByTeacherId(teacherId);
            if (studentsDtos == null)
            {
                return NotFound(new ApiResponse<string>(404, "No students found"));
            }
            return Ok(new ApiResponse<List<StudentResDto>>(200, studentsDtos));
        }

        [HttpPut("students/{studentId}/score")]
        [Authorize(Roles = "TEACHER")]
        public async Task<IActionResult> UpdateStudentScore(int studentId, [FromBody] StudentUpdateScores dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .Distinct();

                var error = string.Join("; ", errorMessages);
                return BadRequest(new ApiResponse<string>(400, error));
                return BadRequest(new ApiResponse<string>(400, error));
            }
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var teacher = await _teacherService.getTeacherByUserIdAsync(userId);
            if (teacher == null)
                return Forbid("Không tìm thấy giáo viên tương ứng");
            int teacherId = teacher.Id;
            var result = await _studentTeacherService.UpdateStudentScore(teacherId, studentId, dto);
            if (!result)
            {
                return BadRequest(new ApiResponse<string>(400, "Lỗi không cập nhật được"));
            }
            return Ok(new ApiResponse<string>(200, "Cập nhật thành công"));
        }

    }
}