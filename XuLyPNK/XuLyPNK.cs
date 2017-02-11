using System;
using Plugins;
using System.Data;
using System.Windows.Forms;
using CDTLib;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraTab;
using FormFactory;

namespace XuLyPNK
{
    public class XuLyPNK : ICControl
    {
        DataRow drCurrent;
        GridView gvSP;
        CDTForm frmSP;
        bool _isBanSP = false;
        DataCustomFormControl _data;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        #region ICControl Members

        public void AddEvent()
        {
            _isBanSP = _data.DrTable.Table.Columns.Contains("ExtraSql") && _data.DrTable["ExtraSql"].ToString().Contains("IsBanSP = 1");
            _data.BsMain.DataSourceChanged += new EventHandler(BsMain_DataSourceChanged);
            BsMain_DataSourceChanged(_data.BsMain, new EventArgs());

            GridControl gcMain = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl);
            RepositoryItemGridLookUpEdit glu = gcMain.RepositoryItems["MaSP"] as RepositoryItemGridLookUpEdit;
            glu.Popup += new EventHandler(glu_Popup);

            gvSP = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;
            LayoutControl lcMain = _data.FrmMain.Controls.Find("lcMain", true)[0] as LayoutControl;
            SimpleButton btnLSX = new SimpleButton();
            btnLSX.Name = "btnLSX";
            btnLSX.Text = "Chọn lệnh sản xuất";
            LayoutControlItem lci1 = lcMain.AddItem("", btnLSX);
            lci1.Name = "cusSP";
            btnLSX.Click += new EventHandler(btnLSX_Click);
        }

        private void btnLSX_Click(object sender, EventArgs e)
        {
            if (!gvSP.Editable)
            {
                XtraMessageBox.Show("Vui lòng chọn chế độ thêm hoặc sửa phiếu",
                    Config.GetValue("PackageName").ToString());
                return;
            }
            Config.NewKeyValue("@IsBanSP", _isBanSP);
            drCurrent = (_data.BsMain.Current as DataRowView).Row;
            frmSP = FormFactory.FormFactory.Create(FormType.Report, "1586") as ReportPreview;
            //viết xử lý cho nút F4-Xử lý trong report
            SimpleButton btnXuLy1 = (frmSP.Controls.Find("btnXuLy", true)[0] as SimpleButton);
            btnXuLy1.Text = "Chọn lệnh sản xuất";
            btnXuLy1.Click += new EventHandler(btnXuLyLSX_Click);
            frmSP.WindowState = FormWindowState.Maximized;
            frmSP.ShowDialog();
        }

        private void btnXuLyLSX_Click(object sender, EventArgs e)
        {
            GridView gvDS = (frmSP.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;
            DataTable dtDS = (gvDS.DataSource as DataView).Table;
            dtDS.AcceptChanges();
            DataRow[] drs = dtDS.Select("Chọn = 1");
            if (drs.Length == 0)
            {
                XtraMessageBox.Show("Bạn chưa chọn sản phẩm", Config.GetValue("PackageName").ToString());
                return;
            }
            frmSP.Close();

            DataTable dtSP = (_data.BsMain.DataSource as DataSet).Tables[1];
            string filter = drCurrent["MTID"].Equals(DBNull.Value) ? "MTID is null and DTKHID = '{0}'" :
                "MTID = '" + drCurrent["MTID"].ToString() + "' and DTKHID = '{0}'";
            foreach (DataRow dr in drs)
            {
                if (dtSP.Select(string.Format(filter, dr["DTID"])).Length > 0)
                    continue;
                gvSP.AddNewRow();
                gvSP.SetFocusedRowCellValue(gvSP.Columns["MaSP"], dr["MaSP"]);
                gvSP.SetFocusedRowCellValue(gvSP.Columns["DTKHID"], dr["DTID"]);
                gvSP.SetFocusedRowCellValue(gvSP.Columns["SoLuong"], dr["Số lượng cần SX"]);
                gvSP.UpdateCurrentRow();
            }
        }

        void glu_Popup(object sender, EventArgs e)
        {
            GridLookUpEdit glu = sender as GridLookUpEdit;
            glu.Properties.View.ClearColumnsFilter();
            if (_isBanSP)
                glu.Properties.View.ActiveFilterString = "[IsBanSP] = 1";
            else
                glu.Properties.View.ActiveFilterString = "[IsBanSP] = 0";
        }

        void BsMain_DataSourceChanged(object sender, EventArgs e)
        {
            if (_data.BsMain.DataSource != null)
            {
                DataSet ds = _data.BsMain.DataSource as DataSet;
                ds.Tables[0].TableNewRow += new DataTableNewRowEventHandler(XuLyDMSP_TableNewRow);
                if (_data.BsMain.Current != null)
                    XuLyDMSP_TableNewRow(ds.Tables[0], new DataTableNewRowEventArgs((_data.BsMain.Current as DataRowView).Row));
            }
        }

        void XuLyDMSP_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["IsBanSP"] = _isBanSP;
        }

        public DataCustomFormControl Data
        {
            set { _data = value; }
        }

        public InfoCustomControl Info
        {
            get { return _info; }
        }

        #endregion
    }
}
