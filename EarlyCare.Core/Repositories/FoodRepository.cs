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
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Repositories
{
    public class FoodRepository : ConnectionRepository, IFoodRepository
    {
        public FoodRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Food> UpdateVerificationStatus(UpdateVerificationStatusModel statusModel)
        {
            var query = @"Update Food set IsVerified = @isVerified,  UpdatedBy= @updatedBy, UpdatedOn =@updatedOn where Id = @id;
                          Select * from Food where Id = @id";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var response = await connection.QueryAsync<Food>(query, new
                {
                    id = statusModel.Id,
                    isVerified = statusModel.MarkVerified,
                    updatedBy = statusModel.UserId,
                    updatedOn = Utilities.GetCurrentTime()
                });

                return response.FirstOrDefault();
            }
        }

        public async Task DeleteSyncedFoodDetails()
        {
            var query = @"Delete from Food where IsSynced = true";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                await connection.QueryAsync(query);
            }
        }

        public async Task<Food> GetFoodByName(string name)
        {
            var query = @"SELECT * from Food where TRIM(Name) = @name ";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Food>(query, new { name = name.Trim() });

                return result.FirstOrDefault();
            }
        }

        public async Task<Food> GetFoodByUserId(int userId)
        {
            var query = @"SELECT * from Food where CreatedBy = @userId AND IsSynced = false";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<Food>(query, new { userId });

                return result.FirstOrDefault();
            }
        }

        public async Task<List<FoodResponseModel>> GetFoods(int cityId, bool hasApprovePermission)
        {
            string whereClause = string.Empty;
            if (!hasApprovePermission)
            {
                whereClause = " and f.IsVerified = 1";
            }

            var query = $@"select f.Id, f.Name,f.Address,f.Area,f.PhoneNumber,f.RegistrationNumber,f.Charges, f.Delivery, f.FoodServed, f.Type, f.IsVerified, f.IsSynced,
                            f.UpdatedOn,  u.FullName as UpdatedBy, {hasApprovePermission} as HasApprovePermission,
                            c.Name as City from Food f
						    join User u on u.id = f.UpdatedBy
                            join Cities c on c.id = f.CityId
                            where f.CityId = @cityId {whereClause}  order by f.UpdatedOn desc";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<FoodResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }

        public async Task<Food> InsertFood(Food food)
        {
            try
            {
                var query = @"INSERT into Food (Name, Address, Area, PhoneNumber, Charges, RegistrationNumber, Delivery, FoodServed,  IsVerified, CreatedOn, UpdatedOn,
                             CreatedBy, UpdatedBy, Type, CityId, IsSynced )
                          Values (@name,@address, @area, @phoneNumber, @charges, @registrationNumber, @delivery, @foodServed, @isVerified, @createdOn,@updatedOn, 
                             @createdBy, @updatedBy,  @type, @cityId, @isSynced);

                          Select * FROM Food where id =(select LAST_INSERT_ID());";

                using (IDbConnection connection = await OpenConnectionAsync())
                {
                    var result = await connection.QueryAsync<Food>(query, new
                    {
                        name = food.Name,
                        address = food.Address,
                        area = food.Area,
                        phoneNumber = food.PhoneNumber,
                        charges = food.Charges,
                        registrationNumber = food.RegistrationNumber,
                        delivery = food.Delivery,
                        foodServed = food.FoodServed,
                        isVerified = food.IsVerified,
                        createdOn = Utilities.GetCurrentTime(),
                        updatedOn = Utilities.GetCurrentTime(),
                        createdBy = food.CreatedBy,
                        updatedBy = food.UpdatedBy,
                        type = food.Type,
                        cityId = food.CityId,
                        isSynced = food.IsSynced
                    });

                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Food> UpdateFood(Food food)
        {
            try
            {
                var query = @"Update  Food set Name = @name,Address = @address, Area =@area, PhoneNumber =@phoneNumber,Charges = @charges,RegistrationNumber = @registrationNumber,
                                Delivery = @delivery, FoodServed =@foodServed, IsVerified =@isVerified, UpdatedOn =@updatedOn,   UpdatedBy =@updatedBy, 
                                Type = @type, CityId = @cityId 
                            where Id = @id;
                            Select * FROM Food where id =@id";

                using (IDbConnection connection = await OpenConnectionAsync())
                {
                    var result = await connection.QueryAsync<Food>(query, new
                    {
                        id = food.Id,
                        name = food.Name,
                        address = food.Address,
                        area = food.Area,
                        phoneNumber = food.PhoneNumber,
                        charges = food.Charges,
                        registrationNumber = food.RegistrationNumber,
                        delivery = food.Delivery,
                        foodServed = food.FoodServed,
                        isVerified = food.IsVerified,
                        updatedOn = Utilities.GetCurrentTime(),
                        updatedBy = food.UpdatedBy,
                        type = food.Type,
                        cityId = food.CityId
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
