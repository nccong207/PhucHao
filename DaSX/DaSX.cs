using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Data;
using System.Text.RegularExpressions;
using CDTDatabase;
using System.Windows.Forms;
using CDTLib;

namespace DaSX
{
    public class DaSX : ICData
    {
        DataTable dtKyHieu;
        DataCustomData _data;
        InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);
        Database db = Database.NewDataDatabase();
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
            DataView dv = new DataView(_data.DsData.Tables[1]);
            dv.RowStateFilter = DataViewRowState.Added | DataViewRowState.Deleted | DataViewRowState.ModifiedCurrent;
            foreach (DataRowView drv in dv)
            {
                string slsx = "update DTLSX set TinhTrang = N'{0}' where DTLSXID = '{1}'";
                string sdh = "update dh set TinhTrang = N'{0}' from DTDonHang dh inner join DTLSX lsx on dh.DTDHID = lsx.DTDHID where lsx.DTLSXID = '{1}'";
                string lsxid = drv["DTLSXID"].ToString();
                bool ht = Boolean.Parse(drv["HT"].ToString());
                switch (drv.Row.RowState)
                {
                    case DataRowState.Added:
                    case DataRowState.Modified:
                        string t = ht ? "Hoàn thành" : "KHSX";
                        db.UpdateByNonQueryNoTrans(string.Format(slsx, t, lsxid));
                        db.UpdateByNonQueryNoTrans(string.Format(sdh, t, lsxid));
                        break;
                    case DataRowState.Deleted: 
                        slsx = "update DTLSX set TinhTrang = null where DTLSXID = '{0}'";
                        db.UpdateByNonQueryNoTrans(string.Format(slsx, lsxid));
                        db.UpdateByNonQueryNoTrans(string.Format(sdh, "LSX", lsxid));
                        break;
                }

                //kiểm tra có thuộc trường hợp đơn hàng chọn phôi sóng không
                if ((drv.Row.RowState == DataRowState.Modified || drv.Row.RowState == DataRowState.Deleted) &&
                    drv.Row["SoLuong", DataRowVersion.Original].ToString() != "" && Convert.ToDouble(drv.Row["SoLuong", DataRowVersion.Original]) > 0)
                {
                    string dtlsxid = drv.Row["DTLSXID", DataRowVersion.Original].ToString();
                    var dtdh = _data.DbData.GetDataTable(string.Format("SELECT dh.DTDHPSID FROM DTLSX lsx INNER JOIN DTDonHang dh on lsx.DTDHID = dh.DTDHID WHERE lsx.DTLSXID = '{0}' and DTDHPSID is not null", dtlsxid));
                    if (dtdh.Rows.Count > 0)
                    {
                        MessageBox.Show("Đã nhập phôi sóng cho lệnh sản xuất này, không thể thay đổi số liệu!",
                            Config.GetValue("PackageName").ToString());
                        _info.Result = false;
                        return;
                    }
                }

                //tinh lai ma ky hieu
                if (drv.Row.RowState == DataRowState.Deleted)
                    continue;

                drv.Row["KyHieu"] =
                    (string.IsNullOrEmpty(drv["Mat1"].ToString()) ? "" : LayKyHieu(drv["Mat1"]) + ".") +
                    (string.IsNullOrEmpty(drv["Song1"].ToString()) ? "" : LayKyHieu(drv["Song1"]) + ".") +
                    (string.IsNullOrEmpty(drv["Mat2"].ToString()) ? "" : LayKyHieu(drv["Mat2"]) + ".") +
                    (string.IsNullOrEmpty(drv["Song2"].ToString()) ? "" : LayKyHieu(drv["Song2"]) + ".") +
                    (string.IsNullOrEmpty(drv["Mat3"].ToString()) ? "" : LayKyHieu(drv["Mat3"]) + ".") +
                    (string.IsNullOrEmpty(drv["Song3"].ToString()) ? "" : LayKyHieu(drv["Song3"]) + ".") +
                    LayKyHieu(drv["Mat"]);

                //tinh lai dinh luong
                string sM = drv["Mat"].ToString();
                sM = sM.Substring(sM.LastIndexOf('.') + 1);
                if (Regex.IsMatch(sM, @"^\d+$") && sM != drv["MDL"].ToString())
                    drv["MDL"] = sM;
                string sM1 = drv["Mat1"].ToString();
                if (sM1 != string.Empty)
                {
                    sM1 = sM1.Substring(sM1.LastIndexOf('.') + 1);
                    if (Regex.IsMatch(sM1, @"^\d+$") && sM1 != drv["MDL1"].ToString())
                        drv["MDL1"] = sM1;
                }
                string sS1 = drv["Song1"].ToString();
                if (sS1 != string.Empty)
                {
                    sS1 = sS1.Substring(sS1.LastIndexOf('.') + 1);
                    if (Regex.IsMatch(sS1, @"^\d+$") && sS1 != drv["SDL1"].ToString())
                        drv["SDL1"] = sS1;
                }
                string sM2 = drv["Mat2"].ToString();
                if (sM2 != string.Empty)
                {
                    sM2 = sM2.Substring(sM2.LastIndexOf('.') + 1);
                    if (Regex.IsMatch(sM2, @"^\d+$") && sM2 != drv["MDL2"].ToString())
                        drv["MDL2"] = sM2;
                }
                string sS2 = drv["Song2"].ToString();
                if (sS2 != string.Empty)
                {
                    sS2 = sS2.Substring(sS2.LastIndexOf('.') + 1);
                    if (Regex.IsMatch(sS2, @"^\d+$") && sS2 != drv["SDL2"].ToString())
                        drv["SDL2"] = sS2;
                }
                string sM3 = drv["Mat3"].ToString();
                if (sM3 != string.Empty)
                {
                    sM3 = sM3.Substring(sM3.LastIndexOf('.') + 1);
                    if (Regex.IsMatch(sM3, @"^\d+$") && sM3 != drv["MDL3"].ToString())
                        drv["MDL3"] = sM3;
                }
                string sS3 = drv["Song3"].ToString();
                if (sS3 != string.Empty)
                {
                    sS3 = sS3.Substring(sS3.LastIndexOf('.') + 1);
                    if (Regex.IsMatch(sS3, @"^\d+$") && sS3 != drv["SDL3"].ToString())
                        drv["SDL3"] = sS3;
                }
            }

