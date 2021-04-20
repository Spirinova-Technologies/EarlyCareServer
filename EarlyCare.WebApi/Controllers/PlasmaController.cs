using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EarlyCare.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/plasma")]
    [ApiController]
    public class PlasmaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlasmaRepository _plasmaRepository;
        private readonly ILogger<PlasmaController> _logger;

        public PlasmaController(IMapper mapper, ILogger<PlasmaController> logger, IPlasmaRepository plasmaRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _plasmaRepository = plasmaRepository;
        }

        [HttpGet("getPlasmas")]
        public async Task<IActionResult> GetPlasmas(int cityId)
        {
            var response = await _plasmaRepository.GetPlasmas(cityId);

            return Ok(response);
        }
    }
}
