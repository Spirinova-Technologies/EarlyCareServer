using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IHospitalRepository
    {
        Task<BedCountDetails> GetBedCounts(int cityId);

        Task<List<Hospital>> SearchHospitals(HospitalFilterModel hospitalFilters);

        Task<List<City>> GetCities();

        Task<Hospital> GetHospitalByName(string name);

        Task<Hospital> InsertHospital(Hospital hospital);

        Task<Hospital> UpdateHospital(Hospital hospital);
    }
}