using EarlyCare.Core.Models;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendNewUserNotification(User user);
        Task SendUpdateNotification(int userId, string subject, string body);
    }
}