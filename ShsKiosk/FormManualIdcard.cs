using Newtonsoft.Json;
using ShsKiosk.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
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
            //labelTitle.Text = "กรอกเลขบัตรประชาชน หรือ HN\nกดตรวจสอบสิทธิ";
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
            label2.Text = "ระบบกำลังตรวจสอบสิทธิ กรุณารอสักครู่...";
            pictureBox1.Visible = true;
            Refresh();

            string textBoxTest = textBox1.Text;
            string hosPtRight = "";
            string idcard = "";
            responseOpcard resultOpcard = new responseOpcard();
            // ตรวจสอบข้อมูลเบื้องต้นจาก HN และเลขบัตรประชาชน
            // ถ้าเป็น hn จะมีขีดกลาง
            if (Regex.IsMatch(textBoxTest, "-", RegexOptions.IgnoreCase))
            {
                Console.WriteLine($"Manual ค้นหาจาก HN {smConfig.searchOpcardUrl}");
                // ตรวจสอบ HN 
                string testOpcard = await Task.Run(() => searchFromSmByHn(smConfig.searchOpcardUrl, textBoxTest));
                if (!string.IsNullOrEmpty(testOpcard))
                {
                    resultOpcard = JsonConvert.DeserializeObject<responseOpcard>(testOpcard);
                    if (resultOpcard.opcardStatus == "n")
                    {
                        label2.Text = resultOpcard.errorMsg;
                        pictureBox1.Visible = false;
                        return;
                    }
                    idcard = resultOpcard.idcard;
                    hosPtRight = resultOpcard.hosPtRight;
                }
            }
            else
            {
                Console.WriteLine($"Manual ค้นหาจาก idcard {smConfig.searchOpcardUrl}");
                if (textBoxTest.Length != 13 )
                {
                    label2.Text = "หมายเลขบัตรประชาชนไม่ครบ13หลัก\nกรุณาตรวจสอบหมายเลขบัตรของท่านอีกครั้ง";
                    pictureBox1.Visible = false;
                    this.ActiveControl = textBox1;
                    return;
                }

                string testOpcard = await Task.Run(() => searchFromSm(smConfig.searchOpcardUrl, textBoxTest));
                if (!string.IsNullOrEmpty(testOpcard))
                {
                    resultOpcard = JsonConvert.DeserializeObject<responseOpcard>(testOpcard);
                    if (resultOpcard.opcardStatus == "n")
                    {
                        label2.Text = resultOpcard.errorMsg;
                        pictureBox1.Visible = false;
                        return;
                    }
                    idcard = resultOpcard.idcard;
                    hosPtRight = resultOpcard.hosPtRight;
                }
            }
            Console.WriteLine(idcard);
            string moreTxt = "";
            /*
            if (resultOpcard.PtRightMain != resultOpcard.PtRightSub)
            {
                label2.Text = "แจ้งเตือน! : สิทธิหลักและสิทธิรองไม่ตรงกัน กรุณาติดต่อห้องทะเบียนเพื่อทบทวนสิทธิ\n";
                pictureBox1.Visible = false;
                return;
            }
            */

            // ดึง Token จากเครื่องแม่
            Console.WriteLine("ตรวจสอบ Token จากเครื่องห้องทะเบียน");
            // ดึง Token จากเครื่องแม่
            /*
            string nhsoContent = await Task.Run(() => LoadRegisterToken($"http://{smConfig.ipUc}/getvalue.php"));
            if (string.IsNullOrEmpty(nhsoContent))
            {
                nhsoContent = await Task.Run(() => LoadRegisterToken($"http://{smConfig.ipUc2}/getvalue.php"));
                if (string.IsNullOrEmpty(nhsoContent))
                {
                    nhsoContent = await Task.Run(() => LoadRegisterToken($"http://{smConfig.ipUc3}/getvalue.php"));
                }
            }
            */

            /*
            HttpResponseMessage response = await client.GetAsync("http://localhost/kioskbroker/getToken.php");
            response.EnsureSuccessStatusCode();
            string nhsoContent = await response.Content.ReadAsStringAsync();
            */

            /*
            if (String.IsNullOrEmpty(nhsoContent))
            {
                label2.Text = "กรุณาติดต่อห้องทะเบียน เพื่อทำการขอรหัส Authentication";
                pictureBox1.Visible = false;
                return;
            }
            */

            /*
            string[] nhso = nhsoContent.Split('#');
            string staffIdCard = nhso[0];
            string nhsoToken = nhso[1];
            // ดึงข้อมูลสิทธิการรักษาจาก สปสช
            try
            {
                Console.WriteLine("ทำการเชื่อมต่อกับ WebService สปสช");
                UCWSTokenP1Client soapClient = new UCWSTokenP1Client();
                pt = new nhsoDataSetC1();
                pt = soapClient.searchCurrentByPID(staffIdCard, nhsoToken, idcard);
                if (pt == null || pt.ws_status == "NHSO-00003")
                {
                    label2.Text = "TOKEN หมดอายุการใช้งาน ติดต่อทะเบียนเพื่อเปิดใช้ UcAuthentication อีกครั้ง";
                    pictureBox1.Visible = false;
                    return;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                label2.Text = "ไม่สามารถติดต่อกับ WebService สปสช ได้";
                pictureBox1.Visible = false;
                return;
            }
            */

            // ถ้า maininscl เป็นค่าว่างแสดงว่าไม่มีสิทธิอะไรเลย ให้สงสัยก่อนว่าเป็นเงินสด
            // ถ้ามี new_maininscl แสดงว่ามีสิทธิใหม่เกิดขึ้น เช่น หมดสิทธิ ปกส. แล้วไปใช้ 30บาท หรืออื่นๆ
            /*
            if (String.IsNullOrEmpty(pt.maininscl) || !String.IsNullOrEmpty(pt.new_maininscl))
            {
                label2.Text = "สิทธิหลักของท่านมีการเปลี่ยนแปลง กรุณาติดต่อห้องทะเบียน\nเพื่อทำการตรวจสอบสิทธิ";
                pictureBox1.Visible = false;
                return;
            }

            if ((!String.IsNullOrEmpty(pt.hmain) && pt.hmain != "11512") || (!String.IsNullOrEmpty(pt.new_hmain) && pt.new_hmain != "11512"))
            {
                moreTxt = "แจ้งเตือน! : สถานพยาบาลหลักของท่านไม่ใช่ โรงพยาบาลค่ายสุรศักดิ์มนตรี ท่านจะได้สิทธิเป็นเงินสด";
            }
            */

            Console.WriteLine($"ค้นหาการนัด {smConfig.searchAppointUrl} {idcard}");
            responseAppoint result = new responseAppoint();
            // ตรวจสอบการนัดหมาย
            string content = await Task.Run(() => searchFromSm(smConfig.searchAppointUrl, idcard));
            Console.WriteLine(content);
            string appointContent = "";
            int appointCount = 0;
            string appointStatus = "";

            if (!string.IsNullOrEmpty(content))
            {
                result = JsonConvert.DeserializeObject<responseAppoint>(content);
                appointStatus = result.appointStatus;
                if (appointStatus == "y")
                {
                    appointContent = result.appointContent;
                    appointCount = int.Parse(result.appointCount);
                    appoint = result.appoint;
                }
                else
                {
                    label2.Text = result.errorMsg;
                    pictureBox1.Visible = false;
                    return;
                }
            }

            // 
            string maininscl = "";
            string maininsclCode = "";
            /*
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
            */

            string subinscl = "";
            /*
            if (!String.IsNullOrEmpty(pt.subinscl))
            {
                subinscl = $"( { pt.subinscl } ) { pt.subinscl_name }";
            }
            else if (!String.IsNullOrEmpty(pt.new_subinscl))
            {
                subinscl = $"( { pt.new_subinscl } ) { pt.new_subinscl_name }";
            }
            */

            string hmain = "";
            /*
            if (!String.IsNullOrEmpty(pt.hmain))
            {
                hmain = $"( { pt.hmain } ) { pt.hmain_name }";
            }
            else if (!String.IsNullOrEmpty(pt.new_hmain))
            {
                hmain = $"( { pt.new_hmain } ) { pt.new_hmain_name }";
            }
            */

            Bitmap origin = (Bitmap)Image.FromFile("Images/avatar.png");
            Bitmap Photo1 = new Bitmap(origin, new Size(160, 200));
            Form2 frm = new Form2();
            //frm.fullname = pt.fname + " " + pt.lname;
            frm.fullname = resultOpcard.ptname;
            frm.idcard = idcard;

            frm.mainInSclName = maininscl;
            frm.subInSclName = subinscl;
            frm.hMainName = hmain;
            frm.personImage = Photo1;
            frm.ptRight = maininsclCode;
            frm.hn = resultOpcard.hn;
            frm.ptname = resultOpcard.ptname;

            frm.appointStatus = appointStatus;
            frm.appointContent = appointContent;
            frm.appointCount = appointCount;
            frm.appoint = appoint;

            frm.moreTxt = moreTxt;

            frm.hosPtRight = hosPtRight;
            label2.Text = "";
            frm.ShowDialog();

            this.Close();
        }

        static async Task<string> LoadRegisterToken(string url)
        {
            string nhsoContent = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
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
                sendSearchOpCard appoint = new sendSearchOpCard();
                appoint.Idcard = idcard;

                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync(posturi, appoint);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return content;
        }

        static async Task<string> searchFromSmByHn(string posturi, string hn)
        {
            string content = null;
            try
            {
                sendSearchOpCard appoint = new sendSearchOpCard();
                appoint.hn = hn;

                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync(posturi, appoint);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return content;
        }
    }
}
