using CDTDatabase;
using CDTLib;
using DevExpress.XtraGrid;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KiemtraUser2
{
    public class KiemtraUser2 : ICReport
    {
        private DataCustomReport _data;
        private InfoCustomReport _info = new InfoCustomReport(IDataType.Report);
        public DataCustomReport Data
        {
            set { _data = value; }
        }

        public InfoCustomReport Info
        {
            get { return _info; }
        }

        public void Execute()
        {
            _data.FrmMain.Activated += FrmMain_Activated;
            _data.FrmMain.HandleCreated += FrmMain_HandleCreated;
        }

        private void FrmMain_HandleCreated(object sender, EventArgs e)
        {
            if (!isAcess())
            {
                LoginForm frm = new LoginForm();
                if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                else
                    _data.FrmMain.Close();
            }
        }

        private void FrmMain_Activated(object sender, EventArgs e)
        {
            if (!isAcess(true))
            {
                _data.FrmMain.Visible = false;
                LoginForm frm = new LoginForm();
                if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    _data.FrmMain.Visible = true;
                    _data.FrmMain.Close();
                }
            }
        }

        private bool isAcess(bool isActiveActin = false)
        {
            string isAdmin = Config.GetValue("Admin").ToString();
            if (!Convert.ToBoolean(isAdmin))
            {
                string sysUserID = Config.GetValue("sysUserID").ToString();

                string sql = string.Format("SELECT TOP 1 * FROM sysHistory WHERE sysUserID = {0} ORDER by hDateTime DESC", sysUserID);
                Database db = Database.NewStructDatabase();
                DataTable dttime = db.GetDataTable(sql);

                if (dttime.Rows.Count > 0)
                {
                    int pos = isActiveActin ? 0 : 1;
                    DateTime timeloginStart = DateTime.Parse(dttime.Rows[pos]["hDateTime"].ToString());
                    int lgintime = 10;
                    int.TryParse(Config.GetValue("LoginTime").ToString(), out lgintime);

                    if ((DateTime.Now - timeloginStart).TotalMinutes > lgintime)
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            return true;
        }
    }
}
