using CDTDatabase;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Windows.Forms;

namespace POSApp
{
    public partial class Main : Form
    {
        DataTable source;
        DataRow loginUser;
        Database db = Database.NewDataDatabase();
        public Main(DataRow drUser)
        {
            InitializeComponent();
            source = new DataTable();
            source.Clear();
            source.Columns.Add("MS1");
            source.Columns.Add("MS2");
            source.Columns.Add("MS3");
            loginUser = drUser;
            ReloadReturnGrid();

            var grid1 = gridControl1.MainView as GridView;
            grid1.OptionsBehavior.Editable = false;

            var grid2 = gridControl2.MainView as GridView;
            grid2.OptionsBehavior.Editable = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        public bool IsQuanLy()
        {
            return loginUser["Quyen"].ToString().Equals("Quản lý");
        }

        public void UpdateLoginUser(DataRow drUser)
        {
            loginUser = drUser;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Add frm = new Add(this);
            frm.ShowDialog();
        }
        public void AddToReturnGrid(MaCuon macuon, decimal duongkinh)
        {
            string ngay = DateTime.Now.ToString();
            decimal soluongBD = Convert.ToDecimal(macuon.SoKg);
            decimal soluongCL = duongkinh * Convert.ToDecimal(macuon.Kho) * Convert.ToDecimal("3.14") * macuon.TileK;
            decimal soluongSD = soluongBD - soluongCL;

            string list = "";
            // tính số lượng đơn hàng sử dụng
            var may1 = GetOrder(macuon, SoMay.May1);
            if (may1 != null)
            {
                list += GetListOder(soluongSD, may1);
            }

            var may2 = GetOrder(macuon, SoMay.May2);
            if (may2 != null)
            {
                list += GetListOder(soluongSD, may2);
            }

            var may3 = GetOrder(macuon, SoMay.May3);
            if (may3 != null)
            {
                list += GetListOder(soluongSD, may3);
            }

            string nguoiduyet = loginUser["HoTen"].ToString();
            string sql = @"INSERT INTO YeuCauXuatKho (Ngay, MaCuon, SoLuongBD, SoLuongSD, SoLuongCL, NguoiDuyet, LSX, Duyet)
                            VALUES ('{0}','{1}',{2},{3},{4},'{5}', '{6}',1)";
            db.UpdateByNonQuery(string.Format(sql, ngay, macuon, soluongBD, soluongSD, soluongCL, nguoiduyet, list));
            ReloadReturnGrid();

        }

        private string GetListOder (decimal soluongSD, DataTable orderSelected)
        {
            string listOrderNo =  "";
            DataView dv = orderSelected.DefaultView;
            dv.Sort = "CutNum asc";
            DataTable sortedDT = dv.ToTable();
            decimal total = 0;
            foreach (DataRow row in sortedDT.Rows)
            {
                total += Convert.ToDecimal(row["CutNum"]);
                listOrderNo += row["OrderNo"] + ",";
                if (total >=     soluongSD)
                {
                    return listOrderNo;
                }
            }
            return listOrderNo;
        }

        public void ReloadReturnGrid()
        {
            string sql = "SELECT * FROM YeuCauXuatKho";
            DataTable dt = db.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                gridControl2.DataSource = dt;
            }
        }

        public void LoadToGrid(MaCuon macuon, SoMay may)
        {
            string data = macuon.Macuon + " - " + macuon.KyHieu + " - " + macuon.SoKg.ToString("###,###") + "KG";
            if (gridView1.RowCount == 0)
            {
                DataRow row = source.NewRow();
                AddNewRow(row, data, may);
            }
            else
            {                string field = "";
                switch (may)
                {
                    case SoMay.May1: field = "MS1"; break;
                    case SoMay.May2: field = "MS2"; break;
                    case SoMay.May3: field = "MS3"; break;
                }

                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    if (string.IsNullOrEmpty(gridView1.GetRowCellValue(i, field).ToString()))
                    {
                        gridView1.SetRowCellValue(i, field, data);
                        return;
                    }
                }

                DataRow row = source.NewRow();
                AddNewRow(row, data, may);

            }
        }

        public DataTable GetOrder(MaCuon macuon, SoMay may)
        {
            // kiểm tra cuộn giấy đó có trong đơn hàng sản xuất 
            Database longwayDb = Database.NewStructDatabase();
            int startNum = 0, endNum = 0;

            switch (may)
            {
                case SoMay.May1:startNum = 3; endNum = 5; break;
                case SoMay.May2:startNum = 7; endNum = 9; break;
                case SoMay.May3:startNum = 11; endNum = 13; break;
                default:
                    break;
            }

            string query = @"SELECT PaperUse, ProduceWid, CutNum, SumSquare, OrderNo FROM LW_Order
            WHERE SUBSTRING(PaperUse, {0}, 2) = '{1}' OR SUBSTRING(PaperUse, {2}, 2) = '{3}'";

            DataTable order = longwayDb.GetDataTable(string.Format(query, startNum, macuon.KyHieu, endNum, macuon.KyHieu));
            return order;
        }

        private void AddNewRow(DataRow row, string data, SoMay may)
        {

            switch (may)
            {
                case SoMay.May1:
                    row[0] = data;
                    row[1] = "";
                    row[2] = "";
                    break;

                case SoMay.May2:
                    row[0] = "";
                    row[1] = data;
                    row[2] = "";
                    break;

                case SoMay.May3:
                    row[0] = "";
                    row[1] = "";
                    row[2] = data;
                    break;

            }
            source.Rows.Add(row);
            gridControl1.DataSource = source;
        }

        private void btnTra_Click(object sender, EventArgs e)
        {
            Add returnFrm = new Add(this, true);
            returnFrm.ShowDialog();
        }
    }
}
