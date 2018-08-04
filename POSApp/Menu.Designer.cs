namespace POSApp
{
    partial class Menufrm
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
            this.components = new System.ComponentModel.Container();
            this.header = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.day = new System.Windows.Forms.Label();
            this.footer = new System.Windows.Forms.Panel();
            this.body = new System.Windows.Forms.Panel();
            this.btn6 = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.btn5 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            this.btn1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.body.SuspendLayout();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.WhiteSmoke;
            this.header.Controls.Add(this.pictureBox1);
            this.header.Controls.Add(this.day);
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(757, 100);
            this.header.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::POSApp.Properties.Resources.SSS;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(116, 72);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // day
            // 
            this.day.AutoSize = true;
            this.day.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.day.ForeColor = System.Drawing.Color.DimGray;
            this.day.Location = new System.Drawing.Point(706, 38);
            this.day.Name = "day";
            this.day.Size = new System.Drawing.Size(42, 24);
            this.day.TabIndex = 21;
            this.day.Text = "day";
            // 
            // footer
            // 
            this.footer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.footer.Location = new System.Drawing.Point(0, 371);
            this.footer.Name = "footer";
            this.footer.Size = new System.Drawing.Size(757, 100);
            this.footer.TabIndex = 0;
            // 
            // body
            // 
            this.body.BackColor = System.Drawing.Color.WhiteSmoke;
            this.body.Controls.Add(this.btn6);
            this.body.Controls.Add(this.btn3);
            this.body.Controls.Add(this.btn5);
            this.body.Controls.Add(this.btn2);
            this.body.Controls.Add(this.btn4);
            this.body.Controls.Add(this.btn1);
            this.body.Location = new System.Drawing.Point(0, 106);
            this.body.Name = "body";
            this.body.Size = new System.Drawing.Size(757, 259);
            this.body.TabIndex = 1;
            // 
            // btn6
            // 
            this.btn6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(155)))), ((int)(((byte)(240)))));
            this.btn6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn6.Location = new System.Drawing.Point(446, 130);
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(211, 126);
            this.btn6.TabIndex = 5;
            this.btn6.Text = "Thoát + Tắt máy";
            this.btn6.UseVisualStyleBackColor = false;
            this.btn6.Click += new System.EventHandler(this.btn6_Click);
            // 
            // btn3
            // 
            this.btn3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(155)))), ((int)(((byte)(240)))));
            this.btn3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.btn3.Location = new System.Drawing.Point(446, 3);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(211, 126);
            this.btn3.TabIndex = 4;
            this.btn3.Text = "-";
            this.btn3.UseVisualStyleBackColor = false;
            // 
            // btn5
            // 
            this.btn5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(155)))), ((int)(((byte)(240)))));
            this.btn5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.btn5.Location = new System.Drawing.Point(229, 130);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(211, 126);
            this.btn5.TabIndex = 3;
            this.btn5.Text = "Cài Đặt";
            this.btn5.UseVisualStyleBackColor = false;
            this.btn5.Click += new System.EventHandler(this.btn5_Click);
            // 
            // btn2
            // 
            this.btn2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(155)))), ((int)(((byte)(240)))));
            this.btn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.btn2.Location = new System.Drawing.Point(229, 3);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(211, 126);
            this.btn2.TabIndex = 2;
            this.btn2.Text = "Xem báo cáo";
            this.btn2.UseVisualStyleBackColor = false;
            // 
            // btn4
            // 
            this.btn4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(155)))), ((int)(((byte)(240)))));
            this.btn4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.btn4.Location = new System.Drawing.Point(12, 130);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(211, 126);
            this.btn4.TabIndex = 1;
            this.btn4.Text = "PosMain";
            this.btn4.UseVisualStyleBackColor = false;
            this.btn4.Click += new System.EventHandler(this.btn4_Click);
            // 
            // btn1
            // 
            this.btn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(155)))), ((int)(((byte)(240)))));
            this.btn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.btn1.Location = new System.Drawing.Point(12, 3);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(211, 126);
            this.btn1.TabIndex = 0;
            this.btn1.Text = "Vào ca";
            this.btn1.UseVisualStyleBackColor = false;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Menufrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 472);
            this.Controls.Add(this.body);
            this.Controls.Add(this.footer);
            this.Controls.Add(this.header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Menufrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Menu_Load);
            this.header.ResumeLayout(false);
            this.header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.body.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel header;
        private System.Windows.Forms.Panel footer;
        private System.Windows.Forms.Panel body;
        private System.Windows.Forms.Button btn6;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label day;
        private System.Windows.Forms.Timer timer1;
    }
}