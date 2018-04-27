using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Data;
using DevExpress.XtraEditors;
using CDTLib;
using System.Windows.Forms;
using CDTDatabase;

namespace KTraXuatAm
{
    public class KTraXuatAm : ICData
    {
        DataCustomData _data;
        InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);
        Database db = CDTDatabase.Database.NewDataDatabase();
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
            DataRow drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted)
                return;
            DataView dv = new DataView(_data.DsData.Tables[1]);
            dv.RowFilter = "MT32ID ='" + drCur["MT32ID"] + "'";

            // kiem tra ngay giao hang
            string isAdmin = Config.GetValue("Admin").ToString();
            if (!Boolean.Parse(isAdmin))
            {
                DataRow[] drs = _data.DsData.Tables[1].Select("MT32ID = '" + drCur["MT32ID"].ToString() + "'");
                foreach (DataRow row in drs)
                {
                    string dtdhid = row["DTDHID"].ToString();
                    if (!string.IsNullOrEmpty(dtdhid))
                    {
                            //ngay gia han
                            string sql3 = "SELECT top 1 NgayGHT FROM DTGiaHan dt left join MTGiaHan mt on mt.MTGHID  = dt.MTGHID where mt.Duyet = 1 and dt.DTDHID = '{0}' order by NgayGHT desc";
                            object ngaygiahan = db.GetValue(string.Format(sql3, dtdhid));
                            // ngay xuat gan nhat
                            string sql2 = "SELECT TOP 1 NgayCT FROM BLVT WHERE (MaCT = 'BH') AND DTDHID = '{0}' order by NgayCT desc";
                            object ngayxuatgannhat = db.GetValue(string.Format(sql2, dtdhid));
                           
                            //ngay nhap gan nhat
                            string sql = "SELECT TOP 1 NgayCT FROM BLVT WHERE (MaCT = 'NTD' OR MaCT = 'NTP') AND DTDHID = '{0}' order by NgayCT desc";
                            object ngaynhapkho = db.GetValue(string.Format(sql, dtdhid));
                            //lay ngay cho phep theo loai hang
                            int days = row["Loai"].ToString().Equals("Thùng") ? 10 : 4;
                            // lay ngay hien tai
                            var ngayht = DateTime.Today; 
                            //tinh ngay ht va ngay gia han
                            var ng1 = 0;
                            var ng2 = 0;
                            var ng3 = 0;
                             
                            if (ngaygiahan != null)
                            {
                                 ng1 = ((DateTime)ngayht - (DateTime)ngaygiahan).Days;
                            }
                            
                            //tinh ngay ht va ngay xuat gan nhat
                            if (ngayxuatgannhat != null) {
                                 ng2 = ((DateTime)ngayht - (DateTime)ngayxuatgannhat).Days -4 ;
                            }
                            
                            //tinh ngay ht va ngay nhap kho
                            if (ngaynhapkho != null) {
                                 ng3 = ((DateTime)ngayht - (DateTime)ngaynhapkho).Days - days;
                            }
                          
                            // kiem tra cac ngay
                            if (ng3>0 && ng2>0 && ng1>0)
                            {
                                XtraMessageBox.Show("Er.1: Ngày hiện tại vượt quá ngày gia hạn đã duyệt, không thể tạo được phiếu bán hàng.", Config.GetValue("PackageName").ToString());
                                _info.Result = false;
                                return;
                            }
                            else if (ng3 > 0 && ng2 > 0 && ng1 == 0)
                            {
                                XtraMessageBox.Show("Er.2: Ngày hiện tại vượt quá ngày gia hạn đã duyệt, không thể tạo được phiếu bán hàng.", Config.GetValue("PackageName").ToString());
                                _info.Result = false;
                                return;
                            }
                            else if (ng3 > 0 && ng2 == 0 && ng1 == 0)
                            {
                                XtraMessageBox.Show("Er.3: Ngày hiện tại vượt quá ngày gia hạn đã duyệt, không thể tạo được phiếu bán hàng.", Config.GetValue("PackageName").ToString());
                                _info.Result = false;
                                return;
                            }
                            else if (ng3 == 0 && ng2 == 0 && ng1 == 0)
                            {
                                _info.Result = true;
                                return;
                            }
                            else if (ng3 > 0 && ng2 < 0 && ng1 < 0)
                            {
                                _info.Result = true;
                                return;
                            }
                            else if (ng3 > 0 && ng2 < 0 && ng1 == 0)
                            {
                                _info.Result = true;
                                return;
                            }
                            else if (ng3 > 0 && ng2 == 0 && ng1 == 0)
                            {
                                _info.Result = true;
                                return;
                            }
                                 
                    }
                }
            }

            // kiem tra xuat am
            
            dv.RowStateFilter = DataViewRowState.Added | DataViewRowState.ModifiedCurrent;

            if (drCur.RowState == DataRowState.Modified)
                if (!drCur["NgayCT", DataRowVersion.Original].ToString().Equals(drCur["NgayCT", DataRowVersion.Current].ToString()))
                {
                    dv.RowStateFilter = DataViewRowState.Unchanged;
                    dv.RowFilter = "MT32ID ='" + drCur["MT32ID"] + "'";
                }

            bool isDieuChinh = false;
            foreach (DataRowView drv in dv)
            {
                string sql = @"select sum(soluong - soluong_x - isnull(slxgp,0)) from BLVT 
                        where Loi = {3} and DTDHID = '{0}' and TenHH = N'{4}' and NgayCT <= '{1}' and MTIDDT <> '{2}'";

                if (Boolean.Parse(drv["isPS"].ToString()))
                {
                    sql = @"select sum(soluong - soluong_x - isnull(slxgp,0)) from wBLPS 
                        where {3} = {3} and DTDHID = '{0}' and TenHH = N'{4}' and NgayCT <= '{1}' and MTIDDT <> '{2}'";
                }
                
                string dtid = drv["DT32ID"].ToString();
                string dtdhid = drv["DTDHID"].ToString();

                string mahh = drv["MaHH"].ToString();
                string ngayct = drCur["NgayCT"].ToString();
                string tenHH = drv["TenHang"].ToString();
                int loi = Boolean.Parse(drv["Loi"].ToString()) ? 1 : 0;

                object o = _data.DbData.GetValue(string.Format(sql, dtdhid, ngayct, dtid, loi, tenHH));
                decimal slt = o == DBNull.Value ? 0 : decimal.Parse(o.ToString());
                decimal slx = decimal.Parse(drv["SoLuong"].ToString());
                decimal slp = String.IsNullOrEmpty(drv["SLTonCuoi"].ToString()) ? 0 : decimal.Parse(drv["SLTonCuoi"].ToString());
                if (slx + slp > 1.05M * slt && drv["SoDH"] != DBNull.Value)
                {
                    XtraMessageBox.Show("Chỉ được xuất vượt tối đa 5% số lượng tồn.\n" +
                        tenHH + ": Số lượng xuất + Phế = " + (slx + slp).ToString("###,##0") + "; Số lượng tồn = " + slt.ToString("###,##0"),
                        Config.GetValue("PackageName").ToString());
                    _info.Result = false;
                    return;
                }
                //Đánh dấu isGP thì bắt buộc nhập ghi chú
                if ((bool)drv["isGP"] && drv["GhiChuID"] == DBNull.Value)
                {
                    XtraMessageBox.Show("Mặt hàng " + tenHH + " chưa nhập ghi chú giấy phế!", Config.GetValue("PackageName").ToString());
                    _info.Result = false;
                    return;
                }
                string soDH = drv.RowVersion == DataRowVersion.Original ? drv.Row["SoDH", DataRowVersion.Original].ToString()
                    : drv.Row["SoDH"].ToString();

                //Các phiếu nào có chọn các phần chiết khấu, chi phí (không có số đơn hàng) => Trạng thái là có điều chỉnh
                if (string.IsNullOrEmpty(soDH) && isDieuChinh == false)
                {
                    isDieuChinh = true;
                }
                //Kiểm tra khi sửa isGP = true thiết lập lại số lượng đang tồn
                //if (drv.Row.RowState == DataRowState.Modified)
                //    if(!drv.Row["isGP",DataRowVersion.Original].ToString().Equals(drv.Row["isGP",DataRowVersion.Current].ToString()))
                //        if((bool)drv.Row["isGP"] && !Convert.ToDecimal(drv["SLDangTon"]).Equals(slt))
                //        {
                //            DialogResult result = XtraMessageBox.Show("Mặt hàng " +tenHH+ " có số lượng tồn thay đổi, bạn có muốn thiết lập lại số lượng tồn không?"
                //                ,Config.GetValue("PackageName").ToString(), MessageBoxButtons.YesNo);
                //            if (result == DialogResult.Yes)
                //            {
                //                drv["SLDangTon"] = slt;
                //            }
                //        }


            }
            drCur["DieuChinh"] = isDieuChinh;

            _info.Result = true;
        }
        public void showerr() {
            XtraMessageBox.Show("Ngày hiện tại vượt quá ngày đã duyệt, không thể tạo được phiếu bán hàng.", Config.GetValue("PackageName").ToString());
            _info.Result = false;
            return;
        }
        public InfoCustomData Info
        {
            get { return _info; }
        }

        #endregion
    }
}
