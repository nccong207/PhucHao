using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Data;
using DevExpress.XtraEditors;
using CDTLib;
using System.Windows.Forms;

namespace KTXPhoi
{
    public class KTXPhoi:ICData
    {
        DataCustomData _data;
        InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);
        #region ICData Members

        public DataCustomData Data
        {
            set { _data = value; }
        }

        public void ExecuteAfter()
        {
            //CapNhatDaSX();
            //CapNhatDaSX_LSX();
        }
        public void ExecuteBefore()
        {
            //KTSLXuat();

            //Không được xóa khi đã nhập slht trong khsx
//            if (_data.CurMasterIndex < 0)
//                return;
//            string sql  = @"select	isnull(sum(kh.slsx),0) [flag]
//                            from	dtkh kh inner join dtlsx l on kh.dtlsxid = l.dtlsxid
//				                            inner join mtlsx m on m.mtlsxid = l.mtlsxid
//                            where	l.dtdhid = '{0}' and m.solsx = '{1}'";
//            DataTable dt = _data.DsData.Tables[1].GetChanges(DataRowState.Deleted);
//            if (dt == null)
//                return;
//            foreach (DataRow dr in dt.Rows)
//            {
//                object obj = _data.DbData.GetValue(string.Format(sql, dr["DTDHID", DataRowVersion.Original], dr["SoLSX", DataRowVersion.Original]));
//                if (Convert.ToInt32(obj) > 0)
//                {
//                    XtraMessageBox.Show(string.Format("Mặt hàng '{0}' đã nhập SLHT, không được xóa!", dr["TenHang", DataRowVersion.Original])
//                             , Config.GetValue("PackageName").ToString());
//                    _info.Result = false;
//                    break;
//                }
//            }
        }

        public InfoCustomData Info
        {
            get { return _info; }
        }

        #endregion

        //Update cho đơn hàng
        private void CapNhatDaSX()
        {
            string sqldh = "";
            DataTable dt = _data.DsData.Tables[1];

            //DataRow[] dtcopy = _data.DsDataCopy.Tables[1].GetChanges().Select("Loai = 'Tấm'");
            //DataRow[] dtChange = _data.DsData.Tables[1].GetChanges().Select("Loai = 'Tấm'");
            DataTable dtChange = _data.DsData.Tables[1].GetChanges();
            if (dtChange == null)
                return;
            foreach (DataRow dr in dtChange.Rows)
            {
                switch (dr.RowState)
                {
                    case DataRowState.Added:
                    case DataRowState.Modified:
                        if (dr["Loai"].ToString().Equals("Thùng"))
                            continue;
                        object oSoLuong = _data.DbData.GetValue("select sum(slphoi) from dtlsx where dtdhid = '" + dr["DTDHID"] + "'");
                        object oSLXuat = _data.DbData.GetValue("select sum(soluong) from dtxphoi where dtdhid = '" + dr["DTDHID"] + "'");
                        //object oSLDat = dt.Compute("SLDat", "DTDHID = '" + dr["DTDHID"] + "'");
                        if (oSoLuong == DBNull.Value || oSLXuat == DBNull.Value)
                            continue;
                        if (Convert.ToDecimal(oSLXuat) >= Convert.ToDecimal(oSoLuong))
                            sqldh += string.Format(";update dtdonhang set dasx = {0} where dtdhid = '{1}'", 1, dr["DTDHID"]);
                        else
                            sqldh += string.Format(";update dtdonhang set dasx = {0} where dtdhid = '{1}'", 0, dr["DTDHID"]);
                        break;
                    case DataRowState.Deleted:
                        if (dr["Loai", DataRowVersion.Original].ToString().Equals("Thùng"))
                            continue;
                        object oSoLuong1 = _data.DbData.GetValue("select sum(slphoi) from dtlsx where dtdhid = '" + dr["DTDHID", DataRowVersion.Original] + "'");
                        object oSLXuat1 = _data.DbData.GetValue("select sum(soluong) from dtxphoi where dtdhid = '" + dr["DTDHID", DataRowVersion.Original] + "'");
                        if (Convert.ToDecimal(oSLXuat1 == DBNull.Value ? 0 : oSLXuat1) >= Convert.ToDecimal(oSoLuong1 == DBNull.Value ? 0 : oSoLuong1) && oSLXuat1 != DBNull.Value)
                            sqldh += string.Format(";update dtdonhang set dasx = {0} where dtdhid = '{1}'", 1, dr["DTDHID", DataRowVersion.Original]);
                        else
                            sqldh += string.Format(";update dtdonhang set dasx = {0} where dtdhid = '{1}'", 0, dr["DTDHID", DataRowVersion.Original]);
                        break;
                }
            }

            if (sqldh != "")
                _data.DbData.UpdateByNonQuery(sqldh);
        }

