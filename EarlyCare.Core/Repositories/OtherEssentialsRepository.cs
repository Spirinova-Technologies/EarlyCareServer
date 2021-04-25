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
    public class OtherEssentialsRepository : ConnectionRepository, IOtherEssentialsRepository
    {
        public OtherEssentialsRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<OtherEssentialsResponseModel>> GetOtherEssentials(int cityId)
        {
            var query = @"select *, u.FullName as UpdatedBy from OtherEssentials o
                            join User u on u.id = o.UpdatedBy
                            where o.CityId = @cityId ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<OtherEssentialsResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }
    }
}