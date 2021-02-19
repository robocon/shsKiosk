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
                config.ipBroker = ipBroker.Text.Trim();
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
                ipUc.Text = c.ipUc;
                ipBroker.Text = c.ipBroker;
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
        public string ipBroker { get; set; }
    }
}
