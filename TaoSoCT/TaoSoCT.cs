using System;
using System.Collections.Generic;
using CDTDatabase;
using CDTLib;
using Plugins;
using System.Data;
using DevExpress.XtraEditors;
using System.Globalization;

namespace TaoSoCT
{
    public class TaoSoCT:ICData
    {
        List<string> lstTable = new List<string>(new string[] { "MT11", "MT12", "MT22", "MT23", "MT32", "MT33", "MT34", "MT41", "MT42", "MT43", "MT44", "MT45", "MT46", "MTBaoGia", "MTDonHang", "MTKH", "MTKH2", "MT47", "MTNPhoi", "MTXPhoi", "mMTDonHang", "mMTBanHang", "mMTKH", "mMTNK", "mMTXK", "mMTBangGia", "mMTKhuyenMai", "MTCKho", "MTMuaHang", "MTDeNghi", "MTDNTT", "MTGiaHan","MT_CP" });
        List<string> lstPrKey = new List<string>(new string[] { "MT11ID", "MT12ID", "MT22ID", "MT23ID", "MT32ID", "MT33ID", "MT34ID", "MT41ID", "MT42ID", "MT43ID", "MT44ID", "MT45ID", "MT46ID", "MTBGID", "MTDHID", "MTKHID", "MTKHID", "MT47ID", "MTID", "MTID", "MTDHID", "MTBHID", "MTID", "MTID", "MTID", "MTID", "MTID", "MTCKID", "MTMHID", "MTDNID", "MTDNTTID", "MTGHID" , "MTCPID" });
        List<string> lstSoCT = new List<string>(new string[] { "SoCT", "SoCT", "SoCT", "SoCT", "SoCT", "SoCT", "SoCT", "SoCT", "SoCT", "SoCT", "SoCT", "SoCT", "SoCT", "SoBG", "SoDH", "SoKH", "SoKH", "SoCT", "SoCT", "SoCT", "SoDH", "SoCT", "SoCT", "SoCT", "SoCT", "SoBG", "So", "SoCT", "SoPhieu", "SoPhieu", "SoPhieu","SoCT","SoCT" });
        List<string> lstColumnNgayCT = new List<string>(new string[] { "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayLap", "NgayBD", "NgayCT", "NgayCT", "NgayCT", "NgayCT", "NgayCT","NgayCT" });
        private InfoCustomData _info;
        private DataCustomData _data;
        Database db = Database.NewDataDatabase();
        Database dbCDT = Database.NewStructDatabase();
        string newSoCT = null;

        #region ICData Members
  
        public TaoSoCT()
        {
            _info = new InfoCustomData(IDataType.MasterDetailDt);
        }

        public DataCustomData Data
        {
            set { _data = value; }
        }

        public void ExecuteAfter()
        {
            if (_data.CurMasterIndex < 0)
                return;
            string tb = _data.DrTableMaster["TableName"].ToString();
            if (!lstTable.Contains(tb))
            {
                return;
            }

            DataRow drMaster = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drMaster.RowState == DataRowState.Deleted) return;

            checkDuplicateSoCT();
        }

