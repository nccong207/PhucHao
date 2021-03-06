using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Plugins;
using CDTDatabase;
using DevExpress.XtraEditors;
using CDTLib;
using DevExpress.XtraGrid.Views.Grid;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using FormFactory;

namespace XuLyDHMi
{
    public class XuLyDHMi : ICData
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
                string maKH = drMaster["MaKH"].ToString();
                DataTable dtBangGia = db.GetDataTable(string.Format(@"select gb.MaKH, gb.MaSP, GiaBan from wBangGia gb left join mDMKH kh on gb.KhuVuc = kh.KhuVuc
                    where gb.Duyet = 1 and (gb.MaKH = '{0}' or kh.MaKH = '{0}')", maKH));
                if (dtBangGia.Rows.Count == 0)
                {
                    XtraMessageBox.Show("Chưa cài đặt giá bán cho khách hàng/khu vực này",
                        Config.GetValue("PackageName").ToString());
                    return;
                }
                DataRow[] drsKhuyenMai;
                if (drMaster.RowState == DataRowState.Modified
                    && (!drMaster["MaKH", DataRowVersion.Current].Equals(drMaster["MaKH", DataRowVersion.Original])
                    || !drMaster["NgayCT", DataRowVersion.Current].Equals(drMaster["NgayCT", DataRowVersion.Original])
                    || !drMaster["TongTien", DataRowVersion.Current].Equals(drMaster["TongTien", DataRowVersion.Original])))
                {
                    dvDetail.RowFilter = string.Format("MTDHID = '{0}' and isKM = 0", drMaster["MTDHID"]);
                    dvDetail.RowStateFilter = DataViewRowState.CurrentRows;
                    drsKhuyenMai = _data.DsData.Tables[1].Select(string.Format("MTDHID = '{0}' and isKM = 1", drMaster["MTDHID"]));
                }
                else
                {
                    dvDetail.RowFilter = "isKM = 0";
                    dvDetail.RowStateFilter = DataViewRowState.Added | DataViewRowState.ModifiedCurrent;
                    drsKhuyenMai = _data.DsData.Tables[1].Select("MTDHID is null and isKM = 1");
                }
                if (dvDetail.Count == 0)
                    return;
                //xoa het chuong trinh khuyen mai tang san pham de chay lai
                foreach (DataRow dr in drsKhuyenMai)
                    dr.Delete();

                //chay lai bang gia va chuong trinh khuyen mai
                DataSet dsKhuyenMai = db.GetDataSet(string.Format("exec LayKhuyenMai @ngayct = '{0}', @makh = '{1}', @tongtien = {2}", drMaster["NgayCT"], maKH, drMaster["TongTien"]));
                List<DataRow> spKMs = new List<DataRow>();
                foreach (DataRowView drv in dvDetail)
                {
                    string maSP = drv["MaSP"].ToString();
                    //kiem tra gia ban theo khach hang truoc
                    DataRow[] drsGiaBan = dtBangGia.Select(string.Format("MaSP = '{0}' and MaKH = '{1}'", maSP, maKH));
                    if (drsGiaBan.Length > 0)
                        drv["DGGoc"] = drsGiaBan[0]["GiaBan"];
                    else
                    {
                        //tiep tuc kiem tra gia ban theo khu vuc neu khong co gia ban theo khach hang
                        drsGiaBan = dtBangGia.Select(string.Format("MaSP = '{0}'", maSP));
                        if (drsGiaBan.Length > 0)
                            drv["DGGoc"] = drsGiaBan[0]["GiaBan"];
                        else
                        {
                            XtraMessageBox.Show("Chưa cài đặt giá bán của sản phẩm " + maSP + " cho khách hàng/khu vực này",
                                Config.GetValue("PackageName").ToString());
                            _info.Result = false;
                        }
                    }
                    //kiem tra giam gia theo san pham ban
                    DataRow[] drsGiamGia = dsKhuyenMai.Tables[0].Select(string.Format("MaSPBan = '{0}' and {1} >= SLTu and ({1} <= SLDen or SLDen is null)", drv["MaSP"], drv["SoLuong"]));
                    if (drsGiamGia.Length > 0)
                    {
                        drv["GiamSP"] = drsGiamGia[0]["TyLeGiam"];
                        drv["KMSP"] = drsGiamGia[0]["MTID"];
                    }
                    else
                    {
                        drv["GiamSP"] = 0;
                        drv["KMSP"] = DBNull.Value;
                    }
                    //kiem tra tang san pham theo san pham ban
                    DataRow[] drsTangSP = dsKhuyenMai.Tables[1].Select(string.Format("MaSPBan = '{0}' and {1} >= SLTu and ({1} <= SLDen or SLDen is null) and (SLDen <> SLTu) and NhieuSP is null", drv["MaSP"], drv["SoLuong"]));
                    if (drsTangSP.Length > 0)
                    {
                        spKMs.Add(drsTangSP[0]);
                    }

                    //kiem tra tang san pham tang nhan theo so luong
                    DataRow[] drsTangSPNhanSl = dsKhuyenMai.Tables[1].Select(string.Format("MaSPBan = '{0}' and (SLDen = SLTu) and NhieuSP is null", drv["MaSP"]));

                    if (drsTangSPNhanSl.Length > 0)
                    {
                        decimal soLuongKm = Convert.ToDecimal(drsTangSPNhanSl[0]["SLDen"].ToString());
                        decimal soLuongHt = Convert.ToDecimal(drv["SoLuong"].ToString());
                        decimal soLuongDcTang = Convert.ToDecimal(drsTangSPNhanSl[0]["SLTang"].ToString());
                        decimal soLuongTang = Convert.ToInt32(Math.Floor(soLuongHt / soLuongKm)) * soLuongDcTang;
                        drsTangSPNhanSl[0]["SLTang"] = soLuongTang;
                        spKMs.Add(drsTangSPNhanSl[0]);

                        drv["KMSP"] = drsTangSPNhanSl[0]["MTID"];
                    }

                    //kiem tra tang nhieu san pham
                    DataRow[] drsTangNhieuSP = dsKhuyenMai.Tables[1].Select(string.Format("MaSPBan = '{0}' and NhieuSP is not null", drv["MaSP"]));

                    if (drsTangNhieuSP.Length > 0)
                    {
                        int heSoNhanSP = 1;
                        string nhieuSP = drsTangNhieuSP[0]["NhieuSP"].ToString();
                        bool isSelectAll = nhieuSP.Equals("Chọn tất cả");
                        int spid = Convert.ToInt32(drsTangNhieuSP[0]["DTSPID"].ToString());
                        string sql = string.Format("select km.*, sp.TenSP, cast(0 as bit) as Chon from mDTKMNhieuSP km inner join mDMSP sp on km.MaSPTang = sp.MaSP WHERE DTSPID = {0}", spid);
                        DataTable nhieuSpDt = db.GetDataTable(sql);
                        // kiem tra truong hop nhan theo so luong
                        decimal soLuongDen = Convert.ToDecimal(drsTangNhieuSP[0]["SLDen"].ToString());
                        decimal soLuongTu = Convert.ToDecimal(drsTangNhieuSP[0]["SLTu"].ToString());
                        if (soLuongTu == soLuongDen)
                        {
                            decimal soLuongBanra = Convert.ToDecimal(drv["SoLuong"].ToString());
                            heSoNhanSP = Convert.ToInt32(Math.Floor(soLuongBanra / soLuongTu));
                        }

                        DataRow[] selectedRow;
                        if (isSelectAll)
                            selectedRow = nhieuSpDt.Select();
                        else
                        {
                            SPListForm frm = new SPListForm(nhieuSpDt);

                            frm.StartPosition = FormStartPosition.CenterScreen;
                            frm.ShowDialog();
                            selectedRow = frm.dsSp.Select("Chon = 1");
                        }

                        foreach (DataRow row in selectedRow)
                        {
                            DataRow r = dsKhuyenMai.Tables[1].NewRow();
                            r["MTID"] = drsTangNhieuSP[0]["MTID"].ToString();
                            r["MaSPTang"] = row["MaSPTang"].ToString();
                            r["SLTang"] = Convert.ToDecimal(row["SLTang"].ToString()) * heSoNhanSP;
                            spKMs.Add(r);
                        }
                    }
                }
                //kiem tra giam gia theo don hang
                if (dsKhuyenMai.Tables.Count >= 3 && dsKhuyenMai.Tables[2].Rows.Count > 0)
                {
                    drMaster["TyLeGiam"] = dsKhuyenMai.Tables[2].Rows[0]["TyLeGiam"];
                    drMaster["CTKM"] = dsKhuyenMai.Tables[2].Rows[0]["MTID"];
                }
                else
                {
                    drMaster["TyLeGiam"] = 0;
                    drMaster["CTKM"] = DBNull.Value;
                }

                //Kiem tra truong hop tang nhieu san pham
                CapNhatTangSP(dsKhuyenMai, spKMs, drMaster);
            }
        }

