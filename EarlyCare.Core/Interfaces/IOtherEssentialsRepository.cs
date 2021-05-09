using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IOtherEssentialsRepository
    {
        Task<List<OtherEssentialsResponseModel>> GetOtherEssentials(int cityId);
        Task DeleteSyncedOtherEssentialsDetails();
    }
}