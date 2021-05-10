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
    [Route("api/food")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFoodRepository _foodRepository;
        private readonly ILogger<FoodController> _logger;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public FoodController(IMapper mapper, ILogger<FoodController> logger, IFoodRepository foodRepository, IEmailService emailService, IUserRepository userRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _foodRepository = foodRepository;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        [HttpGet("getFoods")]
        public async Task<IActionResult> GetFoods([Required] int cityId, int? userId)
        {
            var user = userId.HasValue ? await _userRepository.GetUserById(userId.Value) : null;

            var hasApprovePermission = user != null && (user.UserType == 1 || (user.UserType == 2 && user.IsVerified == true));

            var response = await _foodRepository.GetFoods(cityId, hasApprovePermission);

            return Ok(response);
        }

        [HttpGet("getFoodByUserId")]
        public async Task<IActionResult> GetFoodByUserId([Required] int userId)
        {
            var response = await _foodRepository.GetFoodByUserId(userId);

            return Ok(response);
        }

        [HttpPost("insertFoodDetails")]
        public async Task<IActionResult> InsertFoodDetails([FromBody] FoodDetailsRequestModel foodDetailsRequestModel)
        {
            var oxygenProvider = new Food
            {
                Address = foodDetailsRequestModel.Address,
                FoodServed = foodDetailsRequestModel.FoodServed,
                Delivery = foodDetailsRequestModel.Delivery,
                RegistrationNumber = foodDetailsRequestModel.RegistrationNumber,
                Area = foodDetailsRequestModel.Area,
                Charges = foodDetailsRequestModel.Charges,
                Type = foodDetailsRequestModel.Type,
                CityId = foodDetailsRequestModel.CityId,
                CreatedBy = foodDetailsRequestModel.UserId,
                CreatedOn = Utilities.GetCurrentTime(),
                IsVerified = false,
                Name = foodDetailsRequestModel.Name,
                PhoneNumber = foodDetailsRequestModel.PhoneNumber,
                UpdatedBy = foodDetailsRequestModel.UserId,
                UpdatedOn = Utilities.GetCurrentTime()
            };

            var response = await _foodRepository.InsertFood(oxygenProvider);

            return Ok(new BaseResponseModel { Message = "Data inserted successfully", Result = response, Status = 1 });
        }

        [HttpPost("updateFoodDetails")]
        public async Task<IActionResult> UpdateFoodDetails([FromBody] FoodDetailsRequestModel foodDetailsRequestModel)
        {
            var oxygenProvider = new Food
            {
                Id = foodDetailsRequestModel.Id,
                Address = foodDetailsRequestModel.Address,
                FoodServed = foodDetailsRequestModel.FoodServed,
                Delivery = foodDetailsRequestModel.Delivery,
                RegistrationNumber = foodDetailsRequestModel.RegistrationNumber,
                Area = foodDetailsRequestModel.Area,
                Charges = foodDetailsRequestModel.Charges,
                Type = foodDetailsRequestModel.Type,
                CityId = foodDetailsRequestModel.CityId,
                IsVerified = false,
                Name = foodDetailsRequestModel.Name,
                PhoneNumber = foodDetailsRequestModel.PhoneNumber,
                UpdatedBy = foodDetailsRequestModel.UserId,
                UpdatedOn = Utilities.GetCurrentTime()
            };

            var response = await _foodRepository.UpdateFood(oxygenProvider);

            return Ok(new BaseResponseModel { Message = "Data updated successfully", Result = response, Status = 1 });
        }

        [HttpPost("updateVerificationStatus")]
        public async Task<IActionResult> UpdateVerificationStatus([FromBody] UpdateVerificationStatusModel statusRequestModel)
        {
            var response = await _foodRepository.UpdateVerificationStatus(statusRequestModel);

            //send email
            await _emailService.SendUpdateNotification(response.CreatedBy, Constants.FoodDetailsUpdatedEmailSubject, Constants.FoodDetailsUpdatedEmailBody);

            return Ok(new BaseResponseModel { Message = "Status updated successfully", Status = 1 });
        }
    }
}
