namespace POSApp
{
    partial class Settings
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
            this.somay = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.may1_label = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // somay
            // 
            this.somay.FormatString = "N0";
            this.somay.FormattingEnabled = true;
            this.somay.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.somay.Location = new System.Drawing.Point(124, 32);
            this.somay.Name = "somay";
            this.somay.Size = new System.Drawing.Size(121, 21);
            this.somay.TabIndex = 0;
            this.somay.TextUpdate += new System.EventHandler(this.somay_TextUpdate);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Phân xưởng";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Loại phần mềm";
            // 
            // comboBox5
            // 
            this.comboBox5.FormatString = "N0";
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.comboBox5.Location = new System.Drawing.Point(124, 66);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(121, 21);
            this.comboBox5.TabIndex = 10;
            // 
            // may1_label
            // 
            this.may1_label.AutoSize = true;
            this.may1_label.Location = new System.Drawing.Point(34, 103);
            this.may1_label.Name = "may1_label";
            this.may1_label.Size = new System.Drawing.Size(39, 13);
            this.may1_label.TabIndex = 2;
            this.may1_label.Text = "Máy 1:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(124, 100);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 265);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox5);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.may1_label);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.somay);
            this.Name = "Settings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox somay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Label may1_label;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}