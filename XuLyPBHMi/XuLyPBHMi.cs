using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Plugins;
using CDTDatabase;
using DevExpress.XtraEditors;
using CDTLib;

namespace XuLyPBHMi
{
    public class XuLyPBHMi : ICData
    {
        Database db = Database.NewDataDatabase();
        DataCustomData _data;
        InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);

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
            DataRow drMaster = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drMaster.RowState == DataRowState.Deleted)
                return;
            using (DataView dvDetail = new DataView(_data.DsData.Tables[1]))
            {
                if (drMaster.RowState == DataRowState.Modified)
                {
                    dvDetail.RowFilter = string.Format("MTBHID = '{0}' and DTDHID is null", drMaster["MTBHID"]);
                    dvDetail.RowStateFilter = DataViewRowState.CurrentRows;
                }
                else
                {
                    dvDetail.RowStateFilter = DataViewRowState.Added | DataViewRowState.ModifiedCurrent;
                    dvDetail.RowFilter = "DTDHID is null";
                }
                //lay bang gia ban le
                DataTable dtBangGia = db.GetDataTable("select MaSP, GiaBan from wBangGia where MaKH is null and KhuVuc is null");
                if (dtBangGia.Rows.Count == 0)
                {
                    XtraMessageBox.Show("Chưa cài đặt giá bán lẻ!",
                        Config.GetValue("PackageName").ToString());
                    return;
                }
                foreach (DataRowView drv in dvDetail)
                {
                    string maSP = drv["MaSP"].ToString();
                    //kiem tra gia ban theo khach hang truoc
                    DataRow[] drs = dtBangGia.Select(string.Format("MaSP = '{0}'", maSP));
                    if (drs.Length > 0)
                        drv["DonGia"] = drs[0]["GiaBan"];
                    else
                    {
                        XtraMessageBox.Show("Chưa cài đặt giá bán lẻ cho sản phẩm " + maSP,
                            Config.GetValue("PackageName").ToString());
                        _info.Result = false;
                    }
                }

                // thêm form nhập lý do khi chưa duyệt phiếu bán hàng
                foreach (DataRowView drv in dvDetail)
                {
                    bool isKm = Boolean.Parse(drv["isKM"].ToString());
                    if (isKm)
                    {
                        decimal slDat = Convert.ToDecimal(drv["SLDH"].ToString());
                        decimal sl = Convert.ToDecimal(drv["SoLuong"].ToString());
                        if (sl > slDat)
                        {
                            LyDoFrm frm = new LyDoFrm();
                            frm.LyDo = drMaster["LyDo"].ToString();
                            frm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                            frm.ShowDialog();
                            if (string.IsNullOrEmpty(frm.LyDo))
                            {
                                XtraMessageBox.Show("Chưa nhập lý do cho phiếu bán hàng chưa duyệt",
                                    Config.GetValue("PackageName").ToString());
                                _info.Result = false;
                            }
                            else
                            {
                                drMaster["LyDo"] = frm.LyDo;
                            }
                            return;
                        }
                    }
                }

            }
        }

        public InfoCustomData Info
        {
            get { return _info; }
        }

        #endregion
    }
}
