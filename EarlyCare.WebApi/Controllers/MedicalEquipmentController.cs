using AutoMapper;
using EarlyCare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/medicalequipment")]
    [ApiController]
    public class MedicalEquipmentController : ControllerBase
    {
        private readonly IMedicalEquipmentRepository _medicalEquipmentRepository;
        private readonly ILogger<MedicalEquipmentController> _logger;

        public MedicalEquipmentController(ILogger<MedicalEquipmentController> logger, IMedicalEquipmentRepository medicalEquipmentRepository)
        {
            _logger = logger;
            _medicalEquipmentRepository = medicalEquipmentRepository;
        }

        [HttpGet("getMedicalEquipments")]
        public async Task<IActionResult> GetMedicalEquipments([Required] int cityId)
        {
            var response = await _medicalEquipmentRepository.GetMedicalEquipments(cityId);

            return Ok(response);
        }
    }
}