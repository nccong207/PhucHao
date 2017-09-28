using CDTLib;
using DevExpress.XtraEditors;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CheckTrungMaKho
{
    public class CheckTrungMaKho : ICControl
    {
        DataCustomFormControl _data;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        public DataCustomFormControl Data
        {
            set { _data = value; }
        }

        public InfoCustomControl Info
        {
            get { return _info; }
        }

        public void AddEvent()
        {
            _data.BsMain.DataSourceChanged += new EventHandler(BsMain_DataSourceChanged);
            BsMain_DataSourceChanged(_data.BsMain, new EventArgs());
        }

        void BsMain_DataSourceChanged(object sender, EventArgs e)
        {
            DataSet ds = _data.BsMain.DataSource as DataSet;
            if (ds == null)
                return;

            DataTable dt = ds.Tables[0];
            dt.ColumnChanged += Dt_ColumnChanged;
        }

        private void Dt_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (_data.BsMain.Current == null)
                return;

            if (e.Column.ToString().Equals("MaKho") && e.Column.ToString().Equals("MaKhoN"))
            {
                return;
            }

            DataRow drCur = (_data.BsMain.Current as DataRowView).Row;
            string makhox = drCur["MaKho"].ToString();
            string makhon = drCur["MaKhoN"].ToString();

            if (!string.IsNullOrEmpty(makhox) && !string.IsNullOrEmpty(makhon))
            {
                if (makhox.Equals(makhon))
                {
                    XtraMessageBox.Show("Mã kho nhập không được trùng với mã kho xuất.", Config.GetValue("PackageName").ToString());
                    drCur[e.Column.ToString()] = "";
                    return;
                }
            }
        }
    }
}
