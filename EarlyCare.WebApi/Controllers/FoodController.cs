using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EarlyCare.Core.Interfaces;
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
    }
}
