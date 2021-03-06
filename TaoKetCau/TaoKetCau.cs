using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using CDTLib;
using System.Globalization;
using CDTDatabase;
using DevExpress.XtraGrid.Views.Grid;

namespace TaoKetCau
{
    public class TaoKetCau : ICData
    {
        List<string> lstNL = new List<string>(new string[] { "Mat_", "SB_", "MB_", "SC_", "MC_", "SE_", "ME_" });
        DataCustomData _data;
        InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);
        GridView grDA;
        DataRow drCur;
        #region ICData Members

        public DataCustomData Data
        {
            set { _data = value; }
        }

        public void ExecuteAfter()
        {
        }

        public void ExecuteBefore()
        {
            drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];

            if (drCur.RowState == DataRowState.Added || drCur.RowState == DataRowState.Modified || drCur.RowState == DataRowState.Unchanged)
            {
                DataTable tb1 = _data.DsData.Tables[1];
                DataRow[] rows = tb1.Select(string.Format("MTBGID = '{0}'", drCur["MTBGID"]));
                if (rows.Length == 0)
                {
                    XtraMessageBox.Show("Không thể lưu báo giá khi không có các mặt hàng.", Config.GetValue("PackageName").ToString());
                    _info.Result = false;
                    return;
                }
            }

            if (drCur.RowState == DataRowState.Deleted)
                return;
            //Công thêm ngày 28/12/2015 để sửa lỗi khi copy báo giá hết hạn [Start]
            if (drCur.RowState == DataRowState.Added && Convert.ToBoolean(drCur["HetHan"]))
                drCur["HetHan"] = false;
            //Công thêm ngày 28/12/2015 để sửa lỗi khi copy báo giá hết hạn [End]

            //Công thêm ngày 20/03/2017 thực hiện chức năng kiểm tra ngày hết hạn báo giá theo config.
            var maximumBaoGiaDate = Config.GetValue("MaximumBaoGiaDate");
            if (maximumBaoGiaDate != null)
            {
                var maxMonth = Convert.ToInt32(maximumBaoGiaDate.ToString());
                var ngayhh = (DateTime)drCur["NgayHH"];
                var ngaybd = (DateTime) drCur["NgayCT"];

                if (ngayhh.AddMonths(-maxMonth) > ngaybd)
                {
                    XtraMessageBox.Show(string.Format("Thời gian hết hạn tối đa là {0} tháng,\nbạn đã vượt quá thời gian tối đa, vui lòng kiểm tra lại", maxMonth), Config.GetValue("PackageName").ToString());
                    _info.Result = false;
                    return;
                }
            }

            DataView dv = new DataView(_data.DsData.Tables[1]);
            dv.RowStateFilter = DataViewRowState.Added | DataViewRowState.ModifiedCurrent;
            foreach (DataRowView drv in dv)
            {
                
                int lop = Convert.ToInt32(drv["Lop"]);
                int n = 0;
                List<string> lstKC = new List<string>();
                List<int> lstStt = new List<int>();
                foreach (string snl in lstNL)
                {
                    string nl = drv[snl + "Giay"].ToString();
                    if (nl != "")
                    {
                        n++;
                        string[] s = nl.Split('.');
                        if (s.Length < 2)
                            continue;
                        if (lstKC.Contains(s[1]))
                            lstStt[lstKC.IndexOf(s[1])] = lstStt[lstKC.IndexOf(s[1])] + 1;
                        else
                        {
                            lstKC.Add(s[1]);
                            lstStt.Add(1);
                        }
                    }
                }
                if (lop != n)
                {
                    XtraMessageBox.Show(string.Format("Mặt hàng {0}: Vui lòng nhập vừa đủ {1} loại giấy để có kết cấu đúng!",
                    drv["TenHang"], lop), Config.GetValue("PackageName").ToString());
                    _info.Result = false;
                    return;
                }
                string tmp = "";
                for (int i = 0; i < lstKC.Count; i++)
                    tmp += lstStt[i].ToString() + lstKC[i] + "+";
                if (tmp != "")
                    drv.Row["KetCau"] = tmp.Remove(tmp.Length - 1);

                
            }
            //
            //Kiểm tra loại lần
            //KTLan();
            KTBCao();

            //Kiểm tra báo giá cũ
            string bgCu = drCur["SoBGCopy"].ToString();
            if (!string.IsNullOrEmpty(bgCu))
            {
                KiemTraBaoGiaCu(bgCu);
            }
        }

        private void KiemTraBaoGiaCu(string SoBGCu) {
            string query = "SELECT * FROM MTBaoGia WHERE SoBG = '{0}' AND Huy = 0";

            Database db = Database.NewDataDatabase();
            DataTable dtBg = db.GetDataTable(string.Format(query, SoBGCu));
            if (dtBg.Rows.Count > 0)
            {
                if (XtraMessageBox.Show("Có muốn vô hiệu báo giá cũ không ?", Config.GetValue("PackageName").ToString(), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    query = "UPDATE MTBaoGia SET NgayHH = GETDATE(), HetHan = 1 WHERE SoBG = '{0}'";
                    db.UpdateByNonQuery(string.Format(query, SoBGCu));
                }
            }
        }

        private void KTLan()
        { 
            DataRow drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted)
                return;
            DataView dv = new DataView(_data.DsData.Tables[1]);
            dv.RowStateFilter = DataViewRowState.Added | DataViewRowState.ModifiedCurrent;
            foreach (DataRowView drv in dv)
            {
                if (drv["Lan"] == DBNull.Value && drv["Loai"].ToString() == "Thùng")
                {
                    XtraMessageBox.Show("Chưa nhập loại lằn");
                    _info.Result = false;
                }
            }
        }

        private void KTBCao()
        {
            DataView dv = new DataView(_data.DsData.Tables[1]);
            dv.RowFilter = "MTBGID = '" + drCur["MTBGID"].ToString() + "'";

            if (Convert.ToBoolean(dv[0].Row["KCT"]) == true || dv[0].Row["Loai"].ToString().ToUpper().Equals("TẤM"))
            {
                return;
            }
            if ((Convert.ToBoolean(dv[0].Row["Ghim"]) == false && Convert.ToBoolean(dv[0].Row["Dan"]) == false)
                || (Convert.ToBoolean(dv[0].Row["Ghim"]) == true && Convert.ToBoolean(dv[0].Row["Dan"]) == true))
            {
                XtraMessageBox.Show("Bạn vui lòng cho biết thùng \"Dán\" hay \"Đóng ghim\"");
                _info.Result = false;
            }

            //
        }

        public InfoCustomData Info
        {
            get { return _info; }
        }

        #endregion
    }
}
