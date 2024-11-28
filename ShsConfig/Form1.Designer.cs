
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
            this.notifyHost = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ipBroker = new System.Windows.Forms.TextBox();
            this.notify = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.printerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
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
            this.label2.Text = "Notify Host";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // notifyHost
            // 
            this.notifyHost.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notifyHost.Location = new System.Drawing.Point(272, 64);
            this.notifyHost.Name = "notifyHost";
            this.notifyHost.Size = new System.Drawing.Size(168, 50);
            this.notifyHost.TabIndex = 2;
            this.notifyHost.TabStop = false;
            this.notifyHost.Text = "192.168.130.15";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(130, 285);
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
            this.btnCancel.Location = new System.Drawing.Point(288, 285);
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
            this.label3.Location = new System.Drawing.Point(8, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(256, 50);
            this.label3.TabIndex = 5;
            this.label3.Text = "IP ติดต่อ DB";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ipBroker
            // 
            this.ipBroker.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipBroker.Location = new System.Drawing.Point(272, 176);
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
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ipBroker);
            this.groupBox1.Controls.Add(this.notifyHost);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 250);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ตั้งค่าการใช้งาน";
            // 
            // printerName
            // 
            this.printerName.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printerName.Location = new System.Drawing.Point(272, 120);
            this.printerName.Name = "printerName";
            this.printerName.Size = new System.Drawing.Size(168, 50);
            this.printerName.TabIndex = 8;
            this.printerName.TabStop = false;
            this.printerName.Text = "w80";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 50);
            this.label1.TabIndex = 7;
            this.label1.Text = "เครื่องปริ้นสลิป";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 359);
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
        private System.Windows.Forms.TextBox notifyHost;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ipBroker;
        private System.Windows.Forms.Label notify;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox printerName;
        private System.Windows.Forms.Label label1;
    }
}

