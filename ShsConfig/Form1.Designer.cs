﻿
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
            this.label4 = new System.Windows.Forms.Label();
            this.brokerHost = new System.Windows.Forms.TextBox();
            this.brokerUser = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.brokerPass = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.brokerDb = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.printerName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(256, 40);
            this.label2.TabIndex = 1;
            this.label2.Text = "IP เครื่อง UC ห้องทะเบียน";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ipUc
            // 
            this.ipUc.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipUc.Location = new System.Drawing.Point(288, 64);
            this.ipUc.Name = "ipUc";
            this.ipUc.Size = new System.Drawing.Size(168, 50);
            this.ipUc.TabIndex = 2;
            this.ipUc.TabStop = false;
            this.ipUc.Text = "192.168.142.73";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(269, 416);
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
            this.btnCancel.Location = new System.Drawing.Point(477, 416);
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
            this.label3.Location = new System.Drawing.Point(24, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(256, 40);
            this.label3.TabIndex = 5;
            this.label3.Text = "IP เครื่อง Broker";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ipBroker
            // 
            this.ipBroker.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipBroker.Location = new System.Drawing.Point(288, 120);
            this.ipBroker.Name = "ipBroker";
            this.ipBroker.Size = new System.Drawing.Size(168, 50);
            this.ipBroker.TabIndex = 6;
            this.ipBroker.TabStop = false;
            this.ipBroker.Text = "localhost";
            // 
            // notify
            // 
            this.notify.Font = new System.Drawing.Font("TH Niramit AS", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notify.ForeColor = System.Drawing.Color.Green;
            this.notify.Location = new System.Drawing.Point(16, 360);
            this.notify.Name = "notify";
            this.notify.Size = new System.Drawing.Size(840, 40);
            this.notify.TabIndex = 7;
            this.notify.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(24, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 56);
            this.label4.TabIndex = 8;
            this.label4.Text = "HOST";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // brokerHost
            // 
            this.brokerHost.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brokerHost.Location = new System.Drawing.Point(168, 64);
            this.brokerHost.Name = "brokerHost";
            this.brokerHost.Size = new System.Drawing.Size(176, 50);
            this.brokerHost.TabIndex = 10;
            this.brokerHost.TabStop = false;
            this.brokerHost.Text = "192.168.131.250";
            // 
            // brokerUser
            // 
            this.brokerUser.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brokerUser.Location = new System.Drawing.Point(168, 128);
            this.brokerUser.Name = "brokerUser";
            this.brokerUser.Size = new System.Drawing.Size(176, 50);
            this.brokerUser.TabIndex = 12;
            this.brokerUser.TabStop = false;
            this.brokerUser.Text = "remoteuser";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(24, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 56);
            this.label6.TabIndex = 11;
            this.label6.Text = "USERNAME";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // brokerPass
            // 
            this.brokerPass.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brokerPass.Location = new System.Drawing.Point(168, 192);
            this.brokerPass.Name = "brokerPass";
            this.brokerPass.Size = new System.Drawing.Size(176, 50);
            this.brokerPass.TabIndex = 14;
            this.brokerPass.TabStop = false;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(24, 192);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 56);
            this.label7.TabIndex = 13;
            this.label7.Text = "PASSWORD";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // brokerDb
            // 
            this.brokerDb.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brokerDb.Location = new System.Drawing.Point(168, 256);
            this.brokerDb.Name = "brokerDb";
            this.brokerDb.Size = new System.Drawing.Size(176, 50);
            this.brokerDb.TabIndex = 16;
            this.brokerDb.TabStop = false;
            this.brokerDb.Text = "smdb";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(24, 256);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 56);
            this.label8.TabIndex = 15;
            this.label8.Text = "DATABASE";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.printerName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ipUc);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ipBroker);
            this.groupBox1.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 264);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ตั้งค่าการใช้งาน";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.brokerHost);
            this.groupBox2.Controls.Add(this.brokerDb);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.brokerUser);
            this.groupBox2.Controls.Add(this.brokerPass);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(488, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(368, 328);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ตั้งค่า Database Broker";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 40);
            this.label1.TabIndex = 7;
            this.label1.Text = "เครื่องปริ้น";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // printerName
            // 
            this.printerName.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printerName.Location = new System.Drawing.Point(288, 176);
            this.printerName.Name = "printerName";
            this.printerName.Size = new System.Drawing.Size(168, 50);
            this.printerName.TabIndex = 8;
            this.printerName.TabStop = false;
            this.printerName.Text = "w80";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 477);
            this.Controls.Add(this.groupBox2);
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox brokerHost;
        private System.Windows.Forms.TextBox brokerUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox brokerPass;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox brokerDb;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox printerName;
        private System.Windows.Forms.Label label1;
    }
}

