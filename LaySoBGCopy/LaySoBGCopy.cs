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
        Database db = Database.NewDataDatabase();
        public void AddEvent()
        {
            DataSet dsData = _data.BsMain.DataSource as DataSet;
            
            if (dsData == null)
                return;
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
                    var dt = db.GetDataTable(string.Format("SELECT DL, GiaBan from DMNL WHERE Ma = '{0}'", mat));
                    if (dt.Rows.Count > 0)
                    {
                        e.Row["Mat_DL"] = dt.Rows[0]["DL"];
                        e.Row["Mat_DG"] = dt.Rows[0]["GiaBan"];
                    }
                }

                if (!string.IsNullOrEmpty(sb))
                {
                    var dt = db.GetDataTable(string.Format("SELECT DL, GiaBan from DMNL WHERE Ma = '{0}'", sb));
                    if (dt.Rows.Count > 0)
                    {
                        e.Row["SB_DL"] = dt.Rows[0]["DL"];
                        e.Row["SB_DG"] = dt.Rows[0]["GiaBan"];
                    }
                }

                if (!string.IsNullOrEmpty(mb))
                {
                    var dt = db.GetDataTable(string.Format("SELECT DL, GiaBan from DMNL WHERE Ma = '{0}'", mb));
                    if (dt.Rows.Count > 0)
                    {
                        e.Row["MB_DL"] = dt.Rows[0]["DL"];
                        e.Row["MB_DG"] = dt.Rows[0]["GiaBan"];
                    }
                }

                if (!string.IsNullOrEmpty(sc))
                {
                    var dt = db.GetDataTable(string.Format("SELECT DL, GiaBan from DMNL WHERE Ma = '{0}'", sc));
                    if (dt.Rows.Count > 0)
                    {
                        e.Row["SC_DL"] = dt.Rows[0]["DL"];
                        e.Row["SC_DG"] = dt.Rows[0]["GiaBan"];
                    }
                }

                if (!string.IsNullOrEmpty(mc))
                {
                    var dt = db.GetDataTable(string.Format("SELECT DL, GiaBan from DMNL WHERE Ma = '{0}'", mc));
                    if (dt.Rows.Count > 0)
                    {
                        e.Row["MC_DL"] = dt.Rows[0]["DL"];
                        e.Row["MC_DG"] = dt.Rows[0]["GiaBan"];
                    }
                }

                if (!string.IsNullOrEmpty(se))
                {
                    var dt = db.GetDataTable(string.Format("SELECT DL, GiaBan from DMNL WHERE Ma = '{0}'", se));
                    if (dt.Rows.Count > 0)
                    {
                        e.Row["SE_DL"] = dt.Rows[0]["DL"];
                        e.Row["SE_DG"] = dt.Rows[0]["GiaBan"];
                    }
                }

                if (!string.IsNullOrEmpty(me))
                {
                    var dt = db.GetDataTable(string.Format("SELECT DL, GiaBan from DMNL WHERE Ma = '{0}'", me));
                    if (dt.Rows.Count > 0)
                    {
                        e.Row["ME_DL"] = dt.Rows[0]["DL"];
                        e.Row["ME_DG"] = dt.Rows[0]["GiaBan"];
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