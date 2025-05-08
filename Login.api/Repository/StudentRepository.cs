using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.data;
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

        public async Task<Student?> GetByUserIdAsync(int usedId)
        {
            return await _context.Students
                .Include(s => s.StudentTeachers)
                .FirstOrDefaultAsync(s => s.UserId == usedId);
        }

        public async Task<List<Student>> GetStudentsByTeacherIdAsync(int teacherId)
        {
            return await _context.StudentTeachers
                .Where(st => st.TeacherId == teacherId)
                .Select(st => st.Student)
                .Include(s => s.User)
                .ToListAsync();
        }
    }
}