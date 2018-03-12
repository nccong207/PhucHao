using CDTDatabase;
using CDTLib;
using DevExpress.XtraEditors;
using Plugins;
using System;
using System.Data;

namespace KTraSLPMH
{
    public class KTraSLPMH : ICData
    {

        DataCustomData _data;
        InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);
        Database db = Database.NewDataDatabase();
        DataRow drCur;
        public void ExecuteAfter()
        {
        }

        public void ExecuteBefore()
        {
            //kiem tra da tao phieu de nghi thanh toan
            drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted || drCur.RowState == DataRowState.Modified)
            { 
                string mtmhid = drCur["MTMHID", DataRowVersion.Original].ToString();

                string sqlPdntt = @"SELECT * from DTDNTT dt
                    JOIN DTMuaHang dtmh ON dt.DTMHID = dtmh.DTMHID
                    JOIN MTMuaHang mtmh ON dtmh.MTMHID = mtmh.MTMHID
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

            KiemTraSL();
        }

        private void KiemTraSL()
        {
            string pk = _data.DrTableMaster["Pk"].ToString();
            string pkValue = drCur[pk].ToString();
            DataTable dt = _data.DsData.Tables[1];
            DataRow[] drs = dt.Select(pk + " = '" + pkValue + "'", "", DataViewRowState.Added | DataViewRowState.ModifiedCurrent);

            // getso phieu mua hang cua phieu de nghi
            string sqlDaMua = @"SELECT sum(dt.SoLuong) FROM DTMuaHang dt 
                            WHERE dt.DTDNID = '{0}'";
            string sqlDeNghi = @"SELECT dt.SoLuong FROM DTDeNghi dt 
                            WHERE dt.DTDNID = '{0}'";

            foreach (var row in drs)
            {
                double soluongNew = 0d, tSLDaMua = 0d, delta = 0d;

                soluongNew = Convert.ToDouble(row["SoLuong"].ToString());

                if (drCur.RowState == DataRowState.Modified)
                {
                    double soluongOld = Convert.ToDouble(row["SoLuong", DataRowVersion.Original].ToString());
                    delta = soluongNew - soluongOld;
                }
                else
                {
                    delta = soluongNew;
                }
                
                object sl = db.GetValue(string.Format(sqlDaMua, row["DTDNID"]));
                double soluong = (sl == null || sl.ToString() == "") ? 0 : Convert.ToDouble(sl);
                tSLDaMua = soluong + delta;

                object vl = db.GetValue(string.Format(sqlDeNghi, row["DTDNID"]));
                double tSLDeNghi = (vl == null || vl.ToString() == "") ? 0 : Convert.ToDouble(vl);

                if (tSLDaMua > tSLDeNghi)
                {
                    object ten = db.GetValue(string.Format("SELECT TenVT FROM DMVatTu WHERE ID = '{0}'", row["MaVT"]));
                    string tenvt = ten?.ToString();

                    XtraMessageBox.Show($"số lượng của {tenvt} lớn hơn số lượng có trong phiếu đề nghị mua hàng.\n Kiểm tra lại số lượng của {tenvt}", Config.GetValue("PackageName").ToString());
                    _info.Result = false;
                    return;
                }
            }
        }

        public DataCustomData Data { set { _data = value; } }
        public InfoCustomData Info { get { return _info; } }
    }


}
