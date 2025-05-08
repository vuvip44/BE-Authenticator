using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.api.models
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string? RefreshToken { get; set; }

        public int? RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}