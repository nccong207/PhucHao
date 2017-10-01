using CDTDatabase;
using CDTLib;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TinhNgayGH
{
    public partial class FormDTDonHang : XtraForm
    {
        public string MaKH;
        public DataTable DtDonHang;
        Database db = Database.NewDataDatabase();

        public FormDTDonHang(string maKh)
        {
            InitializeComponent();
            MaKH = maKh;
            GetXuatPS();
            rilookUpEditPS.QueryCloseUp += new System.ComponentModel.CancelEventHandler(rilookUpEditPS_QueryCloseUp);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        void rilookUpEditPS_QueryCloseUp(object sender, System.ComponentModel.CancelEventArgs e)
        {

            GridLookUpEdit glu = sender as GridLookUpEdit;
            if (glu.Properties.View.FocusedRowHandle >= 0 && glu.Properties.View.IsDataRow(glu.Properties.View.FocusedRowHandle))
            {
                double soluongTon = double.Parse(glu.Properties.View.GetFocusedRowCellValue("SL tồn").ToString());

                var grid = gridDTDonHang.MainView as GridView;
                double soluong = double.Parse(grid.GetFocusedRowCellValue("SoLuong").ToString());
                double dao = double.Parse(grid.GetFocusedRowCellValue("Dao").ToString());
              if (soluongTon < soluong * dao)
              {
                    XtraMessageBox.Show("Không đủ số lượng phôi sóng để xuất", Config.GetValue("PackageName").ToString());
                    e.Cancel = true;
               }
            }
        }
        private void GetXuatPS()
        {
            string sql = string.Format("select * from wDTLSX where MaKH = '{0}'", MaKH);
            DataTable dt = db.GetDataTable(sql);
            rilookUpEditPS.DataSource = dt;
            rilookUpEditPS.DisplayMember = "TenHang";
            rilookUpEditPS.ValueMember = "DTDHID";
            rilookUpEditPS.View.BestFitColumns();
            rilookUpEditPS.View.Columns["DTDHID"].Visible = false;

            rilookUpEditPS.View.Columns["Lop"].Caption = "Lớp";
            rilookUpEditPS.View.Columns["Lop"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["Lop"].DisplayFormat.FormatString = "###,###";

            rilookUpEditPS.View.Columns["Ngày"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            rilookUpEditPS.View.Columns["Ngày"].DisplayFormat.FormatString = "dd/MM/yyyy";
            

            rilookUpEditPS.View.Columns["TenHang"].Caption = "Tên hàng";

            rilookUpEditPS.View.Columns["Dai"].Caption = "Dài";
            rilookUpEditPS.View.Columns["Dai"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["Dai"].DisplayFormat.FormatString = "###,###.###";

            rilookUpEditPS.View.Columns["Rong"].Caption = "Rộng";
            rilookUpEditPS.View.Columns["Rong"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["Rong"].DisplayFormat.FormatString = "###,###.###";

            rilookUpEditPS.View.Columns["Cao"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["Cao"].DisplayFormat.FormatString = "###,###.###";

            rilookUpEditPS.View.Columns["SL đã sx"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["SL đã sx"].DisplayFormat.FormatString = "###,###";

            rilookUpEditPS.View.Columns["SL xuất"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["SL xuất"].DisplayFormat.FormatString = "###,###";

            rilookUpEditPS.View.Columns["SL tồn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["SL tồn"].DisplayFormat.FormatString = "###,###";

            rilookUpEditPS.View.Columns["SLNhap"].Caption = "SL nhập";
            rilookUpEditPS.View.Columns["SLNhap"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["SLNhap"].DisplayFormat.FormatString = "###,###.###";

            rilookUpEditPS.View.Columns["MaKH"].Visible = false;
            rilookUpEditPS.PopupFormMinSize = new Size (1000,400);
        }

        private void FormDTDonHang_Load(object sender, EventArgs e)
        {
            gridDTDonHang.DataSource = DtDonHang.DefaultView;
            (gridDTDonHang.MainView as GridView).BestFitColumns();
        }

        private void FormDTDonHang_FormClosed(object sender, FormClosedEventArgs e)
        {
            gridDTDonHang.RefreshDataSource();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
