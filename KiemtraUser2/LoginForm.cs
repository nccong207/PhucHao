using CDTDatabase;
using CDTLib;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KiemtraUser2
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string pass = Security.EnCodePwd(txtPass.Text);
            Database db = Database.NewStructDatabase();

            string sql = string.Format("SELECT TOP 1 * FROM sysUser WHERE UserName = '{0}' and Password = '{1}'", username, pass);
            DataTable dtUser = db.GetDataTable(sql);
            if (dtUser.Rows.Count > 0)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                XtraMessageBox.Show("Thông tin đăng nhập không chính xác vui lòng nhập lại", Config.GetValue("PackageName").ToString(), MessageBoxButtons.OK);
                Config.NewKeyValue("NoBackup", 1);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
