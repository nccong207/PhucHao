using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;

namespace CheckXK
{
    public class CheckXK : ICControl
    {
        private DataCustomFormControl _data;
        private InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        #region ICControl Members

        public void AddEvent()
        {
            GridView gvDetail = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;
            if (_data.DrTable.Table.Columns.Contains("ExtraSql")
                && _data.DrTable["ExtraSql"].ToString().Contains("XK = 0"))
            {
                gvDetail.Columns["Loi"].OptionsColumn.ReadOnly = true;
                //gvDetail.Columns["Loi"].Visible = false;
                gvDetail.Columns["DVTTL"].Visible = false;
                gvDetail.Columns["SLTL"].Visible = false;
                gvDetail.Columns["DGTL"].Visible = false;
                gvDetail.Columns["TTTL"].Visible = false;
                //
                gvDetail.Columns["SLDangTon"].Visible = false;
                gvDetail.Columns["SLTonCuoi"].Visible = true;
                gvDetail.Columns["isGP"].Visible = true;
                gvDetail.Columns["GhiChuGP"].Visible = false;
                gvDetail.Columns["GhiChuID"].Visible = true;
            }
            else if (_data.DrTable.Table.Columns.Contains("ExtraSql")
                && _data.DrTable["ExtraSql"].ToString().Contains("XK = 1"))
            {
                gvDetail.Columns["SLDangTon"].Visible = false;
                gvDetail.Columns["SLTonCuoi"].Visible = false;
                gvDetail.Columns["isGP"].Visible = false;
                gvDetail.Columns["GhiChuGP"].Visible = false;
                gvDetail.Columns["GhiChuID"].Visible = false;
                // 
                gvDetail.Columns["DVTTL"].Visible = true;
                gvDetail.Columns["SLTL"].Visible = true;
                gvDetail.Columns["DGTL"].Visible = true;
                gvDetail.Columns["TTTL"].Visible = true;
            }
            if (_data.BsMain.DataSource != null)
            {
                DataSet dsData = _data.BsMain.DataSource as DataSet;
                if (dsData == null)
                    return;
                dsData.Tables[0].TableNewRow += new DataTableNewRowEventHandler(PBCP_TableNewRow);
            }
            _data.BsMain.DataSourceChanged += new EventHandler(BsMain_DataSourceChanged);
            if (_data.BsMain.Current != null)
            {
                DataRow dr = (_data.BsMain.Current as DataRowView).Row;
                DataTableNewRowEventArgs e = new DataTableNewRowEventArgs(dr);
                PBCP_TableNewRow((_data.BsMain.DataSource as DataSet).Tables[0], e);
            }
        }

        void BsMain_DataSourceChanged(object sender, EventArgs e)
        {
            DataSet dsData = _data.BsMain.DataSource as DataSet;
            if (dsData == null)
                return;
            dsData.Tables[0].TableNewRow += new DataTableNewRowEventHandler(PBCP_TableNewRow);
        }

        void PBCP_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            if (e.Row == null)
                return;
            if (_data.DrTable.Table.Columns.Contains("ExtraSql")
                && _data.DrTable["ExtraSql"].ToString().Contains("XK = 1"))
                e.Row["XK"] = 1;
            else
                e.Row["XK"] = 0;
        }

        public DataCustomFormControl Data
        {
            set { _data = value; }
        }

        public InfoCustomControl Info
        {
            get { return _info; }
        }

        #endregion
    }
}
