using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
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

        private void Form2_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Form2 was loaded");
            label12.BackColor = System.Drawing.Color.Transparent;
            label12.Hide();
            label13.Hide();

            button1.Hide();
            button3.Hide();

            Refresh();
            ptnameHos.Text = ptname;
            label6.Text = idcard;
            label7.Text = fullname;
            label8.Text = mainInSclName;
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
    }

    public class saveVn {
        public string Idcard { set; get; }
        public int appointId { set; get; }
        public string exType { set; get; }
        public string userPtRight { set; get; }
    }
}
