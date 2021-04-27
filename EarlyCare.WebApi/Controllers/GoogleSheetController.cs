﻿using AutoMapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/googleSheet")]
    [ApiController]
    public class GoogleSheetController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGoogleSheetService _googleSheetService;
        private readonly ILogger<GoogleSheetController> _logger;

        public GoogleSheetController(IMapper mapper, ILogger<GoogleSheetController> logger, IGoogleSheetService googleSheetService)
        {
            _mapper = mapper;
            _logger = logger;
            _googleSheetService = googleSheetService;
        }

        [HttpPost("saveGoogleSheetData")]
        public async Task<IActionResult> SaveGoogleSheetData(GoogleSheetRequestModel googleSheetRequestModel)
        {
            var response = await _googleSheetService.GetGoogleSheetData();

            return Ok(new BaseResponseModel
            {
                Message = "Data saved successfully"
            });
        }
    }
}