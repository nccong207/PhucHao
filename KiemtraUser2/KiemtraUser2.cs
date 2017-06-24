using CDTDatabase;
using CDTLib;
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
            if (!isAcess())
            {
                LoginForm frm = new LoginForm();
                if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                //else
                //    _info. = false;
            }
            _data.FrmMain.Activated += FrmMain_Activated;
        }
        private void FrmMain_Activated(object sender, EventArgs e)
        {
            if (!isAcess())
            {
                LoginForm frm = new LoginForm();
                if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                //else
                //    _info. = false;
            }
        }

        private bool isAcess()
        {
            string isAdmin = Config.GetValue("Admin").ToString();
            if (!Convert.ToBoolean(isAdmin))
            {
                string sysUserID = Config.GetValue("sysUserID").ToString();

                string sql = string.Format("SELECT TOP 1 * FROM sysHistory WHERE sysUserID = {0} and Action = 'Login' ORDER by hDateTime DESC", sysUserID);
                Database db = Database.NewStructDatabase();
                DataTable dttime = db.GetDataTable(sql);

                if (dttime.Rows.Count > 0)
                {
                    DateTime timeloginStart = DateTime.Parse(dttime.Rows[0]["hDateTime"].ToString());
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
