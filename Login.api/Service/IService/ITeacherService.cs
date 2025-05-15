using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.dtos.Response;
using Login.api.models;

namespace Login.api.Service.IService
{
    public interface ITeacherService
    {

        Task<List<TeacherResDto>> getAllTeachersAsync();
        Task<Teacher> getTeacherByUserIdAsync(int userId);
    }
}