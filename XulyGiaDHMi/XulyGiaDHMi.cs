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
            gvMain = gcMain.MainView as GridView;
            gvMain.CellValueChanged += gvMain_CellValueChanged;
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
            // Lay bang gia theo ma khach hang.
            DataTable dtBangGia = db.GetDataTable(string.Format(@"select gb.MaKH, gb.MaSP, GiaBan from wBangGia gb left join mDMKH kh on gb.KhuVuc = kh.KhuVuc
                where gb.Duyet = 1 and (gb.MaKH = '{0}' or kh.MaKH = '{0}') and MaSP = '{1}'", maKH, MaSP));
            if (dtBangGia.Rows.Count == 0)
            {
                XtraMessageBox.Show("Chưa cài đặt giá bán cho khách hàng/khu vực này", Config.GetValue("PackageName").ToString());
                return;
            }
            // Cap nhat don gia.
            double dongia = double.Parse(dtBangGia.Rows[0]["GiaBan"].ToString());
            var data = _data.FrmMain.Controls.Find("gcMain", true);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["DGGoc"], dongia);
            
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
