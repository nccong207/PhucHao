using CDTLib;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using FormFactory;
using Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ChonPMH
{
    public class ChonPMH : ICControl
    {
        DataCustomFormControl _data;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        GridView gvMain, gvDS;
        DataRow drCur;
        ReportPreview frmDS;

        public void AddEvent()
        {
            gvMain = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;
            //thêm nút chọn phiếu mua hàng
            LayoutControl lcMain = _data.FrmMain.Controls.Find("lcMain", true)[0] as LayoutControl;
            SimpleButton btnChon = new SimpleButton();
            btnChon.Name = "btnChon";
            btnChon.Text = "Chọn phiếu mua hàng";
            LayoutControlItem lci = lcMain.AddItem("", btnChon);
            lci.Name = "cusChonPMH";
            btnChon.Click += new EventHandler(btnChon_Click);

            //thêm nút xem phiếu mua hàng
            SimpleButton btnView = new SimpleButton();
            btnView.Name = "btnView";
            btnView.Text = "Xem phiếu mua hàng";
            LayoutControlItem lci2 = lcMain.AddItem("", btnView);
            lci2.Name = "cusViewPMH";
            btnView.Click += new EventHandler(btnChon_Click2);
        }
        void btnChon_Click(object sender, EventArgs e)
        {
            if (!gvMain.Editable)
            {
                XtraMessageBox.Show("Vui lòng chọn chế độ thêm hoặc sửa phiếu", Config.GetValue("PackageName").ToString());
                return;
            }
            drCur = (_data.BsMain.Current as DataRowView).Row;

            frmDS = FormFactory.FormFactory.Create(FormType.Report, "1597") as ReportPreview;
            gvDS = (frmDS.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;

            SimpleButton btnXuLy = (frmDS.Controls.Find("btnXuLy", true)[0] as SimpleButton);
            btnXuLy.Text = "Chọn phiếu";
            btnXuLy.Click += new EventHandler(btnXuLy_Click);
            frmDS.WindowState = FormWindowState.Maximized;
            frmDS.ShowDialog();
        }

        void btnChon_Click2(object sender, EventArgs e)
        {
            drCur = (_data.BsMain.Current as DataRowView).Row;
            Config.NewKeyValue("@SoPhieu", drCur["SoPhieu"].ToString());

            frmDS = FormFactory.FormFactory.Create(FormType.Report, "1598") as ReportPreview;
            gvDS = (frmDS.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;

            var para = (frmDS.Controls.Find("SoPhieu", true)[0] as TextEdit);
            para.Enabled = false;

            frmDS.WindowState = FormWindowState.Maximized;
            frmDS.ShowDialog();
        }

        void btnXuLy_Click(object sender, EventArgs e)
        {
            DataTable dtDS = (gvDS.DataSource as DataView).Table;
            dtDS.AcceptChanges();

            DataRow[] drs = dtDS.Select("Chọn = 1");

            frmDS.Close();
            DataTable dtDTKH = (_data.BsMain.DataSource as DataSet).Tables[1];
            string pk = _data.DrTableMaster["Pk"].ToString();


            string masterId = drCur[pk].ToString();

            foreach (DataRow dr in drs)
            {
                gvMain.AddNewRow();
                gvMain.UpdateCurrentRow();
                gvMain.SetFocusedRowCellValue(gvMain.Columns["MTDNTTID"], masterId);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["SoLuong"], dr["SoLuong"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["DonGia"], dr["DonGia"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["ThanhTien"], dr["ThanhTien"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["NgayMua"], dr["Ngày mua"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["MaNCC"], dr["MaNCC"]);
                
                gvMain.SetFocusedRowCellValue(gvMain.Columns["SoPhieu"], dr["SoPhieu"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["MaPX"], dr["MaPX"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["MaMay"], dr["MaMay"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["MaVT"], dr["MaVT"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["GhiChu"], dr["GhiChu"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["DTMHID"], dr["DTMHID"].ToString());
            }
        }

        public DataCustomFormControl Data
        {
            set { _data = value; }
        }

        public InfoCustomControl Info
        {
            get { return _info; }
        }
    }
}
