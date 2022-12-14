﻿using System.ComponentModel.DataAnnotations;

namespace CallRecording.ViewModels
{
    public class CreateUserRequest
    {

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}