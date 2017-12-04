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
                string dt41id = row["DT41ID"].ToString();
                if (string.IsNullOrEmpty(dt41id))
                    continue;
                string manl = row["MaNL"].ToString();
                string soDh = row["SoDH"].ToString();
                var curNL = GetExisted(dt41id, nl);
                if (curNL == null)
                {
                    var ngL = new NguyenLieu();
                    ngL.DT41ID = dt41id;
                    ngL.MaNL = manl;
                    ngL.SoDH = soDh;
                    ngL.SoCuon = 1;
                    nl.Add(ngL);
                }
                else
                {
                    curNL.SoCuon += 1;
                }
            }

            foreach (var nlitem in nl)
            {
                //số cuộn đang có trong db không phải của phiếu mua hàng hiện tại.
                string sql = @"SELECT COUNT(*) as Cuon FROM DT42 
                WHERE DT41ID = '{0}' and MT42ID <> '{1}'";
                object sl = db.GetValue(string.Format(sql, nlitem.DT41ID, pkValue));
                if (sl == null)
                    continue;
                decimal socuonOld = Convert.ToDecimal(sl);

                //Số cuộn có trong đơn hàng
                sql = @"SELECT SoLuong FROM DT41 dt WHERE dt.DT41ID = '{0}'";
                object slDh = db.GetValue(string.Format(sql, nlitem.DT41ID));
                if (slDh == null)
                    continue;
                decimal socuonDh = Convert.ToDecimal(slDh);

                if (nlitem.SoCuon + socuonOld > socuonDh)
                {
                    XtraMessageBox.Show(string.Format("Nguyên liệu {0} có số cuộn vượt quá số cuộn trong đơn hàng {1}",
                        nlitem.MaNL, nlitem.SoDH), Config.GetValue("PackageName").ToString());
                    _info.Result = false;
                    return;
                }
            }
        }

        private NguyenLieu GetExisted(string dt41id, List<NguyenLieu> nl)
        {
            foreach (var item in nl)
            {
                if (item.DT41ID.Equals(dt41id))
                {
                    return item;
                }
            }
            return null;
        }
    }

    public class NguyenLieu
    {
        public string DT41ID { get; set; }
        public string SoDH { get; set; }
        public string MaNL { get; set; }
        public decimal SoCuon { get; set; }

    }
}
