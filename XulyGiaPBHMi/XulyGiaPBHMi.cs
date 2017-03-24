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

namespace XulyGiaPBHMi
{
    public class XulyGiaPBHMi : ICControl
    {
        Database db = Database.NewDataDatabase();
        DataCustomFormControl _data;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        GridView gvMain;
        #region ICControl Members
        public void AddEvent()
        {
            GridControl gcMain = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl);
            gvMain = gcMain.MainView as GridView;
            gvMain.CellValueChanged += gvMain_CellValueChanged;
        }

        void gvMain_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
                if (e.Value == null || e.Column.FieldName != "MaSP") {
                    return;
                }

                DataRowView drMaster = _data.BsMain.Current as DataRowView;
                if (!string.IsNullOrEmpty(drMaster.Row["SoDH"].ToString()))
                {
                    return;
                }

                string maSP = e.Value.ToString();
                // Lay gia theo MaSP
                DataTable dtBangGia = db.GetDataTable(string.Format("select * from wBangGia where MaKH is null and KhuVuc is null and MaSP = '{0}'", maSP));
                if (dtBangGia.Rows.Count == 0)
                {
                    XtraMessageBox.Show("Chưa cài đặt giá bán lẻ!", Config.GetValue("PackageName").ToString());
                }
                else
                {
                    double dongia = double.Parse(dtBangGia.Rows[0]["GiaBan"].ToString());
                    var data = _data.FrmMain.Controls.Find("gcMain", true);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["DonGia"], dongia);
            }
        }
        #endregion

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
