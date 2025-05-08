using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Login.api.dtos.Require
{
    public class UserRegisterDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }  // "student" or "teacher"
        public string TeacherId { get; set; }
    }
}