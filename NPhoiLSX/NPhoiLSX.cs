using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using System.Data;
using CDTLib;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors.Repository;
using System.Drawing;
using DevExpress.XtraGrid.Columns;
using FormFactory;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
namespace NPhoiLSX
{
    public class NPhoiLSX : ICControl
    {
        DataRow drCur;
        ReportPreview frmDS;
        GridView gvDS;
        GridView gvMain;
        DataCustomFormControl _data;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);

        #region ICControl Members

        public void AddEvent()
        {
            //thêm nút chọn LSX
            gvMain = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;
            LayoutControl lcMain = _data.FrmMain.Controls.Find("lcMain", true)[0] as LayoutControl;
            SimpleButton btnChon = new SimpleButton();
            btnChon.Name = "btnChon";
            btnChon.Text = "Chọn LSX";
            LayoutControlItem lci = lcMain.AddItem("", btnChon);
            lci.Name = "cusChon";
            btnChon.Click += new EventHandler(btnChon_Click);
        }

        void btnChon_Click(object sender, EventArgs e)
        {
            drCur = (_data.BsMain.Current as DataRowView).Row;
            if (!gvMain.Editable)
            {
                XtraMessageBox.Show("Vui lòng chọn chế độ thêm hoặc sửa phiếu",
                    Config.GetValue("PackageName").ToString());
                return;
            }
            
            frmDS = FormFactory.FormFactory.Create(FormType.Report, "1541") as ReportPreview;
            gvDS = (frmDS.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;

            //viết xử lý cho nút F4-Xử lý trong report
            SimpleButton btnXuLy = (frmDS.Controls.Find("btnXuLy", true)[0] as SimpleButton);
            btnXuLy.Text = "f4 - Xử lý";
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
                XtraMessageBox.Show("Bạn chưa chọn lệnh sản xuất", Config.GetValue("PackageName").ToString());
                return;
            }
            frmDS.Close();
            DataTable dtDTNPhoi = (_data.BsMain.DataSource as DataSet).Tables[1];
            using (DataTable tmp = dtDTNPhoi.Clone())
            {
                foreach (DataRow dr in drs)
                {
                    if (dtDTNPhoi.Select(string.Format("MTID = '{0}' and DTDHID = '{1}' AND SoLSX='{2}'", drCur["MTID"], dr["DTDHID"],dr["SoLSX"])).Length > 0)
                        continue;

                    gvMain.AddNewRow();
                    gvMain.UpdateCurrentRow();
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["DTDHID"], dr["DTDHID"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["MaHH"],dr["mahh"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["MTID"], drCur["MTID"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["DTID"], Guid.NewGuid());
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["SoDH"], dr["Số đơn hàng"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["NgayDH"], dr["Ngày"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["SoLSX"], dr["SoLSX"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["MaKH"], dr["MaKH"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["TenHang"], dr["TenHang"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["Loai"], dr["Loai"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["Lop"], dr["Lop"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["Dai"], dr["Dai"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["Dao"], dr["Dao"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["Rong"], dr["Rong"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["Cao"], dr["Cao"]);
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["SLPO"], dr["Số lượng PO"]);

                    //tmp.ImportRow(dr);
                    //tmp.Rows[tmp.Rows.Count - 1]["SoDH"] = dr["Số đơn hàng"];
                    //tmp.Rows[tmp.Rows.Count - 1]["NgayDH"] = dr["Ngày"];
                    //tmp.Rows[tmp.Rows.Count - 1]["SoLuong"] = dr["SL chưa nhập"];
                }
 
                //foreach (DataRow dr in tmp.Rows)
                //{
                //    if (dtDTNPhoi.Select(string.Format("MTID = '{0}' and DTID = '{1}'", drCur["MTID"], dr["DTID"])).Length > 0)
                //        continue;


                //    //dr["MTID"] = drCur["MTID"];
                //    //dr["DTID"] = Guid.NewGuid();

                //    //DataRow drNew = dtDTNPhoi.NewRow();
                //    //drNew.ItemArray = (object[])dr.ItemArray.Clone();
                //    //dtDTNPhoi.Rows.Add(drNew);
                    
                //}
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

        #endregion
    }
}