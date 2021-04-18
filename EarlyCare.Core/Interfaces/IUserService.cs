using EarlyCare.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByPhoneNumber(string phoneNumber);
        Task<User> InsertUser(User user);
        Task<List<User>> GetVolunteers();
        Task<User> GetVolunteer(int volunteerId);
    }
}
