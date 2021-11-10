using System;
using System.Collections.Generic;

#nullable disable

namespace _2c2pTest.Models
{
    public partial class Transaction
    {
        public int PkId { get; set; }
        public string TransactionId { get; set; }
        public double? Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string Status { get; set; }
        public string FormatType { get; set; }
        public DateTime TransactionDtCreated { get; set; }
    }
}
