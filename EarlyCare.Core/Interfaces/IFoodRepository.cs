using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IFoodRepository
    {
        Task<List<FoodResponseModel>> GetFoods(int cityId);
    }
}