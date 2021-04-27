using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IOxygenProviderRepository
    {
        Task<List<OxygenProviderResponseModel>> GetOxygenProviders(int cityId);
        Task<OxygenProvider> InsertOxygenProvider(OxygenProvider  oxygenProvider);
        Task<OxygenProvider> UpdateOxygenProvider(OxygenProvider oxygenProvider);
        Task<OxygenProvider> GetOxygenProviderByName(string name);
    }
}