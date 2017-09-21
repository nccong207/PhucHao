using CDTLib;
using DevExpress.XtraEditors;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CheckTrungMaKho
{
    public class CheckTrungMaKho : ICData
    {
        DataCustomData _data;
        InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);

        public DataCustomData Data
        {
            set { _data = value; }
        }

        public InfoCustomData Info
        {
            get { return _info; }
        }

        public void ExecuteAfter()
        {
            
        }

        public void ExecuteBefore()
        {
            DataRow drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted)
                return;

            if (drCur["MaKho"].ToString().Equals(drCur["MaKhoN"].ToString()))
            {
                XtraMessageBox.Show("Mã kho chuyển đến không được trùng với mã kho xuất.",Config.GetValue("PackageName").ToString());
                _info.Result = false;
                return;
            }
        }
    }
}
