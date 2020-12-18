using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShsKiosk
{
    public class SmConfigure
    {
        private string brokerUrl = "http://localhost/smbroker/";

        // ดึงข้อมูลบัตรกับ token เครื่อง UcAuthen
        public string registerComUrl { get; set; }

        public string searchOpcardUrl { get; set; }

        public string searchAppointUrl { get; set; }

        public string createVnUrl { get; set; }

        public string searchByHn { get; set; }

        public SmConfigure()
        {
            this.registerComUrl = "http://192.168.140.159/getvalue.php";
            this.searchOpcardUrl = this.brokerUrl + "searchOpcard.php";
            this.searchAppointUrl = this.brokerUrl + "searchAppoint.php";
            this.createVnUrl = this.brokerUrl + "saveVn.php";
            this.searchByHn = this.brokerUrl + "searchByHn.php";
        }
    }
}
