using Newtonsoft.Json;
using System;
using System.Net.Http;
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
        static readonly HttpClient client = new HttpClient();
        static readonly SmConfigure smConfig = new SmConfigure();

        private void Form3_Load(object sender, EventArgs e)
        {

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
            string content = await Task.Run(() => SaveVn(smConfig.createVnUrl, idcard, appointId, exType));
            responseSaveVn result = JsonConvert.DeserializeObject<responseSaveVn>(content);

            this.Close();

            return;
        }

        static async Task<string> SaveVn(string posturi, string idcard, int appointId, string exType = "")
        {
            string content = null;
            try
            {
                saveVn savevn = new saveVn();
                savevn.Idcard = idcard;
                savevn.appointId = appointId;
                savevn.exType = exType;

                var response = await client.PostAsJsonAsync(posturi, savevn);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return content;
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
