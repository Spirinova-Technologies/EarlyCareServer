using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IAmbulanceRepository
    {
        Task<List<AmbulanceResponseModel>> GetAmbulances(int cityId);
        Task<Ambulance> InsertAmbulance(Ambulance ambulance);
        Task<Ambulance> UpdateAmbulance(Ambulance ambulance);
        Task<Ambulance> GetAmbulanceDetails(string name, string phoneNumber);
    }
}