using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2c2pTest.Models
{
    public class Transactioninfo
    {
        public string id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }

    public class TransactionPay
    {
        public float Amount { get; set; }
        public string CurrencyCode { get; set; }
    }


    public class AllTransaction
    {
        public List<Transactioninfo> tranInfo { get; set; }
        public List<TransactionPay> transPay { get; set; }
    }


}
