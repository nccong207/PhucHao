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
        public SPListForm()
        {
            InitializeComponent();
            //gridView1.OptionsBehavior.Editable = false;
        }

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
