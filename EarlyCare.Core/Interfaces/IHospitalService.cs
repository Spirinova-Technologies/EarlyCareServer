using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Interfaces
{
    public interface IHospitalService
    {
        Task<BedCountDetails> GetBedCounts(int cityId);
        Task<List<HospitalResponseModel>> SearchHospitals(HospitalFilterModel hospitalFilters);
        Task<List<City>> GetCities();
    }
}
