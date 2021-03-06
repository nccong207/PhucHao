using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using CDTDatabase;
using System.Data;
using DevExpress.XtraEditors;
using CDTLib;
using System.Windows.Forms;

namespace KtraChi
{
    public class KtraChi:ICData
    {
        #region ICData Members
        private DataCustomData _data;
        private InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);

        public DataCustomData Data
        {
            set { _data = value; }
        }

        public void ExecuteAfter()
        {

        }

        public void ExecuteBefore()
        {
            DataRow drMaster = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drMaster.RowState == DataRowState.Deleted)
                return;
            _info.Result = true;
            DataRow[] drs = _data.DsData.Tables[1].Select("MT12ID = '" + drMaster["MT12ID"] + "'");
            //kiem tra chon doi tuong neu co theo doi cong no
            bool rs = true;
            Database db = Database.NewDataDatabase();
            foreach (DataRow dr in drs)
            {
                if (dr.RowState == DataRowState.Deleted || dr.RowState == DataRowState.Unchanged)
                    continue;
                string sql = "select IsCN from DMChi where ID = " + dr["LoaiChi"];
                object isCN = db.GetValue(sql);
                if (Convert.ToBoolean(isCN))
                {
                    if (dr["MaNCC"] == DBNull.Value)
                    {
                        rs = false;
                        break;
                    }
                }
            }
            _info.Result = rs;
            if (rs == false)
                XtraMessageBox.Show("Cần chọn nhà cung cấp cho loại chi này (chi công nợ)",
                    Config.GetValue("PackageName").ToString(), MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //kiem tra chon TK ngan hang neu chuyen khoan
            if (drMaster["HinhThucTT"].ToString() == "Chuyển khoản")
            {
                rs = true;
                foreach (DataRow dr in drs)
                {
                    if (dr.RowState == DataRowState.Deleted)
                        continue;
                    if (dr["TaiKhoan"] == DBNull.Value)
                    {
                        rs = false;
                        break;
                    }
                }
                _info.Result = rs;
                if (rs == false)
                    XtraMessageBox.Show("Cần chọn tài khoản ngân hàng đối với hình thức chuyển khoản",
                        Config.GetValue("PackageName").ToString(), MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public InfoCustomData Info
        {
            get { return _info; }
        }

        #endregion
    }
}
