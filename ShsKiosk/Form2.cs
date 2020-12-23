using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        public string hosPtRight;

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
            if (appointCount > 1) // ถ้ามีนัด 2แพทย์
            {
                FormSelectDr frm = new FormSelectDr();
                frm.appoint = appoint;
                frm.idcard = idcard;
                frm.hosPtRight = hosPtRight;
                frm.ShowDialog();
            }
            else // ถ้ามีนัด 1 แพทย์
            {
                SaveVn sv = new SaveVn();

                int appointRowId = appoint.ToArray()[0].rowId;
                await Task.Run(() => sv.save(smConfig.createVnUrl, idcard, appointRowId, ptRight));
                //responseSaveVn result = JsonConvert.DeserializeObject<responseSaveVn>(content);
            }
            this.Close();
        }

        static async void SaveVn(string posturi, string idcard, int appointRowId, string userPtRight = null)
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
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Message :{0} ", ex.Message);
            }

            if (!String.IsNullOrEmpty(content))
            {
                responseSaveVn app = JsonConvert.DeserializeObject<responseSaveVn>(content);

                //if (app.appointStatus == "y")
                //{
                    //string CurrentDat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    // Default Font style
                    Font fontRegular = new Font("TH Sarabun New", 36, FontStyle.Regular, GraphicsUnit.Pixel);
                    Font fontBoldUnderline = new Font("TH Sarabun New", 38, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);
                    Font fontExtra = new Font("TH Sarabun New", 72, FontStyle.Bold, GraphicsUnit.Pixel);

                    // SerialPrinter printer = new SerialPrinter(portName: "COM3", baudRate: 115200);
                    SerialPrinter printer = new SerialPrinter(portName: "COM4", baudRate: 38400);
                    EPSON e = new EPSON();

                    EpsonSlip es = new EpsonSlip();

                    byte[] ImgTxt = es.DrawText($"ใช้บริการโดยตู้ Kiosk {app.dateSave}", fontRegular);
                    byte[] ImgEx = es.DrawText(app.ex, fontBoldUnderline);
                    byte[] ImgVn = es.DrawText("HN : " + app.hn + "\nVN : " + app.vn, fontExtra);
                    byte[] ImgName = es.DrawText($"ชื่อ : {app.ptname}", fontRegular);
                    byte[] ImgHn = es.DrawText($"อายุ {app.age}\nบัตร ปชช. : {app.idcard}\n{app.mx}", fontRegular);
                    byte[] ImgPtright = es.DrawText($"สิทธิ : {app.ptright}", fontBoldUnderline);

                    byte[] ImgHos = es.DrawText("-", fontRegular);
                    if (!String.IsNullOrEmpty(app.hospCode))
                    {
                        ImgHos = es.DrawText(app.hospCode, fontRegular);
                    }

                    byte[] ImgDoctor = es.DrawText("-", fontRegular);
                    if (!String.IsNullOrEmpty(app.hospCode))
                    {
                        ImgDoctor = es.DrawText(app.doctor, fontRegular);
                    }

                    printer.Write(
                        ByteSplicer.Combine(
                            e.CenterAlign(),
                            e.PrintImage(File.ReadAllBytes("Images/LogoWithName2.png"), true, false, 500),
                            e.PrintImage(ImgTxt, true),
                            e.PrintImage(ImgEx, true),
                            e.PrintImage(ImgVn, true),
                            e.PrintImage(ImgName, true),
                            e.PrintImage(ImgHn, true),
                            e.PrintImage(ImgPtright, true),
                            e.PrintImage(ImgHos, true),
                            e.PrintImage(ImgDoctor, true),
                            e.PrintLine(" "),
                            e.SetBarcodeHeightInDots(350),
                            e.SetBarWidth(BarWidth.Thin),
                            e.PrintBarcode(BarcodeType.CODE128, app.hn),
                            e.PrintLine(" "),
                            e.FeedLines(6),
                            e.FullCut()
                        )
                    );

                    if (app.queueStatus == "y")
                    {
                        byte[] ImgTitle = es.DrawText("ตรวจโรคทั่วไป", fontRegular);

                        Font fontBold = new Font("TH Sarabun New", 42, FontStyle.Bold, GraphicsUnit.Pixel);
                        byte[] ImgHnVn = es.DrawText($"HN : {app.hn} VN : {app.vn} \n ชื่อ : {app.ptname}", fontBold);

                        byte[] ImgDetail = es.DrawText($"ประเภท : {app.ptType}", fontRegular);

                        Font fontJumbo = new Font("TH Sarabun New", 102, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);
                        byte[] ImgQueue = es.DrawText(app.queueNumber, fontJumbo);
                        byte[] ImgQueueWait = es.DrawText($"จำนวนคิวที่รอ {app.queueWait} คิว", fontRegular);

                        printer.Write(
                            ByteSplicer.Combine(
                                e.CenterAlign(),
                                e.PrintImage(File.ReadAllBytes("Images/LogoWithNameOPD.png"), true, false, 500),
                                e.PrintImage(ImgTitle, true),
                                e.PrintImage(ImgHnVn, true),
                                e.PrintImage(ImgDetail, true),
                                e.PrintImage(ImgQueue, true),
                                e.PrintImage(ImgQueueWait, true),
                                e.FeedLines(6),
                                e.FullCut()
                            )
                        );
                    }
                //}
                //else if (app.appointStatus == "n")
                //{
                //    Console.WriteLine("appoint status is n");
                //}

            }
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
    }

    public class saveVn {
        public string Idcard { set; get; }
        public int appointId { set; get; }
        public string exType { set; get; }
        public string userPtRight { set; get; }
    }
}
