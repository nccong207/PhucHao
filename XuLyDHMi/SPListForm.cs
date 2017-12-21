using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace XuLyDHMi
{
    public partial class SPListForm : XtraForm
    {
        public DataTable dsSp;
        public SPListForm(DataTable dsSpData)
        {
            InitializeComponent();
            dsSp = dsSpData;
            gridView1.CellValueChanging += GridView1_CellValueChanged; ;
        }

        private void GridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Chon")
            {
                string id = gridView1.GetFocusedRowCellValue("DTKMNSPID").ToString();
                foreach (DataRow row in dsSp.Rows)
                {
                    if (!id.Equals(row["DTKMNSPID"].ToString()))
                    {
                        row["Chon"] = false;
                    }
                }
            }
        }

        //private void GridView1_ColumnChanged(object sender, EventArgs e)
        //{
        //    if (e.Column.FieldName == "NhieuSP" && e.Value != null)
        //    {

        //    }
        //}

        //private void DsSp_RowChanging(object sender, DataRowChangeEventArgs e)
        //{
        //    string id = e.Row["DTKMNSPID"].ToString();
        //    foreach (DataRow row in dsSp.Rows)
        //    {
        //        if (!id.Equals(row["DTKMNSPID"].ToString()))
        //        {
        //            row["Chon"] = false;
        //        }
        //    }
        //}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            gridControl1.RefreshDataSource();
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            gridControl1.RefreshDataSource();
            this.Close();
        }

        private void SPListForm_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = dsSp;
            (gridControl1.MainView as GridView).BestFitColumns();
        }
    }
}
