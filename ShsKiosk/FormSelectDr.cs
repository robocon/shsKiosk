using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShsKiosk
{
    public partial class FormSelectDr : Form
    {
        public List<Appoint> appoint;
        public string idcard;
        public string hosPtRight;
        static readonly HttpClient client = new HttpClient();
        static readonly SmConfigure smConfig = new SmConfigure();

        public FormSelectDr()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            Console.WriteLine("form select doctor from appoint was loaded");

            int yStart = 120; // ความสูง เริ่มต้น
            foreach (Appoint app in appoint)
            {
                // Creating and setting the properties of Button 
                Button Mybutton = new Button();
                Mybutton.Location = new Point(336, yStart);
                Mybutton.TabStop = false;
                Mybutton.Text = app.doctor;
                Mybutton.TextAlign = ContentAlignment.MiddleCenter;
                Mybutton.Height = 150;
                Mybutton.AutoSize = true;
                //Mybutton.Size = new Size(390, 150);
                Mybutton.Font = new Font("TH Niramit AS", 48, FontStyle.Bold);

                Mybutton.Click += (object se, EventArgs ea) => {
                    sendVNandQueue(app.rowId, app.doctor);
                };

                // Adding this button to form 
                this.Controls.Add(Mybutton);

                yStart += 150; // ความสูงเท่ากับขนาดของปุ่ม
            }
        }

        public async void sendVNandQueue(int rowId, string doctor)
        {
            SaveVn sv = new SaveVn();
            string content = await Task.Run(() => sv.save(smConfig.createVnUrl, idcard, rowId, hosPtRight));
            if (!String.IsNullOrEmpty(content))
            {
                responseSaveVn app = JsonConvert.DeserializeObject<responseSaveVn>(content);
                EpsonSlip es = new EpsonSlip();
                es.printOutSlip(app);
                this.Close();
            }
        }
    }
}
