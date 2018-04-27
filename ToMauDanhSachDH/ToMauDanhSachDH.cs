using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

namespace ToMauDanhSachDH
{
    public class ToMauDanhSachDH : ICReport
    {
        private DataCustomReport _data;
        private InfoCustomReport _info = new InfoCustomReport(IDataType.Report);
        GridView gvMain;

        public void Execute()
        {
            gvMain = (_data.FrmMain.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;
            gvMain.OptionsView.EnableAppearanceEvenRow = false;
            gvMain.OptionsView.EnableAppearanceOddRow = false;
            gvMain.Appearance.FocusedRow.BackColor = Color.Transparent;
            gvMain.RowStyle += gridView1_RowStyle;
        }


        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0 && View.IsDataRow(e.RowHandle))
            {
                object ngaygiahan = View.GetRowCellValue(e.RowHandle, "Ngày được gia hạn");
                object ngayxuatgannhat = View.GetRowCellValue(e.RowHandle, "Ngày xuất gần nhất");
                object ngaynhapkho = View.GetRowCellValue(e.RowHandle, "Ngày nhập kho");
                object loai = View.GetRowCellValue(e.RowHandle, "Loại");
                //lay ngay cho phep theo loai hang
                int days = loai.ToString().Equals("Thùng") ? 10 : 4;
                // lay ngay hien tai
                var ngayht = DateTime.Today;
                //tinh ngay ht va ngay gia han
                var ng1 = 0;
                var ng2 = 0;
                var ng3 = 0;

                if (ngaygiahan != null)
                {
                    ng1 = ((DateTime)ngayht - (DateTime)ngaygiahan).Days;
                }

                //tinh ngay ht va ngay xuat gan nhat
                if (ngayxuatgannhat != null)
                {
                    ng2 = ((DateTime)ngayht - (DateTime)ngayxuatgannhat).Days - 4;
                }

                //tinh ngay ht va ngay nhap kho
                if (ngaynhapkho != null)
                {
                    ng3 = ((DateTime)ngayht - (DateTime)ngaynhapkho).Days - days;
                }

                // kiem tra cac ngay
                if (ng3 > 0 && ng2 > 0 && ng1 > 0)
                {
                    e.Appearance.BackColor = Color.Red;
                }
                else if (ng3 > 0 && ng2 > 0 && ng1 == 0)
                {
                    e.Appearance.BackColor = Color.Red;
                }
                else if (ng3 > 0 && ng2 == 0 && ng1 == 0)
                {
                    e.Appearance.BackColor = Color.Red;
                }
                else if (ng3 == 0 && ng2 == 0 && ng1 == 0)
                {
                    e.Appearance.BackColor = Color.Green;
                }
                else if (ng3 > 0 && ng2 < 0 && ng1 < 0)
                {
                    e.Appearance.BackColor = Color.Green;
                }
                else if (ng3 > 0 && ng2 < 0 && ng1 == 0)
                {
                    e.Appearance.BackColor = Color.Green;
                }
                else if (ng3 > 0 && ng2 == 0 && ng1 == 0)
                {
                    e.Appearance.BackColor = Color.Green;
                }

               
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
    }
}
