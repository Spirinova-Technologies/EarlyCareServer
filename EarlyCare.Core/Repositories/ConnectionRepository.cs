using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace EarlyCare.Core.Repositories
{
    public class ConnectionRepository
    {
        private readonly IConfiguration configuration;

        public ConnectionRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IDbConnection> OpenConnectionAsync()
        {
            var dbConnection = new MySqlConnection(configuration["ConnectionStrings:DefaultConnection"]);

            await dbConnection.OpenAsync();


            return dbConnection;
        }

    }
}