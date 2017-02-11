using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;

namespace XuLyDMSP
{
    public class XuLyDMSP : ICControl
    {
        bool _isBanSP = false;
        DataCustomFormControl _data;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        #region ICControl Members

        public void AddEvent()
        {
            _isBanSP = _data.DrTable.Table.Columns.Contains("ExtraSql") && _data.DrTable["ExtraSql"].ToString().Contains("IsBanSP = 1");
            _data.BsMain.DataSourceChanged += new EventHandler(BsMain_DataSourceChanged);
            BsMain_DataSourceChanged(_data.BsMain, new EventArgs());

            GridControl gcMain = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl);
            RepositoryItemGridLookUpEdit glu = gcMain.RepositoryItems["Ma"] as RepositoryItemGridLookUpEdit;
            if (_isBanSP)
                glu.Popup += new EventHandler(glu_Popup);
        }

        void glu_Popup(object sender, EventArgs e)
        {
            GridLookUpEdit glu = sender as GridLookUpEdit;
            glu.Properties.View.ClearColumnsFilter();
            glu.Properties.View.ActiveFilterString = "[IsBanSP] = 0";
        }

        void BsMain_DataSourceChanged(object sender, EventArgs e)
        {
            if (_data.BsMain.DataSource != null)
            {
                DataSet ds = _data.BsMain.DataSource as DataSet;
                ds.Tables[0].TableNewRow += new DataTableNewRowEventHandler(XuLyDMSP_TableNewRow);
                if (_data.BsMain.Current != null)
                    XuLyDMSP_TableNewRow(ds.Tables[0], new DataTableNewRowEventArgs((_data.BsMain.Current as DataRowView).Row));
            }
        }

        void XuLyDMSP_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["IsBanSP"] = _isBanSP;
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
