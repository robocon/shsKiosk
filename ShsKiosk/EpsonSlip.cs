using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace ShsKiosk
{
    class EpsonSlip
    {
        // https://gist.github.com/naveedmurtuza/6600103
        public byte[] DrawText(String text, Font font)
        {
            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //set the stringformat flags to rtl
            StringFormat sf = new StringFormat();
            sf.Trimming = StringTrimming.Word;

            // set text center
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);
            //Adjust for high quality
            drawing.CompositingQuality = CompositingQuality.HighQuality;
            drawing.InterpolationMode = InterpolationMode.HighQualityBilinear;
            drawing.PixelOffsetMode = PixelOffsetMode.HighQuality;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            //paint the background
            drawing.Clear(Color.White);

            //create a brush for the text
            Brush textBrush = new SolidBrush(Color.Black);

            drawing.DrawString(text, font, textBrush, new RectangleF(0, 0, textSize.Width, textSize.Height), sf);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            // https://stackoverflow.com/questions/3801275/how-to-convert-image-to-byte-array
            MemoryStream mStream = new MemoryStream();

            // stream in png
            img.Save(mStream, ImageFormat.Png);

            // 
            //img.Save("D:/testimage.png", ImageFormat.Png);
            img.Dispose();
            
            return mStream.ToArray();
        }

        public void printOutSlip(responseSaveVn app)
        {
            try
            {
                SerialPrinter printer = new SerialPrinter(portName: "COM5", baudRate: 38400);
                EPSON e = new EPSON();

                string fontName = "Tahoma";

                Font fontRegular = new Font(fontName, 26, FontStyle.Regular, GraphicsUnit.Pixel);
                Font fontBoldUnderline = new Font(fontName, 26, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);
                Font fontBold = new Font(fontName, 26, FontStyle.Bold, GraphicsUnit.Pixel);
                Font fontJumbo = new Font(fontName, 48, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);

                byte[] ImgTitle = DrawText($"ใช้บริการโดยตู้ Kiosk {app.dateSave}", fontRegular);
                byte[] ImgEx = DrawText(app.ex, fontBoldUnderline);
                byte[] ImgVn = DrawText("HN : " + app.hn + "\nVN : " + app.vn, fontJumbo);
                byte[] ImgName = DrawText($"ชื่อ : {app.ptname}", fontRegular);

                byte[] ImgPtright = DrawText($"สิทธิ : {app.ptright}", fontBoldUnderline);

                string extraTxt = "";
                if (!String.IsNullOrEmpty(app.hospCode))
                {
                    extraTxt += $"\n{app.hospCode}";
                }

                if (!String.IsNullOrEmpty(app.doctor))
                {
                    extraTxt += $"\n{app.doctor}";
                }

                byte[] ImgHn = DrawText($"อายุ {app.age}\nบัตร ปชช. : {app.idcard}\n{app.mx}{extraTxt}", fontRegular);


                printer.Write(
                    ByteSplicer.Combine(
                        e.CenterAlign(),
                        e.PrintImage(File.ReadAllBytes("Images/LogoWithName2.png"), true, false, 500),
                        e.PrintImage(ImgTitle, true),
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
                    byte[] ImgTitleQueue = DrawText("ใช้บริการโดยตู้ Kiosk\nตรวจโรคทั่วไป\n", fontRegular);
                    byte[] ImgHnVn = DrawText($"HN : {app.hn} VN : {app.vn} \n ชื่อ : {app.ptname}", fontBold);
                    byte[] ImgDetail = DrawText($"ประเภท : {app.ptType}", fontRegular);
                    byte[] ImgQueue = DrawText(app.queueNumber, fontJumbo);
                    byte[] ImgQueueWait = DrawText($"จำนวนคิวที่รอ {app.queueWait} คิว", fontRegular);
                    printer.Write(
                        ByteSplicer.Combine(
                            e.CenterAlign(),
                            e.PrintImage(File.ReadAllBytes("Images/LogoWithNameOPD.png"), true, false, 500),
                            e.PrintImage(ImgTitleQueue, true),
                            e.PrintImage(ImgHnVn, true),
                            e.PrintImage(ImgDetail, true),
                            e.PrintImage(ImgQueue, true),
                            e.PrintImage(ImgQueueWait, true),
                            e.FeedLines(6),
                            e.FullCut()
                        )
                    );
                    printer.Dispose(); // close
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
