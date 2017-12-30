using CDTLib;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LayLSX
{
    public partial class SapXepKhoFrom : XtraForm
    {
        public DataTable data = new DataTable();
        public DateTime NgayBD = DateTime.Now, NgayKT = DateTime.Now;
        public string Kho = "";
        public SapXepKhoFrom(string currentValue)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            data.Columns.Add("Kho");

            repositoryItemCalcEdit1.Mask.EditMask = "#########0";
            repositoryItemCalcEdit1.Mask.UseMaskAsDisplayFormat = true;

            dateEdit1.Properties.Mask.EditMask = "dd/MM/yyyy";
            dateEdit1.Properties.Mask.UseMaskAsDisplayFormat = true;

            dateEdit2.Properties.Mask.EditMask = "dd/MM/yyyy";
            dateEdit2.Properties.Mask.UseMaskAsDisplayFormat = true;

            gridControl1.DataSource = data;
            gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom;
            gridControl1.ProcessGridKey += GridControl1_ProcessGridKey;

            var mainview = gridControl1.MainView as GridView;
            mainview.BestFitColumns();
            //mainview.AddNewRow();
            this.ControlBox = false;

            if (!string.IsNullOrEmpty(currentValue))
            {
                string[] param = currentValue.Split(';');
                NgayBD = DateTime.Parse(param[0]);
                NgayKT = DateTime.Parse(param[1]);

                dateEdit1.EditValue = NgayBD;
                dateEdit2.EditValue = NgayKT;

                NgayKT = (DateTime)dateEdit2.EditValue;

                foreach (var kho in param[2].Split(','))
                {
                    DataRow row = data.NewRow();
                    row["Kho"] = kho;
                    data.Rows.Add(row);
                }
            }
        } 

        private void GridControl1_ProcessGridKey(object sender, KeyEventArgs e)
        {
            var grid = sender as GridControl;
            var view = grid.FocusedView as GridView;
            if (e.KeyData == Keys.Delete)
            {
                view.DeleteSelectedRows();
                e.Handled = true;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dateEdit1.EditValue == null || dateEdit2.EditValue == null || data.Rows.Count == 0)
            {
                XtraMessageBox.Show("Vui lòng nhập đủ thông tin từ ngày và đến ngày và khổ", Config.GetValue("PackageName").ToString());
                return;
            }

            NgayBD = (DateTime) dateEdit1.EditValue;
            NgayKT = (DateTime) dateEdit2.EditValue;
            List<string> KhoList = new List<string>();
            foreach (DataRow row in data.Rows)
            {
                KhoList.Add(row["Kho"].ToString());
            }
            Kho = string.Join(",", KhoList.ToArray());
            this.Close();
        }
    }
}
