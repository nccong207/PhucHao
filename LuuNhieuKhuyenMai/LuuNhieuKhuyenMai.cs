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
                string mtid = drCur["MTID"].ToString();

                //remove before add new
                string remove = @"DELETE km FROM mDTKMNhieuSP km
                        JOIN mDTKhuyenMaiSP sp ON km.DTSPID = sp.DTSPID
                        JOIN mMTKhuyenMai mt ON sp.MTID = mt.MTID
                        WHERE mt.MTID = '{0}'";

                db.UpdateByNonQuery(string.Format(remove, mtid));

                DataTable tb = listTable["mDTKMNhieuSP"];
                string insertSql = "INSERT INTO mDTKMNhieuSP(DTSPID, MaSPTang, SLTang) VALUES({0},'{1}', {2}); ";
                foreach (DataRow row in tb.Rows)
                {
                    string spid = row["DTSPID"].ToString();
                    string masp = row["MaSPTang"].ToString();
                    string sluong = row["SLTang"].ToString();
                    sql += string.Format(insertSql, spid, masp, sluong);
                }
                db.UpdateByNonQuery(sql);
            }
        }

        public void ExecuteBefore()
        {
            var listTable = _data.DsData.Tables;
        }
    }
}
