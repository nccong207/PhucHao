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

namespace LayDSGiaHan
{
    public class LayDSGiaHan : ICControl
    {
        GridView gvMain;
        DataRow drCur;
        ReportPreview frmDS;
        GridView gvDS;
        DataCustomFormControl _data;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);

        public void AddEvent()
        {
            gvMain = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;
            //thêm nút chọn DH
            LayoutControl lcMain = _data.FrmMain.Controls.Find("lcMain", true)[0] as LayoutControl;
            SimpleButton btnChon = new SimpleButton();
            btnChon.Name = "btnChon";
            btnChon.Text = "Chọn đơn hàng quá hạn";
            LayoutControlItem lci = lcMain.AddItem("", btnChon);
            lci.Name = "cusChon";
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

            frmDS = FormFactory.FormFactory.Create(FormType.Report, "1614") as ReportPreview;
            gvDS = (frmDS.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;
   
            //viết xử lý cho nút F4-Xử lý trong report
            SimpleButton btnXuLy = (frmDS.Controls.Find("btnXuLy", true)[0] as SimpleButton);
            btnXuLy.Text = "Chọn đơn hàng";
            btnXuLy.Click += new EventHandler(btnXuLy_Click);
            frmDS.WindowState = FormWindowState.Maximized;
            frmDS.ShowDialog();
        }

        void btnXuLy_Click(object sender, EventArgs e)
        {
            DataTable dtDS = (gvDS.DataSource as DataView).Table;
            dtDS.AcceptChanges();
          
            DataRow[] drs = dtDS.Select("Chọn = 1");
         
            if (drs.Length == 0)
            {
                XtraMessageBox.Show("Bạn chưa chọn đơn hàng để gia hạn", Config.GetValue("PackageName").ToString());
                return;
            }
            frmDS.Close();

            //add du lieu vao danh sach
            
            foreach (DataRow dr in drs)
            {
                gvMain.AddNewRow();
                gvMain.UpdateCurrentRow();
                gvMain.SetFocusedRowCellValue(gvMain.Columns["DTDHID"], dr["DTDHID"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["SoDH"], dr["SoDH"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["MaKH"], dr["MaKH"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["TenHang"], dr["tenhh"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["LoaiDH"], dr["Loai"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["Dai"], dr["Dai"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["Rong"], dr["Rong"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["Cao"], dr["Cao"]);
            }

            gvMain.RefreshData();
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
