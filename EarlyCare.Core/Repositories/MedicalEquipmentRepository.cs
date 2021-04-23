using Dapper;
using EarlyCare.Core.Interfaces;
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
    public class MedicalEquipmentRepository :ConnectionRepository, IMedicalEquipmentRepository
    {
        public MedicalEquipmentRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<MedicalEquipmentResponse>> GetMedicalEquipments(int cityId)
        {
            var query = @"select me.Id, me.Name,me.Address,me.Area,me.PhoneNumber,
                            me.UpdatedOn,  u.FullName as UpdatedBy, et.Name as EquipmentType,  st.Name as SellerType,
                            stt.Name as ServiceType, ect.Name as EquipmentConditionType,
                            c.Name as City from MedicalEquipment me
                            join EquipmentTypes et on et.id = me.EquipmentType
							join SellerTypes st on st.id = me.SellerType
						    join ServiceTypes stt on stt.id = me.ServiceType
                            join EquipmentConditionTypes ect on ect.id = me.EquipmentConditionType
                            join User u on u.id = me.UpdatedBy
                            join Cities c on c.id = me.CityId
                            where me.CityId = @cityId and me.IsVerified = 1";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<MedicalEquipmentResponse>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }
    }
}
