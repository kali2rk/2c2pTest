using _2c2pTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2c2pTest.Services
{
    public interface ITransaction
    {
        TransactionXML[] GetxmlFILE();
    }
}
