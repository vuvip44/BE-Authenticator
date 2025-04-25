using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.models;

namespace Login.api.Repository.IRepository
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetByNameAsync(string name);
    }
}