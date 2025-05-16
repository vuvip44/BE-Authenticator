using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.dtos.Require;
using Login.api.dtos.Response;
using Login.api.models;
using Login.api.Repository.IRepository;
using Login.api.Service.IService;

namespace Login.api.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        public StudentService(IStudentRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }
        public Task<Student> CreateStudent(User user, List<int> teacherIds)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StudentResDto>> GetAllStudents()
        {
            var students = await _studentRepo.GetAllStudents();
            return students.Select(s => new StudentResDto
            {
                Id = s.Id,
                FullName = s.User.FullName,
                Username = s.User.Username,
                DiemCC = s.DiemCC,
                DiemGiuaKy = s.DiemGiuaKy,
                DiemCuoiKy = s.DiemCuoiKy,
                DiemTongKet = s.DiemTongKet,
                XepLoai = s.XepLoai
            }).ToList();

        }

        public async Task<StudentResDto?> GetByUserId(int userId)
        {
            var s = await _studentRepo.GetByUserIdAsync(userId);
            return new StudentResDto
            {
                Id = s.Id,
                FullName = s.User.FullName,
                Username = s.User.Username,
                DiemCC = s.DiemCC,
                DiemGiuaKy = s.DiemGiuaKy,
                DiemCuoiKy = s.DiemCuoiKy,
                DiemTongKet = s.DiemTongKet,
                XepLoai = s.XepLoai
            };
        }

        public async Task<StudentResDto> GetStudentByUsername(string name)
        {
            var s = await _studentRepo.GetStudentByUsername(name);
            return new StudentResDto
            {
                Id = s.Id,
                FullName = s.User.FullName,
                Username = s.User.Username,
                DiemCC = s.DiemCC,
                DiemGiuaKy = s.DiemGiuaKy,
                DiemCuoiKy = s.DiemCuoiKy,
                DiemTongKet = s.DiemTongKet,
                XepLoai = s.XepLoai
            };
        }

        public async Task<bool> UpdateStudent(int usedId, StudentUpdate dto)
        {
            var student = await _studentRepo.GetByUserIdAsync(usedId);
            if (student == null)
            {
                return false;
            }
            student.User.FullName = dto.FullName;

            _studentRepo.Update(student);
            await _studentRepo.SaveChangesAsync();
            return true;

        }

        public async Task<List<StudentStatisticDto>> GetStudentStatisticsAsync()
        {
            return await _studentRepo.GetStudentStatisticsAsync();
        }

    }
}