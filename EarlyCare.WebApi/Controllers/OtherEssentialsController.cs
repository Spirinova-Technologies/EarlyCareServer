using AutoMapper;
using EarlyCare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/otherEssentials")]
    [ApiController]
    public class OtherEssentialsController : ControllerBase
    {
        private readonly IOtherEssentialsRepository _otherEssentialsRepository;
        private readonly ILogger<OtherEssentialsController> _logger;

        public OtherEssentialsController(ILogger<OtherEssentialsController> logger, IOtherEssentialsRepository otherEssentialsRepository)
        {
            _logger = logger;
            _otherEssentialsRepository = otherEssentialsRepository;
        }

        [HttpGet("getOtherEssentials")]
        public async Task<IActionResult> GetAmbulances([Required] int cityId)
        {
            var response = await _otherEssentialsRepository.GetOtherEssentials(cityId);

            return Ok(response);
        }
    }
}