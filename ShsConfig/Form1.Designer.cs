
namespace ShsConfig
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label2 = new System.Windows.Forms.Label();
            this.ipUc = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ipBroker = new System.Windows.Forms.TextBox();
            this.notify = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.printerName = new System.Windows.Forms.TextBox();
            this.ipUc3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ipUc2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(256, 50);
            this.label2.TabIndex = 1;
            this.label2.Text = "UC ทะเบียน เครื่อง1";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ipUc
            // 
            this.ipUc.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipUc.Location = new System.Drawing.Point(272, 64);
            this.ipUc.Name = "ipUc";
            this.ipUc.Size = new System.Drawing.Size(168, 50);
            this.ipUc.TabIndex = 2;
            this.ipUc.TabStop = false;
            this.ipUc.Text = "192.168.142.73";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(130, 398);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(124, 48);
            this.btnSave.TabIndex = 3;
            this.btnSave.TabStop = false;
            this.btnSave.Text = "บันทึก";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(288, 398);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(124, 48);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "ยกเลิก";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 288);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(256, 50);
            this.label3.TabIndex = 5;
            this.label3.Text = "IP ติดต่อ DB";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ipBroker
            // 
            this.ipBroker.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipBroker.Location = new System.Drawing.Point(272, 288);
            this.ipBroker.Name = "ipBroker";
            this.ipBroker.Size = new System.Drawing.Size(168, 50);
            this.ipBroker.TabIndex = 6;
            this.ipBroker.TabStop = false;
            this.ipBroker.Text = "192.168.131.250";
            // 
            // notify
            // 
            this.notify.Font = new System.Drawing.Font("TH Niramit AS", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notify.ForeColor = System.Drawing.Color.Green;
            this.notify.Location = new System.Drawing.Point(15, 464);
            this.notify.Name = "notify";
            this.notify.Size = new System.Drawing.Size(536, 40);
            this.notify.TabIndex = 7;
            this.notify.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.printerName);
            this.groupBox1.Controls.Add(this.ipUc3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.ipUc2);
            this.groupBox1.Controls.Add(this.ipBroker);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.ipUc);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 376);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ตั้งค่าการใช้งาน";
            // 
            // printerName
            // 
            this.printerName.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printerName.Location = new System.Drawing.Point(272, 232);
            this.printerName.Name = "printerName";
            this.printerName.Size = new System.Drawing.Size(168, 50);
            this.printerName.TabIndex = 8;
            this.printerName.TabStop = false;
            this.printerName.Text = "w80";
            // 
            // ipUc3
            // 
            this.ipUc3.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipUc3.Location = new System.Drawing.Point(272, 176);
            this.ipUc3.Name = "ipUc3";
            this.ipUc3.Size = new System.Drawing.Size(168, 50);
            this.ipUc3.TabIndex = 6;
            this.ipUc3.TabStop = false;
            this.ipUc3.Text = "192.168.142.73";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 232);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 50);
            this.label1.TabIndex = 7;
            this.label1.Text = "เครื่องปริ้นสลิป";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(8, 176);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(256, 50);
            this.label10.TabIndex = 5;
            this.label10.Text = "UC ทะเบียน เครื่อง3";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ipUc2
            // 
            this.ipUc2.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipUc2.Location = new System.Drawing.Point(272, 120);
            this.ipUc2.Name = "ipUc2";
            this.ipUc2.Size = new System.Drawing.Size(168, 50);
            this.ipUc2.TabIndex = 4;
            this.ipUc2.TabStop = false;
            this.ipUc2.Text = "192.168.142.73";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(256, 50);
            this.label9.TabIndex = 3;
            this.label9.Text = "UC ทะเบียน เครื่อง2";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 509);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.notify);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "ฟอร์มตั้งค่าการใช้งาน";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ipUc;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ipBroker;
        private System.Windows.Forms.Label notify;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox printerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ipUc3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox ipUc2;
        private System.Windows.Forms.Label label9;
    }
}

