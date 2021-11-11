using _2c2pTest.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _2c2pTest.Services
{
    public class Transervices : ITransaction
    {
        private readonly IHostingEnvironment _env;
        public Transervices(IHostingEnvironment env)
        {
            _env = env;
        }
        public TransactionXML[] GetxmlFILE()
        {
            XmlSerializer ser = new XmlSerializer(typeof(Transactionsed));
            FileStream myFileStream = new FileStream(_env.ContentRootPath + "\\Uploadata\\transa.xml", FileMode.Open);
            return ((Transactionsed)ser.Deserialize(myFileStream)).Transac;
        }
    }
}
