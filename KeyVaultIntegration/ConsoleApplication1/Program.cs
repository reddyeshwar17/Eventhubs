using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Services.Client;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.StuddInfo info = new ServiceReference1.StuddInfo(new Uri("http://localhost:35954/WcfDataService1.svc/"));
           
        }
    }
}
