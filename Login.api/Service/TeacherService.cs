using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.dtos.Response;
using Login.api.Repository.IRepository;
using Login.api.Service.IService;

namespace Login.api.Service
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherService(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<List<TeacherResDto>> getAllTeachersAsync()
        {
            var teachers = await _teacherRepository.GetAllTeacherAsync();
            return teachers.Select(t => new TeacherResDto
            {
                Id = t.Id,
                FullName = t.User.FullName
            }).ToList();
        }
    }
}