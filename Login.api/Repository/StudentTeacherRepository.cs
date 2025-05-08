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

        public async Task<List<Student>> GetStudentsByTeacher(int teacherId)
        {
            return await _context.StudentTeachers
                .Where(st => st.TeacherId == teacherId)
                .Include(st => st.Student)
                    .ThenInclude(s => s.User)
                .Select(st => st.Student)
                .ToListAsync();
        }

    }
}