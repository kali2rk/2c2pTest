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


        [HttpGet("AllTransaction")]
        public IActionResult allTrans()
        {
            try
            {
                // t => t.Title.Substring(0, 5).ToLower()
                //var trans = _db.Transactions.ToList();
                var trans = (from b in _db.Transactions
                             select new
                             {
                                 id = b.TransactionId,
                                 payment = b.Amount + " " + b.CurrencyCode,
                                 status = _db.Transactions.Where(x => x.FormatType == "XML").Select(x => x.Status.Substring(0,1)).FirstOrDefault(),
                             }).ToList();

                if (trans.Count == 0)
                {
                    return StatusCode(404, "No Record Found");
                }

                return Ok(trans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bad Request" + ex.ToString());
            }
        }

    }
}
