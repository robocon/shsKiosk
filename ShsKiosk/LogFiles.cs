using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShsKiosk
{
    class LogFiles
    {
        string fileName = "log.txt";
        StreamWriter w;
        public LogFiles()
        {
            /*
            using (StreamWriter w = File.AppendText("logs/"+fileName))
            {
                Log(logMessage, w);
            }
            */
            w = File.AppendText("logs/" + fileName);
        }

        public void WriteLog(string logMessage)
        {
            w.WriteLine($"\r\nLog Entry : {DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()} :{logMessage}");
        }
    }
}
