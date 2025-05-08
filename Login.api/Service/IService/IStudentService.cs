using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.dtos.Response;
using Login.api.models;

namespace Login.api.Service.IService
{
    public interface IStudentService
    {
        Task<Student> CreateStudent(User user, List<int> teacherIds);
        Task<Student?> GetByUserId(int userId);

    }
}