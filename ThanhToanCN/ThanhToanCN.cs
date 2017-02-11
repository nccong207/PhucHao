using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CDTLib;
using Plugins;
using CDTDatabase;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;

namespace ThanhToanCN
{
    public class ThanhToanCN:ICReport
    {
        private Database db = Database.NewDataDatabase();
        private DataCustomReport _data;
        private InfoCustomReport _info = new InfoCustomReport(IDataType.Report);
        GridView gvMain;
        private bool blAuto = false;
        #region ICControl Members

        public void Execute()
        {
            gvMain = (_data.FrmMain.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;
            //gvMain.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gvMain_CellValueChanged);
            //gvMain.DataSourceChanged += new EventHandler(gvMain_DataSourceChanged);             //xu ly dinh dang trong su kien nay
            SimpleButton btnXL = _data.FrmMain.Controls.Find("btnXuLy", true)[0] as SimpleButton;
            btnXL.Click += new EventHandler(btnXL_Click);
        }

        void btnXL_Click(object sender, EventArgs e)
        {
            //kiem tra quyen su dung
            bool admin = Convert.ToBoolean(Config.GetValue("Admin"));
            bool hasRight = admin ||
                (_data.DrReport.Table.Columns.Contains("sInsert") && Convert.ToBoolean(_data.DrReport["sInsert"])) ||
                (_data.DrReport.Table.Columns.Contains("sUpdate") && Convert.ToBoolean(_data.DrReport["sUpdate"])) ||
                (_data.DrReport.Table.Columns.Contains("sDelete") && Convert.ToBoolean(_data.DrReport["sDelete"]));
            if (hasRight)
                UpdateDaTT();
            else
                XtraMessageBox.Show("Người dùng không có quyền thực hiện chức năng này\nVui lòng liên hệ quản trị hệ thống!",
                    Config.GetValue("PackageName").ToString());
        }

        private void UpdateDaTT()
        {
            DataView dv = gvMain.DataSource as DataView;
            dv.Table.AcceptChanges();
            dv.RowFilter = "[DaTT] = 1";
            if (dv.Count == 0)
            {
                dv.RowFilter = "";
                XtraMessageBox.Show("Vui lòng đánh dấu chọn phiếu để thanh toán", Config.GetValue("PackageName").ToString());
                return;
            }

            string sql = "UPDATE MT32 SET DaTT = 1 WHERE MT32ID IN ({0});" +
                "UPDATE MT33 SET DaTT = 1 WHERE MT33ID IN ({0});" +
                "UPDATE MT44 SET DaTT = 1 WHERE MT44ID IN ({0});";
            string dk = "";
            foreach (DataRowView drv in dv)
            {
                dk += string.Format("'{0}',", drv.Row["MT23ID"]);
                drv.Row.Delete();
            }

            dv.Table.AcceptChanges();

            dv.RowFilter = "";//Bỏ fillter

            if (dk != "")
            {
                dk = dk.Substring(0, dk.Length - 1); //Bỏ dấu ',' ở cuối
                sql = string.Format(sql, dk);
                if (db.UpdateByNonQuery(sql))
                    XtraMessageBox.Show("Cập nhật dữ liệu thành công", Config.GetValue("PackageName").ToString());
            }
        }

        public DataCustomReport Data
        {
            set { _data = value; }
        }

        public InfoCustomReport Info
        {
            get { return _info; }
        }

        #endregion

    }
}
