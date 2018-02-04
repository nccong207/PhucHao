using CDTDatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace POSApp
{
    public partial class Login : Form
    {
        public DataRow drUser;
        Database db = Database.NewStructDatabase();
        public Login()
        {
            InitializeComponent();
            Database db = Database.NewStructDatabase();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM DMNhanVien WHERE Ma = '{0}' AND pin = '{1}'";
            DataTable dt = db.GetDataTable(string.Format(sql, textBox1.Text, textBox2.Text));
            if (dt.Rows.Count > 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            } else
            {
                MessageBox.Show("Thông tin đăng nhập không đúng. Vui lòng kiểm tra lại.");
            }
        }
    }
}
