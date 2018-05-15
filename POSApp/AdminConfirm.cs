using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CDTDatabase;

namespace POSApp
{
    public partial class AdminConfirm : Form
    {
        public DataRow confUser;
        Database db = Database.NewDataDatabase();
        public AdminConfirm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "SELECT HoTen,Quyen FROM DMNhanVien WHERE Secret = '{0}'";
            DataTable dt = db.GetDataTable(string.Format(sql, textBox1.Text));
            if (dt.Rows.Count > 0)
            {
                confUser = dt.Rows[0];
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Mã không hợp lệ. Vui lòng kiểm tra lại.");
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
