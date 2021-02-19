
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ipUc = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ipBroker = new System.Windows.Forms.TextBox();
            this.notify = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("TH Niramit AS", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(248, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 64);
            this.label1.TabIndex = 0;
            this.label1.Text = "ตั้งค่าการใช้งาน";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("TH Niramit AS", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(112, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(296, 56);
            this.label2.TabIndex = 1;
            this.label2.Text = "IP เครื่อง UC ห้องทะเบียน";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ipUc
            // 
            this.ipUc.Font = new System.Drawing.Font("TH Niramit AS", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipUc.Location = new System.Drawing.Point(416, 80);
            this.ipUc.Name = "ipUc";
            this.ipUc.Size = new System.Drawing.Size(216, 57);
            this.ipUc.TabIndex = 2;
            this.ipUc.Text = "192.168.142.73";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("TH Niramit AS", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(204, 208);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(160, 56);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "บันทึก";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("TH Niramit AS", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(380, 208);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(160, 56);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "ยกเลิก";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("TH Niramit AS", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(112, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(293, 50);
            this.label3.TabIndex = 5;
            this.label3.Text = "IP เครื่อง Broker";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ipBroker
            // 
            this.ipBroker.Font = new System.Drawing.Font("TH Niramit AS", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipBroker.Location = new System.Drawing.Point(416, 144);
            this.ipBroker.Name = "ipBroker";
            this.ipBroker.Size = new System.Drawing.Size(216, 57);
            this.ipBroker.TabIndex = 6;
            this.ipBroker.Text = "localhost";
            // 
            // notify
            // 
            this.notify.Font = new System.Drawing.Font("TH Niramit AS", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notify.ForeColor = System.Drawing.Color.Green;
            this.notify.Location = new System.Drawing.Point(8, 272);
            this.notify.Name = "notify";
            this.notify.Size = new System.Drawing.Size(728, 50);
            this.notify.TabIndex = 7;
            this.notify.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 338);
            this.Controls.Add(this.notify);
            this.Controls.Add(this.ipBroker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.ipUc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "ฟอร์มตั้งค่าการใช้งาน";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ipUc;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ipBroker;
        private System.Windows.Forms.Label notify;
    }
}

