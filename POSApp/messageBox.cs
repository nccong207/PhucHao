using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSApp
{
    public partial class messageBox : Form
    {
        public messageBox(string head, string msg, string detail)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            messageLabel.Text = msg;
            detailTextBox.Text = detail;
            this.Text = head;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
                this.Close();
        }

        private void detailTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void messageBox_Load(object sender, EventArgs e)
        {
            
        }
    }
}
