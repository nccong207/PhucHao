using paypi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace POSApp
{
    public partial class keyboard : Form
    {
        public keyboard(TextBox text)
        {
            InitializeComponent();
            numberpad1.TextBox = text;
            numberpad1.VisibleChanged += Numberpad1_VisibleChanged;
        }

        private void Numberpad1_VisibleChanged(object sender, EventArgs e)
        {
            var send = (numberpad) sender;
            if (!send.Visible)
            {
                this.Close();
            }
        }

        private void numberpad1_Load(object sender, EventArgs e)
        {
            //this.Close();
        }
    }
}
