using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.dtos.Require;
using Login.api.dtos.Response;

namespace Login.api.Service
{
    public interface IStudentTeacherService
    {
        Task<List<StudentResDto>> GetStudentsByTeacherId(int teacherId);
        Task<bool> UpdateStudentScore(int teacherId, int studentId, StudentUpdateScores dto);
        Task<bool> DeleteStudentByTeacher(int teacherId, int studentId);
    }
}