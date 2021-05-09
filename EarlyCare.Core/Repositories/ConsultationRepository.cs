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
    public class ConsultationRepository : ConnectionRepository, IConsultationRepository
    {
        public ConsultationRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task DeleteSyncedConsultationDetails()
        {
            var query = @"Delete from Consultation where IsSynced = true";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                await connection.QueryAsync(query);
            }
        }

        public async Task<Consultation> GetConsultationDetails(string name)
        {
            var query = @"SELECT * from Consultation where TRIM(DoctorName) = @name ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Consultation>(query, new { name = name.Trim() });

                return result.FirstOrDefault();
            }
        }

        public async Task<List<ConsultationResponseModel>> GetConsultations(int cityId)
        {
            var query = @"select * from Consultation where CityId = @cityId and IsVerified = 1";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<ConsultationResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }

        public async Task<Consultation> InsertConsultation(Consultation consultation)
        {
            try
            {
                var query = @"INSERT into Consultation (DoctorName, Area, PhoneNumber, Charges, GovRegistraionNumber, Type,  IsVerified, CreatedOn, UpdatedOn,
                             CreatedBy, UpdatedBy,  CityId, IsSynced )
                          Values (@name,@area, @phoneNumber, @charges, @govRegistraionNumber, @type, @isVerified,
                                   @createdOn,@updatedOn,  @createdBy, @updatedBy,  @cityId, @isSynced)";

                using (IDbConnection connection = await OpenConnectionAsync())
                {
                    var result = await connection.QueryAsync<Consultation>(query, new
                    {
                        name = consultation.DoctorName,
                        area = consultation.Area,
                        phoneNumber = consultation.PhoneNumber,
                        charges = consultation.Charges,
                        govRegistraionNumber = consultation.GovRegistraionNumber,
                        type = consultation.Type,
                        isVerified = consultation.IsVerified,
                        createdOn = DateTime.Now,
                        updatedOn = DateTime.Now,
                        createdBy = consultation.CreatedBy,
                        updatedBy = consultation.UpdatedBy,
                        cityId = consultation.CityId,
                        isSynced= consultation.IsSynced
                    });

                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Consultation> UpdateConsultation(Consultation consultation)
        {
            var query = @"Update  Consultation set DoctorName =@name, Area =@area, PhoneNumber =@phoneNumber, Charges =@charges,GovRegistraionNumber = @govRegistraionNumber,
                              Type =@type, IsVerified =@isVerified,UpdatedOn=@updatedOn,  UpdatedBy =@updatedBy, CityId = @cityId 
                         where Id = @id";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Consultation>(query, new
                {
                    id = consultation.Id,
                    name = consultation.DoctorName,
                    area = consultation.Area,
                    phoneNumber = consultation.PhoneNumber,
                    charges = consultation.Charges,
                    govRegistraionNumber = consultation.GovRegistraionNumber,
                    type = consultation.Type,
                    isVerified = consultation.IsVerified,
                    createdOn = DateTime.Now,
                    updatedOn = DateTime.Now,
                    updatedBy = consultation.UpdatedBy,
                    cityId = consultation.CityId
                });

                return result.FirstOrDefault();
            }
        }
    }
}