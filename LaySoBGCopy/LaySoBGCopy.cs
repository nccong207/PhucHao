using CDTDatabase;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using FormFactory;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LaySoBGCopy
{
    public class LaySoBGCopy: ICControl
    {
        private DataCustomFormControl _data;
        private InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        string oldBG = "";
        DataTable dmnl = null;
        Database db = Database.NewDataDatabase();
        public void AddEvent()
        {

            DataSet dsData = _data.BsMain.DataSource as DataSet;
            if (dsData == null)
                return;

            _data.BsMain.DataSourceChanged += BsMain_DataSourceChanged;

            dsData.Tables[0].TableNewRow += new DataTableNewRowEventHandler(BaoGia_TableNewRow);
            dsData.Tables[0].RowChanged += BaoGia_RowChanged;
            dsData.Tables[1].RowChanged += LaySoBGCopy_RowChanged;

        }

        private void BsMain_DataSourceChanged(object sender, EventArgs e)
        {
            DataSet dsData = _data.BsMain.DataSource as DataSet;
            _data.BsMain.DataSourceChanged += BsMain_DataSourceChanged;
            dsData.Tables[0].TableNewRow += new DataTableNewRowEventHandler(BaoGia_TableNewRow);
            dsData.Tables[0].RowChanged += BaoGia_RowChanged;
            dsData.Tables[1].RowChanged += LaySoBGCopy_RowChanged;
        }

        private void LaySoBGCopy_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action != DataRowAction.Add)
            {
                return;
            }

            if (dmnl == null)
                dmnl = db.GetDataTable(string.Format("SELECT Ma, DL, GiaBan from DMNL"));

            CDTForm frm = (_data.FrmMain as CDTForm);
            if (frm.FrmDesigner.formAction == FormAction.Copy)
            {
                var mat = e.Row["Mat_Giay"].ToString();
                var sb = e.Row["SB_Giay"].ToString();
                var mb = e.Row["MB_Giay"].ToString();
                var sc = e.Row["SC_Giay"].ToString();
                var mc = e.Row["MC_Giay"].ToString();
                var se = e.Row["SE_Giay"].ToString();
                var me = e.Row["ME_Giay"].ToString();

                if (!string.IsNullOrEmpty(mat))
                {
                    DataRow[] drs = dmnl.Select(string.Format("Ma = '{0}'", mat));
                    if (drs.Length > 0)
                    {
                        e.Row["Mat_DL"] = drs[0]["DL"];
                        e.Row["Mat_DG"] = drs[0]["GiaBan"];
                    }
                }

                if (!string.IsNullOrEmpty(sb))
                {
                    DataRow[] drs = dmnl.Select(string.Format("Ma = '{0}'", sb));
                    if (drs.Length > 0)
                    {
                        e.Row["SB_DL"] = drs[0]["DL"];
                        e.Row["SB_DG"] = drs[0]["GiaBan"];
                    }
                }

                if (!string.IsNullOrEmpty(mb))
                {
                    DataRow[] drs = dmnl.Select(string.Format("Ma = '{0}'", mb));
                    if (drs.Length > 0)
                    {
                        e.Row["MB_DL"] = drs[0]["DL"];
                        e.Row["MB_DG"] = drs[0]["GiaBan"];
                    }
                }

                if (!string.IsNullOrEmpty(sc))
                {
                    DataRow[] drs = dmnl.Select(string.Format("Ma = '{0}'", sc));
                    if (drs.Length > 0)
                    {
                        e.Row["SC_DL"] = drs[0]["DL"];
                        e.Row["SC_DG"] = drs[0]["GiaBan"];
                    }
                }

                if (!string.IsNullOrEmpty(mc))
                {
                    DataRow[] drs = dmnl.Select(string.Format("Ma = '{0}'", mc));
                    if (drs.Length > 0)
                    {
                        e.Row["MC_DL"] = drs[0]["DL"];
                        e.Row["MC_DG"] = drs[0]["GiaBan"];
                    }
                }

                if (!string.IsNullOrEmpty(se))
                {
                    DataRow[] drs = dmnl.Select(string.Format("Ma = '{0}'", se));
                    if (drs.Length > 0)
                    {
                        e.Row["SE_DL"] = drs[0]["DL"];
                        e.Row["SE_DG"] = drs[0]["GiaBan"];
                    }
                }

                if (!string.IsNullOrEmpty(me))
                {
                    DataRow[] drs = dmnl.Select(string.Format("Ma = '{0}'", me));
                    if (drs.Length > 0)
                    {
                        e.Row["ME_DL"] = drs[0]["DL"];
                        e.Row["ME_DG"] = drs[0]["GiaBan"];
                    }
                }
            }
        }

        void BaoGia_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Add)
            {
                // Set số báo giá cũ cho form mới
                if (string.IsNullOrEmpty(e.Row["SoBGCopy"].ToString()) && !string.IsNullOrEmpty(oldBG))
                {
                    e.Row["SoBGCopy"] = oldBG;
                    oldBG = "";
                }
            }
        }
        private void BaoGia_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            CDTForm frm = (_data.FrmMain as CDTForm);
            if (frm.FrmDesigner.formAction == FormAction.Copy)
            {
                //Luu so Bao gia cu.
                DataRowView drMaster = _data.BsMain.Current as DataRowView;
                oldBG = drMaster.Row["SoBG"].ToString();
            }
            else
            {
                oldBG = "";
            }
        }

        public DataCustomFormControl Data
        {
            set { _data = value; }
        }

        public InfoCustomControl Info
        {
            get { return _info; }
        }
    }
}