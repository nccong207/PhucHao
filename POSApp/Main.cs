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
        public Main(DataRow drUser)
        {
            InitializeComponent();
            source = new DataTable();
            source.Clear();
            source.Columns.Add("MS1");
            source.Columns.Add("MS2");
            source.Columns.Add("MS3");
        }

        private void button1_Click(object sender, EventArgs e)
        {


            //DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
            //row.Cells[0].Value = macuon;
            //row.Cells[1].Value = kyhieu;
            //row.Cells[2].Value = kho;
            //row.Cells[3].Value = soluongTon;
            //dataGridView1.Rows.Add(macuon, kyhieu, kho, soluongTon);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Add frm = new Add(this);
            frm.ShowDialog();
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
    }
}
