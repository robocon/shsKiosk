using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShsConfig
{
    public partial class Form1 : Form
    {
        DirectoryInfo di = new DirectoryInfo(@"Data\");
        string pathFileConfig = Path.Combine(Environment.CurrentDirectory, @"Data\", "configure.json");
        string pathBrokerConfig = Path.Combine(Environment.CurrentDirectory, @"C:\AppServ\www\smbroker\", "brokerDbConfig.json");

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Config config = new Config();
                config.ipUc = ipUc.Text.Trim();
                config.ipUc2 = ipUc2.Text.Trim();
                config.ipUc3 = ipUc3.Text.Trim();
                config.ipBroker = ipBroker.Text.Trim();
                config.printerName = printerName.Text.Trim();
                string jsonTxt = JsonConvert.SerializeObject(config);
                StreamWriter sw = new StreamWriter(pathFileConfig, false);
                sw.WriteLine(jsonTxt);
                sw.Flush();
                sw.Close();

                DbConfig DbConfig = new DbConfig();
                DbConfig.host = brokerHost.Text.Trim();
                DbConfig.user = brokerUser.Text.Trim();
                DbConfig.pass = brokerPass.Text.Trim();
                DbConfig.db = brokerDb.Text.Trim();
                string jsonDbTxt = JsonConvert.SerializeObject(DbConfig);
                StreamWriter swDb = new StreamWriter(pathBrokerConfig, false);
                swDb.WriteLine(jsonDbTxt);
                swDb.Flush();
                swDb.Close();

                notify.Text = "บันทึกข้อมูลเรียบร้อย";
            }
            catch (Exception ex)
            {
                notify.Text = ex.Message;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string json = File.ReadAllText(pathFileConfig);
                Config c = JsonConvert.DeserializeObject<Config>(json);
                ipUc.Text = c.ipUc;
                ipUc2.Text = c.ipUc2;
                ipUc3.Text = c.ipUc3;
                ipBroker.Text = c.ipBroker;
                printerName.Text = c.printerName;
                
                string jsonPath = File.ReadAllText(pathBrokerConfig);
                DbConfig cf = JsonConvert.DeserializeObject<DbConfig>(jsonPath);
                brokerHost.Text = cf.host;
                brokerUser.Text = cf.user;
                brokerPass.Text = cf.pass;
                brokerDb.Text = cf.db;
            }
            catch (Exception ex)
            {
                notify.Text = ex.Message;
            }
        }
    }

    class Config
    {
        public string ipUc { get; set; }
        public string ipUc2 { get; set; }
        public string ipUc3 { get; set; }
        public string ipBroker { get; set; }
        public string printerName { get; set; }
    }

    class DbConfig
    {
        public string host { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string db { get; set; }
    }
}
