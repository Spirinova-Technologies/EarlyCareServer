using Dapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure;
using EarlyCare.Infrastructure.Constants;
using EarlyCare.Infrastructure.SharedModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.Core.Repositories
{
    public class PlasmaRepository : ConnectionRepository, IPlasmaRepository
    {
        public PlasmaRepository(IConfiguration configuration, IEmailService emailService) : base(configuration)
        {
        }

        public async Task DeleteSyncedPlasmaDonorDetails()
        {
            var query = @"Delete from PlasmaDonor where IsSynced = true";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                await connection.QueryAsync(query);
            }
        }

        public async Task<Plasma> UpdateVerificationStatus(UpdateVerificationStatusModel plasmaStatusModel)
        {
            var query = @"Update PlasmaDonor set IsVerified = @IsVerified,  UpdatedBy= @updatedBy, UpdatedOn =@updatedOn where id = @id ;

                         Select * from PlasmaDonor where Id = @id ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var response = await connection.QueryAsync<Plasma>(query, new
                {
                    id = plasmaStatusModel.Id,
                    isVerified = plasmaStatusModel.MarkVerified,
                    updatedBy = plasmaStatusModel.UserId,
                    updatedOn = Utilities.GetCurrentTime()
                });
                return response.FirstOrDefault();
            }
        }

        public async Task<List<PlasmaResponseModel>> GetPlasmas(int cityId, bool hasApprovePermission)
        {
            string whereClause = string.Empty;
            if (!hasApprovePermission)
            {
                whereClause = " and p.IsVerified = 1";
            }

            var query = $@"select p.Id, p.Name,p.Address,p.BloodGroup,p.PhoneNumber,p.IsRtpcrReportAvailable,p.IsAntibodyReportAvailable, p.BloodGroup,
                          p.CovidPositiveDate, p.CovidNegativeDate, p.UpdatedOn, u.FullName as UpdatedBy, p.DonorType, p.IsSynced,  p.IsVerified, c.Name as City,
                           {hasApprovePermission} as HasApprovePermission from PlasmaDonor p
                          join User u on u.id = p.UpdatedBy
                          join Cities c on c.id = p.CityId
                          where p.CityId = @cityId {whereClause} order by p.UpdatedOn desc";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<PlasmaResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }

        public async Task<Plasma> GetPlasmaByUserId(int userId)
        {
            var query = @"select p.* from PlasmaDonor p
                          join User u on u.id = p.CreatedBy
                          where u.Id = @userId  AND p.IsSynced = false";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Plasma>(query, new
                {
                    userId
                });

                return result.FirstOrDefault();
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
                            CovidNegativeDate, IsVerified, CreatedOn, UpdatedOn, CreatedBy, UpdatedBy, DonorType, CityId , IsSynced)
                          Values (@name, @address, @bloodGroup, @phoneNumber, @isRtpcrReportAvailable, @isAntibodyReportAvailable, @covidPositiveDate,
                            @covidNegativeDate, @isVerified, @createdOn,@updatedOn,  @createdBy, @updatedBy,  @donorType, @cityId, @isSynced);

                            Select * FROM PlasmaDonor where id =(select LAST_INSERT_ID());";

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
                        createdOn = Utilities.GetCurrentTime(),
                        updatedOn = Utilities.GetCurrentTime(),
                        createdBy = plasma.CreatedBy,
                        updatedBy = plasma.UpdatedBy,
                        donorType = plasma.DonorType,
                        cityId = plasma.CityId,
                        isSynced = plasma.IsSynced
                    });

                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
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
                         Where Id= @id;

                          Select * FROM PlasmaDonor where id = @id;";

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
                        updatedOn = Utilities.GetCurrentTime(),
                        updatedBy = plasma.UpdatedBy,
                        donorType = plasma.DonorType,
                        cityId = plasma.CityId
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