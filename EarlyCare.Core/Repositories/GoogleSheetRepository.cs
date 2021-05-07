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
    public class GoogleSheetRepository : ConnectionRepository, IGoogleSheetRepository
    {
        public GoogleSheetRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<GoogleSheet> GetGoogleSheetByName(string name)
        {
            var query = @"select * from GoogleSheets where name = @name";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<GoogleSheet>(query, new { name });

                return result.FirstOrDefault();
            }
        }

        public async Task<List<GoogleSheet>> GetGoogleSheets()
        {
            var query = @"select * from GoogleSheets";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<GoogleSheet>(query);

                return result.ToList();
            }
        }
    }
}
