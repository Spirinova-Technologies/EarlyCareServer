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
    [Route("api/tiffin")]
    [ApiController]
    public class TiffinController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITiffinRepository _tiffinRepository;
        private readonly ILogger<TiffinController> _logger;

        public TiffinController(IMapper mapper, ILogger<TiffinController> logger, ITiffinRepository tiffinRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _tiffinRepository = tiffinRepository;
        }

        [HttpGet("getTiffins")]
        public async Task<IActionResult> GetTiffins([Required] int cityId)
        {
            var response = await _tiffinRepository.GetTiffins(cityId);

            return Ok(response);
        }
    }
}
