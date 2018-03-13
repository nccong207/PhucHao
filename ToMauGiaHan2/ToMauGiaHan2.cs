using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

namespace ToMauGiaHan2
{
    public class ToMauGiaHan2 : ICReport
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
                object duyet = View.GetRowCellValue(e.RowHandle, "duyet");
                object ngaygiahan = View.GetRowCellValue(e.RowHandle, "NgayGHT");
                if (duyet == DBNull.Value || Convert.ToBoolean(duyet) == false || ngaygiahan == DBNull.Value)
                    return;

                var ngaygh = (DateTime)ngaygiahan;
                if (ngaygh < DateTime.Today)
                {
                    e.Appearance.BackColor = Color.Red;
                }
                else
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
