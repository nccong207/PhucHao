using CDTDatabase;
using CDTLib;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XulyGiaDHMi
{
    public class XulyGiaDHMi : ICControl
    {
        Database db = Database.NewDataDatabase();
        DataCustomFormControl _data;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        GridView gvMain, gvDH;
        public void AddEvent()
        {
            GridControl gcMain = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl);

            //RepositoryItemGridLookUpEdit glu = gcMain.RepositoryItems["Ma"] as RepositoryItemGridLookUpEdit;
            GridLookUpEdit cbKH = (_data.FrmMain.Controls.Find("MaKH", true)[0] as GridLookUpEdit);
            cbKH.EditValueChanged += cbKH_TextChanged;
            gvMain = gcMain.MainView as GridView;
            gvMain.CellValueChanged += gvMain_CellValueChanged;
            
        }

        void cbKH_TextChanged(object sender, EventArgs e)
        {
            GridLookUpEdit cbKH = sender as GridLookUpEdit;
            if (cbKH.Properties.ReadOnly == true)
            {
                return;
            }
            DataRowView drMaster = _data.BsMain.Current as DataRowView;
            if (drMaster.Row.RowState == DataRowState.Unchanged)
            {
                return;
            }
            
            string maKH = drMaster.Row["MaKH"].ToString();
            if (!string.IsNullOrEmpty(maKH))
            {
                for (int i = 0; i < gvMain.DataRowCount; i++)
                {
                    var maSP = gvMain.GetRowCellValue(i, "MaSP").ToString();
                    double dongia = getGiaDH(maKH, maSP);
                    if ( dongia.Equals((double) -1)) {
                        gvMain.SetRowCellValue(i, "DonGia", null);
                        gvMain.SetRowCellValue(i, "ThanhTien", null);
                    } else {
                        gvMain.SetRowCellValue(i, "DonGia", dongia);
                    }
                }
            }
        }

        private void gvMain_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Value == null || e.Column.FieldName != "MaSP")
            {
                return;
            }

            String MaSP = e.Value.ToString();
            DataRowView drMaster = _data.BsMain.Current as DataRowView;
            
            string maKH = drMaster.Row["MaKH"].ToString();
            if (string.IsNullOrEmpty(maKH))
            {
                XtraMessageBox.Show("Chưa chọn khách hàng cho đơn hàng này!", Config.GetValue("PackageName").ToString());
                return;
            }
            double dongia = getGiaDH(maKH, MaSP);
            if (dongia == -1)
            {
                XtraMessageBox.Show("Chưa cài đặt giá bán cho khách hàng/khu vực này", Config.GetValue("PackageName").ToString());
                return;
            }
            var data = _data.FrmMain.Controls.Find("gcMain", true);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["DGGoc"], dongia);
            
        }

        private double getGiaDH (string maKH, string maSP) {
              DataTable dtBangGia = db.GetDataTable(string.Format(@"select gb.MaKH, gb.MaSP, GiaBan from wBangGia gb left join mDMKH kh on gb.KhuVuc = kh.KhuVuc
                where gb.Duyet = 1 and (gb.MaKH = '{0}' or kh.MaKH = '{0}') and MaSP = '{1}'", maKH, maSP));
            if (dtBangGia.Rows.Count == 0)
            {
                return -1;
            }
            // Cap nhat don gia.
            double dongia = double.Parse(dtBangGia.Rows[0]["GiaBan"].ToString());
            return dongia;
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
