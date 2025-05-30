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
    public class StudentTeacherRepository : Repository<StudentTeacher>, IStudentTeacherRepository
    {
        private readonly ApplicationDBContext _context;
        public StudentTeacherRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Student?> GetDetailStudentOfTeacher(int teacherId, int studentId)
        {
            var student = await _context.StudentTeachers
                    .Include(st => st.Student)
                    .ThenInclude(s => s.User)
                    .FirstOrDefaultAsync(st => st.TeacherId == teacherId && st.StudentId == studentId);
            return student?.Student;
        }

        public async Task<List<Student>> GetStudentsByTeacher(int teacherId)
        {
            return await _context.StudentTeachers
                .Where(st => st.TeacherId == teacherId)
                .Include(st => st.Student)
                    .ThenInclude(s => s.User)
                .Select(st => st.Student)
                .ToListAsync();
        }

        public async Task<StudentTeacher> GetStudentTeacherRelationAsync(int teacherId, int studentId)
        {
            return await _context.StudentTeachers.FirstOrDefaultAsync(st => st.TeacherId == teacherId && st.StudentId == studentId);
        }
    }
}