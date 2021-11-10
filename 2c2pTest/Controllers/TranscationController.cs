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

        [HttpGet("Currency")]
        public IActionResult byCurrency()
        {
            try
            {
                var curency = _db.Transactions.Select(x=>x.CurrencyCode).ToList();
                if (curency.Count == 0)
                {
                    return StatusCode(404, "No Record Found");
                }
                return Ok(curency);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bad Request" + ex.ToString());
            }
        }

        [HttpGet("DateRange")]
        public IActionResult byDateRange()
        {
            try
            {
                var curency = _db.Transactions.Select(x => x.CurrencyCode).ToList();
                if (curency.Count == 0)
                {
                    return StatusCode(404, "No Record Found");
                }
                return Ok(curency);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bad Request" + ex.ToString());
            }
        }

        [HttpGet("Status")]
        public IActionResult byStatus()
        {
            try
            {
                var curency = _db.Transactions.Select(x => x.Status).ToList();
                if (curency.Count == 0)
                {
                    return StatusCode(404, "No Record Found");
                }
                return Ok(curency);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bad Request" + ex.ToString());
            }
        }
    }
}
