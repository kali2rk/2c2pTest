using _2c2pTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2c2pTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranscationController : ControllerBase
    {
        private TestDemoContext _db;
        public TranscationController(TestDemoContext db)
        {
            _db = db;
        }

        [HttpGet("getBookingbyId")]
        public IActionResult Get([FromRoute] String Uid)
        {
            try
            {
               
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error accoured" + ex.ToString());
            }
        }
    }
}
