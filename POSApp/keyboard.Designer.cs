namespace POSApp
{
    partial class keyboard
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
            this.numberpad1 = new paypi.numberpad();
            this.SuspendLayout();
            // 
            // numberpad1
            // 
            this.numberpad1.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numberpad1.Location = new System.Drawing.Point(4, 3);
            this.numberpad1.Margin = new System.Windows.Forms.Padding(6);
            this.numberpad1.Name = "numberpad1";
            this.numberpad1.Size = new System.Drawing.Size(392, 341);
            this.numberpad1.TabIndex = 0;
            this.numberpad1.TextBox = null;
            this.numberpad1.Load += new System.EventHandler(this.numberpad1_Load);
            // 
            // keyboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 350);
            this.Controls.Add(this.numberpad1);
            this.Name = "keyboard";
            this.Text = "keyborad";
            this.ResumeLayout(false);

        }

        #endregion

        private paypi.numberpad numberpad1;
    }
}