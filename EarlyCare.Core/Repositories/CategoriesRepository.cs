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
    public class CategoriesRepository : ConnectionRepository, ICategoriesRepository
    {
        public CategoriesRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<Category>> GetCategories()
        {
            var query = @"SELECT * FROM Categories where IsActive = 1";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Category>(query);

                return result.ToList();
            }
        }

        public async Task<List<Service>> GetServices()
        {
            var query = @"SELECT * FROM Services";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Service>(query);

                return result.ToList();
            }
        }

    }
}
