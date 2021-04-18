﻿using EarlyCare.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByPhoneNumber(string phoneNumber);
        Task<User> InsertUser(User user);
        Task<List<User>> GetVolunteers();
        Task<User> GetVolunteer(int volunteerId);
    }
}