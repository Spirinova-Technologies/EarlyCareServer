using AutoMapper;
using EarlyCare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/rtpcrTest")]
    [ApiController]
    public class RtpcrTestsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRtpcrTestRepository _rtpcrTestRepository;
        private readonly ILogger<RtpcrTestsController> _logger;

        public RtpcrTestsController(IMapper mapper, ILogger<RtpcrTestsController> logger, IRtpcrTestRepository rtpcrTestRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _rtpcrTestRepository = rtpcrTestRepository;
        }

        [HttpGet("getRtpcrTestCenters")]
        public async Task<IActionResult> GetRtpcrTestCenters([Required]  int cityId)
        {
            var response = await _rtpcrTestRepository.GetRtpcrTests(cityId);

            return Ok(response);
        }
    }
}