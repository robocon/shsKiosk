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
        string pathFileConfig = Path.Combine(Environment.CurrentDirectory, @"Data\", "configure.json");
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Config config = new Config();
                config.notifyHost = notifyHost.Text.Trim();
                config.ipBroker = ipBroker.Text.Trim();
                config.printerName = printerName.Text.Trim();
                string jsonTxt = JsonConvert.SerializeObject(config);
                StreamWriter sw = new StreamWriter(pathFileConfig, false);
                sw.WriteLine(jsonTxt);
                sw.Flush();
                sw.Close();
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
                notifyHost.Text = c.notifyHost;
                ipBroker.Text = c.ipBroker;
                printerName.Text = c.printerName;
            }
            catch (Exception ex)
            {
                notify.Text = ex.Message;
            }
        }
    }

    class Config
    {
        public string notifyHost { get; set; }
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
