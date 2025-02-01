﻿using System.ComponentModel.DataAnnotations;

namespace EventosPro.ViewModels.Users
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }

        public string Token { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}
