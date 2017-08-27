using CDTLib;
using DevExpress.XtraEditors;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KTXuatAmPS
{
    public class KTXuatAmPS : ICData
    {
        DataCustomData _data;
        InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);
        public DataCustomData Data { set { _data = value; } }
        public InfoCustomData Info { get { return _info; } }

        public void ExecuteAfter()
        {
        }

        public void ExecuteBefore()
        {
            DataRow drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted)
                return;
            DataView dv = new DataView(_data.DsData.Tables[1]);
            dv.RowStateFilter = DataViewRowState.Added | DataViewRowState.ModifiedCurrent;

            string sql = @"select sum(isnull(soluong,0) - isnull(soluong_x,0)) from wBLPS 
                        where MTIDDT <> '{0}' and DTDHID = '{1}' and NgayCT <= '{2}'";
            foreach (DataRowView drv in dv)
            {
                string dtid = drv["DTID"].ToString();
                string dtdhid = drv["DTDHID"].ToString();
                string tenHH = drv["TenHang"].ToString();
                string ngayct = drCur["NgayCT"].ToString();

                // int loi = Boolean.Parse(drv["Loi"].ToString()) ? 1 : 0;
                object slConLai = _data.DbData.GetValue(string.Format(sql, dtid, dtdhid, ngayct));

                decimal slConLaiNum = slConLai == DBNull.Value ? 0 : decimal.Parse(slConLai.ToString());
                decimal slXuat = decimal.Parse(string.IsNullOrEmpty(drv["SoLuong"].ToString())? "0": drv["SoLuong"].ToString());

                if (slXuat > slConLaiNum)
                {
                    XtraMessageBox.Show("Không được xuất vượt quá số lượng tồn.\n" +
                        tenHH + ": Số lượng xuất = " + slXuat.ToString("###,##0") + "; Số lượng tồn = " + slConLaiNum.ToString("###,##0"),
                        Config.GetValue("PackageName").ToString());
                    _info.Result = false;
                    return;
                }
            }
            _info.Result = true;
        }
    }
}
