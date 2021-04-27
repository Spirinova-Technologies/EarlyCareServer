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
    public class PlasmaRepository : ConnectionRepository, IPlasmaRepository
    {
        public PlasmaRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<PlasmaResponseModel>> GetPlasmas(int cityId)
        {
            var query = @"select p.Id, p.Name,p.Address,p.BloodGroup,p.PhoneNumber,p.IsRtpcrReportAvailable,p.IsAntibodyReportAvailable, p.BloodGroup,
                          p.CovidPositiveDate, p.CovidNegativeDate, p.UpdatedOn, u.FullName as UpdatedBy, p.DonorType,  c.Name as City from PlasmaDonor p
                          join User u on u.id = p.UpdatedBy
                          join Cities c on c.id = p.CityId
                          where p.CityId = @cityId and p.IsVerified = 1";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<PlasmaResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }

        public async Task<Plasma> GetPlasmaDonorByName(string name)
        {
            var query = @"SELECT * from PlasmaDonor where TRIM(Name) = @name ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Plasma>(query, new { name = name.Trim() });

                return result.FirstOrDefault();
            }
        }

        public async Task<Plasma> InsertPlasma(Plasma plasma)
        {
            try
            {
                var query = @"INSERT into PlasmaDonor (Name, Address, BloodGroup, PhoneNumber, IsRtpcrReportAvailable, IsAntibodyReportAvailable, CovidPositiveDate,
                            CovidNegativeDate, IsVerified, CreatedOn, UpdatedOn, CreatedBy, UpdatedBy, DonorType, CityId )
                          Values (@name, @address, @bloodGroup, @phoneNumber, @isRtpcrReportAvailable, @isAntibodyReportAvailable, @covidPositiveDate,
                            @covidNegativeDate, @isVerified, @createdOn,@updatedOn,  @createdBy, @updatedBy,  @donorType, @cityId)";

                using (IDbConnection connection = await OpenConnectionAsync())
                {
                    var result = await connection.QueryAsync<Plasma>(query, new
                    {
                        name = plasma.Name,
                        address = plasma.Address,
                        bloodGroup = plasma.BloodGroup,
                        phoneNumber = plasma.PhoneNumber,
                        isRtpcrReportAvailable = plasma.IsRtpcrReportAvailable,
                        isAntibodyReportAvailable = plasma.IsAntibodyReportAvailable,
                        covidPositiveDate = plasma.CovidPositiveDate,
                        covidNegativeDate = plasma.CovidNegativeDate,
                        isVerified = plasma.IsVerified,
                        createdOn = DateTime.Now,
                        updatedOn = DateTime.Now,
                        createdBy = 0,
                        updatedBy = 0,
                        donorType = plasma.DonorType,
                        cityId = plasma.CityId
                    });

                    return result.FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<Plasma> UpdatePlasma(Plasma plasma)
        {
            try
            {
                var query = @"UPDATE PlasmaDonor set Name = @name, Address = @address, BloodGroup =@bloodGroup, PhoneNumber =@phoneNumber, IsRtpcrReportAvailable = @isRtpcrReportAvailable,
                            IsAntibodyReportAvailable = @isAntibodyReportAvailable,CovidPositiveDate = @covidPositiveDate,
                            CovidNegativeDate =@covidNegativeDate,IsVerified = @isVerified, UpdatedOn =@updatedOn,  UpdatedBy= @updatedBy,  DonorType = @donorType,CityId = @cityId
                         Where Id= @id";

                using (IDbConnection connection = await OpenConnectionAsync())
                {
                    var result = await connection.QueryAsync<Plasma>(query, new
                    {
                        id = plasma.Id,
                        name = plasma.Name,
                        address = plasma.Address,
                        bloodGroup = plasma.BloodGroup,
                        phoneNumber = plasma.PhoneNumber,
                        isRtpcrReportAvailable = plasma.IsRtpcrReportAvailable,
                        isAntibodyReportAvailable = plasma.IsAntibodyReportAvailable,
                        covidPositiveDate = plasma.CovidPositiveDate,
                        covidNegativeDate = plasma.CovidNegativeDate,
                        isVerified = plasma.IsVerified,
                        updatedOn = DateTime.Now,
                        updatedBy = 0,
                        donorType = plasma.DonorType,
                        cityId = plasma.CityId
                    });

                    return result.FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
