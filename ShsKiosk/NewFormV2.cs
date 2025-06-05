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
using System.Management;
using Microsoft.PointOfService;
using System.Web.Script.Serialization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace ShsKiosk
{
    public partial class NewFormV2 : Form
    {

        private ThaiIDCard idcard;
        public Personal person;
        static readonly SmConfigure smConfig = new SmConfigure();
        List<Appoint> appoint;

        private static System.Timers.Timer aTimer;

        string[] cardReaders;
        private string testGetKeyChar = "";
        
        private string searchOpcardUrl = "http://192.168.130.15/kioskbroker/searchOpcard.php";
        private string searchAppointUrl = "http://192.168.130.15/kioskbroker/searchAppoint.php";
        private string saveVnUrl = "http://192.168.130.15/kioskbroker/saveVn.php";

        private string savePhotoUrl = "http://192.168.131.250/sm3/save_photo.php";

        /**
         * Action ที่มาจากการ Scan Barcode
         */
        public string hn = "";
        public string fullTxt = "";

        public NewFormV2()
        {
            InitializeComponent();

            // Event Key สำหรับ Aibecy MP2600
            this.KeyPreview = true;

            aTimer = new System.Timers.Timer(5000);
            aTimer.Elapsed += TimerElapsed;

            iconLoader.Visible = false;
            description.Text = "";

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                description.Text = "ไม่พบเครื่องอ่านบัตรสมาร์ตการ์ด กรุณาต่อเครื่องอ่านบัตรแล้วเปิดโปรแกรมใหม่อีกครั้ง";
            }


        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            Console.WriteLine("TimerElapsed: TIME STOPPPPPPP");
            //input = string.Empty;
            //aTimer.Stop();
            aTimer.Enabled = false;
            testGetKeyChar = "";
            //aTimer = null;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            tableLayoutPanel3.Hide();
            this.WindowState = FormWindowState.Maximized;

        }

        /**
         * เมื่อถอดบัตร
         */
        public void CardRemoveCallback()
        {
            label1SetText("");
            pictureBox1Status(false);
            
            Console.WriteLine("Card was remove");
            Console.WriteLine(cardReaders[0].ToString());
            Console.WriteLine(idcard.Error());

            Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.BeginInvoke(new MethodInvoker(delegate { pictureBox1.Image = bm; }));

            // ล้างค่าเลขบัตรประชาชน + ชื่อสกุล + สิทธิการรักษา
            valueIdcard.BeginInvoke(new MethodInvoker(delegate { valueIdcard.Text = "-"; }));
            valueFullname.BeginInvoke(new MethodInvoker(delegate { valueFullname.Text = "-"; }));
            valuePtright.BeginInvoke(new MethodInvoker(delegate { valuePtright.Text = "-"; }));

            // ซ่อนตารางแสดงรายละเอียด
            tableLayoutPanel3.BeginInvoke(new MethodInvoker(delegate { tableLayoutPanel3.Hide(); }));
            

            /*idcard.MonitorStop(cardReaders[0].ToString());*/
        }

        // ปิดการแสดงรูป
        public void pictureBox1Status(bool status)
        {
            iconLoader.BeginInvoke(new MethodInvoker(delegate { iconLoader.Visible = status; }));
        }

        public void label1SetText(string label1Text)
        {
            description.BeginInvoke(new MethodInvoker(delegate { description.ForeColor = Color.Green; }));
            description.BeginInvoke(new MethodInvoker(delegate { description.Text = label1Text; }));
        }

        private async void CardInsertedCallback(Personal personal)
        {
            Console.WriteLine("card was inserted");
            label1SetText("กำลังตรวจสอบข้อมูลบัตรประชาชน กรุณารอสักครู่...");
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
                logger.Log("พบข้อมูลบัตรประชาชน : " + idcard);
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

        public async void UcwsNhso(string idcard, Bitmap Photo1, Boolean cardStatus)
        {
            var logger = new Logger();

            // ตรวจสอบ HN 
            label1SetText("กำลังตรวจสอบข้อมูลจากแผนกทะเบียน...");
            Console.WriteLine("ตรวจสอบข้อมูลจาก OPCARAD ( searchOpcard.php ) ");

            // searchOpcard.php
            Console.WriteLine($"ค้นหา opcard จากเลขบัตร {searchOpcardUrl} {idcard}");
            string testOpcard = await Task.Run(() => searchFromSm(searchOpcardUrl, idcard));
            if (String.IsNullOrEmpty(testOpcard))
            {
                label1SetText($"ไม่สามารถตรวจสอบข้อมูล Opcard ได้ Error: {searchOpcardUrl}");
                pictureBox1Status(false);
                return;
            }
            responseOpcard resultOpcard = JsonConvert.DeserializeObject<responseOpcard>(testOpcard);
            if (resultOpcard.opcardStatus == "n")
            {
                logger.Log("ไม่พบข้อมูลจาก opcard");
                label1SetText(resultOpcard.errorMsg);
                pictureBox1Status(false);
                return;
            }
            Console.WriteLine(testOpcard);
            var resultOpcardJson = new JavaScriptSerializer().Serialize(resultOpcard);
            logger.Log("ข้อมูลจาก opcard : " + resultOpcardJson);

            string idcardShow = "XXXXXXXX"+resultOpcard.idcard.Substring(8);
            Console.WriteLine(resultOpcard.idcard.Substring(8));

            // แสดงรายละเอียดเบื้องต้น
            pictureBox1.BeginInvoke(new MethodInvoker(delegate { pictureBox1.Image = Photo1; }));
            valueIdcard.BeginInvoke(new MethodInvoker(delegate { valueIdcard.Text = idcardShow; }));
            valueFullname.BeginInvoke(new MethodInvoker(delegate { valueFullname.Text = resultOpcard.ptname; }));
            valuePtright.BeginInvoke(new MethodInvoker(delegate { valuePtright.Text = resultOpcard.hosPtRight; }));

            tableLayoutPanel3.BeginInvoke(new MethodInvoker(delegate { tableLayoutPanel3.Show(); }));

            // ส่งรูปจากบัตรประชาชนไว้ที่ HOST
            //Bitmap Photo1 = new Bitmap(person.PhotoBitmap, new Size(207, 248));
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            Photo1.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

            await Task.Run(async () => {

                savePhoto pho = new savePhoto();
                pho.rawPhoto = Convert.ToBase64String(stream.ToArray());
                // pho.idCard = person.Citizenid;
                pho.idCard = resultOpcard.idcard;

                Console.WriteLine("====== Status : save photo from idcard ======");
                try
                {
                    HttpClient httpClient = new HttpClient();
                    var response = await httpClient.PostAsJsonAsync(savePhotoUrl, pho);
                    response.EnsureSuccessStatusCode();
                    string savePhoto = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("======"+savePhoto+ "======");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("====== Error : " + ex.Message+ "======");
                }

            });
            // ส่งรูปจากบัตรประชาชนไว้ที่ HOST


            label1SetText("กำลังตรวจสอบข้อมูลการนัดพบแพทย์ในวันนี้");
            // ตรวจสอบการนัดหมาย ( searchAppoint.php )
            Console.WriteLine($"ค้นหาการนัด จากเลขบัตรประชาชน {searchAppointUrl} {idcard}");
            string content = await Task.Run(() => searchFromSm(searchAppointUrl, idcard));
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
                    logger.Log("ข้อมูลการนัด " + result.appointContent);
                    appointContent = result.appointContent;
                    appointCount = int.Parse(result.appointCount);
                    appoint = result.appoint;

                }
                else
                {
                    logger.Log("ไม่พบข้อมูลการนัด");
                    System.Threading.Thread.Sleep(2000);
                    label1SetText(result.errorMsg);
                    description.BeginInvoke(new MethodInvoker(delegate { description.ForeColor = Color.Red; }));
                    pictureBox1Status(false);
                    return;
                }
            }

            /*string maininscl = "";*/
            string maininsclCode = "";
            string hmain = resultOpcard.hospcode;
            string ptRight = maininsclCode;
            string correlationId = "";
            logger.Log("cardStatus: " + cardStatus);
            /*string labelAlertForm2 = "";*/
            // ถ้ามีการเสียบบัตรประชาชน
            if (cardStatus == true)
            {
                string pid = "";

                Console.WriteLine("ดึงค่าจาก Service smart card");
                HttpClient localhost = new HttpClient();
                HttpResponseMessage resSmartCard = await localhost.GetAsync("http://localhost:8189/api/smartcard/read?readImageFlag=false");
                if (resSmartCard.IsSuccessStatusCode)
                {
                    //resSmartCard.EnsureSuccessStatusCode();
                    string smartCardString = await resSmartCard.Content.ReadAsStringAsync();
                    Console.WriteLine("Nhso data : " + smartCardString);

                    resSmartCard smartcard = JsonConvert.DeserializeObject<resSmartCard>(smartCardString);
                    logger.Log("ข้อมูลจาก nhso : " + smartcard.correlationId + " --> " + smartcard.pid);

                    correlationId = smartcard.correlationId;
                    pid = smartcard.pid;
                }
                else
                {
                    /*labelAlertForm2 = "ระบบ สปสช.สำนักงานใหญ่มีปัญหา ไม่สามารถขอ Authen Code ได้";*/
                    string nhsoError = "ระบบ สปสช.สำนักงานใหญ่มีปัญหา ไม่สามารถขอ Authen Code ได้";
                    logger.Log(nhsoError);

                    description.BeginInvoke(new MethodInvoker(delegate { description.ForeColor = Color.Red; }));
                    label1SetText(nhsoError);
                }

                saveNhsoService nhso = new saveNhsoService();
                nhso.pid = idcard;
                nhso.claimType = "PG0060001";
                nhso.mobile = resultOpcard.mobile.Trim();
                nhso.correlationId = correlationId;
                nhso.hn = hn;
                nhso.hcode = "11512";

                Console.WriteLine(nhso);
                logger.Log("http://localhost:8189/api/nhso-service/confirm-save");

                var nhsoJson = new JavaScriptSerializer().Serialize(nhso);

                logger.Log("Before Confirm Save : " + nhsoJson);


                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(nhso);
                Console.WriteLine("JSON CONVERT: " + json);
                StringContent jsonContent = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage nhsoSave = await client.PostAsync("http://localhost:8189/api/nhso-service/confirm-save", jsonContent);
                if (nhsoSave.IsSuccessStatusCode)
                {
                    string responseBody = await nhsoSave.Content.ReadAsStringAsync();
                    Console.WriteLine("Response FROM NHSO: " + responseBody);
                    responseNhsoService resNhsoService = JsonConvert.DeserializeObject<responseNhsoService>(responseBody);

                    logger.Log("nhso Authen Code : " + resNhsoService.claimCode);

                    String shsBrokerUrl = "http://" + smConfig.notifyHost + "/newauthen/shsBroker.php?action=save";
                    shsBrokerUrl += "&pid=" + System.Net.WebUtility.UrlEncode(pid);
                    shsBrokerUrl += "&claimType=" + System.Net.WebUtility.UrlEncode(nhso.claimType);
                    shsBrokerUrl += "&mobile=" + System.Net.WebUtility.UrlEncode(nhso.mobile);
                    shsBrokerUrl += "&correlationId=" + System.Net.WebUtility.UrlEncode(correlationId);
                    shsBrokerUrl += "&createdDate=" + resNhsoService.createdDate;
                    shsBrokerUrl += "&claimCode=" + System.Net.WebUtility.UrlEncode(resNhsoService.claimCode);
                    shsBrokerUrl += "&hn=" + System.Net.WebUtility.UrlEncode(hn);
                    shsBrokerUrl += "&hcode=" + System.Net.WebUtility.UrlEncode(nhso.hcode);
                    shsBrokerUrl += "&sOfficer=" + System.Net.WebUtility.UrlEncode("Kiosk");
                    Console.WriteLine("Send to save shsbroker: " + shsBrokerUrl);
                    logger.Log("Send to save shsbroker: " + shsBrokerUrl);
                    HttpClient shs = new HttpClient();
                    HttpResponseMessage resShs = await shs.GetAsync(shsBrokerUrl);
                    //var contentShs = resShs.Content.ReadAsStringAsync();
                    //Console.WriteLine("Response FROM Broker: "+contentShs);

                    label1SetText("ขอ Authen Code จาก สปสช เรียบร้อย");
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    string responseBody = await nhsoSave.Content.ReadAsStringAsync();
                    Console.WriteLine("Error :" + responseBody);
                    logger.Log("Error :" + responseBody);
                }
            }
            
            // ออก VN แล้วปริ้น สลิป
            SaveVn sv = new SaveVn();
            int appointRowId = appoint.ToArray()[0].rowId;
            // smConfig.createVnUrl
            string saveContent = await Task.Run(() => sv.save(saveVnUrl, idcard, appointRowId, ptRight));
            if (!String.IsNullOrEmpty(content))
            {
                responseSaveVn app = JsonConvert.DeserializeObject<responseSaveVn>(saveContent);
                label1SetText("ดำเนินการลงทะเบียนเสร็จเรียบร้อย กำลังพิมพ์ใบสั่งยา กรุณารอสักครู่...");
                System.Threading.Thread.Sleep(2000);

                EpsonSlip es = new EpsonSlip();
                es.printOutSlip(app);
            }

            label1SetText("");
            pictureBox1Status(false);
        }

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
                    string testOpcard = await Task.Run(() => searchFromSmByHn(searchOpcardUrl, hn));
                    if (!string.IsNullOrEmpty(testOpcard))
                    {
                        responseOpcard resultOpcard = JsonConvert.DeserializeObject<responseOpcard>(testOpcard);

                        Bitmap origin = (Bitmap)Image.FromFile("Images/avatar.png");
                        Bitmap Photo1 = new Bitmap(origin, new Size(160, 200));

                        UcwsNhso(resultOpcard.idcard, Photo1, false);
                    }
                    else
                    {
                        label1SetText($"ไม่พบข้อมูล {searchOpcardUrl}");
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
                Console.WriteLine("Error searchFromSm :{0} ", e.Message);
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
                Console.WriteLine("Error searchFromSmByHn :{0} ", e.Message);
            }

            return content;
        }
    }

    public class savePhoto
    {
        public string rawPhoto { set; get; }
        public string idCard { set; get; }
    }
}
