using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace POSApp
{
    public partial class Splash1 : Form
    {
        public Splash1()
        {
            InitializeComponent();
            int h = Screen.PrimaryScreen.WorkingArea.Height;
            int w = Screen.PrimaryScreen.WorkingArea.Width;
            this.Width = w;
            this.Height = h;
            this.StartPosition =FormStartPosition.CenterScreen;
            panel1.Location = new Point((w / 2) - (panel1.Width / 2), (h / 2) - (panel1.Height / 2));
           


           
        }
    }
}
