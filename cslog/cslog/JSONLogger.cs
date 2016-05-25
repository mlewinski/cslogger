using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace cslog
{
    public class JSONLogger
    {
        private string _filename;
        private bool _operatingMode;
        
        public JSONLogger(bool operatingmode, string path, string filename="")
        {
            _operatingMode = operatingmode;
            _filename = Path.Combine(path, filename);
        }

        public JSONLogger(string path, string prefix)
        {
            _operatingMode = true;
            _filename = Path.Combine(path, prefix);
        }

        public void WriteToLog(Categories category, string message)
        {
            string filename = "";
            if (_operatingMode) filename = Path.Combine(_filename, DateTime.Now.Date.ToString("yyyyMMdd"), ".json");
            else filename = _filename;

            string jsonString = "";


            if (File.Exists(filename)) jsonString = File.ReadAllText(filename);

            List<string> logs = JsonConvert.DeserializeObject<List<string>>(jsonString);
            if(logs == null) logs=new List<string>();
            logs.Add(String.Format("Date: {0} | Time: {1} | Category: {2} | Message: {3}", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH:mm:ss:fff"), category, message));
            jsonString = JsonConvert.SerializeObject(logs);
            File.WriteAllText(filename, jsonString);
        }
    }
}
