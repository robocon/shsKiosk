using Newtonsoft.Json;
using ShsKiosk.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Policy;
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
        static nhsoDataSetC1 pt;

        private static System.Timers.Timer aTimer;

        public Form1()
        {
            InitializeComponent();

            // Event Key สำหรับ Aibecy MP2600
            this.KeyPreview = true;

            aTimer = new System.Timers.Timer(5000);
            aTimer.Elapsed += TimerElapsed;
        }
        private string input;
        private void TimerElapsed(object sender, EventArgs e)
        {
            Console.WriteLine("TimerElapsed: TIME STOPPPPPPP");
            //input = string.Empty;
            //aTimer.Stop();
            aTimer.Enabled = false;
            testGetKeyChar = "";
            //aTimer = null;
        }


        string[] cardReaders;
        private void Form1_Load_1(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;

            // แสดงรูปวิธีใช้ที่หน้าตู้
            Image img = Image.FromFile("Images/mainPic.png");
            Bitmap Photo1 = new Bitmap(img, new Size(845, 750));
            mainPicLabel.Text = "";
            mainPicLabel.Image = Photo1;
            mainPicLabel.Size = new Size(845, 750);

            try
            {
                Console.WriteLine("Form1 was loaded");
                idcard = new ThaiIDCard();
                cardReaders = idcard.GetReaders();
                Console.WriteLine(cardReaders[0].ToString());
                idcard.MonitorStart(cardReaders[0].ToString());
                idcard.eventCardInserted += new handleCardInserted(CardInsertedCallback);
                idcard.eventCardRemoved += new handleCardRemoved(CardRemoveCallback);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                label1.Text = "ไม่พบเครื่องอ่านบัตรสมาร์ตการ์ด";
            }
        }

        public void CardRemoveCallback()
        {
            label1SetText("");
            pictureBox1Status(false);
            Console.WriteLine(cardReaders[0].ToString());
            Console.WriteLine("Card was remove");
            Console.WriteLine(idcard.Error());
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
            label1SetText("ระบบกำลังตรวจสอบข้อมูล กรุณารอสักครู่...");
            pictureBox1Status(true);

            var logger = new Logger();

            // ดึงค่าจากบัตรประชาชน
            var person = await RunCardReadder();
            if (person == null)
            {
                label1SetText("ไม่พบข้อมูลบัตรประชาชน กรุณาตรวจสอบชิปการ์ด");
                pictureBox1Status(false);
                logger.Log("เสียบบัตรประชาชน แต่ไม่พบข้อมูล");
            }
            else
            {
                string idcard = person.Citizenid;
                logger.Log("เสียบบัตรประชาชน ข้อมูล "+idcard);
                Bitmap Photo1 = new Bitmap(person.PhotoBitmap, new Size(160, 200));

                UcwsNhso(idcard, Photo1, true);

            }
        }

        public async Task<Personal> RunCardReadder()
        {
            string logTxt = "get data from cardreader";
            Console.WriteLine(logTxt);
            var person = await Task.Run(() => GetPersonalCardreader());
            return person;
        }

        public Personal GetPersonalCardreader()
        {
            idcard = new ThaiIDCard();
            Personal person = idcard.readAllPhoto();
            return person;
        }

        static async Task<string> LoadRegisterToken(string url)
        {
            string nhsoContent = null;
            try
            {
                Console.WriteLine("โหลดข้อมูลจากทะเบียน {0}", url);
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                nhsoContent = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error Message :{0} ", e.Message);
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

        // ฟอร์มกดเลขบัตรประชาชนที่หน้าตู้
        private void button2_Click_1(object sender, EventArgs e)
        {
            labelxx.Focus();
            FormManualIdcard frm = new FormManualIdcard();
            frm.ShowDialog();
        }

        public async void UcwsNhso(string idcard, Bitmap Photo1, Boolean cardStatus)
        {
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

            HttpResponseMessage response = await client.GetAsync("http://localhost/kioskbroker/getToken.php");
            response.EnsureSuccessStatusCode();
            string nhsoContent = await response.Content.ReadAsStringAsync();
            

            if (String.IsNullOrEmpty(nhsoContent))
            {
                label1SetText("กรุณาติดต่อห้องทะเบียน เพื่อทำการขอรหัส Authentication");
                pictureBox1Status(false);
                return;
            }

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
                if (pt.ws_status == "NHSO-00003")
                {
                    label1SetText("TOKEN หมดอายุการใช้งาน ติดต่อทะเบียนเพื่อเปิดใช้ UcAuthentication อีกครั้ง");
                    pictureBox1Status(false);
                    return;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                label1SetText("ไม่สามารถติดต่อกับ WebService สปสช ได้");
                pictureBox1Status(false);
                return;
            }
            */


            string moreTxt = "";
            // ถ้า maininscl เป็นค่าว่างแสดงว่าไม่มีสิทธิอะไรเลย ให้สงสัยก่อนว่าเป็นเงินสด
            // ถ้ามี new_maininscl แสดงว่ามีสิทธิใหม่เกิดขึ้น เช่น หมดสิทธิ ปกส. แล้วไปใช้ 30บาท หรืออื่นๆ
            /*
            if (String.IsNullOrEmpty(pt.maininscl) || !String.IsNullOrEmpty(pt.new_maininscl))
            {
                label1SetText("รหัสสิทธิหลักของท่านมีการเปลี่ยนแปลง\nกรุณาติดต่อห้องทะเบียน เพื่อทำการทบทวนสิทธิอีกครั้ง");
                pictureBox1Status(false);
                return;
            }
            */

            // แจ้งเตือน
            /*
            if ((!String.IsNullOrEmpty(pt.hmain) && pt.hmain != "11512") || (!String.IsNullOrEmpty(pt.new_hmain) && pt.new_hmain != "11512"))
            {
                moreTxt += "แจ้งเตือน! : สถานพยาบาลหลักของท่านไม่ใช่ โรงพยาบาลค่ายสุรศักดิ์มนตรี ท่านจะได้สิทธิเป็นเงินสด \n";
            }
            */

            var logger = new Logger();

            // ตรวจสอบ HN 
            Console.WriteLine("ดึงข้อมูลจาก Opcard");
            string testOpcard = await Task.Run(() => searchFromSm(smConfig.searchOpcardUrl, idcard));
            responseOpcard resultOpcard = JsonConvert.DeserializeObject<responseOpcard>(testOpcard);
            if (resultOpcard.opcardStatus == "n")
            {
                logger.Log("ไม่พบข้อมูลจาก opcard");
                label1SetText(resultOpcard.errorMsg);
                pictureBox1Status(false);
                return;
            }
            Console.WriteLine(testOpcard);
            logger.Log("ข้อมูลจาก opcard HN: "+ resultOpcard.hn);

            string correlationId = "";
            string pid = "";
            if (cardStatus)
            {
                Console.WriteLine("ดึงค่าจาก Service smart card");
                HttpResponseMessage resSmartCard = await client.GetAsync("http://localhost:8189/api/smartcard/read?readImageFlag=true");
                resSmartCard.EnsureSuccessStatusCode();
                string smartCardString = await resSmartCard.Content.ReadAsStringAsync();
                Console.WriteLine("Nhso data : " + smartCardString);
                
                resSmartCard smartcard = JsonConvert.DeserializeObject<resSmartCard>(smartCardString);
                logger.Log("ข้อมูลจาก nhso : " + smartcard.correlationId+" --> "+smartcard.pid);

                correlationId = smartcard.correlationId;
                pid = smartcard.pid;
            }
            
            /*
            if (resultOpcard.PtRightMain != resultOpcard.PtRightSub)
            {
                label1SetText("แจ้งเตือน! : สิทธิหลัก และสิทธิรอง ไม่ตรงกัน กรุณาติดต่อห้องทะเบียนเพื่อทบทวนสิทธิ");
                pictureBox1Status(false);
                return;
            }
            */
            // ตรวจสิทธิหลักสิทธิรอง

            // ตรวจสอบการนัดหมาย
            Console.WriteLine($"ค้นหาการนัด {smConfig.searchAppointUrl} {idcard}");
            string content = await Task.Run(() => searchFromSm(smConfig.searchAppointUrl, idcard));
            Console.WriteLine(content);
            string appointContent = "";
            int appointCount = 0;
            string appointStatus = "";
            if (!string.IsNullOrEmpty(content))
            {
                responseAppoint result = JsonConvert.DeserializeObject<responseAppoint>(content);
                appointStatus = result.appointStatus;
                if (appointStatus == "y")
                {
                    logger.Log("ข้อมูลการนัด "+ result.appointContent);
                    appointContent = result.appointContent;
                    appointCount = int.Parse(result.appointCount);
                    appoint = result.appoint;
                }
                else
                {
                    logger.Log("ไม่พบข้อมูลการนัด");
                    label1SetText(result.errorMsg);
                    pictureBox1Status(false);
                    return;
                }
            }
            pictureBox1Status(false);

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

            string hmain = resultOpcard.hospcode;
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

            Form2 frm = new Form2();
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
            frm.hosPtRight = resultOpcard.hosPtRight;
            frm.moreTxt = moreTxt;

            frm.mobile = resultOpcard.mobile;
            frm.claimType = "PG0060001";
            frm.hcode = "11512";
            frm.correlationId = correlationId;
            frm.pid = pid;
            frm.cardStatus = cardStatus;

            frm.ShowDialog();

            label1SetText("");
        }

        private string testGetKeyChar = "";

        /**
         * Action ที่มาจากการ Scan Barcode
         */
        public string hn = "";
        public string fullTxt = "";
        private async void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            testGetKeyChar += e.KeyChar;
            if (e.KeyChar == (char)13)
            {
                // หน่วงเวลาไว้ที่ 5วิ
                if (!aTimer.Enabled)
                {
                    aTimer.Enabled = true;
                    //Console.WriteLine(testGetKeyChar.Trim());

                    string hn = testGetKeyChar.Trim();
                    Console.WriteLine(hn);

                    Console.WriteLine("QR Code/Barcode Scanner was loaded");
                    //label1SetText("ระบบลงทะเบียนด้วยบาร์โค้ดยังไม่เปิดใช้งาน ขออภัยในความไม่สะดวก\n(" + hn + ")");

                    // ตรวจสอบ HN 
                    string testOpcard = await Task.Run(() => searchFromSmByHn(smConfig.searchOpcardUrl, hn));
                    if (!string.IsNullOrEmpty(testOpcard))
                    {
                        responseOpcard resultOpcard = JsonConvert.DeserializeObject<responseOpcard>(testOpcard);

                        Bitmap origin = (Bitmap)Image.FromFile("Images/avatar.png");
                        Bitmap Photo1 = new Bitmap(origin, new Size(160, 200));

                        UcwsNhso(resultOpcard.idcard, Photo1, false);
                    }
                    else
                    {
                        label1SetText($"ไม่พบข้อมูล {smConfig.searchOpcardUrl}");
                    }

                    hn = testGetKeyChar = "";

                }
                else
                {
                    Console.WriteLine("Timer still working");
                    return;
                }
            }
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

    public class Appoint
    {
        public int rowId { set; get; }
        public string doctor { set; get; }
        public string detail { set; get; }
        public string room { set; get; }
        public string tdMD { set; get; }
    }

    public class responseAppoint
    {
        public string appointStatus { get; set; }
        public string appointContent { get; set; }
        public string appointCount { get; set; }
        public List<Appoint> appoint { get; set; }
        public string errorMsg { get; set; }
    }

    public class sendAppoint
    {
        public string Idcard { get; set; }
        public string hn { get; set; }
    }

    public class sendSearchOpCard
    {
        public string Idcard { get; set; }
        public string hn { get; set; }
    }

    public class responseOpcard
    {
        public string opcardStatus { set; get; }
        public string idcard { set; get; }
        public string hn { set; get; }
        public string ptname { set; get; }
        public string hosPtRight { set; get; }

        public string PtRightMain { set; get; }
        public string PtRightSub { set; get; }
        public string errorMsg { set; get; }
        public string hospcode { set; get; }
        public string mobile { set; get; }
    }

    public class resSmartCard
    {
        public string pid { set; get; }
        public string correlationId { set; get; }
    }

    public abstract class LogBase
    {
        public abstract void Log(string Messsage);
    }

    public class Logger : LogBase
    {

        private String CurrentDirectory
        {
            get;
            set;
        }

        private String FileName
        {
            get;
            set;
        }

        private String FilePath
        {
            get;
            set;
        }

        public Logger()
        {
            this.CurrentDirectory = Directory.GetCurrentDirectory();
            this.FileName = "Log.txt";
            this.FilePath = this.CurrentDirectory + "/logs/" + this.FileName;

        }

        public override void Log(string Messsage)
        {

            System.Console.WriteLine("Logged : {0}", Messsage);

            using (System.IO.StreamWriter w = System.IO.File.AppendText(this.FilePath))
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                w.WriteLine("  :{0}", Messsage);
                w.WriteLine("-----------------------------------------------");
            }
        }
    }

}
