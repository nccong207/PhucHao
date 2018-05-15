using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace paypi
{
    public partial class numberpad : UserControl
    {
        private const int CS_DROPSHADOW = 0x00020000;
        public string numberRole = "MONEY";
        private TextBox textBoxToUse;

        public TextBox TextBox
        {
            get { return this.textBoxToUse; }
            set { this.textBoxToUse = value; }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                // add the drop shadow flag for automatically drawing
                // a drop shadow around the form
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        public numberpad()
        {
            InitializeComponent();
        }
        //private void MakeAmount(string amountTxt)
        //{
        //    decimal meanWhile;
        //    txtValue.Text = txtValue.Text + amountTxt;
        //    if (txtValue.Text.Length == 1)
        //    {
        //        txtValue.Text = "0.0" + txtValue.Text;
        //    }
        //    if (txtValue.Text.Length > 4)
        //    {
        //        meanWhile = Convert.ToDecimal(txtValue.Text) * 10;
        //        txtValue.Text = meanWhile.ToString("G29");
        //    }
        //}

        private void num1_Click(object sender, EventArgs e)
        {
            textBoxToUse.Text = textBoxToUse.Text + "1";
        }

        private void num2_Click(object sender, EventArgs e)
        {
            textBoxToUse.Text = textBoxToUse.Text + "2";
        }

        private void num3_Click(object sender, EventArgs e)
        {
            textBoxToUse.Text = textBoxToUse.Text + "3";
        }

        private void num4_Click(object sender, EventArgs e)
        {
            textBoxToUse.Text = textBoxToUse.Text + "4";
        }

        private void num5_Click(object sender, EventArgs e)
        {
            textBoxToUse.Text = textBoxToUse.Text + "5";
        }

        private void num6_Click(object sender, EventArgs e)
        {
            textBoxToUse.Text = textBoxToUse.Text + "6";
        }

        private void num7_Click(object sender, EventArgs e)
        {
            textBoxToUse.Text = textBoxToUse.Text + "7";
        }

        private void num8_Click(object sender, EventArgs e)
        {
            textBoxToUse.Text = textBoxToUse.Text + "8";
        }

        private void num9_Click(object sender, EventArgs e)
        {
            textBoxToUse.Text = textBoxToUse.Text + "9";
        }

        private void num0_Click(object sender, EventArgs e)
        {
            textBoxToUse.Text = textBoxToUse.Text + "0";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            textBoxToUse.ResetText();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
        }

        private void num1_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(0, 192, 0);
        }
    }
}
