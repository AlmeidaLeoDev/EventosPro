using System.ComponentModel.DataAnnotations;

namespace EventosPro.ViewModels.Users
{
    public class ChangePasswordViewModel
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}
