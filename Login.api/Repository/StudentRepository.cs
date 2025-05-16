using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.data;
using Login.api.dtos.Response;
using Login.api.models;
using Login.api.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Login.api.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly ApplicationDBContext _context;
        public StudentRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            return await _context.Students.Include(s => s.User).ToListAsync();
        }

        public async Task<Student> GetByStudentId(int studentId)
        {
            return await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == studentId);
        }

        public async Task<Student?> GetByUserIdAsync(int usedId)
        {
            return await _context.Students
                .Include(s => s.StudentTeachers)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == usedId);
        }


        public async Task<Student> GetStudentByUsername(string username)
        {
            return await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.User.Username == username);
        }


        public async Task<List<Student>> GetStudentsByTeacherIdAsync(int teacherId)
        {
            return await _context.StudentTeachers
                .Where(st => st.TeacherId == teacherId)
                .Select(st => st.Student)
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<List<StudentStatisticDto>> GetStudentStatisticsAsync()
        {
            return await _context.Students
            .GroupBy(s => s.XepLoai)
            .OrderBy(g => g.Key)
            .Select(g => new StudentStatisticDto
            {
                XepLoai = g.Key,
                SoLuong = g.Count()
            }).ToListAsync();
        }

        public async Task<bool> IsExist(int id)
        {
            return await _context.Students.AnyAsync(x => x.Id == id);
        }
    }
}