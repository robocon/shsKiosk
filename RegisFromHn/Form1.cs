using Newtonsoft.Json;
using ShsKiosk;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RegisFromHn
{
    public partial class Form1 : Form
    {
        static readonly SmConfigure smConfig = new SmConfigure();
        static readonly HttpClient client = new HttpClient();
        public List<Appoint> appoint;

        public Form1()
        {
            InitializeComponent();
        }

        private async void submitHn_Click(object sender, EventArgs e)
        {
            string textBoxTest = hn.Text;
            string ptRight = "";
            string idcard = "";

            Console.WriteLine($"Manual ค้นหาจาก HN {smConfig.searchOpcardUrl}");

            if (!Regex.IsMatch(textBoxTest, "-", RegexOptions.IgnoreCase))
            {
                notify.Text = "อนุญาตให้ใช้เฉพาะ HN";
                return;
            }

                responseOpcard resultOpcard = new responseOpcard();
            // ค้นหาและตรวจสอบ HN 
            string testOpcard = await Task.Run(() => searchFromSmByHn(smConfig.searchOpcardUrl, textBoxTest));
            Console.WriteLine(testOpcard);
            if (!string.IsNullOrEmpty(testOpcard))
            {
                resultOpcard = JsonConvert.DeserializeObject<responseOpcard>(testOpcard);
                if (resultOpcard.opcardStatus == "n")
                {
                    notify.Text = resultOpcard.errorMsg;
                    //pictureBox1.Visible = false;
                    return;
                }
                idcard = resultOpcard.idcard;
                ptRight = resultOpcard.hosPtRight;
                //hosPtRight = resultOpcard.hosPtRight;
            }

            // ค้นหาการนัดหมาย
            Console.WriteLine($"ค้นหาการนัด {smConfig.searchAppointUrl} {idcard}");
            responseAppoint resultAppoint = new responseAppoint();
            // ตรวจสอบการนัดหมาย
            string contentAppoint = await Task.Run(() => searchFromSm(smConfig.searchAppointUrl, idcard));
            string appointContent = "";
            int appointCount = 0;
            string appointStatus = "";
            

            if (!string.IsNullOrEmpty(contentAppoint))
            {
                resultAppoint = JsonConvert.DeserializeObject<responseAppoint>(contentAppoint);
                appointStatus = resultAppoint.appointStatus;
                Console.WriteLine(resultAppoint);
                if (appointStatus == "y")
                {
                    appointContent = resultAppoint.appointContent;
                    appointCount = int.Parse(resultAppoint.appointCount);
                    //appointRowId = result.row
                    appoint = resultAppoint.appoint;
                }
                else
                {
                    notify.Text = resultAppoint.errorMsg;
                    //pictureBox1.Visible = false;
                    return;
                }
            }


            if (resultOpcard.PtRightMain != resultOpcard.PtRightSub)
            {
                notify.Text = "แจ้งเตือน! : สิทธิหลักและสิทธิรองไม่ตรงกัน กรุณาติดต่อห้องทะเบียนเพื่อทบทวนสิทธิ\n";
                /*pictureBox1.Visible = false;*/
                return;
            }

            int appointRowId = appoint.ToArray()[0].rowId;

            /*Console.WriteLine(smConfig.createVnUrl);
            Console.WriteLine(idcard);
            Console.WriteLine(appointRowId);
            Console.WriteLine(ptRight);
            return;*/


            //Console.WriteLine("กดออก VN แพทย์คนเดียว");
            SaveVn sv = new SaveVn();
            
            string contentVn = await Task.Run(() => sv.save(smConfig.createVnUrl, idcard, appointRowId, ptRight));
            if (!String.IsNullOrEmpty(contentVn))
            {
                responseSaveVn appVn = JsonConvert.DeserializeObject<responseSaveVn>(contentVn);
                EpsonSlip es = new EpsonSlip();
                es.printOutSlip(appVn);
            }


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

    public class saveVn
    {
        public string Idcard { set; get; }
        public int appointId { set; get; }
        public string exType { set; get; }
        public string userPtRight { set; get; }
    }
}
