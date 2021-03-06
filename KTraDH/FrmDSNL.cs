using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FormFactory;
using CDTLib;

namespace KTraDH
{
    public partial class FrmDSNL : DevExpress.XtraEditors.XtraForm
    {
        public FrmDSNL(DataTable dtDSNL)
        {
            InitializeComponent();
            gcNL.DataSource = dtDSNL;
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //dùng cách này để truyền tham số vào report
            Config.NewKeyValue("@MaNL", gvNL.GetFocusedRowCellValue("Ma"));
            //dùng report 1522 trong sysReport
            Form frmDS = FormFactory.FormFactory.Create(FormType.Report, "1522") as ReportPreview;
            frmDS.WindowState = FormWindowState.Maximized;
            frmDS.ShowDialog();
        }
    }
}