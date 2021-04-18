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
            var query = @"INSERT INTO  mobileotps(type,mobile,otp,verify,created,modified) 
                        VALUES(@type, @mobile, @otp, @verify, @created, @modified)";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<OtpDetails>(query, new
                {
                    type = otpDetails.Type,
                    mobile = otpDetails.Mobile,
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
            var query = "SELECT * FROM mobileotps WHERE type=1 AND mobile=@mobileNumber AND otp = @otp AND verify = 0 order by id desc";

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
            var query = "UPDATE mobileotps set verify=1 WHERE mobile=@phoneNumber";

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
