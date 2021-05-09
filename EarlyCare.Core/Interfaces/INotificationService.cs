using EarlyCare.Infrastructure.SharedModels;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendMessage(string phoneNumber, string message);

        Task<bool> SendEmail(EmailModel emailModel);
    }
}