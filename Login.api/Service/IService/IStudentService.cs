using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.dtos.Require;
using Login.api.dtos.Response;
using Login.api.models;

namespace Login.api.Service.IService
{
    public interface IStudentService
    {
        Task<Student> CreateStudent(User user, List<int> teacherIds);
        Task<StudentResDto?> GetByUserId(int userId);
        Task<List<StudentResDto>> GetAllStudents();

        Task<StudentResDto> GetStudentByUsername(string name);
        Task<bool> UpdateStudent(int userId, StudentUpdate dto);
        Task<List<StudentStatisticDto>> GetStudentStatisticsAsync();

    }
}