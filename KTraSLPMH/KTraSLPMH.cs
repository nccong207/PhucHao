using CDTDatabase;
using CDTLib;
using DevExpress.XtraEditors;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KTraSLPMH
{
    public class KTraSLPMH : ICData
    {

        DataCustomData _data;
        InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);
        Database db = Database.NewDataDatabase();

        public void ExecuteAfter()
        {
        }

        public void ExecuteBefore()
        {
            //kiem tra da tao phieu de nghi thanh toan
            var drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted || drCur.RowState == DataRowState.Modified)
            { 
                string mtmhid = drCur["MTMHID", DataRowVersion.Original].ToString();

                string sqlPdntt = @"SELECT* from DTDNTT dt
                    LEFT JOIN DTMuaHang dtmh ON dt.DTMHID = dtmh.DTMHID
                    LEFT JOIN MTMuaHang mtmh ON dtmh.MTMHID = mtmh.MTMHID
                    WHERE mtmh.MTMHID = '{0}'";

                DataTable dtdntt = db.GetDataTable(string.Format(sqlPdntt, mtmhid));
                if (dtdntt.Rows.Count > 0)
                {
                    XtraMessageBox.Show("Phiếu mua hàng này đã tạo phiếu đề nghị thanh toán, không được sửa hoặc xóa.",
                         Config.GetValue("PackageName").ToString());
                    _info.Result = false;
                    return;
                }
            }

            //kiem tra so luong trong phieu mua hang khong vuot qua so luong phieu de nghi
            if (drCur.RowState == DataRowState.Deleted)
                return;

            string sophieuDn = drCur["SoPhieuDN"].ToString();
            if (string.IsNullOrEmpty(sophieuDn)) return;

            string pk = _data.DrTableMaster["Pk"].ToString();
            string pkValue = drCur[pk].ToString();
            DataTable dt = _data.DsData.Tables[1];
            DataRow[] drs = dt.Select(pk + " = '" + pkValue + "'");

            string sql = @"SELECT dt.SoLuong
                        FROM DTDeNghi dt JOIN MTDeNghi mt ON
                        dt.MTDNID = mt.MTDNID
                        WHERE mt.SoPhieu = '{0}'
                        and dt.MaVT = '{1}' and dt.MaPX = '{2}'";
            string getPmhSql = "SELECT SoPhieuMH FROM MTDeNghi WHERE SoPhieu = '{0}'";

            // getso phieu mua hang cua phieu de nghi
            bool skip = false;
            string sophieuMh = db.GetValue(string.Format(getPmhSql, sophieuDn)).ToString();
            string getVT = @"SELECT dt.MaVT, sum(dt.SoLuong) as TongSo FROM DTMuaHang dt 
                            JOIN MTMuaHang mt ON dt.MTMHID = mt.MTMHID
                            WHERE mt.SoPhieu in ({0}) and dt.MaVT = {1} and dt.MaPX = '{2}'
                            GROUP BY dt.MaVT";

            string dataMh = "";
            if (!string.IsNullOrEmpty(sophieuMh))
            {
                string[] mhList = sophieuMh.Split(',');
                dataMh =  "'" + string.Join("','", mhList) + "'";
            } else
            {
                skip = true;
            }

            foreach (var row in drs)
            {
                string mavt = row["MaVT"].ToString();
                string mapx = row["MaPX"].ToString();
                double soluongNew = 0d, total = 0d, delta = 0d;

                soluongNew = Convert.ToDouble(row["SoLuong"].ToString());

                if (drCur.RowState == DataRowState.Modified)
                {
                    double soluongOld = Convert.ToDouble(row["SoLuong", DataRowVersion.Original].ToString());
                    delta = soluongNew - soluongOld;
                } else
                {
                    delta = soluongNew;
                }

                if (!skip)
                {
                    DataTable vattuDt = db.GetDataTable(string.Format(getVT, dataMh, mavt, mapx));
                    if (vattuDt.Rows.Count > 0)
                    {
                        double soluong = Convert.ToDouble(vattuDt.Rows[0]["TongSo"].ToString());
                        total = soluong + delta;
                    }
                } else
                {
                    total = soluongNew;
                }
               
                object vl = db.GetValue(string.Format(sql, sophieuDn, mavt, mapx));
                if (vl != null)
                {
                    if (total > Convert.ToDouble(vl.ToString()))
                    {
                        object ten = db.GetValue(string.Format("SELECT TenVT FROM DMVatTu WHERE ID = '{0}'", mavt));
                        string tenvt = ten != null ? ten.ToString(): null;
                        
                        XtraMessageBox.Show($"số lượng của {tenvt} lớn hơn số lượng có trong phiếu đề nghị mua hàng.\n Kiểm tra lại số lượng của {tenvt}", Config.GetValue("PackageName").ToString());
                        _info.Result = false;
                        return;
                    }
                }

            }
        }

        public DataCustomData Data { set { _data = value; } }
        public InfoCustomData Info { get { return _info; } }
    }


}
