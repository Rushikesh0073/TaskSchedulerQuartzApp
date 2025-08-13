using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulerQuartzApp.Services
{
    public interface IEmailSender
    {

        Task SendAsync(string subject, string htmlBody, CancellationToken ct = default);
    }
}