        private void CapNhatTangSP(DataSet dsKhuyenMai, List<DataRow> drsSP, DataRow drMaster)
        {
            Form frmDonHang = null;
            foreach (Form frm in Application.OpenForms)
                if (frm.GetType().BaseType == typeof(CDTForm)
                    && (frm as CDTForm).FrmType == FormType.MasterDetail
                    && frm.IsMdiChild == false)
                {
                    frmDonHang = frm;
                    break;
                }
            if (frmDonHang == null || frmDonHang.Controls.Find("gcMain", true).Length == 0)
            {
                XtraMessageBox.Show("Không cập nhật được khuyến mãi theo sản phẩm do frmDonHang = null!",
                    Config.GetValue("PackageName").ToString());
                return;
            }
            GridView gvMain = (frmDonHang.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;

            //kiem tra tang san pham theo don hang
            if (dsKhuyenMai.Tables.Count >= 4 && dsKhuyenMai.Tables[3].Rows.Count > 0)
                foreach (DataRow dr in dsKhuyenMai.Tables[3].Rows)
                {
                    gvMain.AddNewRow();
                    if (drMaster["MTDHID"].ToString() != string.Empty)
                        gvMain.SetFocusedRowCellValue(gvMain.Columns["MTDHID"], drMaster["MTDHID"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["isKM"], true);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["MaSP"], dr["MaSPTang"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["SoLuong"], dr["SLTang"]);
                    //sau khi cap nhat so luong -> cong thuc nhay -> thay doi focus row handle -> phai reset lai
                    gvMain.FocusedRowHandle = gvMain.DataRowCount - 1;
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["DGGoc"], 0);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["KMSP"], dr["MTID"]);
                    gvMain.UpdateCurrentRow();
                }
            //cap nhat tang san pham theo san pham ban
            foreach (DataRow dr in drsSP)
            {
                gvMain.AddNewRow();
                if (drMaster["MTDHID"].ToString() != string.Empty)
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["MTDHID"], drMaster["MTDHID"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["isKM"], true);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["MaSP"], dr["MaSPTang"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["SoLuong"], dr["SLTang"]);
                //sau khi cap nhat so luong -> cong thuc nhay -> thay doi focus row handle -> phai reset lai
                gvMain.FocusedRowHandle = gvMain.DataRowCount - 1;
                gvMain.SetFocusedRowCellValue(gvMain.Columns["DGGoc"], 0);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["KMSP"], dr["MTID"]);
                gvMain.UpdateCurrentRow();
            }
        }

        public InfoCustomData Info
        {
            get { return _info; }
        }

        #endregion
    }
}
