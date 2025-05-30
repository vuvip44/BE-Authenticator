using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.dtos.Response;
using Login.api.models;

namespace Login.api.Repository.IRepository
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student?> GetByUserIdAsync(int usedId);
        Task<Student> GetByStudentId(int studentId);
        Task<bool> IsExist(int id);
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student> GetStudentByUsername(string username);
        Task<List<StudentStatisticDto>> GetStudentStatisticsAsync();
    }
}