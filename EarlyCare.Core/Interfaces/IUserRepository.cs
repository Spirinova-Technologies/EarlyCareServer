using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByPhoneNumber(string phoneNumber);
        Task<User> InsertUser(User user);
        Task<List<User>> GetVolunteers(bool showAll);
        Task<User> GetVolunteer(int volunteerId);
        Task<bool> IsEmailIdExists(string emailId);
        Task InsertUserServiceData(UserServiceData userServiceData);
        Task<List<Service>> GetUsersServices(int userId);
        Task<User> GetUserById(int userId);
        Task<User> UpdateUser(User user);
        Task DeleteUserServiceMapping(int userId);
        Task<User> UpdateVerificationStatus(UpdateVerificationStatusModel statusModel);
        Task<string> GetUserNameById(int userId);
    }
}