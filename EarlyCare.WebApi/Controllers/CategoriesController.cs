using AutoMapper;
using EarlyCare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IMapper mapper, ILogger<CategoriesController> logger, ICategoriesRepository categoriesRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _categoriesRepository = categoriesRepository;
        }

        [HttpGet("getCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _categoriesRepository.GetCategories();

            return Ok(response);
        }
    }
}