using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.api.dtos.Response
{
    public class UserResDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }
    }
}