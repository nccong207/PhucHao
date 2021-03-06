using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using CDTLib;
using System.Windows.Forms;

namespace KTNPhoi
{
    public class KTNPhoi:ICData 
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
           
        }
        public void ExecuteBefore()
        {
            //KTSLNhap();
        }

        public InfoCustomData Info
        {
            get { return _info; }
        }

        #endregion

        //ràng buộc khi lưu: tổng SL nhập <= SLHT và >= tổng SL xuất
        private void KTSLNhap()
        { 
            if(_data.CurMasterIndex < 0)
                return ;
            
            DataTable dt = _data.DsData.Tables[1].GetChanges();
            
            if (dt == null || dt.Rows.Count == 0)
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
                decimal slHT = 0;
                decimal slXuat = 0;
                decimal slMoi = 0;
                decimal slCu = 0;
                string solsx = "";
                DataTable dtSoLuong = null;
                DataRow drSL = null;
                switch(dr.RowState)
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
                        slNhap = Convert.ToDecimal(drSL["SLNhap"]) + slMoi;
                        slHT = Convert.ToDecimal(drSL["SLHT"]);
                        slXuat = Convert.ToDecimal(drSL["SLXuat"]);
                        //if (slNhap > slHT)
                        //{
                        //    XtraMessageBox.Show(string.Format("Mặt hàng '{0}' có số lượng nhập lớn hơn số lượng đã sản xuất, không thêm được!", dr["TenHang"])
                        //                        , Config.GetValue("PackageName").ToString());
                        //    _info.Result = false;
                        //    return;
                        //}
                        if (slNhap < slXuat)
                        {
                            XtraMessageBox.Show(string.Format("Mặt hàng '{0}' có số lượng nhập nhỏ hơn số lượng xuất của phiếu {1}không thêm được!"
                                                , dr["TenHang"]
                                                , strPhieuXuat(dr["SoLSX"].ToString()))
                                                , Config.GetValue("PackageName").ToString());
                            _info.Result = false;
                            return;
                        }
                        break;
                    case DataRowState.Modified:
                        //số lượng mới nhập
                        slMoi = Convert.ToDecimal(dr["SoLuong"] == DBNull.Value ? "0" : dr["SoLuong"]);
                        slCu = Convert.ToDecimal(dr["SoLuong",DataRowVersion.Original] == DBNull.Value ? "0" : dr["SoLuong",DataRowVersion.Original]);
                        solsx = dr["SoLSX"].ToString();
                        dtSoLuong = _data.DbData.GetDataTable(string.Format(sql, solsx));
                        if (dtSoLuong.Rows.Count == 0)
                            continue;
                        //Lấy tổng các số lượng theo số lsx
                        drSL = dtSoLuong.Rows[0];
                        slNhap = Convert.ToDecimal(drSL["SLNhap"]) + slMoi -slCu ;
                        slHT = Convert.ToDecimal(drSL["SLHT"]);
                        slXuat = Convert.ToDecimal(drSL["SLXuat"]);
                        //if (slNhap > slHT)
                        //{
                        //    XtraMessageBox.Show(string.Format("Mặt hàng '{0}' có số lượng nhập lớn hơn số lượng đã sản xuất, không sửa được!", dr["TenHang"])
                        //                        , Config.GetValue("PackageName").ToString());
                        //    _info.Result = false;
                        //    return;
                        //}
                        if (slNhap < slXuat)
                        {
                            XtraMessageBox.Show(string.Format("Mặt hàng '{0}' có số lượng nhập nhỏ hơn số lượng xuất của phiếu {1}không sửa được!"
                                                , dr["TenHang"]
                                                , strPhieuXuat(dr["SoLSX"].ToString()))
                                                , Config.GetValue("PackageName").ToString());
                            _info.Result = false;
                            return;
                        }
                        break;
                    case DataRowState.Deleted:
                        //số lượng cũ
                        slCu = Convert.ToDecimal(dr["SoLuong", DataRowVersion.Original] == DBNull.Value ? "0" : dr["SoLuong", DataRowVersion.Original]);
                        solsx = dr["SoLSX",DataRowVersion.Original].ToString();
                        dtSoLuong = _data.DbData.GetDataTable(string.Format(sql, solsx));
                        if (dtSoLuong.Rows.Count == 0)
                            continue;
                        //Lấy tổng các số lượng theo số lsx
                        drSL = dtSoLuong.Rows[0];
                        slNhap = Convert.ToDecimal(drSL["SLNhap"]) - slCu;
                        slHT = Convert.ToDecimal(drSL["SLHT"]);
                        slXuat = Convert.ToDecimal(drSL["SLXuat"]);
                        if (slNhap < slXuat)
                        {
                            XtraMessageBox.Show(string.Format("Mặt hàng '{0}' có trong phiếu xuất {1}không xóa được!"
                                                , dr["TenHang",DataRowVersion.Original]
                                                ,strPhieuXuat(dr["SoLSX",DataRowVersion.Original].ToString()))
                                                , Config.GetValue("PackageName").ToString());
                            _info.Result = false;
                            return;
                        }
                        break;
                }
            }
        }

        //Lấy phiếu xuất
        private string strPhieuXuat(string solsx)
        {
            DataTable dtXuat = _data.DbData.GetDataTable(string.Format(@" select	m.soct
                                                                            from	mtxphoi m inner join dtxphoi d on m.mtid = d.mtid 
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
