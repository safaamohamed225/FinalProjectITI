﻿using System.ComponentModel.DataAnnotations;

namespace Waseet.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        
        //[Required(ErrorMessage = "Username is required")]
        //public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }
}
