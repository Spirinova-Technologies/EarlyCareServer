using Dapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure;
using EarlyCare.Infrastructure.SharedModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.Core.Repositories
{
    public class AmbulanceRepository : ConnectionRepository, IAmbulanceRepository
    {
        public AmbulanceRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Ambulance> GetAmbulanceDetails(string name, string phoneNumber)
        {
            var query = @"SELECT * from Ambulance where TRIM(Name) = @name and Trim(PhoneNumber) = @phoneNumber";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Ambulance>(query, new { name = name.Trim(), phoneNumber = phoneNumber.Trim() });

                return result.FirstOrDefault();
            }
        }

        public async Task DeleteSyncedAmbulanceDetails()
        {
            var query = @"Delete from Ambulance where IsSynced = true";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                await connection.QueryAsync(query);
            }
        }

        public async Task<List<AmbulanceResponseModel>> GetAmbulances(int cityId)
        {
            var query = @"select a.Id, a.Name,a.Address,a.Area,a.PhoneNumber,
                            a.UpdatedOn,  u.FullName as UpdatedBy, a.AmbulanceType , a.ProviderType,  c.Name as City from Ambulance a
                            join User u on u.id = a.UpdatedBy
                            join Cities c on c.id = a.CityId
                            where a.CityId = @cityId and a.IsVerified = 1 order by a.UpdatedOn desc";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<AmbulanceResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }

        public async Task<Ambulance> InsertAmbulance(Ambulance ambulance)
        {
            try
            {
                var query = @"INSERT into Ambulance (Name, Address, Area, PhoneNumber, AmbulanceType, NumberOfAmbulances, ProviderType,  IsVerified, CreatedOn, UpdatedOn,
                             CreatedBy, UpdatedBy,  CityId, IsSynced )
                          Values (@name,@address, @area, @phoneNumber, @ambulanceType, @numberOfAmbulances, @providerType, @isVerified, @createdOn,@updatedOn,  @createdBy, @updatedBy,  @cityId, @isSynced)";

                using (IDbConnection connection = await OpenConnectionAsync())
                {
                    var result = await connection.QueryAsync<Ambulance>(query, new
                    {
                        name = ambulance.Name,
                        address = ambulance.Address,
                        area = ambulance.Area,
                        phoneNumber = ambulance.PhoneNumber,
                        ambulanceType = ambulance.AmbulanceType,
                        numberOfAmbulances = ambulance.NumberOfAmbulances,
                        providerType = ambulance.ProviderType,
                        isVerified = ambulance.IsVerified,
                        createdOn = Utilities.GetCurrentTime(),
                        updatedOn = Utilities.GetCurrentTime(),
                        createdBy = ambulance.CreatedBy,
                        updatedBy = ambulance.UpdatedBy,
                        cityId = ambulance.CityId,
                        isSynced= ambulance.IsSynced
                    });

                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Ambulance> UpdateAmbulance(Ambulance ambulance)
        {
            try
            {
                var query = @"Update Ambulance Set Name = @name, Address =@address,Area = @area, PhoneNumber =@phoneNumber,AmbulanceType = @ambulanceType,
                                NumberOfAmbulances = @numberOfAmbulances,ProviderType = @providerType,IsVerified = @isVerified, UpdatedOn = @updatedOn, UpdatedBy= @updatedBy,
                                CityId= @cityId
                              Where Id= @id";

                using (IDbConnection connection = await OpenConnectionAsync())
                {
                    var result = await connection.QueryAsync<Ambulance>(query, new
                    {
                        id = ambulance.Id,
                        name = ambulance.Name,
                        address = ambulance.Address,
                        area = ambulance.Area,
                        phoneNumber = ambulance.PhoneNumber,
                        ambulanceType = ambulance.AmbulanceType,
                        numberOfAmbulances = ambulance.NumberOfAmbulances,
                        providerType = ambulance.ProviderType,
                        isVerified = ambulance.IsVerified,
                        updatedOn = Utilities.GetCurrentTime(),
                        updatedBy = 0,
                        cityId = ambulance.CityId
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