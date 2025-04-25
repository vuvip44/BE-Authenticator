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
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<User> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(u => u.Username == username);
        }

        public Task<User> GetWithRole(int id)
        {
            return _context.Users
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}