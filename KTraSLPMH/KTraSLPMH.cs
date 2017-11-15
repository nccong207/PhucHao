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
            var drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted)
                return;

            string sophieuDn = drCur["SoPhieuDN"].ToString();
            string pk = _data.DrTableMaster["Pk"].ToString();
            string pkValue = drCur[pk].ToString();
            DataTable dt = _data.DsData.Tables[1];
            DataRow[] drs = dt.Select(pk + " = '" + pkValue + "'");

            string sql = @"SELECT dt.SoLuong
                        FROM DTDeNghi dt JOIN MTDeNghi mt ON
                        dt.MTDNID = mt.MTDNID
                        WHERE mt.SoPhieu = '{0}'
                        and dt.MaVT = '{1}' and dt.MaPX = '{2}'";

            foreach (var row in drs)
            {
                string mavt = row["MaVT"].ToString();
                string mapx = row["MaPX"].ToString();
                double soluong = Convert.ToDouble(row["SoLuong"].ToString());
                object vl = db.GetValue(string.Format(sql, sophieuDn, mavt, mapx));
                if (vl != null)
                {
                    if (soluong > Convert.ToDouble(vl.ToString()))
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