        private void checkDuplicateSoCT()
        {
            _data.DbData.EndMultiTrans();
            DataRow drMaster = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            string tb = _data.DrTableMaster["TableName"].ToString();
            string columnct = lstSoCT[lstTable.IndexOf(tb)];

            newSoCT = drMaster[columnct].ToString();
            if (!string.IsNullOrEmpty(newSoCT))
            {
                string sql = string.Format("SELECT * FROM {0} WHERE {1} = '{2}' ", tb, columnct, newSoCT);

                DataTable m1 = db.GetDataTable(sql);
                // Duplicate CT Number
                if (m1.Rows.Count > 1)
                {
                    string prKey = lstPrKey[lstTable.IndexOf(tb)];
                    string mtid = drMaster[prKey].ToString();

                    // Increate SoCT
                    string suffix = newSoCT.Substring(newSoCT.Length - 5, 5);
                    CreateCT();

                    sql = string.Format("UPDATE {0} SET {1} = '{2}' WHERE {3} = '{4}'", tb, columnct, newSoCT, prKey, mtid);
                    if (db.UpdateByNonQuery(sql))
                        drMaster[columnct] = newSoCT;
                }
            }
        }
        private bool KTraBGDH()
        {
            DataRow drCur = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drCur.RowState == DataRowState.Deleted)
                return true;
            string pk = _data.DrTableMaster["Pk"].ToString();
            DataRow[] drs = _data.DsData.Tables[1].Select(string.Format("{0} = '{1}'", pk, drCur[pk]));
            if (drs.Length == 0)
            {
                XtraMessageBox.Show("Chưa có mặt hàng nào, không thể lưu phiếu này!",
                    Config.GetValue("PackageName").ToString());
                _info.Result = false;
                return false;
            }
            return true;
        }

        private bool KTSuaNgay(DataRow drMaster)
        {
            string tb = _data.DrTableMaster["TableName"].ToString();
            string columnct = lstColumnNgayCT[lstTable.IndexOf(tb)];

            DateTime dt1 = DateTime.Parse(drMaster[columnct, DataRowVersion.Current].ToString());
            DateTime dt2 = DateTime.Parse(drMaster[columnct, DataRowVersion.Original].ToString());
            return (dt1.Month != dt2.Month || dt1.Year != dt2.Year);
        }

        void CreateCT()
        {
            if (_data.CurMasterIndex < 0)
                return;
            string mact = _data.DrTable["MaCT"].ToString();
            if (mact == "")
                return;
            DataRow drMaster = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];

            string tb = _data.DrTableMaster["TableName"].ToString();
            string soct = lstSoCT[lstTable.IndexOf(tb)];
            string columnct = lstColumnNgayCT[lstTable.IndexOf(tb)];
            if (!drMaster.Table.Columns.Contains(columnct))
                return;
           
            if (drMaster.RowState == DataRowState.Added
                || (drMaster.RowState == DataRowState.Modified && KTSuaNgay(drMaster)))
            {
                string sql = "", soctNew = "", Thang = "", Nam = "";
                DateTime NgayCT = (DateTime)drMaster[columnct];
                // Tháng: 2 chữ số
                // Năm: 2 số cuối của năm
                Thang = NgayCT.Month.ToString();
                Nam = NgayCT.Year.ToString();

                if (Thang.Length == 1)
                    Thang = "0" + Thang;
                Nam = Nam.Substring(2, 2);

                string suffix = "-" + Thang + Nam;
                
                sql = string.Format(@" SELECT   Top 1 {2}  
                                       FROM     {0}
                                       WHERE    {2} LIKE '{1}%{3}'
                                       ORDER BY {2} DESC", tb, mact, soct, suffix);
                DataTable dt = db.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    string soctOld = dt.Rows[0][soct].ToString();
                    soctNew = GetNewValue(soctOld.Substring(0, soctOld.Length - suffix.Length));
                    //MessageBox.Show(soctNew);
                }
                else
                    soctNew = mact + "-" + "0001";
                if (soctNew != "")
                    drMaster[soct] = soctNew + suffix;

                newSoCT = soctNew + suffix;
            }
        }

        private string GetNewValue(string OldValue)
        {
            try
            {
                int i = OldValue.Length - 1;
                for (; i > 0; i--)
                    if (!Char.IsNumber(OldValue, i))
                        break;
                if (i == OldValue.Length - 1)
                {
                    int NewValue = Int32.Parse(OldValue) + 1;
                    return NewValue.ToString();
                }
                string PreValue = OldValue.Substring(0, i + 1);
                string SufValue = OldValue.Substring(i + 1);
                int intNewSuff = Int32.Parse(SufValue) + 1;
                string NewSuff = intNewSuff.ToString().PadLeft(SufValue.Length, '0');
                return (PreValue + NewSuff);
            }
            catch
            {
                return string.Empty;
            }
        }

        public void ExecuteBefore()
        {
            //--<><>Thông báo ngày lập phiếu của phiếu xuất sản xuất MT43<><>--//
            if (_data.CurMasterIndex < 0)
                return;
            string tb = _data.DrTableMaster["TableName"].ToString();
            if (!lstTable.Contains(tb))
            {
                return;
            }

            KiemTraKhoaSo();
            if (_info.Result == false) return;

            string columnct = lstColumnNgayCT[lstTable.IndexOf(tb)];

            if (tb == "MTBaoGia" || tb == "MTDonHang")
                if (!KTraBGDH())
                    return;
            DataRow drMaster = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
            if (drMaster.RowState == DataRowState.Deleted) return;
            if (tb == "MT43" && (DateTime.Now.Date >= DateTime.Parse(drMaster[columnct].ToString()).Date))
            {
                DateTime dtTam = DateTime.Parse(drMaster[columnct].ToString());
                XtraMessageBox.Show("Ngày lập phiếu: " + dtTam.Day.ToString() + "/" + dtTam.Month.ToString() + "/" + dtTam.Year.ToString());
            }
            else if (tb == "MT43" && (DateTime.Now.Date < DateTime.Parse(drMaster[columnct].ToString()).Date))
            {
                DateTime dtTam = DateTime.Parse(drMaster[columnct].ToString());
                XtraMessageBox.Show("Ngày lập phiếu: " + dtTam.Day.ToString() + "/" + dtTam.Month.ToString() + "/" + dtTam.Year.ToString() + " phải nhỏ hơn ngày hiện tại!");
                _info.Result = false;
            }

            if (!lstTable.Contains(tb))
                return;
            CreateCT();
        }

        private void KiemTraKhoaSo()
        {
            string tb = _data.DrTableMaster["TableName"].ToString();
            if (!lstTable.Contains(tb))
            {
                return;
            }
            if (Config.GetValue("NgayKhoaSo") == null)
                return;
            string tmp = Config.GetValue("NgayKhoaSo").ToString();
            DateTime ngayKhoa;
            DateTimeFormatInfo dtInfo = new DateTimeFormatInfo();
            dtInfo.ShortDatePattern = "dd/MM/yyyy";

            if (DateTime.TryParse(tmp, dtInfo, DateTimeStyles.None, out ngayKhoa))
            {
                string columnNgayCt = lstColumnNgayCT[lstTable.IndexOf(tb)];
                DataView dv = new DataView(_data.DsData.Tables[0]);
                dv.RowStateFilter = DataViewRowState.Added | DataViewRowState.ModifiedOriginal | DataViewRowState.Deleted;
                if (dv.Count == 0)
                {
                    if (_data.CurMasterIndex < 0)
                        return;
                    DataRow drMaster = _data.DsData.Tables[0].Rows[_data.CurMasterIndex];
                    string pk = _data.DrTableMaster["Pk"].ToString();
                    DataView dvdt = new DataView(_data.DsData.Tables[1]);
                    dvdt.RowFilter = pk + " = '" + drMaster[pk].ToString() + "'";
                    dvdt.RowStateFilter = DataViewRowState.Added | DataViewRowState.ModifiedCurrent | DataViewRowState.Deleted;
                    if (dvdt.Count == 0)
                        return;
                    else
                    {
                        dv.RowStateFilter = DataViewRowState.CurrentRows;
                        dv.RowFilter = pk + " = '" + drMaster[pk].ToString() + "'";
                    }
                }
                DateTime ngayCT = DateTime.Parse(dv[0][columnNgayCt].ToString());
                if (ngayCT <= ngayKhoa)
                {
                    string msg = "Kỳ kế toán đã khóa! Không thể chỉnh sửa số liệu!";
                    XtraMessageBox.Show(msg);
                    _info.Result = false;
                }
                else
                    _info.Result = true;
            }
        }

        public InfoCustomData Info
        {
            get { return _info; }
        }

        #endregion
    }
}
