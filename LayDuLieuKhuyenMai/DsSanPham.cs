using CDTDatabase;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LayDuLieuKhuyenMai
{
    public partial class DsSanPham : XtraForm
    {
        Database db = Database.NewDataDatabase();
        public BindingSource source = new BindingSource();
        public DsSanPham()
        {
            InitializeComponent();
            GetDataSP();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
        }

        private void GetDataSP()
        {
            string sql = string.Format(@"SELECT MaSP, TenSP FROM mDMSP");
            DataTable dt = db.GetDataTable(sql);

            rItemLookUpEditMaSP.DataSource = dt;
            rItemLookUpEditMaSP.DisplayMember = "TenSP";
            rItemLookUpEditMaSP.ValueMember = "MaSP";
        }

        private void DsSanPham_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = source;
            (gridControl1.MainView as GridView).BestFitColumns();
            (gridControl1.MainView as GridView).AddNewRow();
        }

        private void DsSanPham_FormClosed(object sender, FormClosedEventArgs e)
        {
            gridControl1.RefreshDataSource();
        }
    }
}
