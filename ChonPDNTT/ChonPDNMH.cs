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

namespace ChonPDNMH
{
    public class ChonPDNMH : ICControl
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
            btnChon.Text = "Chọn phiếu đề nghị";
            LayoutControlItem lci = lcMain.AddItem("", btnChon);
            lci.Name = "cusChonPDN";
            btnChon.Click += new EventHandler(btnChon_Click);
        }

        void btnChon_Click(object sender, EventArgs e)
        {
            if (!gvMain.Editable)
            {
                XtraMessageBox.Show("Vui lòng chọn chế độ thêm hoặc sửa phiếu", Config.GetValue("PackageName").ToString());
                return;
            }
            drCur = (_data.BsMain.Current as DataRowView).Row;

            frmDS = FormFactory.FormFactory.Create(FormType.Report, "1612") as ReportPreview;
            gvDS = (frmDS.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;

            SimpleButton btnXuLy = (frmDS.Controls.Find("btnXuLy", true)[0] as SimpleButton);
            btnXuLy.Text = "Chọn phiếu";
            btnXuLy.Click += new EventHandler(btnXuLy_Click);
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
            string sophieudn = "";
            foreach (DataRow dr in drs)
            {
                gvMain.AddNewRow();
                gvMain.UpdateCurrentRow();
                gvMain.SetFocusedRowCellValue(gvMain.Columns["MTMHID"], masterId);

                gvMain.SetFocusedRowCellValue(gvMain.Columns["SoLuong"], dr["SoLuong"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["DonGia"], dr["DonGia"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["ThanhTien"], dr["ThanhTien"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["MaPX"], dr["MaPX"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["MaMay"], dr["MaMay"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["MaVT"], dr["MaVT"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["DVT"], dr["DVT"]);

                gvMain.SetFocusedRowCellValue(gvMain.Columns["GhiChu"], dr["GhiChu"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["DTDNID"], dr["DTDNID"].ToString());
                sophieudn += "," + dr["SoPhieu"];
            }
            drCur["SoPhieuDNList"] = sophieudn.Substring(1); ;
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
