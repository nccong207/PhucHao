using CDTDatabase;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace POSApp
{
  
    public partial class Menufrm : Form
    {
        public int menuChoice;
        Database db = Database.NewDataDatabase();
        public Menufrm()
        {
                InitializeComponent();
                day.Location = new Point(1100, 50);
                timer1.Enabled = true;
                timer1.Interval = 1000;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            
            
            header.Width = ClientSize.Width;
            header.Height = (ClientSize.Height * 10) / 100;
            header.Location = new Point(0, 0);
            body.Width = ClientSize.Width;
            body.Height = (ClientSize.Height * 80) / 100;
            body.Location = new Point(0, header.Height);
            footer.Width = ClientSize.Width;
            footer.Height = (ClientSize.Height * 10) / 100;
            footer.Location = new Point(0, header.Height + body.Height);
            int bw = body.Width;
            int bh = body.Height;
            int btnw = (bw / 3) - (bw * 10) / 100;
            int btnh = (bh / 2) - (bh * 10) / 100;
            //btn1
            int btn1y = ((bh / 2) * 10) / 100;
            int btn1x = ((bw / 3) * 10) / 100;
            btn1.Location = new Point(btn1x, btn1y);
            btn1.Width = btnw;
            btn1.Height = btnh;
            //btn2
            int btn2y = ((bh / 2) * 10) / 100;
            int btn2x = (bw/3)+((bw / 3) * 10) / 100;
            btn2.Location = new Point(btn2x, btn2y);
            btn2.Width = btnw;
            btn2.Height = btnh;
            //btn3
            int btn3y = ((bh / 2) * 10) / 100;
            int btn3x = (bw / 3) + (bw / 3) + ((bw / 3) * 10) / 100;
            btn3.Location = new Point(btn3x, btn3y);
            btn3.Width = btnw;
            btn3.Height = btnh;

            //btn4
            int btn4y = (bh/2)+((bh / 2) * 10) / 100;
            int btn4x = ((bw / 3) * 10) / 100;
            btn4.Location = new Point(btn4x, btn4y);
            btn4.Width = btnw;
            btn4.Height = btnh;
            //btn5
            int btn5y = (bh / 2) + ((bh / 2) * 10) / 100;
            int btn5x = (bw / 3) + ((bw / 3) * 10) / 100;
            btn5.Location = new Point(btn5x, btn5y);
            btn5.Width = btnw;
            btn5.Height = btnh;
            //btn6
            int btn6y = (bh / 2) + ((bh / 2) * 10) / 100;
            int btn6x = (bw / 3) + (bw / 3) + ((bw / 3) * 10) / 100;
            btn6.Location = new Point(btn6x, btn6y);
            btn6.Width = btnw;
            btn6.Height = btnh;

        }

        private void btn1_Click(object sender, EventArgs e)
        {
            // main Frm 
            menuChoice = 1;
            this.DialogResult = DialogResult.OK;
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            menuChoice = 6;
            this.DialogResult = DialogResult.OK;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            day.Text = string.Format("{0:dd/mm/yy - HH:MM:ss}", DateTime.Now);
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            menuChoice = 4;
            this.DialogResult = DialogResult.OK;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            menuChoice = 5;
            this.DialogResult = DialogResult.OK;
        }
       

    }
}
