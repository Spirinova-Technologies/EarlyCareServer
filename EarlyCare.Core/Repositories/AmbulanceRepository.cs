using Dapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Repositories
{
    public class AmbulanceRepository : ConnectionRepository, IAmbulanceRepository
    {
        public AmbulanceRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<AmbulanceResponseModel>> GetAmbulances(int cityId)
        {
            var query = @"select a.Id, a.Name,a.Address,a.Area,a.PhoneNumber,
                            a.UpdatedOn,  u.FullName as UpdatedBy, at.Name as AmbulanceType,  c.Name as City from Ambulance a
                            join AmbulanceTypes at on at.id = a.Type
                            join User u on u.id = a.UpdatedBy
                            join Cities c on c.id = a.CityId
                            where a.CityId = @cityId and a.IsVerified = 1";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<AmbulanceResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }
    }
}
