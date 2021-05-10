using AutoMapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure;
using EarlyCare.Infrastructure.Constants;
using EarlyCare.Infrastructure.SharedModels;
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
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public OxygenProviderController(ILogger<OxygenProviderController> logger, IOxygenProviderRepository oxygenProviderRepository,
            IEmailService emailService, IUserRepository userRepository)
        {
            _logger = logger;
            _emailService = emailService;
            _oxygenProviderRepository = oxygenProviderRepository;
            _userRepository = userRepository;
        }

        [HttpGet("getOxygenProviders")]
        public async Task<IActionResult> GetOxygenProviders([Required] int cityId, int? userId)
        {
            var user = userId.HasValue ? await _userRepository.GetUserById(userId.Value) : null;

            var hasApprovePermission = user != null && (user.UserType == 1 || (user.UserType == 2 && user.IsVerified == true));

            var response = await _oxygenProviderRepository.GetOxygenProviders(cityId, hasApprovePermission);

            return Ok(response);
        }

        [HttpGet("getOxygenProviderByUserId")]
        public async Task<IActionResult> GetOxygenProviderByUserId([Required] int userId)
        {
            var response = await _oxygenProviderRepository.GetOxygenProviderByUserId(userId);

            return Ok(response);
        }

        [HttpPost("updateVerificationStatus")]
        public async Task<IActionResult> UpdateVerificationStatus([FromBody] UpdateVerificationStatusModel updateRequestModel)
        {
           var response = await _oxygenProviderRepository.UpdateVerificationStatus(updateRequestModel);

            //send email
            await _emailService.SendUpdateNotification(response.CreatedBy, Constants.OxygenUpdatedEmailSubject, Constants.OxygenDetailsUpdatedEmailBody);

            return Ok(new BaseResponseModel { Message = "Status updated successfully", Status = 1 });
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
                CreatedOn = Utilities.GetCurrentTime(),
                IsVerified = false,
                Name = oxygenDetailsRequestModel.Name,
                PhoneNumber = oxygenDetailsRequestModel.PhoneNumber,
                UpdatedBy = oxygenDetailsRequestModel.UserId,
                UpdatedOn = Utilities.GetCurrentTime()
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
                UpdatedOn = Utilities.GetCurrentTime()
            };

            var response = await _oxygenProviderRepository.UpdateOxygenProvider(oxygenProvider);

            return Ok(new BaseResponseModel { Message = "Data updated successfully", Result = response, Status = 1 });
        }
    }
}