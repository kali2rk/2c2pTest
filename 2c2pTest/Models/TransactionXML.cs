using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2c2pTest.Models
{
    public class TransactionXML
    {
        public string id { get; set; }
        public string TransactionDate { get; set; }
        public string Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Status { get; set; }


        //public string id { get; set; }
        //public DateTime TransactionDate { get; set; }
        //public float Amount { get; set; }
        //public string CurrencyCode { get; set; }
        //public string Status { get; set; }

    }

    public class Transactionsed
    {
        public TransactionXML[] Transac { get; set; }
    }
}
