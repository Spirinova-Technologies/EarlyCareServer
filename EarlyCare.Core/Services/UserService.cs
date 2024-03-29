﻿using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IGlobalSettingsRepository _globalSettingsRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public UserService(ILogger<UserService> logger,
            IUserRepository userRepository, IGlobalSettingsRepository globalSettingsRepository, ITokenService tokenService, IEmailService emailService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _globalSettingsRepository = globalSettingsRepository;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public UserResponseModel GenerateUserResponse(User user, List<Service> services)
        {
            var token = _tokenService.GenerateToken(user.Id);

            var response = new UserResponseModel
            {
                AccessToken = token,
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id,
                MobileNumber = user.MobileNumber,
                ProfilePhoto = user.ProfilePhoto,
                UserType = user.UserType,
                CityId = user.CityId,
                IsVerified= user.IsVerified
            };

            response.Services = services.Select(p => new ServiceData()
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image
            }).ToList();

            return response;
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return await _userRepository.GetUserByPhoneNumber(phoneNumber);
        }

        public async Task<User> GetVolunteer(int volunteerId)
        {
            return await _userRepository.GetVolunteer(volunteerId);
        }

        public async Task<List<User>> GetVolunteers(bool showAll)
        {
            return await _userRepository.GetVolunteers(showAll);
        }

        public async Task<User> InsertUser(User user)
        {
            //  user.AccessToken = "dummy token";
            user.IsVerified = false;
            user.Password = "";

            var response = await _userRepository.InsertUser(user);
            await _emailService.SendNewUserNotification(response);
            return response;
        }
    }
}