using CDTDatabase;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TaoMaCuon
{
    public class TaoMaCuon : ICData
    {
        private InfoCustomData _info;
        private DataCustomData _data;
        Database db = Database.NewDataDatabase();
        string[] months = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" };

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
            
        }

        public void ExecuteBefore()
        {
            DataRow drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted || drCur.RowState == DataRowState.Modified)
                return;

            DateTime ngayCT = (DateTime) drCur["NgayCT"];
            string mt42id = drCur["MT42ID"].ToString();
            DataRow[] drs = _data.DsData.Tables[1].Select("MT42ID = '" + mt42id + "'");
            string code = ngayCT.ToString("yy") + months[ngayCT.Month] + "%";
            int startNumber = GetStartCode(code);
            foreach (DataRow row in drs)
            {
                startNumber++;
                row["MaCuon"] = startNumber;
            }
        }

        private int GetStartCode(string code)
        {
            string query = string.Format("Select Max(MaCuon) as Max from DT42 where MaCuon like '{0}'", code);
            DataTable dt = db.GetDataTable(query);
            if (dt.Rows[0]["Max"] != DBNull.Value)
            {
                string value = dt.Rows[0]["Max"].ToString().Substring(3);
                return Convert.ToInt32(value);
            }
            return 1;
        }
    }
}
