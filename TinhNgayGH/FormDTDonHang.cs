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
            string sql = string.Format(@"SELECT *, [SL đã nhập] + [SL hàng trả] - [SL đã xuất] - [SL giấy phế] as [SL tồn]
                                        FROM wDTTONTP WHERE makh = '{0}'", MaKH);
            DataTable dt = db.GetDataTable(sql);
            rilookUpEditPS.DataSource = dt;
            rilookUpEditPS.DisplayMember = "tenhang";
            rilookUpEditPS.ValueMember = "dtdhid";
            rilookUpEditPS.View.BestFitColumns();

            //hide column
            rilookUpEditPS.View.Columns["makh"].Visible = false;
            rilookUpEditPS.View.Columns["tenkh"].Visible = false;
            rilookUpEditPS.View.Columns["dongia"].Visible = false;
            rilookUpEditPS.View.Columns["MTDHID"].Visible = false;
            rilookUpEditPS.View.Columns["dtdhid"].Visible = false;
            rilookUpEditPS.View.Columns["mahh"].Visible = false;
            rilookUpEditPS.View.Columns["ghichu"].Visible = false;
            rilookUpEditPS.View.Columns["SL giấy phế"].Visible = false;


            //show column
            rilookUpEditPS.View.Columns["sodh"].VisibleIndex = 0;
            rilookUpEditPS.View.Columns["sodh"].Caption = "Số đơn hàng";

            rilookUpEditPS.View.Columns["ngaydh"].VisibleIndex = 1;
            rilookUpEditPS.View.Columns["ngaydh"].Caption = "Ngày đơn hàng";
            rilookUpEditPS.View.Columns["ngaydh"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            rilookUpEditPS.View.Columns["ngaydh"].DisplayFormat.FormatString = "dd/MM/yyyy";


            rilookUpEditPS.View.Columns["tenhang"].VisibleIndex = 2;
            rilookUpEditPS.View.Columns["tenhang"].Caption = "Tên hàng";

            rilookUpEditPS.View.Columns["loai"].VisibleIndex = 3;
            rilookUpEditPS.View.Columns["loai"].Caption = "Loại";

            rilookUpEditPS.View.Columns["lop"].Caption = "Lớp";
            rilookUpEditPS.View.Columns["lop"].VisibleIndex = 4;
            rilookUpEditPS.View.Columns["lop"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["lop"].DisplayFormat.FormatString = "###,###";

            rilookUpEditPS.View.Columns["dai"].Caption = "Dài";
            rilookUpEditPS.View.Columns["dai"].VisibleIndex = 5;
            rilookUpEditPS.View.Columns["dai"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["dai"].DisplayFormat.FormatString = "###,###.###";

            rilookUpEditPS.View.Columns["rong"].Caption = "Rộng";
            rilookUpEditPS.View.Columns["rong"].VisibleIndex = 6;
            rilookUpEditPS.View.Columns["rong"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["rong"].DisplayFormat.FormatString = "###,###.###";

            rilookUpEditPS.View.Columns["cao"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["cao"].VisibleIndex = 7;
            rilookUpEditPS.View.Columns["cao"].DisplayFormat.FormatString = "###,###.###";

            rilookUpEditPS.View.Columns["SL đặt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["SL đặt"].VisibleIndex = 8;
            rilookUpEditPS.View.Columns["SL đặt"].DisplayFormat.FormatString = "###,###";

            rilookUpEditPS.View.Columns["SL đã xuất"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["SL đã xuất"].VisibleIndex = 9;
            rilookUpEditPS.View.Columns["SL đã xuất"].DisplayFormat.FormatString = "###,###";

            rilookUpEditPS.View.Columns["SL đã nhập"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["SL đã nhập"].VisibleIndex = 10;
            rilookUpEditPS.View.Columns["SL đã nhập"].DisplayFormat.FormatString = "###,###";

            rilookUpEditPS.View.Columns["SL hàng trả"].VisibleIndex = 11;
            rilookUpEditPS.View.Columns["SL hàng trả"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["SL hàng trả"].DisplayFormat.FormatString = "###,###";

            rilookUpEditPS.View.Columns["SL tồn"].VisibleIndex = 12;
            rilookUpEditPS.View.Columns["SL tồn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            rilookUpEditPS.View.Columns["SL tồn"].DisplayFormat.FormatString = "###,###.###";

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
