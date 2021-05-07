using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
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

        public FoodController(IMapper mapper, ILogger<FoodController> logger, IFoodRepository foodRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _foodRepository = foodRepository;
        }

        [HttpGet("getFoods")]
        public async Task<IActionResult> GetFoods([Required] int cityId)
        {
            var response = await _foodRepository.GetFoods(cityId);

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
                CreatedOn = DateTime.Now,
                IsVerified = false,
                Name = foodDetailsRequestModel.Name,
                PhoneNumber = foodDetailsRequestModel.PhoneNumber,
                UpdatedBy = foodDetailsRequestModel.UserId,
                UpdatedOn = DateTime.Now
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
                UpdatedOn = DateTime.Now
            };

            var response = await _foodRepository.UpdateFood(oxygenProvider);

            return Ok(new BaseResponseModel { Message = "Data updated successfully", Result = response, Status = 1 });
        }
    }
}
