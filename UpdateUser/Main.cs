using CDTDatabase;
using CDTLib;
using DevExpress.XtraEditors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UpdateUser
{
    public partial class Main : DevExpress.XtraEditors.XtraForm
    {
        Database db = Database.NewStructDatabase();
        public Main()
        {
            InitializeComponent();
        }
      
        private void Main_Load(object sender, EventArgs e)
        {
            getUser();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            txtPass.Text = RandomString(6);
            txtPass.ReadOnly = true;
            txtDate.DateTime = DateTime.Today;
        }

        void getUser()
        {
            string sql = @"SELECT DISTINCT u.sysUserID, UserName, FullName, s.IsFullTime, s.IsFilter 
                            FROM sysUser u JOIN sysUserSite s ON u.sysUserID = s.sysUserID
                            WHERE u.IsGroup = 0 AND s.sysSiteID = 23
                            ORDER BY u.UserName";

            DataTable dt = db.GetDataTable(sql);

            gridLookUpEdit1.Properties.DataSource = dt;
            gridLookUpEdit1.Properties.DisplayMember = "FullName";
            gridLookUpEdit1.Properties.ValueMember = "sysUserID";

            gridLookUpEdit1View.Columns[0].Visible = false;
            gridLookUpEdit1View.Columns[1].Width = 100;
            gridLookUpEdit1View.Columns[2].Width = 100;
            gridLookUpEdit1View.Columns[3].Width = 100;
            gridLookUpEdit1View.Columns[4].Width = 100;

            gridLookUpEdit1.Properties.PopupFormMinSize = new Size(500,300);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string userid = gridLookUpEdit1.EditValue.ToString();
            string pass = Security.EnCodePwd(txtPass.Text.Trim());
            DateTime dateExp = txtDate.DateTime;
            if (dateExp < DateTime.Today)
            {
                XtraMessageBox.Show("Ngày hết hạn không được nhỏ hơn ngày hiện tại", "Lỗi nhập liệu");
            }
            else
            {
                // update mat khau cho user
                string sqlUpdate = string.Format("UPDATE sysUser SET Password = '{0}' WHERE sysUserID = {1}", pass, userid);
                db.UpdateByNonQuery(sqlUpdate);

                // update ngay het han cho user
                sqlUpdate = string.Format("UPDATE sysUserSite SET ExpireDate = '{1}' WHERE sysUserID = {0} and sysSiteID = 23", userid, dateExp.ToString("MM-dd-yyyy HH:mm:ss"));
                db.UpdateByNonQuery(sqlUpdate);
             
                XtraMessageBox.Show("Cập nhật dữ liệu thành công", "Thông báo");
            }
        }

       
        public static string RandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        private void gridLookUpEdit1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            txtPass.Text = RandomString(6);
            txtDate.DateTime = DateTime.Today;
        }
    }
}
Người AE Việt Nam thân mới. Cho mình Spam cái này để nhận ÁO THUN FREE nha. Cảm ơn người AE Việt NAM. SV nghèo :(
