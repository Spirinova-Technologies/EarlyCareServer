using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Repositories
{
    public class OtpRepository : ConnectionRepository, IOtpRepository
    {
        public OtpRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<OtpDetails> InsertOtpDetails(OtpDetails otpDetails)
        {
            var query = @"INSERT INTO  MobileOtps(MobileNumber,Otp,IsVerified,CreatedAt,ModifiedAt) 
                        VALUES(@mobile, @otp, @verify, @created, @modified)";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<OtpDetails>(query, new
                {
                    mobile = otpDetails.MobileNumber,
                    otp = otpDetails.Otp,
                    verify = otpDetails.IsVerified,
                    created = DateTime.Now,
                    modified = DateTime.Now
                });

                return result.FirstOrDefault();
            }

        }

        public async Task<OtpDetails> GetOtpDetailsAsync(string mobileNumber, string otp)
        {
            var query = "SELECT * FROM MobileOtps WHERE MobileNumber=@mobileNumber AND Otp = @otp AND IsVerified = 0 order by id desc";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<OtpDetails>(query, new
                {
                    mobileNumber,
                    otp
                });

                return result.FirstOrDefault();
            }
        }

        public async Task UpdateOtpDetailsAsync(string phoneNumber)
        {
            var query = "UPDATE MobileOtps set IsVerified=1 WHERE MobileNumber=@phoneNumber";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<OtpDetails>(query, new
                {
                    phoneNumber
                });
            }
        }
    }
}
