using AutoMapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.SharedModels;
using EarlyCare.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
        private readonly IGoogleSheetRepository _googleSheetRepository;

        public GoogleSheetController(IMapper mapper, ILogger<GoogleSheetController> logger, IGoogleSheetRepository googleSheetRepository,
            IGoogleSheetService googleSheetService)
        {
            _mapper = mapper;
            _logger = logger;
            _googleSheetService = googleSheetService;
            _googleSheetRepository = googleSheetRepository;
        }

        [HttpPost("saveGoogleSheetData")]
        public async Task<IActionResult> SaveGoogleSheetData(GoogleSheetRequestModel googleSheetRequestModel)
        {
            var response = await _googleSheetService.GetGoogleSheetData(googleSheetRequestModel);

            return Ok(new BaseResponseModel
            {
                Message = "Data saved successfully"
            });
        }

        [HttpGet("getGoogleSheets")]
        public async Task<IActionResult> getGoogleSheetNames()
        {
            var googleSheets = await _googleSheetRepository.GetGoogleSheets();
            var response = _mapper.Map<List<GoogleSheet>, List<GoogleSheetResponse>>(googleSheets);

            return Ok(response);
        }
    }
}