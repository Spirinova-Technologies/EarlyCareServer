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
    public class OxygenProviderRepository : ConnectionRepository, IOxygenProviderRepository
    {
        public OxygenProviderRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<OxygenProviderResponseModel>> GetOxygenProviders(int cityId)
        {
            var query = @"select o.Id, o.Name,o.Address,o.Area,o.PhoneNumber,
	                        o.UpdatedOn,  u.FullName as UpdatedBy, o.Units, ost.Name as ServiceType, ot.Name as OxygenSupplier,
                            c.Name as City from OxygenProvider o
	                        join OxygenServiceTypes ost on ost.id = o.OxygenServiceType
                            join OxygenSupplierTypes ot on ot.id = o.ProviderType
	                        join User u on u.id = o.UpdatedBy
	                        join Cities c on c.id = o.CityId
                            where o.CityId = @cityId and o.IsVerified = 1";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<OxygenProviderResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }
    }
}
