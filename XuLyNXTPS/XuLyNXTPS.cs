using CDTDatabase;
using CDTLib;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XuLyNXTPS
{
    public class XuLyNXTPS : ICReport
    {
        private Database db = Database.NewDataDatabase();
        private DataCustomReport _data;
        private InfoCustomReport _info = new InfoCustomReport(IDataType.Report);
        GridView gvMain;
        private bool blAuto = false;

        public void Execute()
        {
            gvMain = (_data.FrmMain.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;
            SimpleButton btnXL = _data.FrmMain.Controls.Find("btnXuLy", true)[0] as SimpleButton;
            btnXL.Text = "F4 - Xử lý";
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
                UpdateNhapXuatTonPS();
            else
                XtraMessageBox.Show("Người dùng không có quyền thực hiện chức năng này\nVui lòng liên hệ quản trị hệ thống!",
                    Config.GetValue("PackageName").ToString());
        }

        private void UpdateNhapXuatTonPS()
        {
            DataView dv = gvMain.DataSource as DataView;
            dv.Table.AcceptChanges();
            dv.RowFilter = "[Chon] = 1";
            if (dv.Count == 0)
            {
                dv.RowFilter = "";
                XtraMessageBox.Show("Vui lòng đánh dấu chọn đơn hàng cần xử lý", Config.GetValue("PackageName").ToString());
                return;
            }

            string sql = @"UPDATE wBLPS SET KoXuatBC = 1 WHERE DTDHID = '{0}'";

            bool rs = true;
            foreach (DataRowView drv in dv)
            {
                rs = db.UpdateByNonQuery(string.Format(sql, drv["DTDHID"]));
                if (rs)
                    drv.Row.Delete();
                else
                    break;
            }

            dv.Table.AcceptChanges();
            dv.RowFilter = "";//Bỏ fillter

            if (rs)
                XtraMessageBox.Show("Cập nhật dữ liệu thành công", Config.GetValue("PackageName").ToString());
        }
        public DataCustomReport Data
        {
            set { _data = value; }
        }

        public InfoCustomReport Info
        {
            get { return _info; }
        }
    }
}
