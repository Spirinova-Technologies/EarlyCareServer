﻿using Dapper;
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
    public class DrugsRepository : ConnectionRepository, IDrugsRepository
    {
        public DrugsRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Drug> GetDrugDetails(string name)
        {
            var query = @"SELECT *,  Name as SupplierName from Drugs where TRIM(name) = @name ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Drug>(query, new { name = name.Trim() });

                return result.FirstOrDefault();
            }
        }

        public async Task<List<DrugsResponseModel>> GetDrugProviders(int cityId)
        {
            var query = @"Select *,  Name as SupplierName from Drugs
                            where CityId = @cityId";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<DrugsResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }

        public async Task<Drug> InsertDrugs(Drug drug)
        {
            try
            {
                var query = @"INSERT into Drugs (Name, Address, Coordinator, PhoneNumber, GovPhoneNumber,  IsVerified, CreatedOn, UpdatedOn,
                                CreatedBy, UpdatedBy,  CityId )
                            Values (@name,@address,@coordinator, @phoneNumber, @govPhoneNumber,  @isVerified, @createdOn,@updatedOn,  @createdBy, @updatedBy,  @cityId)";

                using (IDbConnection connection = await OpenConnectionAsync())
                {
                    var result = await connection.QueryAsync<Drug>(query, new
                    {
                        name = drug.SupplierName,
                        address = drug.Address,
                        phoneNumber = drug.PhoneNumber,
                        coordinator = drug.Coordinator,
                        govPhoneNumber = drug.GovPhoneNumber,
                        isVerified = drug.IsVerified,
                        createdOn = DateTime.Now,
                        updatedOn = DateTime.Now,
                        createdBy = 0,
                        updatedBy = 0,
                        cityId = drug.CityId
                    });

                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Drug> UpdateDrugs(Drug drug)
        {
            try
            {
                var query = @"Update Drugs set Name = @name,Address =@address, Coordinator =@coordinator, PhoneNumber =@phoneNumber, GovPhoneNumber =@govPhoneNumber,  
                               IsVerified = @isVerified, UpdatedOn =@updatedOn, UpdatedBy= @updatedBy,  CityId =@cityId where Id = @id";

                using (IDbConnection connection = await OpenConnectionAsync())
                {
                    var result = await connection.QueryAsync<Drug>(query, new
                    {
                        id = drug.Id,
                        name = drug.SupplierName,
                        address = drug.Address,
                        phoneNumber = drug.PhoneNumber,
                        coordinator = drug.Coordinator,
                        govPhoneNumber = drug.GovPhoneNumber,
                        isVerified = drug.IsVerified,
                        updatedOn = DateTime.Now,
                        createdBy = 0,
                        updatedBy = 0,
                        cityId = drug.CityId
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