using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/facebook")]
    [ApiController]
    public class FacebookController : ControllerBase
    {

        public FacebookController()
        {
  
        }

        //[HttpGet("getAmbulances")]
        //public async Task<IActionResult> GetAmbulances([Required] int cityId)
        //{
           
        //    return Ok(response);
        //}
    }
}
