using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.models;

namespace Login.api.Repository.IRepository
{
    public interface IStudentTeacherRepository : IRepository<StudentTeacher>
    {
        Task<List<Student>> GetStudentsByTeacher(int teacher);
        Task<Student?> GetDetailStudentOfTeacher(int teacherId, int studentId);
        Task<StudentTeacher> GetStudentTeacherRelationAsync(int teacherId, int studentId);
    }
}