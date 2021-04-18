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
    public class GlobalSettingsRepository : ConnectionRepository, IGlobalSettingsRepository
    {
        public GlobalSettingsRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<GlobalSetting>> GetGlobalSetting()
        {
            var query = @"SELECT * FROM GlobalSettings";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<GlobalSetting>(query);

                return result.ToList();
            }
        }
    }
}
