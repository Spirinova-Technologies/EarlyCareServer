using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IAmbulanceRepository
    {
        Task<List<AmbulanceResponseModel>> GetAmbulances(int cityId);
    }
}