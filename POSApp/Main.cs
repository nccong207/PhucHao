using CDTDatabase;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
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

            gridView1.DoubleClick += GridView1_DoubleClick;
            LoadToMainGrid();
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                string colCaption = info.Column == null ? "N/A" : info.Column.Name;
                //MessageBox.Show(string.Format("DoubleClick on row: {0}, column: {1}.", info.RowHandle, colCaption));

                var data = gridView1.GetRowCellValue(info.RowHandle, colCaption.ToUpper()).ToString();
                if (!string.IsNullOrEmpty(data))
                {
                    var macuon = data.Split('-')[0].Trim();
                    Input frm = new Input();
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog();

                    Add addfrm = new Add(this, true);
                    var mc = addfrm.GetMaCuon(macuon);

                    if (frm.DialogResult != DialogResult.Cancel)
                    {
                        RemoveMainGrid(mc.Macuon);
                        AddToReturnGrid(mc, frm.duongkinh);
                        SyncMainGrid();
                    }
                }
            }
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

        public void RemoveMainGrid(string macuon)
        {

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                if (!string.IsNullOrEmpty(gridView1.GetRowCellValue(i, "MS1").ToString()))
                {
                    var data = gridView1.GetRowCellValue(i, "MS1").ToString();
                    if (data.Contains(macuon))
                    {
                        gridView1.SetRowCellValue(i, "MS1", "");
                        return;
                    }

                    var data2 = gridView1.GetRowCellValue(i, "MS2").ToString();
                    if (data2.Contains(macuon))
                    {
                        gridView1.SetRowCellValue(i, "MS2", "");
                        return;
                    }

                    var data3 = gridView1.GetRowCellValue(i, "MS3").ToString();
                    if (data3.Contains(macuon))
                    {
                        gridView1.SetRowCellValue(i, "MS3", "");
                        return;
                    }

                }
            }
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
            string sql = @"INSERT INTO YeuCauXuatKho (Ngay, MaCuon, SoLuongBD, SoLuongSD, SoLuongCL, NguoiDuyet, LSX, Duyet, NguoiLap)
                            VALUES ('{0}','{1}',{2},{3},{4},'{5}', '{6}',1, '{7}')";
            db.UpdateByNonQuery(string.Format(sql, ngay, macuon.Macuon, soluongBD, soluongSD, soluongCL, nguoiduyet, list, loginUser["Ma"].ToString()));
            ReloadReturnGrid();

        }

        private string GetListOder(decimal soluongSD, DataTable orderSelected)
        {
            string listOrderNo = "";
            DataView dv = orderSelected.DefaultView;
            dv.Sort = "CutNum asc";
            DataTable sortedDT = dv.ToTable();
            decimal total = 0;
            foreach (DataRow row in sortedDT.Rows)
            {
                total += Convert.ToDecimal(row["CutNum"]);
                listOrderNo += row["OrderNo"] + ",";
                if (total >= soluongSD)
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

        public void SyncMainGrid()
        {
            string sql = "DELETE FROM QLMacuon";
            db.UpdateByNonQuery(sql);
          
            string format = "INSERT INTO QLMacuon (MS1, MS2, MS3)  VALUES ('{0}','{1}','{2}');";

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                if (!string.IsNullOrEmpty(gridView1.GetRowCellValue(i, "MS1").ToString()))
                {
                    var info1 = gridView1.GetRowCellValue(i, "MS1").ToString();
                    var info2 = gridView1.GetRowCellValue(i, "MS2").ToString();
                    var info3 = gridView1.GetRowCellValue(i, "MS3").ToString();
                    db.UpdateByNonQuery(string.Format(format, info1, info2, info3));
                }
            }
        }

        public void LoadToMainGrid()
        {
            string sql = "SELECT * FROM QLMacuon";
            DataTable dt = db.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                gridControl1.DataSource = dt;
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
            {
                string field = "";
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
                        SyncMainGrid();
                        return;
                    }
                }

                DataRow row = source.NewRow();
                AddNewRow(row, data, may);
            }

            SyncMainGrid();
        }

        public DataTable GetOrder(MaCuon macuon, SoMay may)
        {
            // kiểm tra cuộn giấy đó có trong đơn hàng sản xuất 
            Database longwayDb = Database.NewStructDatabase();

            //string connect = "Server =LINH-PC\\HOATIEU; database = CPMS; user = sa; pwd = ht";
            //Database longwayDb = Database.NewCustomDatabase(connect);

            int startNum = 0, endNum = 0;

            switch (may)
            {
                case SoMay.May1: startNum = 3; endNum = 5; break;
                case SoMay.May2: startNum = 7; endNum = 9; break;
                case SoMay.May3: startNum = 11; endNum = 13; break;
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
