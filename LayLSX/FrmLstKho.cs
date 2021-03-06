using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace LayLSX
{
    public partial class FrmLstKho : DevExpress.XtraEditors.XtraForm
    {
        public DataTable dtKho = new DataTable();
        public FrmLstKho(List<string> lstKho)
        {
            InitializeComponent();
            dtKho.Columns.Add("Kho", typeof(Double));
            dtKho.Columns.Add("Stt", typeof(Int32));
            lstKho.Sort();
            foreach (string kho in lstKho)
                dtKho.Rows.Add(new object[] { kho, lstKho.IndexOf(kho) + 1 });
            gridControl1.DataSource = dtKho;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            using (DataView dv = new DataView(dtKho))
            {
                dv.Sort = "Stt";
                for (int i = 0; i < dv.Count; i++)
                    if (i + 1 != Convert.ToInt32(dv[i]["Stt"]))
                    {
                        XtraMessageBox.Show("Số thứ tự của khổ " + dv[i]["Kho"] + " chưa đúng");
                        return;
                    }
            }
            this.Close();
        }
    }
}