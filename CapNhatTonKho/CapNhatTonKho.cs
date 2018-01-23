using CDTDatabase;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CapNhatTonKho
{
    public class CapNhatTonKho : ICData
    {
        DataCustomData _data;
        InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);
        Database db = Database.NewDataDatabase();
        List<string> tables = new List<string>() { "MT47", "MT42", "MTCKho", "MT43", "MT44" };

        public DataCustomData Data { set { _data = value; } }
        public InfoCustomData Info { get { return _info; } }


        public void ExecuteAfter()
        {
            string tb = _data.DrTableMaster["TableName"].ToString();
            if (!tables.Contains(tb)) return;

            var drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            DataTable dtDTKH = _data.DsData.Tables[1];
            string pk = _data.DrTableMaster["Pk"].ToString();
            if (dtDTKH.Select(string.Format(pk + " = '{0}'", drCur[pk])).Length == 0)
                return;

            string makho = drCur["MaKho"].ToString();
            string makho2 = null;
            if (tb.Equals("MTCKho")) makho2 = drCur["MaKhoN"].ToString();

            var DetailList = dtDTKH.Select(string.Format(pk + " = '{0}'", drCur[pk]));
            string sql = @"SELECT DT42ID, MaKho, SUM(SoLuong) - SUM(Soluong_x) as Ton from BLNL 
            WHERE DT42ID = '{0}' AND MaKho = '{1}' GROUP BY DT42ID, MaKho";

            string sqlUpdate = @"begin tran
                                if exists (select * from TonKhoNL where MaCuon = '{0}' AND MaKho = '{1}')
                                begin
                                   update TonKhoNL set SoLuong = {3}
                                   where MaCuon = '{0}' AND MaKho = '{1}'
                                end
                                else
                                begin
                                   insert into TonKhoNL (MaCuon,MaKho,SoLuong)
                                   values ('{0}','{1}', {3})
                                end
                                commit tran";

            string maCuonSql = "SELECT * FROM DT42 WHERE DT42ID = '{0}'";
            foreach (DataRow row in DetailList)
            {
                if (row.RowState == DataRowState.Added || row.RowState == DataRowState.Modified || row.RowState == DataRowState.Deleted)
                {

                    string dt42id = row["DT42ID", DataRowVersion.Original].ToString();
                   
                    DataTable dtTon = db.GetDataTable(string.Format(sql, dt42id, makho));
                    if (dtTon.Rows.Count > 0)
                    {
                        string slTon = dtTon.Rows[0]["Ton"].ToString();
                        DataTable dtMaCuon = db.GetDataTable(string.Format(maCuonSql, dt42id));
                        if (dtMaCuon.Rows.Count > 0)
                        {
                            string macuon = dtMaCuon.Rows[0]["MaCuon"].ToString();
                           
                            if (tb.Equals("MTCKho"))
                            {
                                db.UpdateByNonQuery(string.Format(sqlUpdate, macuon, makho, slTon));
                                db.UpdateByNonQuery(string.Format(sqlUpdate, macuon, makho2, slTon));
                            }
                            else
                            {
                                db.UpdateByNonQuery(string.Format(sqlUpdate, macuon, makho, slTon));
                            }
                        }
                       
                    }

                }
            }
        }

        public void ExecuteBefore()
        {
        }
    }
}
