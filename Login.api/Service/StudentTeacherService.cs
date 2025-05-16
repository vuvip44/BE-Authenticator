using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.data;
using Login.api.dtos.Require;
using Login.api.dtos.Response;
using Login.api.Repository.IRepository;

namespace Login.api.Service
{
    public class StudentTeacherService : IStudentTeacherService
    {
        private readonly IStudentTeacherRepository _studentTeacherRepo;
        private readonly ITeacherRepository _teacherRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly IUserRepository _userRepo;
        private readonly ApplicationDBContext _context;
        public StudentTeacherService(IStudentTeacherRepository studentTeacherRepo, ITeacherRepository teacherRepo, IStudentRepository studentRepo, IUserRepository userRepo, ApplicationDBContext context)
        {
            _studentTeacherRepo = studentTeacherRepo;
            _teacherRepo = teacherRepo;
            _studentRepo = studentRepo;
            _userRepo = userRepo;
            _context = context;
        }

        public async Task<bool> DeleteStudentByTeacher(int teacherId, int studentId)
        {
            var relation = await _studentTeacherRepo.GetStudentTeacherRelationAsync(teacherId, studentId);
            if (relation == null)
            {
                return false;
            }

            var student = await _studentRepo.GetByStudentId(studentId);
            if (student == null)
            {
                return false;
            }
            var user = await _userRepo.GetByIdAsync(student.UserId);
            if (user == null)
            {
                return false;
            }

            _studentTeacherRepo.Delete(relation);
            _studentRepo.Delete(student);
            _userRepo.Delete(user);
            var affected = await _context.SaveChangesAsync();
            return affected > 0;
        }

        public async Task<List<StudentResDto>> GetStudentsByTeacherId(int teacherId)
        {
            var students = await _studentTeacherRepo.GetStudentsByTeacher(teacherId);
            var studentDtos = students.Select(s => new StudentResDto
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
            return studentDtos;
        }

        public async Task<bool> UpdateStudentScore(int teacherId, int studentId, StudentUpdateScores dto)
        {
            if (!await _teacherRepo.isExist(teacherId))
            {
                return false;
            }
            if (!await _studentRepo.IsExist(studentId))
            {
                return false;
            }
            var student = await _studentTeacherRepo.GetDetailStudentOfTeacher(teacherId, studentId);
            student.DiemCC = dto.DiemCC;
            student.DiemGiuaKy = dto.DiemGiuaKy;
            student.DiemCuoiKy = dto.DiemCuoiKy;
            var rawScore = (dto.DiemCC * 10f / 100f) +
               (dto.DiemGiuaKy * 20f / 100f) +
               (dto.DiemCuoiKy * 70f / 100f);

            student.DiemTongKet = (float)(Math.Ceiling((double)rawScore * 100) / 100);

            student.XepLoai = student.DiemTongKet >= 8 ? "A" :
                            student.DiemTongKet >= 6.5 ? "B" :
                            student.DiemTongKet >= 5 ? "C" :
                            student.DiemTongKet >= 4 ? "D" : "F";
            _studentRepo.Update(student);
            await _studentRepo.SaveChangesAsync();
            return true;
        }
    }
}