            TaoPhieuXuat();
        }

        private void TaoPhieuXuat()
        {
            var drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted)
                return;
            _data.DbData.EndMultiTrans();

            string pk = _data.DrTableMaster["Pk"].ToString();
            string pkValue = drCur[pk].ToString();
            DataTable dt = _data.DsData.Tables[1];
            DataRow[] drs = dt.Select(pk + " = '" + pkValue + "'");
            Database db = Database.NewDataDatabase();

            foreach (DataRow row in drs)
            {
                if (row["SoLuong"].ToString() == "" || Convert.ToDouble(row["SoLuong"]) == 0)
                    continue;
                var soluong = double.Parse(row["SoLuong"].ToString());
                var dao = double.Parse(row["Dao"].ToString());
                string dtlsxid = row["DTLSXID"].ToString();
                var dtdh = db.GetDataTable(string.Format("SELECT dh.DTDHPSID FROM DTLSX lsx INNER JOIN DTDonHang dh on lsx.DTDHID = dh.DTDHID WHERE lsx.DTLSXID = '{0}' and DTDHPSID is not null", dtlsxid));
                if (dtdh.Rows.Count > 0)
                {
                    var dtdhid = dtdh.Rows[0]["DTDHPSID"].ToString();
                    string mact = "PNP";
                    string mtid = drCur["MTKHID"].ToString();
                    string soct = drCur["SoKH"].ToString();
                    string ngayct = drCur["NgayCT"].ToString();
                    string diengiai = "Nhập phôi sóng từ kế hoạch sản xuất";
                    string makh = row["MaKH"].ToString();
                    string nhomDk = "PNP1";

                    string query = "SELECT * FROM wDTTONTP WHERE dtdhid = '{0}'";
                    DataTable data = db.GetDataTable(string.Format(query, dtdhid));
                    if (data.Rows.Count > 0)
                    {
                        var currentRow = data.Rows[0];
                        string mahh = currentRow["mahh"].ToString();
                        string dongia = currentRow["dongia"].ToString();
                        var soluongkh = dao == 0 ? soluong : soluong / dao;
                        string loai = currentRow["loai"].ToString();
                        string tentat = currentRow["tenkh"].ToString();
                        string tenhang = currentRow["tenhang"].ToString();
                        string mtiddt = row["DTKHID"].ToString();
                        string sql = @"INSERT INTO wBLPS (MaCT,MTID,SoCT,NgayCT,DienGiai,MaKH,TenTat,NhomDk,Loai,MaHH,SoLuong,MTIDDT,DTDHID,TenHH)
                                                      VALUES ('{0}','{1}' ,'{2}' ,'{3}', N'{4}' ,'{5}' ,'{6}' ,'{7}','{8}' ,'{9}' ,'{10}' ,'{11}','{12}' ,'{13}')";
                        db.UpdateByNonQuery(string.Format(sql, mact, mtid, soct, ngayct, diengiai, makh, tentat, nhomDk, loai, mahh, soluongkh, mtiddt, dtdhid, tenhang));
                    }
                }
            }
        }

        private string LayKyHieu(object maNL)
        {
            if (dtKyHieu == null)
            {
                dtKyHieu = _data.DbData.GetDataTable("select Ma, KyHieu from DMNL");
            }
            string ma;
            if (maNL.ToString().Contains(":"))
                ma = maNL.ToString().Split(':')[1];
            else
                ma = maNL.ToString();
            DataRow[] drs = dtKyHieu.Select(string.Format("Ma like '%{0}%'", ma));
            if (drs.Length == 0 || drs[0]["KyHieu"] == null || drs[0]["KyHieu"] == DBNull.Value)
                return "";
            return drs[0]["KyHieu"].ToString();
        }

        public InfoCustomData Info
        {
            get { return _info; }
        }

        #endregion
    }
}
