using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByPhoneNumber(string phoneNumber);

        Task<User> InsertUser(User user);

        Task<List<User>> GetVolunteers(bool showAll);

        Task<User> GetVolunteer(int volunteerId);

        UserResponseModel GenerateUserResponse(User user, List<Service> services);
    }
}