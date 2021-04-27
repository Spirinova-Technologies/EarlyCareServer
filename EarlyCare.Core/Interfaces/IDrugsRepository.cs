using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IDrugsRepository
    {
        Task<List<DrugsResponseModel>> GetDrugProviders(int cityId);
        Task<Drug> InsertDrugs(Drug  drug);
        Task<Drug> UpdateDrugs(Drug  drug);
        Task<Drug> GetDrugDetails(string name);
    }
}