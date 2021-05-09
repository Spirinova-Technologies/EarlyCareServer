using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.Constants;
using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly INotificationService _notificationService;
        private readonly IGlobalSettingsRepository _globalSettingsRepository;
        private readonly IUserRepository _userRepository;

        public EmailService(INotificationService notificationService, IGlobalSettingsRepository globalSettingsRepository, IUserRepository userRepository)
        {
            _notificationService = notificationService;
            _globalSettingsRepository = globalSettingsRepository;
            _userRepository = userRepository;
        }

        public async Task SendNewUserNotification(User user)
        {
            var emailSettings = await GetGlobalEmailSettings();
            var emailModel = new EmailModel
            {
                Body = Constants.NewUserEmailBody,
                Subject = Constants.NewUserEmailSubject,
                FromEmail = emailSettings.Item1,
                ToEmailAddresses = new List<string> { user.Email },
                BccEmail = emailSettings.Item2
            };

            await _notificationService.SendEmail(emailModel);
        }

        public async Task SendUpdateNotification(int userId, string subject, string body)
        {
            var user = await _userRepository.GetUserById(userId);
            var emailSettings = await GetGlobalEmailSettings();
            var emailModel = new EmailModel
            {
                Body = subject,
                Subject = body,
                FromEmail = emailSettings.Item1,
                ToEmailAddresses = new List<string> { user.Email },
                BccEmail = emailSettings.Item2
            };

            await _notificationService.SendEmail(emailModel);
        }

        private async Task<(string, List<string>)> GetGlobalEmailSettings()
        {
            var globalSettings = await _globalSettingsRepository.GetGlobalSetting();

            var fromEmail = globalSettings.First(x => x.Name == Constants.FromEmail).Value;
            var bccEmails = globalSettings.First(x => x.Name == Constants.BccEmails).Value.Split(',').ToList();

            return (fromEmail, bccEmails);
        }
    }
}