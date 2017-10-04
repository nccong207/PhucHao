using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Data;
using System.Text.RegularExpressions;
using CDTDatabase;

namespace DaSX
{
    public class DaSX : ICData
    {
        DataTable dtKyHieu;
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
            DataView dv = new DataView(_data.DsData.Tables[1]);
            dv.RowStateFilter = DataViewRowState.Added | DataViewRowState.Deleted | DataViewRowState.ModifiedCurrent;
            foreach (DataRowView drv in dv)
            {
                string slsx = "update DTLSX set TinhTrang = N'{0}' where DTLSXID = '{1}'";
                string sdh = "update DTDonHang set TinhTrang = N'{0}' from DTLSX lsx where DTDonHang.DTDHID = lsx.DTDHID and lsx.DTLSXID = '{1}'";
                string lsxid = drv["DTLSXID"].ToString();
                bool ht = Boolean.Parse(drv["HT"].ToString());
                switch (drv.Row.RowState)
                {
                    case DataRowState.Added:
                    case DataRowState.Modified:
                        string t = ht ? "Hoàn thành" : "KHSX";
                        _data.DbData.UpdateByNonQuery(string.Format(slsx, t, lsxid));
                        _data.DbData.UpdateByNonQuery(string.Format(sdh, t, lsxid));
                        break;
                    case DataRowState.Deleted: 
                        slsx = "update DTLSX set TinhTrang = null where DTLSXID = '{0}'";
                        _data.DbData.UpdateByNonQuery(string.Format(slsx, lsxid));
                        _data.DbData.UpdateByNonQuery(string.Format(sdh, "LSX", lsxid));
                        break;
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

            var drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted)
                return;

            string pk = _data.DrTableMaster["Pk"].ToString();
            string pkValue = drCur[pk].ToString();
            DataTable dt = _data.DsData.Tables[1];
            DataRow[] drs = dt.Select(pk + " = '" + pkValue + "'");
            Database db = Database.NewDataDatabase();

            foreach (DataRow row in drs)
            {
                string dtlsxid = row["DTLSXID"].ToString();
                var dtlsx = db.GetDataTable(string.Format("SELECT * FROM DTLSX WHERE DTLSXID = '{0}'", dtlsxid));
                if (dtlsx.Rows.Count > 0)
                {
                    string dtdhid = dtlsx.Rows[0]["DTDHID"].ToString();
                    var dtdh = db.GetDataTable(string.Format("SELECT * FROM DTDonHang WHERE DTDHID = '{0}'", dtdhid));
                    if (dtdh.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dtdh.Rows[0]["DTDHPSID"].ToString()) && double.Parse(dtdh.Rows[0]["SoLuong"].ToString()) > 0)
                        {
                            var soluong = double.Parse(dtdh.Rows[0]["SoLuong"].ToString());
                            var dao = double.Parse(dtdh.Rows[0]["Dao"].ToString());
                            string mact = "PNP";
                            string mtid = drCur["MTKHID"].ToString();
                            string soct = drCur["SoKH"].ToString();
                            string ngayct = drCur["NgayCT"].ToString();
                            string diengiai = row["GhiChu"].ToString();
                            string makh = row["MaKH"].ToString();
                            string tentat = row["TenTat"].ToString();
                            string nhomDk = "PNP1";
                            string loai = row["Loai"].ToString();

                            var dai = row["Dai"].ToString();
                            var rong = row["Rong"].ToString();
                            var cao = row["Cao"].ToString();
                            var lop = row["Lop"].ToString();

                            string mahh = makh + "_" + dai + "*" + rong + (!string.IsNullOrEmpty(cao) ? "*" + cao : "")	+ "_" + lop + "L";

                            var soluongkh = dao == 0 ? soluong : soluong/dao; 
                            string mtiddt = row["DTKHID"].ToString();
                            string tenhang = row["TenHang"].ToString();

                            string sql = @"INSERT INTO wBLPS (MaCT,MTID,SoCT,NgayCT,DienGiai,MaKH,TenTat,NhomDk,Loai,MaHH,SoLuong,MTIDDT,DTDHID,TenHH)
                                                      VALUES ('{0}','{1}' ,'{2}' ,'{3}' ,'{4}' ,'{5}' ,'{6}' ,'{7}','{8}' ,'{9}' ,'{10}' ,'{11}','{12}' ,'{13}')";
                            //MaCT,MTID, SoCT, NgayCT, DienGiai, MaKH, TenTat, NhomDk, Loai, MaHH, SoLuong,   MTIDDT, DTDHID, TenHH
                            db.UpdateByNonQuery(string.Format(sql, mact, mtid, soct, ngayct, diengiai, makh, tentat, nhomDk, loai, mahh, soluongkh, mtiddt, dtdhid, tenhang));
                        }
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
