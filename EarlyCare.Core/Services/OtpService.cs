using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.Constants;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.Core.Services
{
    public class OtpService : IOtpService
    {
        private readonly ILogger<OtpService> _logger;
        private readonly IGlobalSettingsRepository _globalSettingsRepository;
        private readonly IOtpRepository _otpRepository;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public OtpService(ILogger<OtpService> logger, IGlobalSettingsRepository globalSettingsRepository, IOtpRepository otpRepository,
            INotificationService notificationService, IUserService userService)
        {
            _logger = logger;
            _globalSettingsRepository = globalSettingsRepository;
            _otpRepository = otpRepository;
            _notificationService = notificationService;
            _userService = userService;
        }

        public async Task<bool> SendVerificationOtp(string mobileNumber)
        {
            var otp = await GenerateOTP();
            var message = $"Your OTP is {otp}";

            //save otp into database
            var otpDetails = new OtpDetails
            {
                Mobile = mobileNumber,
                IsVerified = false,
                Otp = otp.ToString(),
                Type = 1
            };

            await _otpRepository.InsertOtpDetails(otpDetails);

            //TODO - if testing in dev do not send sms based on setting. Add caching
            var globalSettings = await _globalSettingsRepository.GetGlobalSetting();

            var isSmsEnabled = Convert.ToBoolean(globalSettings.First(x => x.Name == Constants.EnableSMS).Value);

            if (isSmsEnabled)
            {
                return await _notificationService.SendMessage(mobileNumber, message);
            }

            return true;
        }

        public async Task<OtpDetails> GetOtpDetails(string mobileNumber, string otp)
        {
            return await _otpRepository.GetOtpDetailsAsync(mobileNumber, otp);
        }

        public async Task UpdateOtpDetailsAsync(string phoneNumber)
        {
            await _otpRepository.UpdateOtpDetailsAsync(phoneNumber);
        }

        private async Task<int> GenerateOTP()
        {
            var globalSettings = await _globalSettingsRepository.GetGlobalSetting();

            if (globalSettings == null || globalSettings.Count == 0)
            {
                _logger.LogError("Global Settings are empty");
                throw new ArgumentNullException("Global Settings are empty");
            }

            var isSmsEnabled = Convert.ToBoolean(globalSettings.First(x => x.Name == Constants.EnableSMS).Value);

            if (isSmsEnabled)
            {
                var otp = GetRandomNumber();

                _logger.LogInformation($"Random Otp is {otp}");

                return otp;
            }

            _logger.LogInformation("Sending default otp");

            return Convert.ToInt32(globalSettings.First(x => x.Name == Constants.DefaultOTP).Value);
        }

        private int GetRandomNumber()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }


    }
}