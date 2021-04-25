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
    public class DrugsRepository : ConnectionRepository, IDrugsRepository
    {
        public DrugsRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<DrugsResponseModel>> GetDrugProviders(int cityId)
        {
            var query = @"Select * from Drugs
                            where CityId = @cityId";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<DrugsResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }
    }
}