using ESC_POS_USB_NET.Printer;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShsKiosk
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public string idcard;
        public int appointId;
        public string hosPtRight;
        static readonly HttpClient client = new HttpClient();
        static readonly SmConfigure smConfig = new SmConfigure();

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        /**
         * ตรวจโรคทั่วไป
         */
        private void button1_Click(object sender, EventArgs e)
        {
            presetVn("ex01");
        }

        /// <summary>
        /// ออก VN แทนทะเบียนพร้อมกับ ปริ้นสลิป VN + คิว
        /// </summary>
        /// <param name="exType"></param>
        public async void presetVn(string exType)
        {
            SaveVn sv = new SaveVn();

            /*
            Console.WriteLine(smConfig.createVnUrl);
            Console.WriteLine(idcard);
            Console.WriteLine(appointId);
            Console.WriteLine(hosPtRight);
            Console.WriteLine(exType);
            return;
            */

            string content = await Task.Run(() => sv.save(smConfig.createVnUrl, idcard, appointId, hosPtRight, exType));
            //string content = await Task.Run(() => SaveVn(smConfig.createVnUrl, idcard, appointId, exType));
            responseSaveVn result = JsonConvert.DeserializeObject<responseSaveVn>(content);


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = System.Text.Encoding.GetEncoding(874);
            byte[] byTHList = encoding.GetBytes("สวัสดีชาวโลก");

            var en8 = Encoding.UTF8;
            var thai2 = en8.GetString(byTHList);

            EpsonSlip es = new EpsonSlip();
            Font fontRegular = new Font("TH Sarabun New", 36, FontStyle.Regular, GraphicsUnit.Pixel);
            byte[] ImgTxt = es.DrawText(thai2, fontRegular);

            Bitmap bmp;
            using (var ms = new MemoryStream(ImgTxt))
            {
                bmp = new Bitmap(ms);
            }


            Printer printer = new Printer("EPSON TM-T82X Receipt");
            Bitmap image = new Bitmap(Bitmap.FromFile("Images/LogoWithName2.bmp"));

            //var test = Image.FromFile("image.png");

            //printer.Image(image);
            printer.Append("ภาษาไทย");
            printer.FullPaperCut();
            printer.PrintDocument();

            //printSlip(result);
            this.Close();

            return;
        }

        static async Task<string> SaveVn(string posturi, string idcard, int appointId, string exType = "")
        {
            string content = null;
            try
            {
                saveVn savevn = new saveVn();
                savevn.Idcard = idcard;
                savevn.appointId = appointId;
                savevn.exType = exType;

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

        public void printSlip(responseSaveVn app)
        {
            if (app.appointStatus == "y")
            {
                //string CurrentDat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // Default Font style
                Font fontRegular = new Font("TH Sarabun New", 36, FontStyle.Regular, GraphicsUnit.Pixel);
                Font fontBoldUnderline = new Font("TH Sarabun New", 38, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);
                Font fontExtra = new Font("TH Sarabun New", 72, FontStyle.Bold, GraphicsUnit.Pixel);

                // SerialPrinter printer = new SerialPrinter(portName: "COM3", baudRate: 115200);
                //SerialPrinter printer = new SerialPrinter(portName: "COM4", baudRate: 38400);
                SerialPrinter printer = new SerialPrinter(portName: "COM4", baudRate: 115200);
                EPSON e = new EPSON();

                EpsonSlip es = new EpsonSlip();

                byte[] ImgTxt = es.DrawText($"ใช้บริการโดยตู้ Kiosk {app.dateSave}", fontRegular);
                byte[] ImgEx = es.DrawText(app.ex, fontBoldUnderline);
                byte[] ImgVn = es.DrawText("HN : " + app.hn + "\nVN : " + app.vn, fontExtra);
                byte[] ImgName = es.DrawText($"ชื่อ : {app.ptname}", fontRegular);

                byte[] ImgPtright = es.DrawText($"สิทธิ : {app.ptright}", fontBoldUnderline);

                string extraTxt = "";
                if (!String.IsNullOrEmpty(app.hospCode))
                {
                    extraTxt += $"\n{app.hospCode}";
                }

                if (!String.IsNullOrEmpty(app.doctor))
                {
                    extraTxt += $"\n{app.doctor}";
                }

                byte[] ImgHn = es.DrawText($"อายุ {app.age}\nบัตร ปชช. : {app.idcard}\n{app.mx}{extraTxt}", fontRegular);

                /*
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
                        e.PrintLine(" "),
                        e.SetBarcodeHeightInDots(350),
                        e.SetBarWidth(BarWidth.Thin),
                        e.PrintBarcode(BarcodeType.CODE128, app.hn),
                        e.PrintLine(" "),
                        e.FeedLines(6),
                        e.FullCut()
                    )
                );
                */

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
            }
        }

        /**
         * ทันตกรรม
         */
        private void button2_Click(object sender, EventArgs e)
        {
            presetVn("ex07");
        }

        /**
         * ตา
         */
        private void button3_Click(object sender, EventArgs e)
        {
            presetVn("ex25");
        }

        /**
         * ทันตกรรม
         */
        private void button4_Click(object sender, EventArgs e)
        {
            presetVn("ex17");
        }

        /**
         * นวดแผนไทย
         */
        private void button6_Click(object sender, EventArgs e)
        {
            presetVn("ex20");
        }

        /**
         * ฝังเข็ม
         */
        private void button5_Click(object sender, EventArgs e)
        {
            presetVn("ex92");
        }

        /**
         * ไตเทียม
         */
        private void button8_Click(object sender, EventArgs e)
        {
            presetVn("ex10");
        }

        /**
         * ฉีดยาต่อเนื่อง
         */
        private void button7_Click(object sender, EventArgs e)
        {
            presetVn("ex23");
        }
    }
}
