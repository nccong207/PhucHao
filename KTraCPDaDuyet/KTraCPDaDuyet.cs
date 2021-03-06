using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Data;
using CDTDatabase;
using DevExpress.XtraEditors;
using CDTLib;

namespace KTraCPDaDuyet
{
    public class KTraCPDaDuyet : ICData
    {
        private DataCustomData _data;
        private InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);
        private List<string> _lstTb = new List<string>(new string[] { "MT12", "MT43", "MT22", "MTNPhoi" });
        #region ICData Members

        public DataCustomData Data
        {
            set { _data = value; }
        }

        public void ExecuteAfter()
        {
            
        }

        public void ExecuteBefore()
        {
            if (!_lstTb.Contains(_data.DrTableMaster["TableName"].ToString()))
                return;
            string sql = "select count(*) from MTCPSX where Thang = {0} and Nam = {1} and Duyet = 1";
            DataRow drMaster = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            int t = 0;
            Database db = Database.NewDataDatabase();
            if (drMaster.RowState == DataRowState.Added)
            {
                DateTime dt = Convert.ToDateTime(drMaster["NgayCT"]);
                t = Convert.ToInt32(db.GetValue(string.Format(sql, dt.Month, dt.Year)));
            }
            if (drMaster.RowState == DataRowState.Deleted)
            {
                DateTime dt = Convert.ToDateTime(drMaster["NgayCT", DataRowVersion.Original]);
                t = Convert.ToInt32(db.GetValue(string.Format(sql, dt.Month, dt.Year)));
            }
            if (drMaster.RowState == DataRowState.Modified)
            {
                DateTime dt = Convert.ToDateTime(drMaster["NgayCT"]);
                DateTime dt1 = Convert.ToDateTime(drMaster["NgayCT", DataRowVersion.Original]);
                t = Convert.ToInt32(db.GetValue(string.Format(sql, dt.Month, dt.Year))) +
                    Convert.ToInt32(db.GetValue(string.Format(sql, dt1.Month, dt1.Year)));
            }
            if (t > 0)
                XtraMessageBox.Show("Chi phí sản xuất tháng này đã được duyệt, không thể thay đổi số liệu liên quan!",
                    Config.GetValue("PackageName").ToString(), System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            _info.Result = (t == 0);
        }

        public InfoCustomData Info
        {
            get { return _info; }
        }

        #endregion
    }
}
