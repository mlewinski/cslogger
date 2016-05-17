using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cslog;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlLogger Logger = new XmlLogger(false, "logs/", "log_number_");
            for(int i = 0; i<1000; i++) Logger.WriteToLog(i.ToString(), Categories.Error);
        }
    }
}
