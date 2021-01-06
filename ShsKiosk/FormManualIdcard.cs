using Newtonsoft.Json;
using ShsKiosk.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThaiNationalIDCard;

namespace ShsKiosk
{
    public partial class FormManualIdcard : Form
    {
        private ThaiIDCard idcard;
        public Personal person;
        static readonly HttpClient client = new HttpClient();
        List<Appoint> appoint;
        static readonly SmConfigure smConfig = new SmConfigure();
        static nhsoDataSetC1 pt;

        public FormManualIdcard()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            Refresh();
            Console.WriteLine("Form manual was loaded");
            labelTitle.Text = "กรอกเลขบัตรประชาชน หรือ HN\nกดตรวจสอบสิทธิ";
        }

        public void ButtonAddIdcard_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            textBox1.Text = textBox1.Text + button.Text;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var tl = textBox1.Text.Length;
            if (tl > 0) {
                textBox1.Text = textBox1.Text.Substring(0, tl - 1);
            }
        }

        // ปุ่มเช็กสิทธิ
        private async void button12_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Button เช็กสิทธิ์ was clicked");
            label2.Text = "ระบบกำลังตรวจสอบสิทธิ กรุณารอสักครู่...";
            pictureBox1.Visible = true;
            Refresh();

            string idcard = textBox1.Text;
            string hosPtRight;

            // ถ้าเป็น hn จะมีขีดกลาง
            if (Regex.IsMatch(idcard, "-", RegexOptions.IgnoreCase))
            {
                Console.WriteLine("Check from HN");
                // ตรวจสอบ HN 
                string testOpcard = await Task.Run(() => searchFromSm(smConfig.searchByHn, idcard));
                responseOpcard resultOpcard = JsonConvert.DeserializeObject<responseOpcard>(testOpcard);
                if (resultOpcard.opcardStatus == "n")
                {
                    label2.Text = "ไม่พบข้อมูลผู้ป่วย กรุณาติดต่อห้องทะเบียนเพื่อลงทะเบียน";
                    pictureBox1.Visible = false;
                    return;
                }
                else
                {
                    idcard = resultOpcard.idcard;
                    hosPtRight = resultOpcard.hosPtRight;
                }
            }
            else
            {
                Console.WriteLine("Check from idcard");
                if ( idcard.Length != 13 )
                {
                    label2.Text = "หมายเลขบัตรประชาชนไม่ครบ13หลัก\nกรุณาตรวจสอบหมายเลขบัตรของท่านอีกครั้ง";
                    pictureBox1.Visible = false;
                    return;
                }

                string testOpcard = await Task.Run(() => searchFromSm(smConfig.searchOpcardUrl, idcard));
                responseOpcard resultOpcard = JsonConvert.DeserializeObject<responseOpcard>(testOpcard);
                hosPtRight = resultOpcard.hosPtRight;

            }

            // ดึง Token จากเครื่องแม่
            string nhsoContent = await Task.Run(() => Ajax());
            if (string.IsNullOrEmpty(nhsoContent))
            {
                label2.Text = "ไม่สามารถติดต่อเซิฟเวอร์ตรวจสอบสิทธิได้ กรุณาติดต่อห้องทะเบียน เพื่อให้เจ้าหน้าที่ตรวจสอบข้อมูล";
                pictureBox1.Visible = false;
                return;
            }
            Console.WriteLine("Get nhso content from 192");

            string[] nhso = nhsoContent.Split('#');

            string staffIdCard = nhso[0];
            string nhsoToken = nhso[1];

            // ดึงข้อมูลสิทธิการรักษาจาก สปสช
            try
            {
                UCWSTokenP1Client soapClient = new UCWSTokenP1Client();
                pt = new nhsoDataSetC1();
                pt = soapClient.searchCurrentByPID(staffIdCard, nhsoToken, idcard);
                if (pt == null || pt.ws_status == "NHSO-00003")
                {
                    label2.Text = "TOKEN หมดอายุการใช้งาน กรุณายืนยันตัวตนผ่านโปรแกรม UcAuthentication MX อีกครั้ง";
                    pictureBox1.Visible = false;
                    return;
                }
                Console.WriteLine("Get soap data from nhso");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                label2.Text = "ขออภัยในความไม่สะดวก\nการเชือมต่อมีปัญหา\nกรุณากรอก HN หรือเลขบัตรประชาชนอีกครั้ง";
                return;
            }

            // ตรวจสอบการนัดหมาย
            string content = await Task.Run(() => searchFromSm(smConfig.searchAppointUrl, idcard));
            responseAppoint result = JsonConvert.DeserializeObject<responseAppoint>(content);

            string appointContent = "";
            int appointCount = 0;
            string appointStatus = "";

            appointStatus = result.appointStatus;
            if (appointStatus == "y")
            {
                Console.WriteLine("Get appoint data");
                appointContent = result.appointContent;
                appointCount = int.Parse(result.appointCount);
                appoint = result.appoint;
            }

            //pictureBox1.Visible = false;

            // ถ้า maininscl เป็นค่าว่างแสดงว่าไม่มีสิทธิอะไรเลย ให้สงสัยก่อนว่าเป็นเงินสด
            // ถ้ามี new_maininscl แสดงว่ามีสิทธิใหม่เกิดขึ้น เช่น หมดสิทธิ ปกส. แล้วไปใช้ 30บาท หรืออื่นๆ
            if (String.IsNullOrEmpty(pt.maininscl) || !String.IsNullOrEmpty(pt.new_maininscl))
            {
                label2.Text = "สิทธิหลักของท่านมีการเปลี่ยนแปลง กรุณาติดต่อห้องทะเบียน\nเพื่อทำการตรวจสอบสิทธิ";
                //Console.WriteLine("MAININSCL: "+pt.maininscl+" NEW_MAININSCL: "+pt.new_maininscl);
                pictureBox1.Visible = false;
                return; 
            }

            string moreTxt = "";
            if ((!String.IsNullOrEmpty(pt.hmain) && pt.hmain != "11512") || (!String.IsNullOrEmpty(pt.new_hmain) && pt.new_hmain != "11512"))
            {
                moreTxt = "แจ้งเตือน! : สถานพยาบาลหลักของท่านไม่ใช่ โรงพยาบาลค่ายสุรศักดิ์มนตรี ท่านจะได้สิทธิเป็นเงินสด";
            }

            string maininscl = "";
            string maininsclCode = "";
            if (!String.IsNullOrEmpty(pt.maininscl))
            {
                maininsclCode = pt.maininscl;
                maininscl = $"( { pt.maininscl } ) { pt.maininscl_name }";
            }
            else if (!String.IsNullOrEmpty(pt.new_maininscl))
            {
                maininsclCode = pt.new_maininscl;
                maininscl = $"( { pt.new_maininscl } ) { pt.new_maininscl_name }";
            }

            string subinscl = "";
            if (!String.IsNullOrEmpty(pt.subinscl))
            {
                subinscl = $"( { pt.subinscl } ) { pt.subinscl_name }";
            }
            else if (!String.IsNullOrEmpty(pt.new_subinscl))
            {
                subinscl = $"( { pt.new_subinscl } ) { pt.new_subinscl_name }";
            }

            string hmain = "";
            if (!String.IsNullOrEmpty(pt.hmain))
            {
                hmain = $"( { pt.hmain } ) { pt.hmain_name }";
            }
            else if (!String.IsNullOrEmpty(pt.new_hmain))
            {
                hmain = $"( { pt.new_hmain } ) { pt.new_hmain_name }";
            }

            Bitmap origin = (Bitmap)Image.FromFile("Images/avatar.png");
            Bitmap Photo1 = new Bitmap(origin, new Size(160, 200));
            Form2 frm = new Form2();
            frm.fullname = pt.fname + " " + pt.lname;
            frm.idcard = idcard;

            frm.mainInSclName = maininscl;
            frm.subInSclName = subinscl;
            frm.hMainName = hmain;
            frm.personImage = Photo1;
            frm.ptRight = maininsclCode;

            frm.appointStatus = appointStatus;
            frm.appointContent = appointContent;
            frm.appointCount = appointCount;
            frm.appoint = appoint;

            frm.moreTxt = moreTxt;

            frm.hosPtRight = hosPtRight;

            frm.ShowDialog();

            this.Close();
        }

        static async Task<string> Ajax()
        {
            string nhsoContent = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(smConfig.registerComUrl);
                response.EnsureSuccessStatusCode();
                nhsoContent = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return nhsoContent;
        }

        public Personal GetPersonalCardreader()
        {
            idcard = new ThaiIDCard();
            Personal person = idcard.readAllPhoto();
            return person;
        }

        static async Task<string> searchFromSm(string posturi, string idcard)
        {
            string content = null;
            try
            {
                sendAppoint appoint = new sendAppoint();
                appoint.Idcard = idcard;

                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync(posturi, appoint);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(content);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return content;
        }
    }
}
