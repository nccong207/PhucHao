using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using System.Drawing;

namespace HighlightPS
{
    public class HighlightPS:ICReport
    {

        #region ICReport Members
        private DataCustomReport _data;
        private InfoCustomReport _info = new InfoCustomReport(IDataType.Report);
        GridView gvMain;

        public DataCustomReport Data
        {
            set { _data = value; }
        }

        public void Execute()
        {
            gvMain = (_data.FrmMain.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;
            //gvMain.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(gvMain_CustomDrawCell);
            gvMain.RowStyle += new RowStyleEventHandler(gvMain_RowStyle);
            gvMain.OptionsView.EnableAppearanceOddRow = false;
            gvMain.OptionsView.EnableAppearanceEvenRow = false;
        }

        void gvMain_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (gvMain.GetRowCellValue(e.RowHandle, "Số lượng nhập") != null && gvMain.GetRowCellValue(e.RowHandle, "Số lượng nhập") != DBNull.Value
                && gvMain.GetRowCellValue(e.RowHandle, "Tồn cuối") != null && gvMain.GetRowCellValue(e.RowHandle, "Tồn cuối") != DBNull.Value)
            {
                decimal nhap = Convert.ToDecimal(gvMain.GetRowCellValue(e.RowHandle, "Số lượng nhập"));
                decimal ton = Convert.ToDecimal(gvMain.GetRowCellValue(e.RowHandle, "Tồn đầu"));
                decimal toncuoi = Convert.ToDecimal(gvMain.GetRowCellValue(e.RowHandle, "Tồn cuối"));
                decimal a = toncuoi / ((nhap + ton) == 0 ? 1:(nhap + ton)) * 100;
                if (a < -2)
                {
                   e.Appearance.BackColor = Color.Yellow;
                }
            }
        }

        void gvMain_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName.Equals("Tồn cuối") && e.CellValue != null && e.CellValue != DBNull.Value)
            {
                decimal nhap = Convert.ToDecimal(gvMain.GetRowCellValue(e.RowHandle,"Số lượng nhập")) * 100;
                decimal toncuoi = Convert.ToDecimal(e.CellValue);
                decimal a = toncuoi / (nhap == 0 ? 1 : nhap);
                if (a >= -5 && a <= 5)
                    e.Appearance.BackColor = Color.Yellow;
                gvMain.Appearance.FocusedRow.BackColor = Color.Yellow;
            }
        }

        public InfoCustomReport Info
        {
            get { return _info; }
        }

        #endregion
    }
}
