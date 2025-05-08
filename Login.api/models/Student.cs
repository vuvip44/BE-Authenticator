using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.api.models
{
    public class Student
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<StudentTeacher> StudentTeachers { get; set; } = null;


    }
}