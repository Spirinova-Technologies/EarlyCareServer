using AutoMapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/oxygenProvider")]
    [ApiController]
    public class OxygenProviderController : ControllerBase
    {
        private readonly IOxygenProviderRepository _oxygenProviderRepository;
        private readonly ILogger<OxygenProviderController> _logger;

        public OxygenProviderController(ILogger<OxygenProviderController> logger, IOxygenProviderRepository oxygenProviderRepository)
        {
            _logger = logger;
            _oxygenProviderRepository = oxygenProviderRepository;
        }

        [HttpGet("getOxygenProviders")]
        public async Task<IActionResult> GetOxygenProviders([Required] int cityId)
        {
            var response = await _oxygenProviderRepository.GetOxygenProviders(cityId);

            return Ok(response);
        }

        [HttpGet("getOxygenProviderByUserId")]
        public async Task<IActionResult> GetOxygenProviderByUserId([Required] int userId)
        {
            var response = await _oxygenProviderRepository.GetOxygenProviderByUserId(userId);

            return Ok(response);
        }

        [HttpPost("insertOxygenDetails")]
        public async Task<IActionResult> InsertOxygenDetails([FromBody] OxygenDetailsRequestModel oxygenDetailsRequestModel)
        {
            var oxygenProvider = new OxygenProvider
            {
                Area = oxygenDetailsRequestModel.Area,
                Charges = oxygenDetailsRequestModel.Charges,
                GovRegistraionNumber = oxygenDetailsRequestModel.GovRegistraionNumber,
                Type = oxygenDetailsRequestModel.Type,
                CityId = oxygenDetailsRequestModel.CityId,
                CreatedBy = oxygenDetailsRequestModel.UserId,
                CreatedOn = DateTime.Now,
                IsVerified = false,
                Name = oxygenDetailsRequestModel.Name,
                PhoneNumber = oxygenDetailsRequestModel.PhoneNumber,
                UpdatedBy = oxygenDetailsRequestModel.UserId,
                UpdatedOn = DateTime.Now
            };

            var response = await _oxygenProviderRepository.InsertOxygenProvider(oxygenProvider);

            return Ok(new BaseResponseModel { Message = "Data inserted successfully", Result = response, Status = 1 });
        }

        [HttpPost("updateOxygenDetails")]
        public async Task<IActionResult> UpdateOxygenDetails(OxygenDetailsRequestModel oxygenDetailsRequestModel)
        {
            var oxygenProvider = new OxygenProvider
            {
                Id = oxygenDetailsRequestModel.Id,
                Area = oxygenDetailsRequestModel.Area,
                Charges = oxygenDetailsRequestModel.Charges,
                GovRegistraionNumber = oxygenDetailsRequestModel.GovRegistraionNumber,
                Type = oxygenDetailsRequestModel.Type,
                CityId = oxygenDetailsRequestModel.CityId,
                IsVerified = false,
                Name = oxygenDetailsRequestModel.Name,
                PhoneNumber = oxygenDetailsRequestModel.PhoneNumber,
                UpdatedBy = oxygenDetailsRequestModel.UserId,
                UpdatedOn = DateTime.Now
            };

            var response = await _oxygenProviderRepository.UpdateOxygenProvider(oxygenProvider);

            return Ok(new BaseResponseModel { Message = "Data updated successfully", Result = response, Status = 1 });
        }
    }
}