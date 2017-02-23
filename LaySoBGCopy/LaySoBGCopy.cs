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
        public void AddEvent()
        {
            DataSet dsData = _data.BsMain.DataSource as DataSet;
            
            if (dsData == null)
                return;
            dsData.Tables[0].TableNewRow += new DataTableNewRowEventHandler(BaoGia_TableNewRow);
            dsData.Tables[0].RowChanged += BaoGia_RowChanged;

        }

        void BaoGia_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            // Set số báo giá cũ cho form mới
            if (string.IsNullOrEmpty(e.Row["SoBGCopy"].ToString()) && !string.IsNullOrEmpty(oldBG))
            {
                e.Row["SoBGCopy"] = oldBG;
                oldBG = "";
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