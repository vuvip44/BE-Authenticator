using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.dtos.Response;
using Login.api.Repository.IRepository;

namespace Login.api.Service
{
    public class StudentTeacherService : IStudentTeacherService
    {
        private readonly IStudentTeacherRepository _studentTeacherRepo;
        public StudentTeacherService(IStudentTeacherRepository studentTeacherRepo)
        {
            _studentTeacherRepo = studentTeacherRepo;
        }
        public async Task<List<StudentResDto>> GetStudentsByTeacherId(int teacherId)
        {
            var students = await _studentTeacherRepo.GetStudentsByTeacher(teacherId);
            var studentDtos = students.Select(s => new StudentResDto
            {
                Id = s.Id,
                FullName = s.User.FullName
            }).ToList();
            return studentDtos;
        }
    }
}