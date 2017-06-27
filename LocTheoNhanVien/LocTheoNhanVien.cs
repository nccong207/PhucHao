
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Plugins;
using FormFactory;

using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors;
using CDTLib;
using CBSControls;

namespace LocTheoNhanVien
{
    public class LocTheoNhanVien : ICControl
    {
        DataCustomFormControl _data;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetail);
        GridLookUpEdit KHList;
        List<string> tbList = new List<string>() { "MTBAOGIA", "MTLSX", "MTDONHANG" };
        string sysUser;
        public void AddEvent()
        {
            string tb = _data.DrTableMaster["TableName"].ToString();
            if (tbList.Contains(tb.ToUpper()))
            {
                sysUser = Config.GetValue("UserName").ToString();
                var isFilterKH = Config.GetValue("IsFilter").ToString();
                if (!string.IsNullOrEmpty(isFilterKH) && Convert.ToBoolean(isFilterKH))
                {
                    KHList = (_data.FrmMain.Controls.Find("MaKH", true)[0]) as GridLookUpEdit;
                    if (KHList != null)
                    {
                        KHList.Popup += KHList_Popup;
                    }
                }
            }
        }

        private void KHList_Popup(object sender, EventArgs e)
        {
            GridLookUpEdit gluKH = sender as GridLookUpEdit;
            gluKH.Properties.View.ActiveFilterString = string.Format("NVPT = '{0}'", sysUser);
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
