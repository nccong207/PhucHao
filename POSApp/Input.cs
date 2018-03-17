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
            keyb = new keyboard(textBox1);
            keyb.StartPosition = FormStartPosition.Manual;
            keyb.Location = new Point(this.Location.X + 70, this.Location.Y + 170);
            keyb.ShowDialog();
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            keyb = new keyboard(textBox1);
            keyb.StartPosition = FormStartPosition.Manual;
            keyb.Location = new Point(this.Location.X + 70, this.Location.Y + 170);
            keyb.ShowDialog();
            textBox1.Focus();
        }
    }
}
