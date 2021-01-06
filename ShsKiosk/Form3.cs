using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShsKiosk
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public string idcard;
        public int appointId;
        public string hosPtRight;
        static readonly SmConfigure smConfig = new SmConfigure();

        private void Form3_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Form3 was loaded");
        }

        /**
         * ตรวจโรคทั่วไป
         */
        private void button1_Click(object sender, EventArgs e)
        {
            presetVn("ex01");
        }

        /// <summary>
        /// ออก VN แทนทะเบียนพร้อมกับ ปริ้นสลิป VN + คิว
        /// </summary>
        /// <param name="exType"></param>
        public async void presetVn(string exType)
        {
            Console.WriteLine(exType + " was click");
            SaveVn sv = new SaveVn();
            string content = await Task.Run(() => sv.save(smConfig.createVnUrl, idcard, appointId, hosPtRight, exType));
            if (!String.IsNullOrEmpty(content))
            {
                responseSaveVn app = JsonConvert.DeserializeObject<responseSaveVn>(content);
                EpsonSlip es = new EpsonSlip();
                es.printOutSlip(app);
                this.Close();
            }
        }

        /**
         * ทันตกรรม
         */
        private void button2_Click(object sender, EventArgs e)
        {
            presetVn("ex07");
        }

        /**
         * ตา
         */
        private void button3_Click(object sender, EventArgs e)
        {
            presetVn("ex25");
        }

        /**
         * ทันตกรรม
         */
        private void button4_Click(object sender, EventArgs e)
        {
            presetVn("ex17");
        }

        /**
         * นวดแผนไทย
         */
        private void button6_Click(object sender, EventArgs e)
        {
            presetVn("ex20");
        }

        /**
         * ฝังเข็ม
         */
        private void button5_Click(object sender, EventArgs e)
        {
            presetVn("ex92");
        }

        /**
         * ไตเทียม
         */
        private void button8_Click(object sender, EventArgs e)
        {
            presetVn("ex10");
        }

        /**
         * ฉีดยาต่อเนื่อง
         */
        private void button7_Click(object sender, EventArgs e)
        {
            presetVn("ex23");
        }
    }
}
