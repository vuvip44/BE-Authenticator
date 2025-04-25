using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.models;

namespace Login.api.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);

        Task<User> GetByRefreshTokenAsync(string refreshToken);

        Task<User> GetWithRole(int id);
    }
}