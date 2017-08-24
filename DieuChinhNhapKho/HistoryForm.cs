using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DieuChinhNhapKho
{
    public partial class HistoryForm : XtraForm
    {
        public HistoryForm(DataTable source)
        {
            InitializeComponent();
            this.gridControl1.DataSource = source;
            this.gridView1.OptionsBehavior.Editable = false;
        }
    }
}
