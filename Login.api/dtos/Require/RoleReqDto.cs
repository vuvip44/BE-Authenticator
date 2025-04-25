using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Login.api.dtos.Require
{
    public class RoleReqDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}