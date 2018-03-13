using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

namespace ToMauGiaHan3
{
    public class ToMauGiaHan3 : ICReport
    {
        private DataCustomReport _data;
        private InfoCustomReport _info = new InfoCustomReport(IDataType.Report);
        GridView gvMain;
        DataTable dtNCC;

        public void Execute()
        {
            gvMain = (_data.FrmMain.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;
            gvMain.RowStyle += new RowStyleEventHandler(gridView1_RowStyle);
        }


        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0 && View.IsDataRow(e.RowHandle))
            {
                object duyet = View.GetRowCellValue(e.RowHandle, "duyet");

                if (duyet != DBNull.Value)
                {
                    var isDuyet = (bool)duyet;
                    if (isDuyet)
                    {
                        e.Appearance.BackColor = Color.Green;
                        e.Appearance.BackColor2 = Color.Green;

                        object ngaygiahan = View.GetRowCellValue(e.RowHandle, "NgayGHT");
                        if (ngaygiahan != DBNull.Value)
                        {
                            var ngaygh = (DateTime)ngaygiahan;
                            if (ngaygh <= DateTime.Today)
                            {
                                e.Appearance.BackColor = Color.OrangeRed;
                                e.Appearance.BackColor2 = Color.OrangeRed;
                            }
                        }
                    }
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