        //Cập nhập lệnh sản xuất
        private void CapNhatDaSX_LSX()
        {
            string sqldh = "";
            DataTable dt = _data.DsData.Tables[1];

            //DataRow[] dtcopy = _data.DsDataCopy.Tables[1].GetChanges().Select("Loai = 'Tấm'");
            //DataRow[] dtChange = _data.DsData.Tables[1].GetChanges().Select("Loai = 'Tấm'");
            DataTable dtChange = _data.DsData.Tables[1].GetChanges();
            if (dtChange == null)
                return;
            foreach (DataRow dr in dtChange.Rows)
            {
                switch (dr.RowState)
                {
                    case DataRowState.Added:
                    case DataRowState.Modified:
                        if (dr["Loai"].ToString().Equals("Thùng"))
                            continue;
                        object oSLPhoi = _data.DbData.GetValue(@"select sum(d.slphoi) from dtlsx d inner join mtlsx m on d.mtlsxid = m.mtlsxid
                                                                 where d.dtdhid = '" + dr["DTDHID"] + "' and m.solsx ='" + dr["solsx"] + "'");
                        object oSoLuong = _data.DbData.GetValue(@"select sum(soluong) from dtxphoi
                                                                  where dtdhid = '" + dr["DTDHID"] + "' and solsx ='" + dr["solsx"] + "'");
                        if (oSoLuong == DBNull.Value || oSLPhoi == DBNull.Value)
                            continue;
                        if (Convert.ToDecimal(oSoLuong) >= Convert.ToDecimal(oSLPhoi))
                            sqldh += string.Format(@";update dtlsx set dasx = {0} 
                                                      from dtlsx d inner join mtlsx m on d.mtlsxid = m.mtlsxid 
                                                      where m.solsx = '{1}' and d.dtdhid = '{2}'", 1, dr["solsx"],dr["dtdhid"]);
                        else
                            sqldh += string.Format(@";update dtlsx set dasx = {0} 
                                                      from dtlsx d inner join mtlsx m on d.mtlsxid = m.mtlsxid
                                                      where m.solsx = '{1}' and d.dtdhid = '{2}'", 0, dr["solsx"],dr["DTDHID"]);
                        break;
                    case DataRowState.Deleted:
                        if (dr["Loai", DataRowVersion.Original].ToString().Equals("Thùng"))
                            continue;
                        object oSLPhoi1 = _data.DbData.GetValue(@"select sum(d.slphoi) from dtlsx d inner join mtlsx m on d.mtlsxid = m.mtlsxid
                                                                 where d.dtdhid = '" + dr["DTDHID",DataRowVersion.Original] + "' and m.solsx ='" + dr["solsx",DataRowVersion.Original] + "'");
                        object oSoLuong1 = _data.DbData.GetValue(@"select sum(soluong) from dtxphoi
                                                                  where dtdhid = '" + dr["DTDHID",DataRowVersion.Original] + "' and solsx ='" + dr["solsx",DataRowVersion.Original] + "'");
                        if (Convert.ToDecimal(oSoLuong1 == DBNull.Value ? 0 : oSoLuong1) >= Convert.ToDecimal(oSLPhoi1 == DBNull.Value ? 0 : oSLPhoi1))
                            sqldh += string.Format(@";update dtlsx set dasx = {0} 
                                                      from dtlsx d inner join mtlsx m on d.mtlsxid = m.mtlsxid 
                                                      where m.solsx = '{1}' and d.dtdhid = '{2}'", 1, dr["solsx",DataRowVersion.Original], dr["dtdhid",DataRowVersion.Original]);
                        else
                            sqldh += string.Format(@";update dtlsx set dasx = {0} 
                                                      from dtlsx d inner join mtlsx m on d.mtlsxid = m.mtlsxid
                                                      where m.solsx = '{1}' and d.dtdhid = '{2}'", 0, dr["solsx",DataRowVersion.Original], dr["DTDHID",DataRowVersion.Original]);
                        break;
                }
            }
            if (sqldh != "")
                _data.DbData.UpdateByNonQuery(sqldh);
        }

        // ràng buộc khi lưu: tổng SL xuất của lệnh <= tổng SL nhập
        private void KTSLXuat()
        {
            if (_data.CurMasterIndex < 0)
                return;
            DataRow drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            DataTable dt = _data.DsData.Tables[1].GetChanges();
            if (dt.Rows.Count == 0 || dt == null)
                return;
            string sql = @"select	0 [SLNhap]
                                    ,isnull(sum(k.SLSX),0) [SLHT]
                                    ,0 [SLXuat]
                            into	#tmpSL22
                            from	dtkh k
                            where	k.solsx = '{0}'
                            union all
                            select	isnull(sum(n.soluong),0) ,0,0
                            from	dtnphoi n
                            where	n.solsx = '{0}'
                            union all
                            select	0,0,isnull(sum(x.SoLuong),0)
                            from	dtxphoi x
                            where	x.solsx = '{0}'

                            select	sum(SLNhap) [SLNhap],SUM(SLHT) [SLHT],sum(slxuat) [SLXuat]
                            from	#tmpSL22

                            drop table #tmpSL22
                           ";

            foreach (DataRow dr in dt.Rows)
            {
                decimal slNhap = 0;
                decimal slXuat = 0;
                decimal slMoi = 0;
                decimal slCu = 0;
                string solsx = "";
                DataTable dtSoLuong = null;
                DataRow drSL = null;
                switch (dr.RowState)
                {
                    case DataRowState.Added:
                        //số lượng mới nhập
                        slMoi = Convert.ToDecimal(dr["SoLuong"] == DBNull.Value ? "0" : dr["SoLuong"]);
                        solsx = dr["SoLSX"].ToString();
                        dtSoLuong = _data.DbData.GetDataTable(string.Format(sql, solsx));
                        if (dtSoLuong.Rows.Count == 0)
                            continue;
                        //Lấy tổng các số lượng theo số lsx
                        drSL = dtSoLuong.Rows[0];
                        slNhap = Convert.ToDecimal(drSL["SLNhap"]);
                        slXuat = Convert.ToDecimal(drSL["SLXuat"]) + slMoi;
                        if (slXuat > slNhap)
                        {
                            XtraMessageBox.Show(string.Format("Mặt hàng '{0}' có số lượng xuất lớn hơn số lượng nhập của phiếu {1}không thêm được!"
                                                , dr["TenHang"],strPhieuNhap(dr["SoLSX"].ToString()))
                                                , Config.GetValue("PackageName").ToString());
                            _info.Result = false;
                            return;
                        }
                        //Kiểm tra check isGP 
                        if ((bool)dr["isGP"] && dr["GhiChuID"] == DBNull.Value)
                        {
                            XtraMessageBox.Show(string.Format("Mặt hàng {0} chưa chọn ghi chú phôi lỗi!",dr["TenHang"]));
                            _info.Result = false;
                            return;
                        }
                        break;
                    case DataRowState.Modified:
                        //số lượng mới nhập
                        slMoi = Convert.ToDecimal(dr["SoLuong"] == DBNull.Value ? "0" : dr["SoLuong"]);
                        slCu = Convert.ToDecimal(dr["SoLuong", DataRowVersion.Original] == DBNull.Value ? "0" : dr["SoLuong", DataRowVersion.Original]);
                        solsx = dr["SoLSX"].ToString();
                        dtSoLuong = _data.DbData.GetDataTable(string.Format(sql, solsx));
                        if (dtSoLuong.Rows.Count == 0)
                            continue;
                        //Lấy tổng các số lượng theo số lsx
                        drSL = dtSoLuong.Rows[0];
                        slNhap = Convert.ToDecimal(drSL["SLNhap"]);
                        slXuat = Convert.ToDecimal(drSL["SLXuat"]) + slMoi - slCu;
                        if (slXuat > slNhap)
                        {
                            XtraMessageBox.Show(string.Format("Mặt hàng '{0}' có số lượng xuất lớn hơn số lượng nhập của phiếu {1}không sửa được!"
                                                , dr["TenHang"], strPhieuNhap(dr["SoLSX"].ToString()))
                                                , Config.GetValue("PackageName").ToString());
                            _info.Result = false;
                            return;
                        }
                        //Kiểm tra check isGP 
                        if ((bool)dr["isGP"] && dr["GhiChuID"] == DBNull.Value)
                        {
                            XtraMessageBox.Show(string.Format("Mặt hàng {0} chưa chọn ghi chú phôi lỗi!", dr["TenHang"]), Config.GetValue("PackageName").ToString());
                            _info.Result = false;
                            return;
                        }
                        break;
                    case DataRowState.Deleted:
                        break;
                }
                //KTSLTon();
            }
        }

        //Kiểm tra số lượng tồn khi isGP = true
        private void KTSLTon()
        {
            DataRow drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted)
                return;
            DataView dv = new DataView(_data.DsData.Tables[1]);
            dv.RowStateFilter = DataViewRowState.ModifiedCurrent;

            if (drCur.RowState == DataRowState.Modified)
                if (!drCur["NgayCT", DataRowVersion.Original].ToString().Equals(drCur["NgayCT", DataRowVersion.Current].ToString()))
                {
                    dv.RowStateFilter = DataViewRowState.Unchanged;
                    dv.RowFilter = "MT32ID ='" + drCur["MT32ID"] + "'";
                }

            foreach (DataRowView drv in dv)
            {
                //Kiểm tra khi sửa isGP = true thiết lập lại số lượng đang tồn
                if (!drv.Row["isGP", DataRowVersion.Current].ToString().Equals(drv.Row["isGP", DataRowVersion.Original].ToString()))
                {
                    string sql1 = @"select sum(soluong - soluong_x - isnull(slxgp,0)) from wblps
                                            where DTDHID = '{0}' and NgayCT <= '{1}' and MTIDDT <> '{2}'";
                    string dtid = drv.Row["DTID"].ToString();
                    string dtdhid = drv.Row["DTDHID"].ToString();
                    string ngayct = drCur["NgayCT"].ToString();
                    string tenHH = drv.Row["TenHang"].ToString();
                    object obj = _data.DbData.GetValue(string.Format(sql1, dtdhid, ngayct, dtid));
                    decimal slt = obj == DBNull.Value ? 0 : decimal.Parse(obj.ToString());
                    if ((bool)drv.Row["isGP"] && !Convert.ToDecimal(drv.Row["SLDangTon"]).Equals(slt))
                    {
                        DialogResult result = XtraMessageBox.Show("Mặt hàng " + tenHH + " có số lượng tồn thay đổi, bạn có muốn thiết lập lại số lượng tồn không?"
                        , Config.GetValue("PackageName").ToString(), MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            drv.Row["SLDangTon"] = slt;
                        }
                    }
                }
            }
        }

        //Lấy phiếu nhập
        private string strPhieuNhap(string solsx)
        {
            DataTable dtXuat = _data.DbData.GetDataTable(string.Format(@" select	m.soct
                                                                            from	mtnphoi m inner join dtnphoi d on m.mtid = d.mtid 
                                                                            where	solsx= '{0}'", solsx));
            string strMe = "";
            foreach (DataRow i in dtXuat.Rows)
            {
                strMe += i["soct"].ToString() + ", ";
            }
            return strMe;
        }

    }
}
