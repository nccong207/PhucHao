using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace POSApp
{
    public partial class Input : Form
    {
        public decimal duongkinh;
        public Input()
        {
            InitializeComponent();

            numberpad1.Visible = false;
            numberpad1.TextBox = textBox1;
            numberpad1.VisibleChanged += Numberpad1_VisibleChanged;
        }

        private void Numberpad1_VisibleChanged(object sender, EventArgs e)
        {
            if (numberpad1.Visible)
            {
                //panelControl1.Height += 300;
                this.Height += 300;
            }
            else
            {
                //.Height -= 300;
                this.Height -= 300;
                simpleButton4.Focus();
            }
           
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                duongkinh = Convert.ToDecimal(textBox1.Text);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Chỉ được phép nhập số.");
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            numberpad1.Visible = true;
        }
    }
}
