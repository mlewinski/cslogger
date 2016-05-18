using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace cslog
{
    /// <summary>
    /// Class writes XML log files in one of two modes:
    /// - single-file: entire log is stored in one file
    /// - separate-file: logs from each day are stored in separate files
    /// </summary>
    public class XmlLogger
    {
        private bool _operatingMode = false;    // false - single file, true - separate files for each day
        private string _filePrefix = "";        // file name prefix for separate files mode

        private string _filename = "cslog.xml"; // file name for single file mode

        private string _path = "";              // directory for storing files

        #region Constructors
            /// <summary>
            /// Separate files logging mode. Logs are stored in separate files, each containing logs from single day
            /// </summary>
            /// <param name="operatingMode">Unused parameter</param>
            /// <param name="path">Directory for log files</param>
            /// <param name="filePrefix">Prefix for log files names. Default is null</param>
            public XmlLogger(bool operatingMode, string path, string filePrefix = "")
            {
                _operatingMode = true;
                _filePrefix = filePrefix;
                _path = path;
            }

            /// <summary>
            /// Default logging mode - stores log in single file
            /// </summary>
            /// <param name="path">Directory for log file</param>
            /// <param name="filename">Name of file to store logs in. Default is cslog.xml</param>
            public XmlLogger(string path, string filename = "cslog.xml")
            {
                _operatingMode = false;
                _path = path;
                _filename = filename;
            }

            /// <summary>
            /// Select operating mode with default filenames
            /// </summary>
            /// <param name="operatingMode">Operating mode. False for single-file, true for separate-files</param>
            public XmlLogger(bool operatingMode)
            {
                _operatingMode = operatingMode;
            }

            /// <summary>
            /// Default values, single-file mode
            /// </summary>
            public XmlLogger()
            {
                _operatingMode = false;
            }
        #endregion

        /// <summary>
        /// Writes log message into log file. If file does not exists creates it with adequate directories from path
        /// </summary>
        /// <param name="message">Log message to write</param>
        /// <param name="category">Log category <seealso cref="Categories"/></param>
        public void WriteToLog(string message, Categories category)
        {
            while (true)
            {
                string filename;
                if (!_operatingMode)
                {
                    filename = _filename;
                }
                else
                {
                    filename = _filePrefix + DateTime.Now.Date.ToString("yyyMMdd") + ".xml";
                }
                filename = Path.Combine(_path, filename);
                if (File.Exists(filename))
                {
                    XmlDocument xmlDocument = new XmlDocument();

                    try
                    {
                        xmlDocument.Load(filename);
                    }
                    catch (XmlException)
                    {
                        XmlTextWriter writer = new XmlTextWriter(filename, null);
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Log");
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Close();
                        xmlDocument.Load(filename);
                    }

                    if (xmlDocument.SelectSingleNode("//Log") == null)
                    {
                        XmlWriter writer = XmlWriter.Create(filename);
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Log");
                        writer.WriteStartElement("message");
                        writer.WriteAttributeString("date", DateTime.Now.Date.ToString("yyyy-MM-dd"));
                        writer.WriteAttributeString("time", DateTime.Now.Date.ToString("HH:mm:ss:fff"));
                        writer.WriteAttributeString("type", category.ToString());
                        
                        writer.WriteString(message);
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                    else
                    {
                        XmlNode node = xmlDocument.CreateElement("message");
                        XmlAttribute type = xmlDocument.CreateAttribute("type");
                        type.Value = category.ToString();
                        XmlAttribute date = xmlDocument.CreateAttribute("date");
                        date.Value = DateTime.Now.Date.ToString("yyyy-MM-dd");
                        XmlAttribute time = xmlDocument.CreateAttribute("time");
                        time.Value = DateTime.Now.ToString("HH:mm:ss:fff");
                        node.Attributes.Append(date);
                        node.Attributes.Append(type);
                        node.Attributes.Append(time);
                        node.InnerText = message;
                        xmlDocument.DocumentElement.AppendChild(node);
                        xmlDocument.Save(filename);
                    }
                }
                else
                {
                    CreateLogFile(filename);
                    continue;
                }
                break;
            }
        }

        private void CreateLogFile(string filename)
        {
            if (File.Exists(filename)) return;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                XmlWriter writer = XmlWriter.Create(filename);
                writer.WriteStartDocument();
                writer.WriteStartElement("Log");
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //String.Format("<message category='{0}' time='{1}>{2}</message>", category.ToString(),DateTime.Now.ToString("HH:mm:ss:fff"), message);
        //to-do : createLogFile, if not exists create, if node not exists create file
    }
}
