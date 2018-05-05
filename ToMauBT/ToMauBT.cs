using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

namespace ToMauBT
{
    public class ToMauBT : ICControl
    {
        private DataCustomFormControl _data;
        private InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetail);
        GridView gvMain;

        public void AddEvent()
        {
            var tb = _data.DrTableMaster["TableName"].ToString();
            if (tb != "MTDeNghi" && tb != "MTDNTT" && tb != "MTMuaHang" && tb != "MT41" && tb != "MT42")
                return;

            gvMain = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;
            //gvMain.RowStyle += GvMain_RowStyle;
            _data.FrmMain.Load += new EventHandler(FrmMain_Load);
        }

        //private void GvMain_RowStyle(object sender, RowStyleEventArgs e)
        //{
        //    GridView View = sender as GridView;
        //    if (e.RowHandle >= 0)
        //    {
        //        bool duyet = (bool) View.GetRowCellValue(e.RowHandle, View.Columns["Duyet"]);

        //        if (duyet)
        //        {
        //            //green
        //            e.Appearance.BackColor = Color.LightGreen;
        //            e.Appearance.BackColor2 = Color.LightGreen;
        //        }
        //    }
        //}

        void FrmMain_Load(object sender, EventArgs e)
        {
            if ((_data.BsMain.DataSource as DataSet).Tables[0].Columns.Contains("Duyet"))
            {
                //dung class StyleFormatCondition cho Duyet
                StyleFormatCondition d = new StyleFormatCondition();
                gvMain.FormatConditions.Add(d);
                //thiet lap dieu kien
                d.Column = gvMain.Columns["Duyet"];
                d.Condition = FormatConditionEnum.Equal;
                d.Value1 = true;
                //thiet lap dinh dang
                d.Appearance.BackColor = Color.LightGreen;
                d.ApplyToRow = true;
            }
        }
        InfoCustomControl ICControl.Info {get { return _info; } }
        DataCustomFormControl ICControl.Data { set { _data = value; } }
    }
}
