using AutoMapper;
using EarlyCare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/consultation")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConsultationRepository  _consultationRepository;
        private readonly ILogger<ConsultationController> _logger;

        public ConsultationController(IMapper mapper, ILogger<ConsultationController> logger, IConsultationRepository consultationRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _consultationRepository = consultationRepository;
        }

        [HttpGet("getConsultations")]
        public async Task<IActionResult> GetConsultations([Required] int cityId)
        {
            var response = await _consultationRepository.GetConsultations(cityId);

            return Ok(response);
        }
    }
}