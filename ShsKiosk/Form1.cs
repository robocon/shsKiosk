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

    public partial class Form1 : Form
    {
        private ThaiIDCard idcard;
        public Personal person;
        static readonly HttpClient client = new HttpClient();
        List<Appoint> appoint;
        static readonly SmConfigure smConfig = new SmConfigure();

        public Form1()
        {
            InitializeComponent();

            // Event Key สำหรับ Aibecy MP2600
            this.KeyPreview = true;
            //this.KeyDown += Form1_KeyDown;
        }

        string[] cardReaders;

        private void Form1_Load_1(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;

            try
            {
                Console.WriteLine("Form1 was loaded");
                idcard = new ThaiIDCard();
                cardReaders = idcard.GetReaders();
                idcard.MonitorStart(cardReaders[0].ToString());
                idcard.eventCardInserted += new handleCardInserted(CardInsertedCallback);
                idcard.eventCardRemoved += new handleCardRemoved(CardRemoveCallback);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                button2.Hide();
                label1.Text = "ไม่พบเครื่องอ่านบัตรสมาร์ตการ์ด";
            }
        }

        public void CardRemoveCallback()
        {
            label1SetText("");
            pictureBox1Status(false);

            idcard.MonitorStop(cardReaders[0].ToString());
        }

        public void pictureBox1Status(bool status)
        {
            pictureBox1.BeginInvoke(new MethodInvoker(delegate { pictureBox1.Visible = status; }));
        }

        public void label1SetText(string label1Text)
        {
            label1.BeginInvoke(new MethodInvoker(delegate { label1.Text = label1Text; }));
        }

        private async void CardInsertedCallback(Personal personal)
        {
            Console.WriteLine("card was inserted");

            label1SetText("ระบบกำลังตรวจสอบสิทธิ กรุณารอสักครู่...");
            pictureBox1Status(true);

            // ดึงค่าจากบัตรประชาชน
            var person = await RunCardReadder();
            if (person == null)
            {
                label1SetText("ไม่พบข้อมูลบัตรประชาชน");
                pictureBox1Status(false);
            }
            else
            {
                string idcard = person.Citizenid;

                // ดึง Token จากเครื่องแม่
                string nhsoContent = await Task.Run(() => Ajax());
                if (String.IsNullOrEmpty(nhsoContent))
                {
                    label1SetText("ไม่พบ Token กรุณาประสานห้องทะเบียน");
                    return;
                }

                string[] nhso = nhsoContent.Split('#');

                Console.WriteLine("nhso token found");

                string staffIdCard = nhso[0];
                string nhsoToken = nhso[1];

                // ดึงข้อมูลสิทธิการรักษาจาก สปสช
                UCWSTokenP1Client soapClient = new UCWSTokenP1Client();
                nhsoDataSetC1 pt = new nhsoDataSetC1();
                pt = soapClient.searchCurrentByPID(staffIdCard, nhsoToken, idcard);

                if (pt.ws_status == "NHSO-00003")
                {
                    label1SetText("TOKEN หมดอายุการใช้งาน กรุณายืนยันตัวตนผ่านโปรแกรม UcAuthentication MX อีกครั้ง");
                    pictureBox1Status(false);
                    return;
                }

                // ตรวจสอบ HN 
                string testOpcard = await Task.Run(() => searchFromSm(smConfig.searchOpcardUrl, idcard));
                responseOpcard resultOpcard = JsonConvert.DeserializeObject<responseOpcard>(testOpcard);
                if (resultOpcard.opcardStatus == "n")
                {
                    label1SetText("ไม่พบ HN กรุณาติดต่อห้องทะเบียนเพื่อลงทะเบียน");
                    pictureBox1Status(false);
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
                    appointContent = result.appointContent;
                    appointCount = int.Parse(result.appointCount);
                    appoint = result.appoint;
                }

                pictureBox1Status(false);

                // ถ้า maininscl เป็นค่าว่างแสดงว่าไม่มีสิทธิอะไรเลย ให้สงสัยก่อนว่าเป็นเงินสด
                // ถ้ามี new_maininscl แสดงว่ามีสิทธิใหม่เกิดขึ้น เช่น หมดสิทธิ ปกส. แล้วไปใช้ 30บาท หรืออื่นๆ
                if (String.IsNullOrEmpty(pt.maininscl) || !String.IsNullOrEmpty(pt.new_maininscl))
                {
                    label1SetText("สิทธิหลักของท่านมีการเปลี่ยนแปลง กรุณาติดต่อห้องทะเบียน\nเพื่อทำการตรวจสอบสิทธิ");
                    pictureBox1Status(false);
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

                Bitmap Photo1 = new Bitmap(person.PhotoBitmap, new Size(160, 200));
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
                frm.hosPtRight = resultOpcard.hosPtRight;
                frm.moreTxt = moreTxt;
                frm.ShowDialog();

                label1SetText("");
            }
        }

        // ฟอร์มกดเลขบัตรประชาชนที่หน้าตู้
        private void button2_Click(object sender, EventArgs e)
        {
            FormManualIdcard frm = new FormManualIdcard();
            frm.ShowDialog();
        }

        public string hn = "";
        public string fullTxt = "";

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            KeysConverter kc = new KeysConverter();
            string testKey = kc.ConvertToString(e.KeyValue);
            fullTxt += testKey;
            
            if (testKey == "OemMinus")
            {
                hn += "-";
            }
            else if (Regex.IsMatch(testKey, "[0-9]", RegexOptions.IgnoreCase))
            {
                hn += testKey;
            }

            if (Regex.IsMatch(fullTxt, "(ControlKey)"))
            {
                Console.WriteLine(hn);

                label1SetText("ระบบลงทะเบียนด้วยบาร์โค้ดยังไม่เปิดใช้งาน ขออภัยในความไม่สะดวก\n(" + hn + ")");

                hn = fullTxt = "";
            }
        }

        public async Task<Personal> RunCardReadder()
        {
            Console.WriteLine("get data from cardreader");
            var person = await Task.Run(() => GetPersonalCardreader());
            return person;
        }

        public Personal GetPersonalCardreader()
        {
            idcard = new ThaiIDCard();
            Personal person = idcard.readAllPhoto();
            return person;
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
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return content;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            FormManualIdcard frm = new FormManualIdcard();
            frm.ShowDialog();
        }
    }

    public class Appoint
    {
        public int rowId { set; get; }

        public string doctor { set; get; }
    }

    public class responseAppoint
    {
        public string appointStatus { get; set; }
        public string appointContent { get; set; }
        public string appointCount { get; set; }
        public List<Appoint> appoint { get; set; }
    }

    public class sendAppoint
    {
        public string Idcard { get; set; }
    }

    public class responseOpcard
    {
        public string opcardStatus { set; get; }
        public string idcard { set; get; }
        public string hosPtRight { set; get; }
    }

}
