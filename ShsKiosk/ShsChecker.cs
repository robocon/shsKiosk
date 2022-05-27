using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShsKiosk.ServiceReference1;
using System.Net.Http;
using System.Drawing;
using Newtonsoft.Json;

namespace ShsKiosk
{
    internal class ShsChecker
    {
        static nhsoDataSetC1 pt;
        static readonly SmConfigure smConfig = new SmConfigure();
        static readonly HttpClient client = new HttpClient();
        private string errorMessage = "";
        public async void main(string idcard, Bitmap Photo1)
        {
            ///
            /// Console.WriteLine("ตรวจสอบ Token จากเครื่องห้องทะเบียน");

            // ดึง Token จากเครื่องแม่
            string nhsoContent = await Task.Run(() => LoadRegisterToken($"http://{smConfig.ipUc}/getvalue.php"));
            if (string.IsNullOrEmpty(nhsoContent))
            {
                nhsoContent = await Task.Run(() => LoadRegisterToken($"http://{smConfig.ipUc2}/getvalue.php"));
                if (string.IsNullOrEmpty(nhsoContent))
                {
                    nhsoContent = await Task.Run(() => LoadRegisterToken($"http://{smConfig.ipUc3}/getvalue.php"));
                }
            }

            if (String.IsNullOrEmpty(nhsoContent))
            {
                //label1SetText("กรุณาติดต่อห้องทะเบียน เพื่อทำการขอรหัส Authentication");
                //pictureBox1Status(false);
                //return;

                this.errorMessage = "กรุณาติดต่อห้องทะเบียน เพื่อทำการขอรหัส Authentication";
            }

            string[] nhso = nhsoContent.Split('#');
            string staffIdCard = nhso[0];
            string nhsoToken = nhso[1];

            // ดึงข้อมูลสิทธิการรักษาจาก สปสช
            try
            {
                //Console.WriteLine("ทำการเชื่อมต่อกับ WebService สปสช");
                UCWSTokenP1Client soapClient = new UCWSTokenP1Client();
                pt = new nhsoDataSetC1();
                pt = soapClient.searchCurrentByPID(staffIdCard, nhsoToken, idcard);
                if (pt.ws_status == "NHSO-00003")
                {
                    this.errorMessage = "TOKEN หมดอายุการใช้งาน ติดต่อทะเบียนเพื่อเปิดใช้ UcAuthentication อีกครั้ง";
                    //label1SetText("TOKEN หมดอายุการใช้งาน ติดต่อทะเบียนเพื่อเปิดใช้ UcAuthentication อีกครั้ง");
                    //pictureBox1Status(false);
                    
                }
            }
            catch (Exception ex)
            {
                this.errorMessage = "ไม่สามารถติดต่อกับ WebService สปสช ได้ "+ex.Message;
                //Console.WriteLine(ex.Message);
                //label1SetText("ไม่สามารถติดต่อกับ WebService สปสช ได้");
                //pictureBox1Status(false);
                //return;
            }


            //string moreTxt = "";
            // ถ้า maininscl เป็นค่าว่างแสดงว่าไม่มีสิทธิอะไรเลย ให้สงสัยก่อนว่าเป็นเงินสด
            // ถ้ามี new_maininscl แสดงว่ามีสิทธิใหม่เกิดขึ้น เช่น หมดสิทธิ ปกส. แล้วไปใช้ 30บาท หรืออื่นๆ
            if (String.IsNullOrEmpty(pt.maininscl) || !String.IsNullOrEmpty(pt.new_maininscl))
            {
                
                //label1SetText("รหัสสิทธิหลักของท่านมีการเปลี่ยนแปลง\nกรุณาติดต่อห้องทะเบียน เพื่อทำการทบทวนสิทธิอีกครั้ง");
                //pictureBox1Status(false);
                //return;
            }

            // แจ้งเตือน
            if ((!String.IsNullOrEmpty(pt.hmain) && pt.hmain != "11512") || (!String.IsNullOrEmpty(pt.new_hmain) && pt.new_hmain != "11512"))
            {
                //moreTxt += "แจ้งเตือน! : สถานพยาบาลหลักของท่านไม่ใช่ โรงพยาบาลค่ายสุรศักดิ์มนตรี ท่านจะได้สิทธิเป็นเงินสด \n";
            }

            // ตรวจสอบ HN 
            string testOpcard = await Task.Run(() => searchFromSm(smConfig.searchOpcardUrl, idcard));
            responseOpcard resultOpcard = JsonConvert.DeserializeObject<responseOpcard>(testOpcard);
            if (resultOpcard.opcardStatus == "n")
            {
                //label1SetText(resultOpcard.errorMsg);
                //pictureBox1Status(false);
                //return;
            }

            if (resultOpcard.PtRightMain != resultOpcard.PtRightSub)
            {
                //label1SetText("แจ้งเตือน! : สิทธิหลัก และสิทธิรอง ไม่ตรงกัน กรุณาติดต่อห้องทะเบียนเพื่อทบทวนสิทธิ");
                //pictureBox1Status(false);
                //return;
            }
            // ตรวจสิทธิหลักสิทธิรอง

            // ตรวจสอบการนัดหมาย
            //Console.WriteLine($"ค้นหาการนัด {smConfig.searchAppointUrl} {idcard}");
            string content = await Task.Run(() => searchFromSm(smConfig.searchAppointUrl, idcard));
            //Console.WriteLine(content);
            string appointContent = "";
            int appointCount = 0;
            string appointStatus = "";
            if (!string.IsNullOrEmpty(content))
            {
                responseAppoint result = JsonConvert.DeserializeObject<responseAppoint>(content);
                appointStatus = result.appointStatus;
                if (appointStatus == "y")
                {
                    appointContent = result.appointContent;
                    appointCount = int.Parse(result.appointCount);
                    //appoint = result.appoint;
                }
                else
                {
                    //label1SetText(result.errorMsg);
                    //pictureBox1Status(false);
                    //return;
                }
            }
            //pictureBox1Status(false);

            string maininscl = "";
            string maininsclCode = "";
            if (!String.IsNullOrEmpty(pt.maininscl))
            {
                maininsclCode = pt.maininscl;
                maininscl = $"( {pt.maininscl} ) {pt.maininscl_name}";
            }
            else if (!String.IsNullOrEmpty(pt.new_maininscl))
            {
                maininsclCode = pt.new_maininscl;
                maininscl = $"( {pt.new_maininscl} ) {pt.new_maininscl_name}";
            }

            string subinscl = "";
            if (!String.IsNullOrEmpty(pt.subinscl))
            {
                subinscl = $"( {pt.subinscl} ) {pt.subinscl_name}";
            }
            else if (!String.IsNullOrEmpty(pt.new_subinscl))
            {
                subinscl = $"( {pt.new_subinscl} ) {pt.new_subinscl_name}";
            }

            string hmain = "";
            if (!String.IsNullOrEmpty(pt.hmain))
            {
                hmain = $"( {pt.hmain} ) {pt.hmain_name}";
            }
            else if (!String.IsNullOrEmpty(pt.new_hmain))
            {
                hmain = $"( {pt.new_hmain} ) {pt.new_hmain_name}";
            }

            /*
            Form2 frm = new Form2();
            frm.fullname = pt.fname + " " + pt.lname;
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
            frm.ShowDialog();
            */

            //label1SetText("");
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

        public string getMessage()
        {
            return this.errorMessage;
        }
    }
}
