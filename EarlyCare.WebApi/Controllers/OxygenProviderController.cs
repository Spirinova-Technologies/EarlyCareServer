using AutoMapper;
using EarlyCare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/oxygenProvider")]
    [ApiController]
    public class OxygenProviderController : ControllerBase
    {
        private readonly IOxygenProviderRepository  _oxygenProviderRepository;
        private readonly ILogger<OxygenProviderController> _logger;

        public OxygenProviderController( ILogger<OxygenProviderController> logger, IOxygenProviderRepository oxygenProviderRepository)
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
    }
}