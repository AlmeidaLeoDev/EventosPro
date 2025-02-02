using System.ComponentModel.DataAnnotations;

namespace EventosPro.ViewModels.Users
{
    public class RegisterViewModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
