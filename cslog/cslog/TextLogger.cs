using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cslog
{
    public class TextLogger
    {
        private bool _operatingMode; // false - single file, true - separate files for each day
        private string _logFile;
        private string _prefix;

        /// <summary>
        /// Create object with default values (single file named cslog.log)
        /// </summary>
        public TextLogger()
        {
            _operatingMode = false;
            _logFile = "cslog.log";
        }

        /// <summary>
        /// Create object with custom parameters. If exception is thrown object will use default log file (cslog.log)
        /// </summary>
        /// <param name="operatingMode">false - single file, true - separate files</param>
        /// <param name="filePath">Path to the log file</param>
        /// <param name="fileName">Name of file in single-file mode (Leave blank in separate-files mode)</param>
        public TextLogger(bool operatingMode, string filePath, string fileName="cslog.log")
        {
            _operatingMode = operatingMode;
            if (!operatingMode)
                try
                {
                    _logFile = Path.Combine(filePath, fileName);
                }
                catch (Exception ex)
                {
                    _logFile = "cslog.log";
                }
            else _logFile = Path.Combine(filePath, fileName);
        }

        /// <summary>
        /// Creates object with custom parameters. Mode - separate-files
        /// </summary>
        /// <param name="filePath">Path to store log files</param>
        /// <param name="namePrefix">Name prefix for log files</param>
        public TextLogger(string filePath, string namePrefix)
        {
            _operatingMode = true;
            _logFile = filePath;
            _prefix = namePrefix;
        }

        public void WriteToLog(Categories category, string message)
        {
            string filename;
            if (_operatingMode)
            {
                filename = Path.Combine(_logFile, (_prefix + DateTime.Now.Date.ToString("yyyyMMdd") + ".log"));
            }
            else
            {
                filename = _logFile;
            }
            string entry = String.Format("{4} Date: {0} | Time: {1} | Category: {2} | Message: {3} {4}", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH:mm:ss:fff"), category, message, Environment.NewLine);
            File.AppendAllText(filename, entry);
        }
    }
}
