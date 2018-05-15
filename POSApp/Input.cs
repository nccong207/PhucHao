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
        keyboard keyb;
        public Input()
        {
            InitializeComponent();
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
                if (duongkinh < 2200)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else { MessageBox.Show("Đường kính không đúng."); }
            }
            catch (Exception)
            {
                MessageBox.Show("Chỉ được phép nhập số.");
            }
            
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            numberpad1.TextBox = textBox1;
        }

       
    }
}
