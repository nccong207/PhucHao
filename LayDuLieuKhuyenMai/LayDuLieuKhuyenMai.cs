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
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Repository;

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
            gvSP = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;
            gvDH = (_data.FrmMain.Controls.Find("mDTKhuyenMaiDH", true)[0] as GridControl).MainView as GridView;
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
            (gvSP.GridControl.RepositoryItems["NhieuSP"] as RepositoryItemComboBox).CloseUp += KhuyenMaiNhieuSP_CloseUp;
            gvSP.GridControl.MouseDoubleClick += LayDuLieuKhuyenMai_MouseDoubleClick;

            GetKMNhieuSP();
            gvDH.DataSourceChanged += GvDH_DataSourceChanged;
        }

        private void KhuyenMaiNhieuSP_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            if (e.Value != null)
            {
                string spid = gvSP.GetFocusedRowCellValue("DTSPID").ToString();
                string nhieuSp = gvSP.GetFocusedRowCellValue("NhieuSP").ToString();
                DataSet dsData = _data.BsMain.DataSource as DataSet;
                drCurrent = (_data.BsMain.Current as DataRowView).Row;
                var listTable = dsData.Tables;


                DsSanPham frm = new DsSanPham();
                frm.source.DataSource = GetDataSource(spid);
                frm.StartPosition = FormStartPosition.CenterScreen;
                var result = frm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (listTable.Contains("mDTKMNhieuSP"))
                    {
                        var dt = (DataTable)frm.source.DataSource;

                        if (drCurrent.RowState == DataRowState.Added)
                        {
                            var dataRows = dsData.Tables["mDTKMNhieuSP"].Select("DTSPID = " + spid);
                            foreach (var row in dataRows)
                            {
                                dsData.Tables["mDTKMNhieuSP"].Rows.Remove(row);
                            }
                        }

                        foreach (DataRow row in dt.Rows)
                        {
                            if (row.RowState == DataRowState.Added)
                            {
                                DataRow dr = dsData.Tables["mDTKMNhieuSP"].NewRow();
                                if (row["SLTang"] != DBNull.Value && row["MaSPTang"] != DBNull.Value)
                                {
                                    dr["DTSPID"] = spid;
                                    dr["MaSPTang"] = row["MaSPTang"].ToString();
                                    dr["SLTang"] = row["SLTang"].ToString();
                                    dsData.Tables["mDTKMNhieuSP"].Rows.Add(dr);
                                }

                            }
                            else if (row.RowState == DataRowState.Modified)
                            {
                                string rowid = row["DTKMNSPID", DataRowVersion.Original].ToString();
                                var curRow = dsData.Tables["mDTKMNhieuSP"].Select(string.Format("DTKMNSPID = {0}", rowid));
                                if (curRow.Length > 0)
                                {
                                    curRow[0]["MaSPTang"] = row["MaSPTang"].ToString();
                                    curRow[0]["SLTang"] = row["SLTang"].ToString();
                                }
                            }
                            else if (row.RowState == DataRowState.Deleted)
                            {
                                string rowid = row["DTKMNSPID", DataRowVersion.Original].ToString();
                                var curRow = dsData.Tables["mDTKMNhieuSP"].Select(string.Format("DTKMNSPID = {0}", rowid));
                                if (curRow.Length > 0)
                                {
                                    curRow[0].Delete();
                                }
                            }
                        }

                        var tb = dsData.Tables["mDTKMNhieuSP"];
                    }
                    else
                    {
                        var dt = (DataTable)frm.source.DataSource;
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row.RowState != DataRowState.Deleted)
                            {
                                row["DTSPID"] = spid;
                            }
                        }

                        dt.TableName = "mDTKMNhieuSP";
                        dsData.Tables.Add(dt);
                    }
                }
            }
            else
            {
                DataSet dsData = _data.BsMain.DataSource as DataSet;
                var listTable = dsData.Tables;
                string spid = gvSP.GetFocusedRowCellValue("DTSPID").ToString();
                if (listTable.Contains("mDTKMNhieuSP"))
                {
                    var dataRows = dsData.Tables["mDTKMNhieuSP"].Select("DTSPID = " + spid);
                    foreach (var row in dataRows)
                    {
                        row.Delete();
                    }
                }
            }
        }

        private void LayDuLieuKhuyenMai_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridControl grid = sender as GridControl;
            ColumnView view = grid.FocusedView as ColumnView;
            GridHitInfo hi = view.CalcHitInfo(e.Location) as GridHitInfo;
            if (hi.HitTest == GridHitTest.RowCell)
            {
                string spid = gvSP.GetFocusedRowCellValue("DTSPID").ToString();
                DsSanPham frm = new DsSanPham(!gvDH.Editable);
                frm.source.DataSource = GetDataSource(spid);
                frm.StartPosition = FormStartPosition.CenterScreen;
                var result = frm.ShowDialog();
            } 
            return;
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
            if (drCurrent.RowState != DataRowState.Added)
            {
                GetDataForGrid(mtid);
            }
        }
        private DataTable GetDataSource (string spid)
        {
            DataSet dsData = _data.BsMain.DataSource as DataSet;
            var listTable = dsData.Tables;
            DataTable data = new DataTable();
            data.Clear();
            data.Columns.Add("DTKMNSPID");
            data.Columns.Add("DTSPID");
            data.Columns.Add("MaSPTang");
            data.Columns.Add("SLTang");

            if (listTable.Contains("mDTKMNhieuSP"))
            {
                // chi lay nhung row lien quan
                var rows = dsData.Tables["mDTKMNhieuSP"].Select("DTSPID = " + spid);
                foreach (DataRow row in rows)
                {
                    data.ImportRow(row);
                }
            }
            else
            {
                //Get all khuyen mai in DB
                string mtid = drCurrent["MTID"].ToString();
                if (drCurrent.RowState != DataRowState.Added)
                {
                    GetDataForGrid(mtid);

                    var rows = dsData.Tables["mDTKMNhieuSP"].Select("DTSPID = " + spid);
                    foreach (DataRow row in rows)
                    {
                        data.ImportRow(row);
                    }
                }
            }

            return data;
        }

        private void GetDataForGrid(string mtid)
        {
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
            tcMain.SelectedTabPageIndex = 0;
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

            DataTable dtSP = (_data.BsMain.DataSource as DataSet).Tables["mDTKhuyenMaiSP"];
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

            DataTable dtKV = (_data.BsMain.DataSource as DataSet).Tables["mDTKhuyenMaiKV"];
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
