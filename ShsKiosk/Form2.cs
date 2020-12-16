using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThaiNationalIDCard;

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
        
        public string ptRight;

        public string appointStatus;
        public string appointContent;
        public int appointCount;
        public List<Appoint> appoint;

        public string moreTxt;

        public Bitmap personImage;
        static readonly HttpClient client = new HttpClient();
        static readonly SmConfigure smConfig = new SmConfigure();

        private void Form2_Load(object sender, EventArgs e)
        {
            label12.BackColor = System.Drawing.Color.Transparent;
            label12.Hide();
            label13.Hide();

            button1.Hide();
            button3.Hide();

            Refresh();

            label6.Text = idcard;
            label7.Text = fullname;
            label8.Text = mainInSclName;
            label9.Text = subInSclName;
            label10.Text = hMainName;
            pictureBox1.Image = personImage;

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

                label12.Text = appointContent;
            }

            if (!string.IsNullOrEmpty(moreTxt)) {
                labelAlert.Text = moreTxt;
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
            if (appointCount > 1) // ถ้ามีนัด 2แพทย์
            {
                FormSelectDr frm = new FormSelectDr();
                frm.appoint = appoint;
                frm.idcard = idcard;
                frm.ShowDialog();
            }
            else // ถ้ามีนัด 1 แพทย์
            {
                int appointRowId = appoint.ToArray()[0].rowId;
                string content = await Task.Run(() => SaveVn(smConfig.createVnUrl, idcard, appointRowId, ptRight));
                responseSaveVn result = JsonConvert.DeserializeObject<responseSaveVn>(content);
            }
            this.Close();
        }

        static async Task<string> SaveVn(string posturi, string idcard, int appointRowId, string userPtRight = null)
        {
            string content = null;
            try
            {
                saveVn savevn = new saveVn();
                savevn.Idcard = idcard;
                savevn.appointId = appointRowId;
                savevn.exType = "ex04";
                savevn.userPtRight = userPtRight;

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
    }

    public class responseSaveVn
    {
        public string appointStatus { set; get; }
    }

    public class saveVn {
        public string Idcard { set; get; }
        public int appointId { set; get; }
        public string exType { set; get; }
        public string userPtRight { set; get; }
    }
}
