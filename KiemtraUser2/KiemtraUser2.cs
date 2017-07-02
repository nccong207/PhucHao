using CDTDatabase;
using CDTLib;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
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
        private bool blockAccess = false;
        GridView gvMain;
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
            gvMain = (_data.FrmMain.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;
            if (!isAcess())
            {
                LoginForm frm = new LoginForm();
                if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    blockAccess = true;
            }

            _data.FrmMain.Activated += FrmMain_Activated;
            _data.FrmMain.Shown += FrmMain_Shown;
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            if (blockAccess)
            {
                blockAccess = false;
                _data.FrmMain.Close();
            }
        }

        private void FrmMain_Activated(object sender, EventArgs e)
        {
            if (!isAcess(true))
            {
                gvMain.ActiveFilterString = "1 = 0";
                 LoginForm frm = new LoginForm();
                if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    _data.FrmMain.Close();
                }
                else
                {
                    gvMain.ClearColumnsFilter();
                }
            }
        }

        private bool isAcess(bool isActiveActin = false)
        {
            string sysUserID = Config.GetValue("sysUserID").ToString();

            string sql = string.Format("SELECT TOP 3 * FROM sysHistory WHERE sysUserID = {0} ORDER by hDateTime DESC", sysUserID);
            Database db = Database.NewStructDatabase();
            DataTable dttime = db.GetDataTable(sql);

            if (dttime.Rows.Count > 0)
            {
                int pos = isActiveActin ? 0 : 2;
                DateTime timeloginStart = DateTime.Parse(dttime.Rows[pos]["hDateTime"].ToString());
                int lgintime = 10;
                int.TryParse(Config.GetValue("LoginTime").ToString(), out lgintime);

                if ((DateTime.Now - timeloginStart).TotalMinutes > lgintime)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
    }
}
