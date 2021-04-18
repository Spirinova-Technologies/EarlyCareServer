using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/globalSettings")]
    [ApiController]
    public class GlobalSettingsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly ILogger<GlobalSettingsController> _logger;

        public GlobalSettingsController(IMapper mapper, ILogger<GlobalSettingsController> logger,
            IGlobalSettingService globalSettingService)
        {
            _mapper = mapper;
            _logger = logger;
            _globalSettingService = globalSettingService;
        }


        [HttpGet("getGlobalSettings")]
        public async Task<IActionResult> GetVolunteers()
        {
            var response = await _globalSettingService.GetGlobalSetting();

            return Ok(new BaseResponseModel { Status = 1, Result = response });
        }

    }
}
