using CDTLib;
using DevExpress.XtraEditors;
using Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocKHTheoNV
{
    public class LocKHTheoNV : ICControl
    {
        DataCustomFormControl _data;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        GridLookUpEdit KHList, TenKHList;
        
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

                    TenKHList = (_data.FrmMain.Controls.Find("MaKH_Tentat", true)[0]) as GridLookUpEdit;
                    if (TenKHList != null)
                    {
                        TenKHList.Popup += KHList_Popup;
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
