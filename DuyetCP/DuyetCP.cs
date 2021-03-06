using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Data;
using CDTDatabase;

namespace DuyetCP
{
    public class DuyetCP : ICData
    {
        private DataCustomData _data;
        private InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);
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
            DataRow drMaster = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drMaster.RowState == DataRowState.Added
                || (drMaster.RowState == DataRowState.Deleted && !Convert.ToBoolean(drMaster["Duyet", DataRowVersion.Original]))
                || (drMaster.RowState == DataRowState.Modified && !Convert.ToBoolean(drMaster["Duyet"]) && !Convert.ToBoolean(drMaster["Duyet", DataRowVersion.Original])))
                return;
            int duyet = 1;
            if ((drMaster.RowState == DataRowState.Deleted && Convert.ToBoolean(drMaster["Duyet", DataRowVersion.Original]))
                || (drMaster.RowState == DataRowState.Modified && Convert.ToBoolean(drMaster["Duyet", DataRowVersion.Original]) && !Convert.ToBoolean(drMaster["Duyet"])))
                duyet = 0;
            string sql = @"update DTTSCD set DaTinhCP = {0} from MTTSCD mt 
                            where month(Thang) = {1} and year(Thang) = {2} and DTTSCD.MTID = mt.MTID and mt.ThanhLy = 0";
            object thang = drMaster.RowState == DataRowState.Deleted ? drMaster["Thang", DataRowVersion.Original] : drMaster["Thang"];
            object nam = drMaster.RowState == DataRowState.Deleted ? drMaster["Nam", DataRowVersion.Original] : drMaster["Nam"];
            Database db = Database.NewDataDatabase();
            _info.Result = db.UpdateByNonQuery(string.Format(sql, duyet, thang, nam));
            //bổ sung cập nhật TG đã KH...
            if (_info.Result)
            {
                sql = @"update MTTSCD set TGDaKH = TGDaKH + (select COUNT(*) from DTTSCD dt where MTTSCD.MTID = dt.MTID and dt.DaTinhCP = 1);
                        update MTTSCD set TGCL = TGKH - TGDaKH, GTCL = ROUND(GTKH * (TGKH - TGDaKH), 0)";
                _info.Result = db.UpdateByNonQuery(sql);
            }
        }

        public InfoCustomData Info
        {
            get { return _info; }
        }

        #endregion
    }
}
