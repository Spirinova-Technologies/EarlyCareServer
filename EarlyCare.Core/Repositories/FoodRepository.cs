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
    public class FoodRepository : ConnectionRepository, IFoodRepository
    {
        public FoodRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<FoodResponseModel>> GetFoods(int cityId)
        {
            var query = @"select f.Id, f.Name,f.Address,f.Area,f.PhoneNumber,f.MealTypes,f.Charges,
                            f.UpdatedOn,  u.FullName as UpdatedBy, 
                            dt.Name as DeliveryType, ft.Name as FoodProviderType,
                            c.Name as City from Food f
                            join FoodProviderTypes ft on ft.id = f.FoodProviderType
							join DeliveryTypes dt on dt.id = f.DeliveryType
						    join User u on u.id = f.UpdatedBy
                            join Cities c on c.id = f.CityId
                            where f.CityId = @cityId and f.IsVerified = 1";

            using (IDbConnection connection = await OpenConnectionAsync())
            {
                var result = await connection.QueryAsync<FoodResponseModel>(query, new
                {
                    cityId
                });

                return result.ToList();
            }
        }
    }
}
