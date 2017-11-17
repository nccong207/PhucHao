using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Plugins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ToMauBT
{
    public class ToMauBT : ICControl
    {
        private DataCustomFormControl _data;
        private InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetail);
      

        public void AddEvent()
        {
            var tb = _data.DrTableMaster["TableName"].ToString();
            if (tb != "MTDeNghi" && tb != "MTMuaHang" )
                return;

            var gvMain = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;
            gvMain.RowStyle += GvMain_RowStyle;
        }

        private void GvMain_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                bool duyet = (bool) View.GetRowCellValue(e.RowHandle, View.Columns["Duyet"]);

                if (duyet)
                {
                    //green
                    e.Appearance.BackColor = Color.LightGreen;
                    e.Appearance.BackColor2 = Color.LightGreen;
                }
            }
        }

        InfoCustomControl ICControl.Info {get { return _info; } }
        DataCustomFormControl ICControl.Data { set { _data = value; } }
    }
}
