using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCare.Core.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly ILogger<HospitalService> _logger;
        private readonly IHospitalRepository _hospitalRepository;

        public HospitalService(ILogger<HospitalService> logger, IHospitalRepository hospitalRepository)
        {
            _logger = logger;
            _hospitalRepository = hospitalRepository;
        }
        public async Task<BedCountDetails> GetBedCounts(int cityId)
        {
            return await _hospitalRepository.GetBedCounts(cityId);
        }

        public async Task<List<City>> GetCities()
        {
            _logger.LogInformation("Retrieving cities");
            return await _hospitalRepository.GetCities();
        }

        public async Task<List<HospitalResponseModel>> SearchHospitals(HospitalFilterModel hospitalFilters)
        {
            return await _hospitalRepository.SearchHospitals(hospitalFilters);
        }
    }
}
