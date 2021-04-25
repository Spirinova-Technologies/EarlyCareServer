using AutoMapper;
using EarlyCare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/drug")]
    [ApiController]
    public class DrugsController : ControllerBase
    {
        private readonly IDrugsRepository _drugsRepository;
        private readonly ILogger<DrugsController> _logger;

        public DrugsController(ILogger<DrugsController> logger, IDrugsRepository drugsRepository)
        {
            _logger = logger;
            _drugsRepository = drugsRepository;
        }

        [HttpGet("getDrugProviders")]
        public async Task<IActionResult> GetDrugProviders([Required] int cityId)
        {
            var response = await _drugsRepository.GetDrugProviders(cityId);

            return Ok(response);
        }
    }
}