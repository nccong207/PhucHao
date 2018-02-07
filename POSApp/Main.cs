using CDTDatabase;
using CDTLib;
using DevExpress.XtraEditors;
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
        AppCon ac = new AppCon();
        public Main(DataRow drUser)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string StructConnection = ac.GetValue("StructDb");
            if (string.IsNullOrEmpty(StructConnection))
            {
                XtraMessageBox.Show("Không tìm thấy chuỗi kết nối database", Config.GetValue("PackageName").ToString());
                this.Close();
            }
            StructConnection = Security.DeCode(StructConnection);
            StructConnection = StructConnection.Replace("POS", "HTCPH");

            Database db = Database.NewCustomDatabase(StructConnection);
            var macuon = textBox1.Text;
            var soTon = db.GetValue(string.Format("SELECT SoLuong FROM TonKhoNL WHERE MaCuon = '{0}'", macuon.Trim()));
            decimal soluongTon = 0;
            if (soTon != null)
            {
                soluongTon = Convert.ToDecimal(soTon.ToString());
            }

            var manl = db.GetValue(string.Format("SELECT MaNL FROM DT42 WHERE MaCuon = '{0}'", macuon.Trim()));
            string kyhieu = "", kho = "";
            if (manl != null)
            {
                DataTable dmNL = db.GetDataTable(string.Format("SELECT KyHieu, Kho FROM wDMNL2 WHERE MaNL = '{0}'", manl.ToString()));
                if (dmNL.Rows.Count > 0)
                {
                    kyhieu = dmNL.Rows[0]["KyHieu"].ToString();
                    kho = dmNL.Rows[0]["Kho"].ToString();
                }
            }

            //DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
            //row.Cells[0].Value = macuon;
            //row.Cells[1].Value = kyhieu;
            //row.Cells[2].Value = kho;
            //row.Cells[3].Value = soluongTon;
            dataGridView1.Rows.Add(macuon, kyhieu, kho, soluongTon);
        }

        private void AddToGrid()
        {

        }
    }
}
