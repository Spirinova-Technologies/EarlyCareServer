using Dapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Repositories
{
    public class TiffinRepository : ConnectionRepository, ITiffinRepository
    {
        public TiffinRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<Tiffin>> GetTiffins(int cityId)
        {
            var query = @"select * from Tiffin
                         where CityId = @cityId";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Tiffin>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }
    }
}
