using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors;
using FormFactory;
using System.Data;
using DevExpress.XtraGrid.Views.Grid;
using CDTLib;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraTab;
using CDTDatabase;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace LayDuLieuKhuyenMai
{
    public class LayDuLieuKhuyenMai : ICControl
    {
        XtraTabControl tcMain;
        DataRow drCurrent;
        GridView gvDH, gvSP, gvKV;
        CDTForm frmDH, frmSP, frmKV;
        DataCustomFormControl _data;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        Database db = Database.NewDataDatabase();
        #region ICControl Members

        public void AddEvent()
        {
            tcMain = _data.FrmMain.Controls.Find("tcMain", true)[0] as XtraTabControl;
            gvDH = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;
            gvSP = (_data.FrmMain.Controls.Find("mDTKhuyenMaiSP", true)[0] as GridControl).MainView as GridView;
            gvKV = (_data.FrmMain.Controls.Find("mDTKhuyenMaiKV", true)[0] as GridControl).MainView as GridView;
            LayoutControl lcMain = _data.FrmMain.Controls.Find("lcMain", true)[0] as LayoutControl;
            SimpleButton btnSP = new SimpleButton();
            btnSP.Name = "btnSP";
            btnSP.Text = "Chọn sản phẩm";
            LayoutControlItem lci1 = lcMain.AddItem("", btnSP);
            lci1.Name = "cusSP";
            btnSP.Click += new EventHandler(btnSP_Click);

            SimpleButton btnKV = new SimpleButton();
            btnKV.Name = "btnKV";
            btnKV.Text = "Chọn khu vực";
            LayoutControlItem lci2 = lcMain.AddItem("", btnKV);
            lci2.Name = "cusKV";
            btnKV.Click += new EventHandler(btnKV_Click);

            DataSet dsData = _data.BsMain.DataSource as DataSet;
            gvSP.CellValueChanged += GvSP_CellValueChanged;
            gvSP.DoubleClick += GridControl_DoubleClick;

            GetKMNhieuSP();
            gvDH.DataSourceChanged += GvDH_DataSourceChanged;
        }

        private void GridControl_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = view.CalcHitInfo(pt);
            if (!info.InRow && !info.InRowCell)
                return;
            if (!view.IsDataRow(view.FocusedRowHandle))
                return;

            //GridView view = (GridView)sender;
            //Point pt = view.GridControl.PointToClient(Control.MousePosition);
            DoRowDoubleClick(view, pt);
        }

        private static void DoRowDoubleClick(GridView view, Point pt)
        {
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRow || info.InRowCell)
            {
                string colCaption = info.Column == null ? "N/A" : info.Column.Caption;
                MessageBox.Show(string.Format("DoubleClick on row: {0}, column: {1}.", info.RowHandle, colCaption));
            }
        }

        private void GvDH_DataSourceChanged(object sender, EventArgs e)
        {
            GetKMNhieuSP();
        }

        private void GetKMNhieuSP()
        {
            //Get all khuyen mai in DB
            drCurrent = (_data.BsMain.Current as DataRowView).Row;
            string mtid = drCurrent["MTID"].ToString();
            string sql = @"SELECT km.* FROM mDTKMNhieuSP km
                        JOIN mDTKhuyenMaiSP sp ON km.DTSPID = sp.DTSPID
                        JOIN mMTKhuyenMai mt ON sp.MTID = mt.MTID
                        WHERE mt.MTID = '{0}'";
            var data = db.GetDataTable(string.Format(sql, mtid));

            DataSet dsData = _data.BsMain.DataSource as DataSet;
            var listTable = dsData.Tables;

            // remove if exist
            if (listTable.Contains("mDTKMNhieuSP"))
            {
                var tb = dsData.Tables["mDTKMNhieuSP"];
                if (dsData.Tables.CanRemove(tb))
                    dsData.Tables.Remove(tb);
            }

            data.TableName = "mDTKMNhieuSP";
            dsData.Tables.Add(data);
        }

        private void GvSP_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "NhieuSP" && e.Value != DBNull.Value)
            {
                string spid = gvSP.GetFocusedRowCellValue("DTSPID").ToString();
                DataSet dsData = _data.BsMain.DataSource as DataSet;
                var listTable = dsData.Tables;
                DataTable data = new DataTable();
                if (listTable.Contains("mDTKMNhieuSP"))
                {
                    data.Clear();
                    data.Columns.Add("DTSPID");
                    data.Columns.Add("MaSPTang");
                    data.Columns.Add("SLTang");
                    // chi lay nhung row lien quan
                    var rows = dsData.Tables["mDTKMNhieuSP"].Select("DTSPID = " + spid);
                    foreach (DataRow row in rows)
                    {
                        data.ImportRow(row);
                    }
                }
                else
                {
                    string query = string.Format(@"SELECT DTSPID, MaSPTang, SLTang  FROM mDTKMNhieuSP
                                                WHERE DTSPID = {0}", spid);
                    data = db.GetDataTable(query);
                }

                DsSanPham frm = new DsSanPham();
                frm.source.DataSource = data;
                frm.StartPosition = FormStartPosition.CenterScreen;
                var result = frm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (listTable.Contains("mDTKMNhieuSP"))
                    {
                        // xoa nhung row cu.
                        var rows = dsData.Tables["mDTKMNhieuSP"].Select("DTSPID = " + spid);
                        foreach (var row in rows)
                        {
                            dsData.Tables["mDTKMNhieuSP"].Rows.Remove(row);
                        }

                        // them nhung row moi dc update tu form
                        var dt = (DataTable)frm.source.DataSource;
                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dsData.Tables["mDTKMNhieuSP"].NewRow();
                            dr["DTSPID"] = spid;
                            dr["MaSPTang"] = row["MaSPTang"].ToString();
                            dr["SLTang"] = row["SLTang"].ToString();
                            dsData.Tables["mDTKMNhieuSP"].Rows.Add(dr);
                        }
                    }
                    else
                    {
                        var dt = (DataTable)frm.source.DataSource;
                        foreach (DataRow row in dt.Rows)
                        {
                            row["DTSPID"] = spid;
                        }

                        dt.TableName = "mDTKMNhieuSP";
                        dsData.Tables.Add(dt);
                    }

                    //dsData.Tables.Add((DataTable) frm.source.DataSource);
                }
            }
        }

        void btnKV_Click(object sender, EventArgs e)
        {
            if (!gvSP.Editable)
            {
                XtraMessageBox.Show("Vui lòng chọn chế độ thêm hoặc sửa phiếu",
                    Config.GetValue("PackageName").ToString());
                return;
            }
            tcMain.SelectedTabPageIndex = 2;
            drCurrent = (_data.BsMain.Current as DataRowView).Row;
            frmKV = FormFactory.FormFactory.Create(FormType.Report, "1585") as ReportPreview;
            //viết xử lý cho nút F4-Xử lý trong report
            SimpleButton btnXuLy3 = (frmKV.Controls.Find("btnXuLy", true)[0] as SimpleButton);
            btnXuLy3.Text = "Chọn khu vực";
            btnXuLy3.Click += new EventHandler(btnXuLyKV_Click);
            frmKV.WindowState = FormWindowState.Maximized;
            frmKV.ShowDialog();
        }

        void btnSP_Click(object sender, EventArgs e)
        {
            if (!gvSP.Editable)
            {
                XtraMessageBox.Show("Vui lòng chọn chế độ thêm hoặc sửa phiếu",
                    Config.GetValue("PackageName").ToString());
                return;
            }
            tcMain.SelectedTabPageIndex = 1;
            drCurrent = (_data.BsMain.Current as DataRowView).Row;
            frmSP = FormFactory.FormFactory.Create(FormType.Report, "1583") as ReportPreview;
            //viết xử lý cho nút F4-Xử lý trong report
            SimpleButton btnXuLy1 = (frmSP.Controls.Find("btnXuLy", true)[0] as SimpleButton);
            btnXuLy1.Text = "Chọn sản phẩm";
            btnXuLy1.Click += new EventHandler(btnXuLySP_Click);
            frmSP.WindowState = FormWindowState.Maximized;
            frmSP.ShowDialog();
        }

        void btnXuLySP_Click(object sender, EventArgs e)
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

            DataTable dtSP = (_data.BsMain.DataSource as DataSet).Tables[2];
            string filter = drCurrent["MTID"].Equals(DBNull.Value) ? "MTID is null and MaSPBan = '{0}'" :
                "MTID = " + drCurrent["MTID"].ToString() + " and MaSPBan = '{0}'";
            foreach (DataRow dr in drs)
            {
                if (dtSP.Select(string.Format(filter, dr["MaSP"])).Length > 0)
                    continue;
                gvSP.AddNewRow();
                gvSP.SetFocusedRowCellValue(gvSP.Columns["MaSPBan"], dr["MaSP"]);
                gvSP.UpdateCurrentRow();
            }
        }

        void btnXuLyKV_Click(object sender, EventArgs e)
        {
            GridView gvDS = (frmKV.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;
            DataTable dtDS = (gvDS.DataSource as DataView).Table;
            dtDS.AcceptChanges();
            DataRow[] drs = dtDS.Select("Chọn = 1");
            if (drs.Length == 0)
            {
                XtraMessageBox.Show("Bạn chưa chọn khu vực", Config.GetValue("PackageName").ToString());
                return;
            }
            frmKV.Close();

            DataTable dtKV = (_data.BsMain.DataSource as DataSet).Tables[3];
            string filter = drCurrent["MTID"].Equals(DBNull.Value) ? "MTID is null and KhuVuc = {0}" :
                "MTID = " + drCurrent["MTID"].ToString() + " and KhuVuc = {0}";
            foreach (DataRow dr in drs)
            {
                if (dtKV.Select(string.Format(filter, dr["ID"])).Length > 0)
                    continue;
                gvKV.AddNewRow();
                gvKV.SetFocusedRowCellValue(gvKV.Columns["KhuVuc"], dr["ID"]);
                gvKV.UpdateCurrentRow();
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
