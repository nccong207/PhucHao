using CDTDatabase;
using CDTLib;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace POSApp
{
    public partial class Main : Form
    {
        DataTable source;
        DataRow loginUser;
        Database db = Database.NewStructDatabase();
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
        public void AddToReturnGrid(string macuon, decimal duongkinh)
        {
            string ngay = DateTime.Now.ToString();
            string soluongbd = IsExisted(macuon);
           
            if (soluongbd == null)
            {
                MessageBox.Show("Mã cuộn không tồn tại trong danh sách sử dụng");
            } else
            {
                decimal soluongbdNum =Convert.ToDecimal(soluongbd.Split('-')[2].Replace(",", "").Replace("KG", ""));
                decimal soluongCL = soluongbdNum - duongkinh;
                string nguoiduyet = loginUser["HoTen"].ToString();

                string sql = @"INSERT INTO YeuCauXuatKho (Ngay, MaCuon, SoLuongBD, SoLuongSD, SoLuongCL, NguoiDuyet, Duyet)
                                VALUES ('{0}','{1}',{2},{3},{4},'{5}',0)";
                db.UpdateByNonQuery(string.Format(sql, ngay, macuon, soluongbdNum, duongkinh, soluongCL, nguoiduyet));
                ReloadReturnGrid();
            }

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
                        return;
                    }
                }

                DataRow row = source.NewRow();
                AddNewRow(row, data, may);

            }
        }

        public string IsExisted(string macuon)
        {
            if (gridView1.RowCount == 0)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    string value1 = gridView1.GetRowCellValue(i, "MS1").ToString();
                    if (!string.IsNullOrEmpty(value1) && value1.Split('-')[0].Contains(macuon))
                    {
                        return value1;
                    }

                    string value2 = gridView1.GetRowCellValue(i, "MS2").ToString();
                    if (!string.IsNullOrEmpty(value2) && value2.Split('-')[0].Contains(macuon))
                    {
                        return value2;
                    }

                    string value3 = gridView1.GetRowCellValue(i, "MS3").ToString();
                    if (!string.IsNullOrEmpty(value3) && value3.Split('-')[0].Contains(macuon))
                    {
                        return value3;
                    }
                }

                return null;
            }
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
