
namespace RegisFromHn
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
            this.submitHn = new System.Windows.Forms.Button();
            this.hn = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.notify = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // submitHn
            // 
            this.submitHn.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.submitHn.Location = new System.Drawing.Point(104, 88);
            this.submitHn.Name = "submitHn";
            this.submitHn.Size = new System.Drawing.Size(360, 64);
            this.submitHn.TabIndex = 0;
            this.submitHn.Text = "ลงทะเบียน";
            this.submitHn.UseVisualStyleBackColor = true;
            this.submitHn.Click += new System.EventHandler(this.submitHn_Click);
            // 
            // hn
            // 
            this.hn.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.hn.Location = new System.Drawing.Point(176, 24);
            this.hn.Name = "hn";
            this.hn.Size = new System.Drawing.Size(288, 53);
            this.hn.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("TH Niramit AS", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(104, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 46);
            this.label1.TabIndex = 2;
            this.label1.Text = "HN : ";
            // 
            // notify
            // 
            this.notify.AutoSize = true;
            this.notify.Font = new System.Drawing.Font("TH Niramit AS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.notify.Location = new System.Drawing.Point(0, 160);
            this.notify.Name = "notify";
            this.notify.Size = new System.Drawing.Size(21, 29);
            this.notify.TabIndex = 3;
            this.notify.Text = "-";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.Controls.Add(this.notify);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hn);
            this.Controls.Add(this.submitHn);
            this.Name = "Form1";
            this.Text = "ลงทะเบียนจากHN";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button submitHn;
        private System.Windows.Forms.TextBox hn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label notify;
    }
}

