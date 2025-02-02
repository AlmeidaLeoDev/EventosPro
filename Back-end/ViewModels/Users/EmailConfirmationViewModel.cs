using System.ComponentModel.DataAnnotations;

namespace EventosPro.ViewModels.Users
{
    public class EmailConfirmationViewModel
    {
        public string Email { get; set; }

        public string Token { get; set; }
    }
}
