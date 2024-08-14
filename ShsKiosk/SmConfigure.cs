using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShsKiosk
{
    public class SmConfigure
    {
        DirectoryInfo di = new DirectoryInfo(@"Data\");
        string pathFileConfig = Path.Combine(Environment.CurrentDirectory, @"Data\", "configure.json");

        // ดึงข้อมูลบัตรกับ token เครื่อง UcAuthen
        public string registerComUrl { get; set; }

        public string searchOpcardUrl { get; set; }

        public string searchAppointUrl { get; set; }

        public string createVnUrl { get; set; }

        public string searchByHn { get; set; }

        public string printerName { get; set; }

        public string ipUc { get; set; }
        public string ipUc2 { get; set; }
        public string ipUc3 { get; set; }

        public string notifyHost { get; set; }


        public SmConfigure()
        {
            string json = File.ReadAllText(pathFileConfig);
            Config c = JsonConvert.DeserializeObject<Config>(json);

            string brokerUrl = $"http://{c.ipBroker}/kioskbroker/";

            this.registerComUrl = $"http://{c.ipUc}/getvalue.php";
            this.searchOpcardUrl = brokerUrl + "searchOpcard.php";
            this.searchAppointUrl = brokerUrl + "searchAppoint.php";
            this.createVnUrl = brokerUrl + "saveVn.php";
            this.searchByHn = brokerUrl + "searchByHn.php";
            this.printerName = c.printerName;

            this.ipUc = c.ipUc;
            this.ipUc2 = c.ipUc2;
            this.ipUc3 = c.ipUc3;
            this.notifyHost = c.notifyHost;
            
        }
    }

    class Config
    {
        public string ipUc { get; set; }
        public string ipUc2 { get; set; }
        public string ipUc3 { get; set; }
        public string ipBroker { get; set; }
        public string printerName { get; set; }
        public string notifyHost { get; set; }
    }
}
