using EarlyCare.Core.Interfaces;
using EarlyCare.Infrastructure;
using Jose;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyCare.Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(int userId)
        {
            var userToken = new UserToken
            {
                UserId = userId,
                Expiration = Utilities.GetCurrentTime().AddDays(365),
                IssuedAt = Utilities.GetCurrentTime()
            };

            var payload = UserToken.ConvertToJwtToken(userToken);
            var token = JWT.Encode(payload, Encoding.UTF8.GetBytes(configuration["Jwt:Key"]), JwsAlgorithm.HS512);

            return token;
        }

        public bool IsTokenValid(string authToken, out UserToken decodedToken)
        {
            try
            {
                var jwtToken = JWT.Decode<JwtToken>(authToken, Encoding.UTF8.GetBytes(configuration["Jwt:Key"]), JwsAlgorithm.HS512);
                decodedToken = UserToken.ConvertFromJwtToken(jwtToken);

                var isExpired = Utilities.GetCurrentTime() > decodedToken.Expiration;

                return !isExpired;
            }
            catch (Exception)
            {
                decodedToken = null;
                return false;
            }
        }
    }
}
