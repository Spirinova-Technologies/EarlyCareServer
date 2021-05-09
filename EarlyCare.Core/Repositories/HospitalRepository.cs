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
    public class HospitalRepository : ConnectionRepository, IHospitalRepository
    {
        public HospitalRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task DeleteSyncedHospitalsDetails()
        {
            var query = @"Delete from Hospitals where IsSynced = true";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                await connection.QueryAsync(query);
            }
        }

        public async Task<BedCountDetails> GetBedCounts(int cityId)
        {
            var query = @"SELECT SUM(VacantIsolation) AS VacantIsolationBeds , SUM(VacantWithOxygen) AS VacantWithOxygenBeds
                            , SUM(VacantWithICU) AS VacantWithICUBeds, SUM(VacantWithICUVentilator) AS VacantWithICUVentilatorBeds,
                            SUM(TotalIsolation) AS TotalIsolationBeds, SUM(TotalWithOxygen) AS TotalWithOxygenBeds,
                            SUM(TotalWithICU) AS TotalWithICUBeds, SUM(TotalWithICUVentilator) AS TotalWithICUVentilatorBeds FROM Hospitals
                         where CityId = @cityId";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<BedCountDetails>(query, new
                {
                    cityId
                });

                return result.FirstOrDefault();
            }
        }

        public async Task<List<City>> GetCities()
        {
            var query = @"SELECT * from Cities where IsActive = 1 ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<City>(query);

                return result.ToList();
            }
        }

        public async Task<Hospital> GetHospitalByName(string name)
        {
            var query = @"SELECT * from Hospitals where TRIM(Name) = @name ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Hospital>(query, new { name = name.Trim() });

                return result.FirstOrDefault();
            }
        }

        public async Task<Hospital> InsertHospital(Hospital hospital)
        {
            var query = @"INSERT INTO  Hospitals(Name,Address,Latitude,Longitude,CityId,PhoneNumber1,PhoneNumber2,
                          PhoneNumber3,PhoneNumber4, CreatedAt, ModifiedAt, CreatedBy, UpdatedBy, HospitalType, IsAvailableIsolationBed,
                          IsAvailableICUVentilator, IsAvailableICU, IsAvailableOxygen, IsSynced )
                        VALUES(@name,@address,@latitude,@longitude,@cityId,@phoneNumber1,@phoneNumber2,
                          @phoneNumber3,@phoneNumber4, @createdAt, @modifiedAt, @createdBy, @updatedBy, @hospitalType, @isAvailableIsolationBed,
                          @isAvailableICUVentilator, @isAvailableICU, @isAvailableOxygen, @isSynced)";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Hospital>(query, new
                {
                    name = hospital.Name,
                    address = hospital.Address,
                    latitude = hospital.Latitude,
                    longitude = hospital.Longitude,
                    cityId = hospital.CityId,
                    phoneNumber1 = hospital.PhoneNumber1,
                    phoneNumber2 = hospital.PhoneNumber2,
                    phoneNumber3 = hospital.PhoneNumber3,
                    phoneNumber4 = hospital.PhoneNumber4,
                    createdAt = DateTime.Now,
                    modifiedAt = hospital.ModifiedAt,
                    createdBy = hospital.CreatedBy,
                    updatedBy = hospital.UpdatedBy,
                    hospitalType = hospital.HospitalType,
                    isAvailableIsolationBed = hospital.IsAvailableIsolationBed,
                    isAvailableICUVentilator = hospital.IsAvailableICUVentilator,
                    isAvailableICU = hospital.IsAvailableICU,
                    isAvailableOxygen = hospital.IsAvailableOxygen,
                    isSynced = hospital.IsSynced
                });

                return result.FirstOrDefault();
            }
        }

        public async Task<Hospital> UpdateHospital(Hospital hospital)
        {
            var query = @"update Hospitals set Name = @name ,Address = @address,Latitude =@latitude ,Longitude=@longitude,CityId =@cityId,PhoneNumber1=@phoneNumber1,
                          PhoneNumber2 =@phoneNumber2 , PhoneNumber3 = @phoneNumber3,PhoneNumber4 = @phoneNumber4,  ModifiedAt=@modifiedAt,
                          CreatedBy = @createdBy, UpdatedBy =@updatedBy, HospitalType= @hospitalType, IsAvailableIsolationBed = @isAvailableIsolationBed,
                          IsAvailableICUVentilator=@isAvailableICUVentilator, IsAvailableICU=@isAvailableICU, IsAvailableOxygen= @isAvailableOxygen
                         where Id = @id";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Hospital>(query, new
                {
                    id = hospital.Id,
                    name = hospital.Name,
                    address = hospital.Address,
                    latitude = hospital.Latitude,
                    longitude = hospital.Longitude,
                    cityId = hospital.CityId,
                    phoneNumber1 = hospital.PhoneNumber1,
                    phoneNumber2 = hospital.PhoneNumber2,
                    phoneNumber3 = hospital.PhoneNumber3,
                    phoneNumber4 = hospital.PhoneNumber4,
                    modifiedAt = hospital.ModifiedAt,
                    createdBy = hospital.CreatedBy,
                    updatedBy = hospital.UpdatedBy,
                    hospitalType = hospital.HospitalType,
                    isAvailableIsolationBed = hospital.IsAvailableIsolationBed,
                    isAvailableICUVentilator = hospital.IsAvailableICUVentilator,
                    isAvailableICU = hospital.IsAvailableICU,
                    isAvailableOxygen = hospital.IsAvailableOxygen,
                });

                return result.FirstOrDefault();
            }
        }

        public async Task<List<HospitalResponseModel>> SearchHospitals(HospitalFilterModel hospitalFilters)
        {
            string whereCluase = string.Empty;

            if (hospitalFilters.BedType.Count == 0)
            {
                whereCluase = " AND 1 = 0";
            }
            else if (!hospitalFilters.BedType.Contains(0))
            {
                foreach (var bedType in hospitalFilters.BedType)
                {
                    switch (bedType)
                    {
                        case 1:
                            whereCluase += string.IsNullOrWhiteSpace(whereCluase) ? " AND (h.IsAvailableIsolationBed = 1"
                                : " OR  h.IsAvailableIsolationBed = 1";
                            break;

                        case 2:
                            whereCluase += string.IsNullOrWhiteSpace(whereCluase) ? " AND (h.IsAvailableICU = 1"
                                : " OR  h.IsAvailableICU = 1";
                            break;

                        case 3:
                            whereCluase += string.IsNullOrWhiteSpace(whereCluase) ? " AND (h.IsAvailableOxygen = 1"
                                : " OR  h.IsAvailableOxygen = 1";
                            break;

                        case 4:
                            whereCluase += string.IsNullOrWhiteSpace(whereCluase) ? " AND (h.IsAvailableICUVentilator = 1"
                                : " OR  h.IsAvailableICUVentilator = 1";
                            break;

                        default:
                            break;
                    }
                }

                if (whereCluase.Contains("("))
                {
                    whereCluase += " )";
                }
            }

            var query = $@"select h.* , c.Name as City  from Hospitals h
                          join Cities c on c.id = h.CityId  where CityId = @cityId {whereCluase}";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<HospitalResponseModel>(query, new
                {
                    cityId = hospitalFilters.CityId
                });

                return result.ToList();
            }
        }
    }
}