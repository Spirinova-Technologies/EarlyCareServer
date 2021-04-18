using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IGlobalSettingsRepository _globalSettingsRepository;

        public UserService(ILogger<UserService> logger, INotificationService notificationService,
            IUserRepository userRepository, IGlobalSettingsRepository globalSettingsRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _globalSettingsRepository = globalSettingsRepository;
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return await _userRepository.GetUserByPhoneNumber(phoneNumber);
        }

        public async Task<User> GetVolunteer(int volunteerId)
        {
            return await _userRepository.GetVolunteer(volunteerId);
        }

        public async Task<List<User>> GetVolunteers()
        {
            return await _userRepository.GetVolunteers();
        }

        public async Task<User> InsertUser(User user)
        {
            user.AccessToken = "dummy token";
            user.IsVerified = false;
            user.Password = "";

            return await _userRepository.InsertUser(user);
        }
    }
}