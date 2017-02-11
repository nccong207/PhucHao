using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace TienIchBG
{
    public partial class FrmDSNL : DevExpress.XtraEditors.XtraForm
    {
        public FrmDSNL(DataTable dtGia)
        {
            InitializeComponent();
            gcNL.DataSource = dtGia;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}