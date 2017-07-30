using System;
using DevExpress.XtraGrid.Views.Grid;
using Plugins;
using CDTLib;
using CDTDatabase;
using System.Data;
using DevExpress.XtraGrid;
using System.Windows.Forms;

namespace KiemtraUser
{
    public class KiemtraUser : ICReport
    {
        private DataCustomReport _data;
        private InfoCustomReport _info = new InfoCustomReport(IDataType.Report);
        private bool isFirstTime = true;
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
            _data.FrmMain.Shown += FrmMain_Shown;
            _data.FrmMain.Activated += FrmMain_Activated;
        }

        private void CheckAccess()
        {
            if (!isAcess(true))
            {
                gvMain.ActiveFilterString = "1 = 0";
                LoginForm frm = new LoginForm();
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    _data.FrmMain.Close();
                }
                else
                {
                    gvMain.ActiveFilterString = "";
                }
            }
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            if (_data.FrmMain.Modal)
                return;

            if (!isFirstTime)
                return;
            isFirstTime = false;

            CheckAccess();
        }

        private void FrmMain_Activated(object sender, EventArgs e)
        {
            if (_data.FrmMain.Modal)
                return;

            if (isFirstTime)
                return;

            CheckAccess();
        }

        private bool isAcess(bool isActivated)
        {
            string sysUserID = Config.GetValue("sysUserID").ToString();

            string sql =
                (isActivated) ? string.Format("SELECT TOP 1 * FROM sysHistory WHERE sysUserID = {0} ORDER by hDateTime DESC", sysUserID)
                : string.Format("SELECT TOP 1 * FROM sysHistory WHERE sysUserID = {0} and (sysMenuID is null or (sysMenuID != 9519 and sysMenuID != 9530)) ORDER by hDateTime DESC", sysUserID);

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
                else
                {
                    return true;
                }
            }
            return false;
        }
    }
}
