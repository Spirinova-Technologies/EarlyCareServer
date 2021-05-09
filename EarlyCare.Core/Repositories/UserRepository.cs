using Dapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.Core.Repositories
{
    public class UserRepository : ConnectionRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            var query = @"SELECT * FROM User WHERE MobileNumber=@phoneNumber";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<User>(query, new { phoneNumber });

                return result.FirstOrDefault();
            }
        }

        public async Task<User> GetUserById(int userId)
        {
            var query = @"SELECT * FROM User WHERE Id=@userId";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<User>(query, new { userId });

                return result.FirstOrDefault();
            }
        }

        public async Task<List<User>> GetVolunteers()
        {
            var query = @"SELECT * FROM User where  UserType = 2";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<User>(query);

                return result.ToList();
            }
        }

        public async Task<User> GetVolunteer(int volunteerId)
        {
            var query = @"SELECT * FROM User where id = @volunteerId";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<User>(query, new { volunteerId });

                return result.FirstOrDefault();
            }
        }

        public async Task<User> InsertUser(User user)
        {
            var query = @"Insert into User (FullName, Email, Password, MobileNumber,  CityId, IsVerified, ProfilePhoto, UserType, socialId, IsActive,  created, modified)
                          Values(@fullName, @email, @password, @mobile, @cityId,  @isVerify, @profilePhoto, @userType, @socialId,
                             @isActive, @created, @modified);

                        SELECT * FROM User where id =(select LAST_INSERT_ID());";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<User>(query, new
                {
                    fullName = user.FullName,
                    email = user.Email.ToLower(),
                    password = user.Password,
                    mobile = user.MobileNumber,
                    cityId = user.CityId,
                    isVerify = user.IsVerified,
                    userType = user.UserType,
                    profilePhoto = user.ProfilePhoto,
                    socialId = "",
                    isActive = 1,
                    created = DateTime.Now,
                    modified = DateTime.Now
                });

                return result.FirstOrDefault();
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            var query = @"Update User set FullName =@name, UserType = @userType
                             where Id = @id;

                           Select * FROM User where id =@id";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<User>(query, new
                {
                    id = user.Id,
                    name = user.FullName,
                    userType = user.UserType,
                });

                return result.FirstOrDefault();
            }
        }

        public async Task<bool> IsEmailIdExists(string emailId)
        {
            var query = @"SELECT * FROM User WHERE Email=@emailId";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<User>(query, new { emailId });

                return result.FirstOrDefault() != null;
            }
        }

        public async Task InsertUserServiceData(UserServiceData userServiceData)
        {
            var query = @"Insert into UserServiceMapping (UserId, ServiceId)
                          Values(@userId, @serviceId)";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                await connection.QueryAsync(query, new { userId = userServiceData.UserId, serviceId = userServiceData.ServiceId });
            }
        }

        public async Task<List<Service>> GetUsersServices(int userId)
        {
            var query = @"SELECT s.* FROM UserServiceMapping usc 
                            join Services s on s.Id = usc.ServiceId
                            join User u on u.Id = usc.UserId
                            where u.Id = @userId order by id ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Service>(query, new { userId });

                return result.ToList();
            }
        }

        public async Task DeleteUserServiceMapping(int userId)
        {
            var query = @"delete from UserServiceMapping
                            where UserId = @userId ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync(query, new { userId });

            }
        }
    }
}