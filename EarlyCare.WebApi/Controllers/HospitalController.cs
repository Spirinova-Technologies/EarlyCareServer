using AutoMapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Infrastructure.Constants;
using EarlyCare.Infrastructure.SharedModels;
using EarlyCare.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/hospital")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HospitalController> _logger;
        private readonly IHospitalService _hospitalService;

        public HospitalController(IMapper mapper, ILogger<HospitalController> logger, IHospitalService hospitalService)
        {
            _mapper = mapper;
            _logger = logger;
            _hospitalService = hospitalService;
        }

        [HttpGet("getBedCounts")]
        public async Task<IActionResult> GetBedCounts([Required] int cityId)
        {
            var response = await _hospitalService.GetBedCounts(cityId);

            //convert response to appropriate format
            List<string> bedTypes = new List<string>() {
              Constants.IsolationBeds,
              Constants.ICUBeds,
              Constants.OxigenBed,
              Constants.VentilatorBeds
            };

            List<BedDetailsModel> bedDetails = new List<BedDetailsModel>();
            foreach (var bedType in bedTypes)
            {
                if (bedType == Constants.IsolationBeds)
                {
                    bedDetails.Add(new BedDetailsModel
                    {
                        BedType = bedType,
                        TotalBeds = response.TotalIsolationBeds,
                        VacantBeds = response.VacantIsolationBeds,
                        OccupiedBeds = response.TotalIsolationBeds - response.VacantIsolationBeds
                    }); ;
                    continue;
                }
                else if (bedType == Constants.ICUBeds)
                {
                    bedDetails.Add(new BedDetailsModel
                    {
                        BedType = bedType,
                        TotalBeds = response.TotalWithICUBeds,
                        VacantBeds = response.VacantWithICUBeds,
                        OccupiedBeds = response.TotalWithICUBeds - response.VacantWithICUBeds
                    });
                    continue;
                }
                else if (bedType == Constants.OxigenBed)
                {
                    bedDetails.Add(new BedDetailsModel
                    {
                        BedType = bedType,
                        TotalBeds = response.TotalWithOxygenBeds,
                        VacantBeds = response.VacantWithOxygenBeds,
                        OccupiedBeds = response.TotalWithOxygenBeds - response.VacantWithOxygenBeds
                    });
                    continue;
                }
                else if (bedType == Constants.VentilatorBeds)
                {
                    bedDetails.Add(new BedDetailsModel
                    {
                        BedType = bedType,
                        TotalBeds = response.TotalWithICUVentilatorBeds,
                        VacantBeds = response.VacantWithICUVentilatorBeds,
                        OccupiedBeds = response.TotalWithICUVentilatorBeds - response.VacantWithICUVentilatorBeds
                    });
                    continue;
                }
            }

            return Ok(bedDetails);
        }

        [HttpPost("searchHospitals")]
        public async Task<IActionResult> SearchHospitals([FromBody] HospitalFilterModel hospitalFilterModel)
        {
            var response = await _hospitalService.SearchHospitals(hospitalFilterModel);

            return Ok(response);
        }

        [HttpGet("getCities")]
        public async Task<IActionResult> GetCities()
        {
            var response = await _hospitalService.GetCities();

            return Ok(response);
        }
    }
}