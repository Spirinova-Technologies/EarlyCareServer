using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IFoodRepository
    {
        Task<List<FoodResponseModel>> GetFoods(int cityId, int? userType);
        Task<Food> InsertFood(Food plasma);
        Task<Food> UpdateFood(Food plasma);
        Task<Food> GetFoodByName(string name);
        Task<Food> GetFoodByUserId(int userId);
        Task DeleteSyncedFoodDetails();
        Task UpdateVerificationStatus(UpdateVerificationStatusModel statusModel);
    }
}