using CDTDatabase;
using CDTLib;
using DevExpress.XtraEditors;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KTraPNNL
{
    public class KTraPNNL : ICData
    {
        DataCustomData _data;
        InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);
        Database db = Database.NewDataDatabase();
        public DataCustomData Data { set { _data = value; } }
        public InfoCustomData Info { get { return _info; } }

        public void ExecuteAfter()
        {
        }

        public void ExecuteBefore()
        {
            //kiem tra da tao phieu de nghi thanh toan
            var drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted)
                return;

            string pkValue = drCur["MT42ID"].ToString();

            DataTable dt = _data.DsData.Tables[1];
            DataRow[] drs = dt.Select("MT42ID = '" + pkValue + "'");

            List<NguyenLieu> nl = new List<NguyenLieu>();
            foreach (var row in drs)
            {
                string manl = row["MaNL"].ToString();
                string soDh = row["SoDH"].ToString();
                var curNL = GetExisted(manl, soDh, nl);
                if (!string.IsNullOrEmpty(manl) && curNL == null)
                {
                    NguyenLieu ngL = new NguyenLieu();
                    ngL.MaNL = manl;
                    ngL.SoDH = soDh;
                    ngL.SoLuong = 1;
                    nl.Add(ngL);
                }
                else if (!string.IsNullOrEmpty(manl))
                {
                    curNL.SoLuong += 1;
                }
            }


            foreach (var nlitem in nl)
            {
                string sql = @"SELECT dt.MaNL, nl.Ten, mt.SoCT, sum(dt.SoLuong) as Cuon FROM MT41 mt
                                LEFT JOIN DT41 dt on dt.MT41ID = mt.MT41ID
                                LEFT JOIN DMNL nl ON dt.MaNL = nl.Ma
                                WHERE mt.SoCT = '{0}' and dt.MaNL = '{1}'
                                GROUP BY dt.MaNL, mt.SoCT, nl.Ten";
                DataTable dtDh = db.GetDataTable(string.Format(sql, nlitem.SoDH, nlitem.MaNL));
                if (dtDh.Rows.Count > 0)
                {
                    decimal socuon = Convert.ToDecimal(dtDh.Rows[0]["Cuon"].ToString());
                    if (nlitem.SoLuong > socuon)
                    {
                        string tennl = dtDh.Rows[0]["Ten"].ToString();
                        XtraMessageBox.Show("Mã nguyên liệu " + tennl + " có số cuộn vượt quá số cuộn trong đơn hàng giấy cuộn. Kiểm tra lại số cuộn của mã nguyên liệu " + tennl,
                         Config.GetValue("PackageName").ToString());
                        _info.Result = false;
                        return;
                    }
                }
            }
        }

        private NguyenLieu GetExisted(string maNl, string soDh, List<NguyenLieu> nl)
        {
            foreach (var item in nl)
            {
                if (item.MaNL.Equals(maNl) && item.SoDH.Equals(soDh))
                {
                    return item;
                }
            }
            return null;
        }
    }

    public class NguyenLieu {
        public string SoDH { get; set; }
        public string TenNL { get; set; }
        public string MaNL { get; set; }
        public decimal SoLuong { get; set; }

    }
}
