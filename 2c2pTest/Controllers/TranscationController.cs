using _2c2pTest.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace _2c2pTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranscationController : ControllerBase
    {
        private TestDemoContext _db;
        private IWebHostEnvironment environment;

        public TranscationController(TestDemoContext db, IWebHostEnvironment _env)
        {
            _db = db;
            environment = _env;
        }

        [Route("GetCurrency")]
        [HttpPost]
        public IActionResult byCurrency([FromBody] string status)
        {
            try
            {
                var curency = (from b in _db.Transactions where b.Status == status
                               select new
                               {
                                   transactionId = b.TransactionId,
                                   amount = b.Amount,
                                   currencyCode = b.CurrencyCode,
                                   transactionDate = b.TransactionDate,
                                   status = b.Status,

                               }).ToList();
                //var curency = _db.Transactions.Select(x => x.CurrencyCode).ToList();
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


        //Call by json value in body
        [Route("GetDate")]
        [HttpPost]
        public IActionResult byDateRange([FromBody] string date)
        {
            try
            {
                List<string> dtList = date.Split(',').ToList<string>();
                dtList.Reverse();

                //var curency = _db.Transactions.Select(x => x.CurrencyCode).ToList();
                var dateRange = (from b in _db.Transactions
                            where b.TransactionDate <= Convert.ToDateTime(dtList[0]) && b.TransactionDate >= Convert.ToDateTime(dtList[1])
                            select new
                            {
                                transactionId = b.TransactionId,
                                amount = b.Amount,
                                currencyCode = b.CurrencyCode,
                                transactionDate = b.TransactionDate,
                                status = b.Status,

                            }).ToList();

                if (dateRange.Count == 0)
                {
                    return StatusCode(404, "No Record Found");
                }
                return Ok(dateRange);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bad Request" + ex.ToString());
            }
        }

        [Route("GetStatus")]
        [HttpPost]
        public IActionResult byStatus([FromBody] string status)
        {
            try
            {
                var curency = _db.Transactions.Where(x => x.Status == status).ToList();
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
                                 status = _db.Transactions.Where(x => x.FormatType == "XML").Select(x => x.Status.Substring(0, 1)).FirstOrDefault(),
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

        [Route("AttchFile")]
        [RequestSizeLimit(1024)]
        [HttpPost]
        public IActionResult UploadData()
        {
            try
            {
                string message = "Failed";
                var httpRqst = Request.Form;

                foreach (var fileNames in httpRqst.Files)
                {

                    if (fileNames != null && fileNames.Length != 0)
                    {
                        var posted = fileNames;
                        var filename = posted.FileName;
                        var ext = Path.GetExtension(posted.FileName);

                        if (ext == ".xml" || ext == ".txt")
                        {
                            var physicalPath = environment.ContentRootPath + "/Uploadata/" + filename;
                            using (var stream = new FileStream(physicalPath, FileMode.Create))
                            {
                                posted.CopyTo(stream);
                            }

                            if(ext == ".txt")
                            {
                                DataTable dt = new DataTable();
                                dt.Columns.AddRange(new DataColumn[5] { 
                                   new DataColumn("id", typeof(string)),
                                   new DataColumn("amount", typeof(float)),
                                   new DataColumn("curency",typeof(string)),
                                   new DataColumn("datetime",typeof(string)),
                                   new DataColumn("status",typeof(string))
                                });


                                string csvData = System.IO.File.ReadAllText(physicalPath);
                                foreach (string row in csvData.Split('\n'))
                                {
                                    if (!string.IsNullOrEmpty(row))
                                    {
                                        dt.Rows.Add();
                                        int i = 0;
                                        foreach (string cell in row.Split(','))
                                        {
                                            if (i == 2)
                                            {
                                                var checkCurency = Currencies.ContainsKey(cell);
                                                if(checkCurency!= false)
                                                {
                                                    dt.Rows[dt.Rows.Count - 1][i] = cell;
                                                    i++;
                                                }
                                                else
                                                {
                                                    message = "Currency Code invalid";
                                                    return StatusCode(400, message);
                                                }
                                            }
                                            else if(i == 3)
                                            {
                                                DateTime dateTm = Convert.ToDateTime(cell, CultureInfo.GetCultureInfo("ms-MY").DateTimeFormat);

                                                dt.Rows[dt.Rows.Count - 1][i] = dateTm;
                                                i++;
                                            }
                                            else
                                            {
                                                dt.Rows[dt.Rows.Count - 1][i] = cell;
                                                i++;
                                            }
                                        }
                                    }
                                }

                                try
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        DataRow dr = dt.Rows[i];
                                        Transaction tr = new Transaction();
                                        tr.CurrencyCode = dr["curency"].ToString();
                                        tr.TransactionDate = Convert.ToDateTime(dr["datetime"]);
                                        tr.TransactionId = dr["id"].ToString();
                                        tr.Amount = Convert.ToDouble(dr["amount"]);
                                        tr.Status = dr["status"].ToString();
                                        tr.FormatType = ext.Substring(1,3);
                                        
                                        _db.Transactions.Add(tr);
                                        _db.SaveChanges();

                                        message = "Success";
                                        return StatusCode(200, message);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    message = "error on uploading...";
                                    return StatusCode(400, message);
                                }


                            }
                            else // XML 
                            {
                                try
                                {

                                    List<TransactionXML> transXML = new List<TransactionXML>();

                                    //Load the XML file in XmlDocument.
                                    XmlDocument doc = new XmlDocument();
                                    doc.Load(physicalPath);

                                    //Loop through the selected Nodes.
                                    foreach (XmlNode node in doc.SelectNodes("/Transactions/Transaction"))
                                    {
                                        //Fetch the Node values and assign it to Model.
                                        transXML.Add(new TransactionXML
                                        {
                                            //id = int.Parse(node["Id"].InnerText),
                                            id = node["id"].InnerText,
                                            CurrencyCode = node["Name"].InnerText,
                                            Status = node["Country"].InnerText
                                        });
                                    }

                                    //List<TransactionXML> obList = new List<TransactionXML>();
                                    //XmlDocument doc = new XmlDocument();
                                    //doc.Load(physicalPath);

                                    //XmlNodeList elemList = doc.GetElementsByTagName("blog");
                                    //TransactionXML _obj = null;

                                    //foreach (XmlNode chldNode in elemList)
                                    //{
                                    //    _obj = new TransactionXML();
                                    //    _obj.id = chldNode.Attributes["id"].Value;
                                    //    _obj.TransactionDate = chldNode.Attributes["TransactionDate"].Value;
                                    //    _obj.Amount = chldNode.Attributes["Amount"].Value;
                                    //    _obj.CurrencyCode = chldNode.Attributes["CurrencyCode"].Value;
                                    //    _obj.Status = chldNode.Attributes["Status"].Value;

                                    //    obList.Add(_obj);
                                    //}

                                }catch(Exception xx)
                                {
                                    message = xx.ToString();
                                    return StatusCode(400, message);
                                }
                            }
                        }
                        else
                        {
                            message = "Unknown format";
                        }
                    }
                }
                return new JsonResult(message);
            }
            catch (Exception x)
            {
                return new JsonResult(x.Message);
            }
        }

        public static Dictionary<string, string> Currencies = new Dictionary<string, string>() {
                                                    {"AED", "د.إ.‏"},
                                                    {"AFN", "؋ "},
                                                    {"ALL", "Lek"},
                                                    {"AMD", "դր."},
                                                    {"ARS", "$"},
                                                    {"AUD", "$"},
                                                    {"AZN", "man."},
                                                    {"BAM", "KM"},
                                                    {"BDT", "৳"},
                                                    {"BGN", "лв."},
                                                    {"BHD", "د.ب.‏ "},
                                                    {"BND", "$"},
                                                    {"BOB", "$b"},
                                                    {"BRL", "R$"},
                                                    {"BYR", "р."},
                                                    {"BZD", "BZ$"},
                                                    {"CAD", "$"},
                                                    {"CHF", "fr."},
                                                    {"CLP", "$"},
                                                    {"CNY", "¥"},
                                                    {"COP", "$"},
                                                    {"CRC", "₡"},
                                                    {"CSD", "Din."},
                                                    {"CZK", "Kč"},
                                                    {"DKK", "kr."},
                                                    {"DOP", "RD$"},
                                                    {"DZD", "DZD"},
                                                    {"EEK", "kr"},
                                                    {"EGP", "ج.م.‏ "},
                                                    {"ETB", "ETB"},
                                                    {"EUR", "€"},
                                                    {"GBP", "£"},
                                                    {"GEL", "Lari"},
                                                    {"GTQ", "Q"},
                                                    {"HKD", "HK$"},
                                                    {"HNL", "L."},
                                                    {"HRK", "kn"},
                                                    {"HUF", "Ft"},
                                                    {"IDR", "Rp"},
                                                    {"ILS", "₪"},
                                                    {"INR", "रु"},
                                                    {"IQD", "د.ع.‏ "},
                                                    {"IRR", "ريال "},
                                                    {"ISK", "kr."},
                                                    {"JMD", "J$"},
                                                    {"JOD", "د.ا.‏ "},
                                                    {"JPY", "¥"},
                                                    {"KES", "S"},
                                                    {"KGS", "сом"},
                                                    {"KHR", "៛"},
                                                    {"KRW", "₩"},
                                                    {"KWD", "د.ك.‏ "},
                                                    {"KZT", "Т"},
                                                    {"LAK", "₭"},
                                                    {"LBP", "ل.ل.‏ "},
                                                    {"LKR", "රු."},
                                                    {"LTL", "Lt"},
                                                    {"LVL", "Ls"},
                                                    {"LYD", "د.ل.‏ "},
                                                    {"MAD", "د.م.‏ "},
                                                    {"MKD", "ден."},
                                                    {"MNT", "₮"},
                                                    {"MOP", "MOP"},
                                                    {"MVR", "ރ."},
                                                    {"MXN", "$"},
                                                    {"MYR", "RM"},
                                                    {"NIO", "N"},
                                                    {"NOK", "kr"},
                                                    {"NPR", "रु"},
                                                    {"NZD", "$"},
                                                    {"OMR", "ر.ع.‏ "},
                                                    {"PAB", "B/."},
                                                    {"PEN", "S/."},
                                                    {"PHP", "PhP"},
                                                    {"PKR", "Rs"},
                                                    {"PLN", "zł"},
                                                    {"PYG", "Gs"},
                                                    {"QAR", "ر.ق.‏ "},
                                                    {"RON", "lei"},
                                                    {"RSD", "Din."},
                                                    {"RUB", "р."},
                                                    {"RWF", "RWF"},
                                                    {"SAR", "ر.س.‏ "},
                                                    {"SEK", "kr"},
                                                    {"SGD", "$"},
                                                    {"SYP", "ل.س.‏ "},
                                                    {"THB", "฿"},
                                                    {"TJS", "т.р."},
                                                    {"TMT", "m."},
                                                    {"TND", "د.ت.‏ "},
                                                    {"TRY", "TL"},
                                                    {"TTD", "TT$"},
                                                    {"TWD", "NT$"},
                                                    {"UAH", "₴"},
                                                    {"USD", "$"},
                                                    {"UYU", "$U"},
                                                    {"UZS", "so'm"},
                                                    {"VEF", "Bs. F."},
                                                    {"VND", "₫"},
                                                    {"XOF", "XOF"},
                                                    {"YER", "ر.ي.‏ "},
                                                    {"ZAR", "R"},
                                                    {"ZWL", "Z$"} 
        };

    }
}
