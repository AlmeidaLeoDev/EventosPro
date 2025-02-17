using System.Threading.Tasks;

namespace EventosPro.Services.Interfaces
{
    public interface IGmailService
    {
        Task<string> SendEmailAsync(string to, string subject, string htmlbody);
    }
}
