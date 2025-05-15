using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.dtos.Response;
using Login.api.models;

namespace Login.api.Repository.IRepository
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        Task<Teacher?> GetByUserIdAsync(int userId);
        Task<Teacher?> GetByIdWithStudentsAsync(int teacherId);

        Task<IEnumerable<Teacher>> GetAllTeacherAsync();

        Task<bool> isExist(int id);
    }
}