using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.dtos.Response;
using Login.api.models;
using Login.api.Repository.IRepository;
using Login.api.Service.IService;

namespace Login.api.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        public StudentService(IStudentRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }
        public Task<Student> CreateStudent(User user, List<int> teacherIds)
        {
            throw new NotImplementedException();
        }

        public Task<Student?> GetByUserId(int userId)
        {
            throw new NotImplementedException();
        }


    }
}