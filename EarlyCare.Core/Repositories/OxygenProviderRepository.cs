using Dapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.Core.Repositories
{
    public class OxygenProviderRepository : ConnectionRepository, IOxygenProviderRepository
    {
        public OxygenProviderRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task UpdateVerificationStatus(UpdateVerificationStatusModel statusModel)
        {
            var query = @"Update OxygenProvider set IsVerified = @IsVerified,  UpdatedBy= @updatedBy,
                           UpdatedOn =@updatedOn where id = @id ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                await connection.QueryAsync(query, new
                {
                    id = statusModel.Id,
                    isVerified = statusModel.MarkVerified,
                    updatedBy = statusModel.UserId,
                    updatedOn = DateTime.Now
                });
            }
        }

        public async Task DeleteSyncedOxygenProviderDetails()
        {
            var query = @"Delete from OxygenProvider where IsSynced = true";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                await connection.QueryAsync(query);
            }
        }

        public async Task<OxygenProvider> GetOxygenProviderByName(string name)
        {
            var query = @"SELECT * from OxygenProvider where TRIM(Name) = @name ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<OxygenProvider>(query, new { name = name.Trim() });

                return result.FirstOrDefault();
            }
        }

        public async Task<OxygenProvider> GetOxygenProviderByUserId(int userId)
        {
            var query = @"SELECT * from OxygenProvider where CreatedBy = @userId ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<OxygenProvider>(query, new { userId });

                return result.FirstOrDefault();
            }
        }

        public async Task<List<OxygenProviderResponseModel>> GetOxygenProviders(int cityId, int? userType)
        {
            string whereClause = string.Empty;
            if (!userType.HasValue || userType.Value != 2)
            {
                whereClause = " and o.IsVerified = 1";
            }

            var query = $@"select o.Id, o.Name,o.Area,o.PhoneNumber, o.Charges, o.GovRegistraionNumber,
	                        o.UpdatedOn,  u.FullName as UpdatedBy,  o.Type, o.IsSynced, o.isVerified,
                            c.Name as City from OxygenProvider o
	                        join User u on u.id = o.UpdatedBy
	                        join Cities c on c.id = o.CityId
                            where o.CityId = @cityId {whereClause}";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<OxygenProviderResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }

        public async Task<OxygenProvider> InsertOxygenProvider(OxygenProvider oxygenProvider)
        {
            try
            {
                var query = @"INSERT into OxygenProvider (Name, Area, PhoneNumber, Charges, GovRegistraionNumber, IsVerified, CreatedOn, UpdatedOn,
                             CreatedBy, UpdatedBy, Type, CityId, IsSynced )
                          Values (@name, @area, @phoneNumber, @charges, @govRegistraionNumber, @isVerified, @createdOn,@updatedOn,  @createdBy, @updatedBy,  @type, @cityId, @isSynced);

                            Select * FROM OxygenProvider where id =(select LAST_INSERT_ID());";

                using (IDbConnection connection = await OpenConnectionAsync())
                {
                    var result = await connection.QueryAsync<OxygenProvider>(query, new
                    {
                        name = oxygenProvider.Name,
                        area = oxygenProvider.Area,
                        phoneNumber = oxygenProvider.PhoneNumber,
                        charges = oxygenProvider.Charges,
                        govRegistraionNumber = oxygenProvider.GovRegistraionNumber,
                        isVerified = oxygenProvider.IsVerified,
                        createdOn = DateTime.Now,
                        updatedOn = DateTime.Now,
                        createdBy = oxygenProvider.CreatedBy,
                        updatedBy = oxygenProvider.UpdatedBy,
                        type = oxygenProvider.Type,
                        cityId = oxygenProvider.CityId,
                        isSynced = oxygenProvider.IsSynced
                    });

                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<OxygenProvider> UpdateOxygenProvider(OxygenProvider oxygenProvider)
        {
            try
            {
                var query = @"Update OxygenProvider set Name =@name, Area = @area, PhoneNumber = @phoneNumber, Charges =@charges, GovRegistraionNumber = @govRegistraionNumber,
                                IsVerified = @isVerified, UpdatedOn=@updatedOn,  UpdatedBy= @updatedBy, Type= @type, CityId= @cityId
                             where Id = @id;

                           Select * FROM OxygenProvider where id =@id";

                using (IDbConnection connection = await OpenConnectionAsync())
                {
                    var result = await connection.QueryAsync<OxygenProvider>(query, new
                    {
                        id = oxygenProvider.Id,
                        name = oxygenProvider.Name,
                        area = oxygenProvider.Area,
                        phoneNumber = oxygenProvider.PhoneNumber,
                        charges = oxygenProvider.Charges,
                        govRegistraionNumber = oxygenProvider.GovRegistraionNumber,
                        isVerified = oxygenProvider.IsVerified,
                        updatedOn = DateTime.Now,
                        updatedBy = oxygenProvider.UpdatedOn,
                        type = oxygenProvider.Type,
                        cityId = oxygenProvider.CityId
                    });

                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}