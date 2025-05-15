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
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        private readonly ApplicationDBContext _context;
        public TeacherRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Teacher>> GetAllTeacherAsync()
        {
            return await _context.Teachers.Include(t => t.User).ToListAsync();
        }
        public async Task<Teacher?> GetByIdWithStudentsAsync(int teacherId)
        {
            return await _context.Teachers.Include(t => t.StudentTeachers)
                        .ThenInclude(st => st.Student)
                        .ThenInclude(s => s.User)
                        .FirstOrDefaultAsync(t => t.Id == teacherId);
        }

        public async Task<Teacher?> GetByUserIdAsync(int userId)
        {
            return await _context.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task<bool> isExist(int id)
        {
            return await _context.Teachers.AnyAsync(x => x.Id == id);
        }
    }
}