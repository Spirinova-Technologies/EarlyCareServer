using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using Dapper;
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
            var query = @"SELECT * FROM users WHERE mobile=@phoneNumber";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<User>(query, new { phoneNumber });

                return result.FirstOrDefault();
            }
        }

        public async Task<List<User>> GetVolunteers()
        {
            var query = @"SELECT * FROM User where UserType = 1 OR  UserType = 2";

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
            var query = @"Insert into users (fullName, email, password, mobile, otp, isVerify, profilePhoto, loginType, accessToken, socialId, role, address, isActive, created, modified)
                          Values(@fullName, @email, @password, @mobile, @otp, @isVerify, @profilePhoto, @loginType, @accessToken, @socialId, @role, @address,
                             @isActive, @created, @modified)";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<User>(query, new
                {
                    fullName = user.FullName,
                    email = user.Email,
                    password = user.Password,
                    mobile = user.Mobile,
                    otp = user.Otp,
                    isVerify = user.IsVerified,
                    profilePhoto = user.ProfilePhoto,
                    loginType = 1,
                    accessToken = user.AccessToken,
                    socialId = user.SocialId,
                    role = user.Role,
                    address = user.Address,
                    isActive = 1,
                    created = DateTime.Now,
                    modified = DateTime.Now
                });

                return result.FirstOrDefault();
            }
        }
    }
}