﻿using System.ComponentModel.DataAnnotations;

namespace Miracle.Core.Api.Models.Authentication
{
    public class AuthenticationModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
