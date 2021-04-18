using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
   public interface INotificationService
    {
        Task<bool> SendMessage(string phoneNumber, string message);
        bool SendEmail();
    }
}
