using CDTDatabase;
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
using System.Linq;

namespace LayNKSX
{
    public class LayNKSX : ICControl
    {
        GridView gvMain;
        DataRow drCur;
        GridView gvDS;
        DataCustomFormControl _data;
        Database db;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);

        public void AddEvent()
        {
            db = Database.NewDataDatabase();
            gvMain = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;
            //thêm nút chọn DH
            LayoutControl lcMain = _data.FrmMain.Controls.Find("lcMain", true)[0] as LayoutControl;
            SimpleButton btnChon = new SimpleButton();
            btnChon.Name = "btnChon";
            btnChon.Text = "Lấy nhật ký sản xuất";
            LayoutControlItem lci = lcMain.AddItem("", btnChon);
            lci.Name = "cusChon";
            btnChon.Click += new EventHandler(btnChon_Click);
            SimpleButton btnChon2 = new SimpleButton();
            btnChon2.Name = "btnChon2";
            btnChon2.Text = "Xóa lấy lại";
            LayoutControlItem lci2 = lcMain.AddItem("", btnChon2);
            lci2.Name = "cusChon2";
            btnChon2.Click += new EventHandler(btnChon2_Click);
            _data.FrmMain.Load += new System.EventHandler(FormLoad);

            _data.BsMain.DataSourceChanged += new EventHandler(BsMain_DataSourceChanged);
            BsMain_DataSourceChanged(_data.BsMain, new EventArgs());
            gvMain.DoubleClick += new EventHandler(gvMain_DoubleClick);

        }
        void gvMain_DoubleClick(object sender, EventArgs e)
        {
           // GridView gvMain = sender as GridView;
            //frm1 f1 = new frm1();
           // f1.ShowDialog();
           // gvMain.SetFocusedRowCellValue(gvMain.Columns["ID"], f1.may.ToString());
           // frm.frm fx = new frm.frm();
           
        }
        void FormLoad(object sender, EventArgs e)
        {
            _data.FrmMain.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }
        void CellClick(object sender, EventArgs e)
        {
            _data.FrmMain.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }
        void BsMain_DataSourceChanged(object sender, EventArgs e)
        {
           // if (_data.BsMain == null)
             //   return;
            //DataSet ds = _data.BsMain.DataSource as DataSet;
        //    ds.Tables[1].ColumnChanged += new DataColumnChangeEventHandler(TinhTienPhat_ColumnChanged);
           
        }
        void btnChon2_Click(object sender, EventArgs e)
        {
            if (!gvMain.Editable)
            {
            XtraMessageBox.Show("Vui lòng chọn chế độ thêm hoặc sửa phiếu", Config.GetValue("PackageName").ToString());
                return;
            }
            for (int i = 0; i < gvMain.RowCount; i++) {
                gvMain.SelectAll();
                gvMain.DeleteSelectedRows();
                gvMain.RefreshData();
            }
               
        }

        void btnChon_Click(object sender, EventArgs e)
        {
            if (!gvMain.Editable)
            {
                XtraMessageBox.Show("Vui lòng chọn chế độ thêm hoặc sửa phiếu", Config.GetValue("PackageName").ToString());
                return;
            }
            drCur = (_data.BsMain.Current as DataRowView).Row;
            if (Int32.Parse(drCur["Vang"].ToString()) > 0)
            {
                if ((drCur["NguoiVang"].ToString().Length) == 0)
                {
                    XtraMessageBox.Show("Vui lòng nhập thông tin người vắng.", Config.GetValue("PackageName").ToString());

                    return;
                }

            }
          //  frmDS = FormFactory.FormFactory.Create(FormType.Report, "1614") as ReportPreview;
          //  gvDS = (frmDS.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;

            //viết xử lý cho nút F4-Xử lý trong report
            //SimpleButton btnXuLy = (frmDS.Controls.Find("btnXuLy", true)[0] as SimpleButton);
           // btnXuLy.Text = "Chọn đơn hàng";
           // btnXuLy.Click += new EventHandler(btnXuLy_Click);
           // frmDS.WindowState = FormWindowState.Maximized;
           // frmDS.ShowDialog();
            string time1 = drCur["GioBatDau"].ToString();
            string time2 = drCur["GioKetThuc"].ToString();
            string msx = drCur["MaySX"].ToString();
            string casx = drCur["CaSX_PM"].ToString();
            string sql = string.Format("SELECT  ID, OrderNumber as SoLSX, CustomerCode as MaKH, Material as KyHieu, CuttingQTY as SLPO, FinishJobQTY as SLMay, BadPaperQTY as SLHu1, AdjustQTY as SLHu2, TotalQTY as SLTP ,StartTime, AverageSpeed as [AVG] , dt.Dai as ChDai, dt.Rong as ChRong FROM [HTCPH].[dbo].[POResultLog] t INNER JOIN [HTCPH].[dbo].[DTLSX] dt ON t.OrderNumber = dt.SoLSX where UpdatedDate between '{1}' and '{2}' and ProductionLineCode = '{0}' and Shift = '{3}'  and Posted = 1 order by StartTime  ", msx, time1, time2, casx); //sql lay nhat ky san xuat   
            DataTable dtDSDH = db.GetDataTable(sql);
            DataRow[] drs = dtDSDH.Select();
            //add du lieu vao danh sach
            gvMain.SelectAll();
            gvMain.DeleteSelectedRows();
            decimal avgx = 0;
            int avgk = 0;
            List<string> chKho = new List<string>();

            foreach (DataRow dr in drs)
            {
                gvMain.AddNewRow();
                gvMain.UpdateCurrentRow();

                avgx = avgx + Decimal.Parse(dr["AVG"].ToString());
                avgk = avgk + 1;
                gvMain.SetFocusedRowCellValue(gvMain.Columns["SoLSX"], dr["SoLSX"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["MaKH"], dr["MaKH"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["KyHieu"], dr["KyHieu"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["SLPO"], dr["SLPO"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["SLMay"], dr["SLMay"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["SLHu1"], dr["SLHu1"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["SLHu2"], dr["SLHu2"]);
                if (Int32.Parse(dr["SLHu2"].ToString()) > 0)
                {

                    gvMain.SetFocusedRowCellValue(gvMain.Columns["TongHH"], dr["SLHu1"]);
                }
                else {
                    int x = Int32.Parse(dr["SLHu1"].ToString());
                    int x1 = Int32.Parse(dr["SLHu2"].ToString());
                    int xtt = x + (-1 * x1); 
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["TongHH"], xtt);
                }
                gvMain.SetFocusedRowCellValue(gvMain.Columns["SLTP"], dr["SLTP"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["StartTime"], dr["StartTime"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["AVG"], dr["AVG"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["ChKho"], dr["ChRong"]);
                chKho.Add(String.Format("{0:#####}", dr["ChRong"])); 
                gvMain.SetFocusedRowCellValue(gvMain.Columns["ChDai"], dr["ChDai"]);
                gvMain.SetFocusedRowCellValue(gvMain.Columns["ID2"], dr["ID2"]);
            }

            gvMain.RefreshData();
            decimal avgy = avgx / avgk;
            drCur["SXAVG"] = avgy;
            string khochay = "";
           
            foreach (string kho in chKho.Distinct()) {
                khochay = khochay + kho + ";";
            }
           drCur["CacKhoChay"] = khochay.ToString();
            
            
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
