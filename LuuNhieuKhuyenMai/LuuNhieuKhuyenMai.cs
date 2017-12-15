using CDTDatabase;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LuuNhieuKhuyenMai
{
    public class LuuNhieuKhuyenMai : ICData
    {
        DataCustomData _data;
        InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);
        Database db = Database.NewDataDatabase();
        public DataCustomData Data
        {
            set { _data = value; }
        }

        public InfoCustomData Info
        {
            get { return _info; }
        }

        public void ExecuteAfter()
        {
            if (_data.CurMasterIndex < 0)
                return;
           
            var drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted)
                return;

            _data.DbData.EndMultiTrans();
            var listTable = _data.DsData.Tables;
            string sql = "";
            if (listTable.Contains("mDTKMNhieuSP"))
            {
                DataTable tb = listTable["mDTKMNhieuSP"];

                string insertSql = "INSERT INTO mDTKMNhieuSP(DTSPID, MaSPTang, SLTang) VALUES({0},'{1}', {2}); ";
                string updateSql = "UPDATE mDTKMNhieuSP SET MaSPTang = '{0}', SLTang = '{1}' WHERE DTKMNSPID = '{2}'; ";
                string removeSql = "DELETE FROM mDTKMNhieuSP WHERE DTKMNSPID = '{0}'; ";

                foreach (DataRow row in tb.Rows)
                {
                    if (row.RowState == DataRowState.Added)
                    {
                        string spid = row["DTSPID"].ToString();
                        string masp = row["MaSPTang"].ToString();
                        string sluong = row["SLTang"].ToString();
                        sql += string.Format(insertSql, spid, masp, sluong);
                    }
                    else if (row.RowState == DataRowState.Modified)
                    {
                        string rowid = row["DTKMNSPID", DataRowVersion.Original].ToString();
                        string masp = row["MaSPTang"].ToString();
                        string sluong = row["SLTang"].ToString();
                        sql += string.Format(updateSql, masp, sluong, rowid);
                    }
                    else if (row.RowState == DataRowState.Deleted)
                    {
                        string rowid = row["DTKMNSPID", DataRowVersion.Original].ToString();
                        sql += string.Format(removeSql, rowid);
                    }
                }

                if (!string.IsNullOrEmpty(sql))
                db.UpdateByNonQueryNoTrans(sql);

                 _data.DsData.Tables.Remove(tb);
            }
        }

        public void ExecuteBefore()
        {
        }
    }
}
