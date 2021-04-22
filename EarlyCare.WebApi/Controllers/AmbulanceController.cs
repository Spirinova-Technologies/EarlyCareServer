using AutoMapper;
using EarlyCare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/ambulance")]
    [ApiController]
    public class AmbulanceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAmbulanceRepository _ambulanceRepository;
        private readonly ILogger<AmbulanceController> _logger;

        public AmbulanceController(IMapper mapper, ILogger<AmbulanceController> logger, IAmbulanceRepository ambulanceRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _ambulanceRepository = ambulanceRepository;
        }

        [HttpGet("getAmbulances")]
        public async Task<IActionResult> GetAmbulances([Required] int cityId)
        {
            var response = await _ambulanceRepository.GetAmbulances(cityId);

            return Ok(response);
        }
    }
}