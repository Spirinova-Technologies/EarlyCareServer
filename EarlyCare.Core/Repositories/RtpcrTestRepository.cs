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
    public class RtpcrTestRepository : ConnectionRepository, IRtpcrTestRepository
    {
        public RtpcrTestRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task DeleteSyncedRTPCRTestDetails()
        {
            var query = @"Delete from RTPCRTest where IsSynced = true";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                await connection.QueryAsync(query);
            }
        }

        public async Task<List<RtpcrTest>> GetRtpcrTests(int cityId)
        {
            var query = @"select * from RTPCRTest
                         where CityId = @cityId";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<RtpcrTest>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }
    }
}
