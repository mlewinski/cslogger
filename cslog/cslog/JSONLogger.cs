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

        /// <summary>
        /// Create object with default values (single file named cslog.json)
        /// </summary>
        public JSONLogger()
        {
            _operatingMode = false;
            _filename = "cslog.json";
        }

        /// <summary>
        /// Create object with custom parameters. If exception is thrown object will use default log file (cslog.json)
        /// </summary>
        /// <param name="operatingMode">false - single file, true - separate files</param>
        /// <param name="filePath">Path to the log file</param>
        /// <param name="fileName">Name of file in single-file mode (Leave blank in separate-files mode)</param>
        public JSONLogger(bool operatingmode, string path, string filename="")
        {
            try
            {
                _operatingMode = operatingmode;
                _filename = Path.Combine(path, filename);
            }
            catch (Exception ex)
            {
                _filename = "cslog.json";
            }
        }

        /// <summary>
        /// Creates object with custom parameters. Mode - separate-files
        /// </summary>
        /// <param name="filePath">Path to store log files</param>
        /// <param name="namePrefix">Name prefix for log files</param>
        public JSONLogger(string path, string prefix)
        {
            _operatingMode = true;
            _filename = Path.Combine(path, prefix);
        }

        /// <summary>
        /// Writes log message into log file. If file does not exists creates it with adequate directories from path
        /// </summary>
        /// <param name="message">Log message to write</param>
        /// <param name="category">Log category <seealso cref="Categories"/></param>
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
