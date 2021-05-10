using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure;
using EarlyCare.Infrastructure.Constants;
using EarlyCare.Infrastructure.SharedModels;
using EarlyCare.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/plasma")]
    [ApiController]
    public class PlasmaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlasmaRepository _plasmaRepository;
        private readonly ILogger<PlasmaController> _logger;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        public PlasmaController(IMapper mapper, ILogger<PlasmaController> logger, IPlasmaRepository plasmaRepository, IEmailService emailService, IUserRepository userRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _plasmaRepository = plasmaRepository;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        [HttpGet("getPlasmas")]
        public async Task<IActionResult> GetPlasmas([Required] int cityId, int? userId)
        {
            var user = userId.HasValue ? await _userRepository.GetUserById(userId.Value) : null;

            var hasApprovePermission = user != null && (user.UserType == 1 || (user.UserType == 2 && user.IsVerified == true));

            var response = await _plasmaRepository.GetPlasmas(cityId, hasApprovePermission);

            return Ok(response);
        }

        [HttpGet("getPlasmaByUserId")]
        public async Task<IActionResult> GetPlasmaByUserId([Required] int userId)
        {
            var response = await _plasmaRepository.GetPlasmaByUserId(userId);

            return Ok(response);
        }

        [HttpPost("insertPlasma")]
        public async Task<IActionResult> InsertPlasma([FromBody] PlasmaRequestModel plasmaRequestModel)
        {
            var plasma = new Plasma
            {
                CityId = plasmaRequestModel.CityId,
                Address = plasmaRequestModel.Address,
                BloodGroup = plasmaRequestModel.BloodGroup,
                CovidNegativeDate = plasmaRequestModel.CovidNegativeDate,
                CovidPositiveDate = plasmaRequestModel.CovidPositiveDate,
                CreatedBy = plasmaRequestModel.UserId,
                CreatedOn = Utilities.GetCurrentTime(),
                DonorType = plasmaRequestModel.DonorType,
                IsVerified = false,
                IsAntibodyReportAvailable = plasmaRequestModel.IsAntibodyReportAvailable,
                IsRtpcrReportAvailable = plasmaRequestModel.IsRtpcrReportAvailable,
                Name = plasmaRequestModel.Name,
                PhoneNumber = plasmaRequestModel.PhoneNumber,
                UpdatedBy = plasmaRequestModel.UserId,
                UpdatedOn = Utilities.GetCurrentTime()
            };

            var response = await _plasmaRepository.InsertPlasma(plasma);

            return Ok(new BaseResponseModel { Message = "Data inserted successfully", Result = response, Status = 1 });
        }

        [HttpPost("updatePlasma")]
        public async Task<IActionResult> UpdatePlasma([FromBody] PlasmaRequestModel plasmaRequestModel)
        {
            var plasma = new Plasma
            {
                Id = plasmaRequestModel.Id,
                CityId = plasmaRequestModel.CityId,
                Address = plasmaRequestModel.Address,
                BloodGroup = plasmaRequestModel.BloodGroup,
                CovidNegativeDate = plasmaRequestModel.CovidNegativeDate.HasValue ? ConvertToISTDateTime(plasmaRequestModel.CovidNegativeDate.Value) : default,
                CovidPositiveDate = plasmaRequestModel.CovidPositiveDate.HasValue ? ConvertToISTDateTime(plasmaRequestModel.CovidPositiveDate.Value) : default,
                DonorType = plasmaRequestModel.DonorType,
                IsVerified = false,
                IsAntibodyReportAvailable = plasmaRequestModel.IsAntibodyReportAvailable,
                IsRtpcrReportAvailable = plasmaRequestModel.IsRtpcrReportAvailable,
                Name = plasmaRequestModel.Name,
                PhoneNumber = plasmaRequestModel.PhoneNumber,
                UpdatedBy = plasmaRequestModel.UserId,
                UpdatedOn = Utilities.GetCurrentTime()
            };

            var response = await _plasmaRepository.UpdatePlasma(plasma);

            return Ok(new BaseResponseModel { Message = "Data updated successfully", Result = response, Status = 1 });
        }

        [HttpPost("updateVerificationStatus")]
        public async Task<IActionResult> UpdateVerificationStatus([FromBody] UpdateVerificationStatusModel plasmaRequestModel)
        {
            var response = await _plasmaRepository.UpdateVerificationStatus(plasmaRequestModel);

            //send email
            await _emailService.SendUpdateNotification(response.CreatedBy, Constants.PlasmaDetailsUpdatedEmailSubject, Constants.PlasmaDetailsUpdatedEmailBody);

            return Ok(new BaseResponseModel { Message = "Status updated successfully", Status = 1 });
        }

        [NonAction]
        private DateTime ConvertToISTDateTime(DateTime utcdate)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcdate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }
    }
}
