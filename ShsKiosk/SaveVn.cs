using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShsKiosk
{
    class SaveVn
    {
        static readonly HttpClient client = new HttpClient();
        static readonly SmConfigure smConfig = new SmConfigure();

        public async void save(string posturi, string idcard, int appointRowId, string userPtRight = null, string exType = "ex04")
        {
            string content = null;
            try
            {
                saveVn savevn = new saveVn();
                savevn.Idcard = idcard;
                savevn.appointId = appointRowId;
                savevn.exType = exType;
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
        }
    }
}
