using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace ShsKiosk
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public string fullname;
        public string idcard;
        public string mainInSclName;
        public string subInSclName;
        public string hMainName;
        public string hn;
        public string ptname;
        
        public string ptRight;

        public string appointStatus;
        public string appointContent;
        public int appointCount;
        public List<Appoint> appoint;

        public string moreTxt;

        public string hosPtRight;

        public Bitmap personImage;
        static readonly HttpClient client = new HttpClient();
        static readonly SmConfigure smConfig = new SmConfigure();

        public string mobilePhone;
        public string claimType;
        public string hcode;
        public string correlationId;
        public string pid;
        public Boolean cardStatus;

        private void Form2_Load(object sender, EventArgs e)
        {
            Console.WriteLine(cardStatus);
            Console.WriteLine("Form2 was loaded");
            label12.BackColor = System.Drawing.Color.Transparent;
            label12.Hide();
            label13.Hide();

            button1.Hide();
            button3.Hide();

            loadingForm2.Hide();

            Refresh();
            ptnameHos.Text = ptname;
            label6.Text = idcard;
            label7.Text = fullname;
            label8.Text = hosPtRight;
            label9.Text = subInSclName;
            label10.Text = hMainName;
            pictureBox1.Image = personImage;

            labelAlert.Text = "";

            //
            //bool testLock = true;
            /*
            foreach (Appoint app in appoint)
            {
                if (app.room == "อาคารเฉลิมพระเกียรติ")
                {
                    //testLock = false;
                }
            }
            */
            bool testLock = false;


            if (testLock == true)
            {
                labelAlert.Text += "ขออภัยในความไม่สะดวก\nระบบอยู่ในระหว่างการทดสอบเฉพาะห้องตรวจโรคผู้ป่วยนอกอาคารเฉลิมพระเกียรติ\nกรุณาติดต่อห้องทะเบียน\n";
                button3.Hide();
                return;
            }

            // ถ้าไม่มีนัด จะแสดงปุ่มให้เปิด Form3
            if (appointCount == 0)
            {
                button1.Show();
            }
            else if(appointCount > 0) // ถ้ามี 1นัด
            {
                label12.Show();
                label13.Show();
                button3.Show();
                label12.Text = "";
                label12.Text += appointContent;
            }

            if (!string.IsNullOrEmpty(moreTxt)) {
                labelAlert.Text += moreTxt;
            }
        }

        /**
         * เปิดหน้า Form 3 หน้ารายการแต่ละแผนก
         */
        private void button1_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3();
            frm.idcard = idcard;
            frm.appointId = 0;
            frm.hosPtRight = hosPtRight;
            frm.ShowDialog();
            this.Close();
        }

        /**
         * ปิดหน้า Form 2
         */
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /**
         * ออก VN
         */
        private async void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            var logger = new Logger();
            if (cardStatus == true)
            {
                //mobilePhone = "0999999999";
                saveNhsoService nhso = new saveNhsoService();
                nhso.pid = pid;
                nhso.claimType = claimType;
                nhso.mobile = mobilePhone;
                nhso.correlationId = correlationId;
                nhso.hn = hn;
                nhso.hcode = hcode;

                Console.WriteLine(nhso);
                logger.Log("http://localhost:8189/api/nhso-service/confirm-save");

                var nhsoJson = new JavaScriptSerializer().Serialize(nhso);

                logger.Log("Before Confirm Save : " + nhsoJson);

                /*HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync("http://localhost:8189/api/nhso-service/confirm-save", nhso);
                response.EnsureSuccessStatusCode();
                string nhsoSave = await response.Content.ReadAsStringAsync();*/

                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(nhso);
                Console.WriteLine("JSON CONVERT: "+json);
                StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage nhsoSave = await client.PostAsync("http://localhost:8189/api/nhso-service/confirm-save", content);
                if (nhsoSave.IsSuccessStatusCode)
                {
                    string responseBody = await nhsoSave.Content.ReadAsStringAsync();
                    Console.WriteLine("Response FROM NHSO: "+responseBody);
                    responseNhsoService resNhsoService = JsonConvert.DeserializeObject<responseNhsoService>(responseBody);

                    logger.Log("nhso Authen Code : " + resNhsoService.claimCode);

                    String shsBrokerUrl = "http://"+ smConfig.notifyHost + "/newauthen/shsBroker.php?action=save";
                    shsBrokerUrl += "&pid=" + System.Net.WebUtility.UrlEncode(pid);
                    shsBrokerUrl += "&claimType=" + System.Net.WebUtility.UrlEncode(claimType);
                    shsBrokerUrl += "&mobile=" + System.Net.WebUtility.UrlEncode(mobilePhone);
                    shsBrokerUrl += "&correlationId=" + System.Net.WebUtility.UrlEncode(correlationId);
                    shsBrokerUrl += "&createdDate=" + resNhsoService.createdDate;
                    shsBrokerUrl += "&claimCode=" + System.Net.WebUtility.UrlEncode(resNhsoService.claimCode);
                    shsBrokerUrl += "&hn=" + System.Net.WebUtility.UrlEncode(hn);
                    shsBrokerUrl += "&hcode=" + System.Net.WebUtility.UrlEncode(hcode);
                    shsBrokerUrl += "&sOfficer=" + System.Net.WebUtility.UrlEncode("Kiosk");
                    Console.WriteLine("Send to save shsbroker: "+shsBrokerUrl);
                    logger.Log("Send to save shsbroker: " + shsBrokerUrl);
                    HttpClient shs = new HttpClient();
                    HttpResponseMessage resShs = await shs.GetAsync(shsBrokerUrl);
                    //var contentShs = resShs.Content.ReadAsStringAsync();
                    //Console.WriteLine("Response FROM Broker: "+contentShs);
                }
                else
                {
                    string responseBody = await nhsoSave.Content.ReadAsStringAsync();
                    Console.WriteLine("Error :"+ responseBody);
                    logger.Log("Error :" + responseBody);
                }
                // {"pid":"1509900231582","claimType":"PG0060001","correlationId":"a76b1ce8-40e8-4062-97cc-dc869ecbf19e","createdDate":"2023-08-04T15:32:17","claimCode":"PP1238504971"}

            }

            loadingForm2.Show();
            Console.WriteLine("Button3(ออกvn) was clicked");
            if (appointCount > 1) // ถ้ามีนัด 2แพทย์
            {
                Console.WriteLine("แสดงข้อมูลแพทย์หลายคน");
                FormSelectDr frm = new FormSelectDr();
                frm.appoint = appoint;
                frm.idcard = idcard;
                frm.hosPtRight = hosPtRight;
                frm.ShowDialog();
            }
            else // ถ้ามีนัด 1 แพทย์
            {
                Console.WriteLine("กดออก VN แพทย์คนเดียว");
                SaveVn sv = new SaveVn();
                int appointRowId = appoint.ToArray()[0].rowId;
                string content = await Task.Run(() => sv.save(smConfig.createVnUrl, idcard, appointRowId, ptRight));
                if (!String.IsNullOrEmpty(content))
                {
                    responseSaveVn app = JsonConvert.DeserializeObject<responseSaveVn>(content);
                    EpsonSlip es = new EpsonSlip();
                    es.printOutSlip(app);
                }
            }
            button3.Enabled = true;
            loadingForm2.Hide();
            this.Close();
        }
    }

    public class responseSaveVn
    {
        public string appointStatus { set; get; }
        public string dateSave { set; get; }
        public string ex { set; get; }
        public string vn { set; get; }
        public string ptname { set; get; }
        public string hn { set; get; }
        public string age { set; get; }
        public string mx { set; get; }
        public string ptright { set; get; }
        public string idcard { set; get; }
        public string hospCode { set; get; }
        public string doctor { set; get; }
        public string room { set; get; }
        public string queueStatus { set; get; }
        public string ptType { set; get; }
        public string queueNumber { set; get; }
        public int queueWait { set; get; }
        public string queueRoom { set; get; }
        public string runNumber { set; get; }
        public string fakeQueue { set; get; }
    }

    public class saveVn {
        public string Idcard { set; get; }
        public int appointId { set; get; }
        public string exType { set; get; }
        public string userPtRight { set; get; }
    }

    public class saveNhsoService
    {
        public string pid { set; get; }
        public string claimType { set; get; }
        public string mobile { set; get; }
        public string correlationId { set; get; }
        public string hn { set; get; }
        public string hcode { set; get; }
    }

    public class responseNhsoService
    {
        public string claimCode { set; get; }
        public string createdDate { set; get; }
    }
}
