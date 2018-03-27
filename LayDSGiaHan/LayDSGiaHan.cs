using CDTDatabase;
using CDTLib;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using FormFactory;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LayDSGiaHan
{
    public class LayDSGiaHan : ICControl
    {
        GridView gvMain;
        DataRow drCur;
        ReportPreview frmDS;
        GridView gvDS;
        DataCustomFormControl _data;
        Database db;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);

        public void AddEvent()
        {
            db = Database.NewDataDatabase();
            gvMain = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;
            //thêm nút chọn DH
            LayoutControl lcMain = _data.FrmMain.Controls.Find("lcMain", true)[0] as LayoutControl;
            SimpleButton btnChon = new SimpleButton();
            btnChon.Name = "btnChon";
            btnChon.Text = "Chọn đơn hàng quá hạn";
            LayoutControlItem lci = lcMain.AddItem("", btnChon);
            lci.Name = "cusChon";
            btnChon.Click += new EventHandler(btnChon_Click);

            _data.BsMain.DataSourceChanged += new EventHandler(BsMain_DataSourceChanged);
            BsMain_DataSourceChanged(_data.BsMain, new EventArgs());
        }

        void BsMain_DataSourceChanged(object sender, EventArgs e)
        {
            if (_data.BsMain == null)
                return;
            DataSet ds = _data.BsMain.DataSource as DataSet;
            ds.Tables[1].ColumnChanged += new DataColumnChangeEventHandler(TinhTienPhat_ColumnChanged);
        }

        void TinhTienPhat_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Row.RowState == DataRowState.Deleted)
                return;
            DataRow drMaster = (_data.BsMain.Current as DataRowView).Row;
            if (drMaster.RowState == DataRowState.Deleted || drMaster["NgayCT"] == DBNull.Value)
                return;

            if (e.Column.ColumnName == "NgayGHT")
            {
                ////xu li so tien phat
                var ngayht = DateTime.Today; //B
                var ngaygiaohang = (DateTime)e.Row["NgayGH"]; //A
                var ngaygiahan = (DateTime)e.Row["NgayGHT"]; //C
                var dtdhid = e.Row["DTDHID"].ToString(); 
                string sql = "select Loai from dtdonhang where dtdhid = '{0}'";
                object type = db.GetValue(string.Format(sql, dtdhid));
                if (type == DBNull.Value) return;
                string typedh = type.ToString();
                int days = typedh.Equals("Thùng") ? 10 : 4;

                var num = (ngayht - ngaygiahan).Days - days;

                if (num <= 0) return;
                 
                e.Row["SoNgayTre"] = num; //D
                var tienphat = Config.GetValue("TienPhatTreHan") == null ? 20000 : Convert.ToInt32(Config.GetValue("TienPhatTreHan"));
                e.Row["TienPhat"] = num * tienphat;
            }
        }

        void btnChon_Click(object sender, EventArgs e)
        {
            if (!gvMain.Editable)
            {
                XtraMessageBox.Show("Vui lòng chọn chế độ thêm hoặc sửa phiếu", Config.GetValue("PackageName").ToString());
                return;
            }
            drCur = (_data.BsMain.Current as DataRowView).Row;

            frmDS = FormFactory.FormFactory.Create(FormType.Report, "1614") as ReportPreview;
            gvDS = (frmDS.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;

            //viết xử lý cho nút F4-Xử lý trong report
            SimpleButton btnXuLy = (frmDS.Controls.Find("btnXuLy", true)[0] as SimpleButton);
            btnXuLy.Text = "Chọn đơn hàng";
            btnXuLy.Click += new EventHandler(btnXuLy_Click);
            frmDS.WindowState = FormWindowState.Maximized;
            frmDS.ShowDialog();
        }

        void btnXuLy_Click(object sender, EventArgs e)
        {
            DataTable dtDS = (gvDS.DataSource as DataView).Table;
            dtDS.AcceptChanges();

            DataRow[] drs = dtDS.Select("Chọn = 1");

            if (drs.Length == 0)
            {
                XtraMessageBox.Show("Bạn chưa chọn đơn hàng để gia hạn", Config.GetValue("PackageName").ToString());
                return;
            }
            frmDS.Close();

            //add du lieu vao danh sach

            foreach (DataRow dr in drs)
            {
                gvMain.AddNewRow();
                gvMain.UpdateCurrentRow();
                gvMain.SetFocusedRowCellValue(gvMain.Columns["DTDHID"], dr["DTDHID"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["SoDH"], dr["SoDH"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["MaKH"], dr["MaKH"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["TenHang"], dr["tenhh"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["LoaiDH"], dr["Loai"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["Dai"], dr["Dai"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["Rong"], dr["Rong"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["Cao"], dr["Cao"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["NgayGH"], dr["NgayGH"]);



            }

            gvMain.RefreshData();
        }

        public DataCustomFormControl Data
        {
            set { _data = value; }
        }

        public InfoCustomControl Info
        {
            get { return _info; }
        }
    }
}
