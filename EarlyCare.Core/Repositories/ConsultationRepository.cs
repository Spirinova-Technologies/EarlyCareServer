using Dapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Infrastructure.SharedModels;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.Core.Repositories
{
    public class ConsultationRepository : ConnectionRepository, IConsultationRepository
    {
        public ConsultationRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<ConsultationResponseModel>> GetConsultations(int cityId)
        {
            var query = @"select * from Consultation where CityId = @cityId and IsVerified = 1";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<ConsultationResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }
    }
}