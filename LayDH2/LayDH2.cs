using System;
using Plugins;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using System.Data;
using CDTLib;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using FormFactory;
using System.Windows.Forms;
using CDTDatabase;
using DevExpress.XtraLayout.Utils;
using System.Collections.Generic;

namespace LayDH2
{
    public class LayDH2 : ICControl
    {
        GridView gvMain;
        DataRow drCur;
        ReportPreview frmDS;
        GridView gvDS;
        DateEdit deNgayCT;
        TextEdit teSoXe;
        TextEdit teKmDau;
        LayoutControl lcMain;
        GridLookUpEdit gluLoai;
        DataCustomFormControl _data;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        Database db = Database.NewDataDatabase();
        #region ICControl Members

        public void AddEvent()
        {
            gvMain = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;
            gvMain.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gvMain_CellValueChanged);
            deNgayCT = _data.FrmMain.Controls.Find("NgayCT", true)[0] as DateEdit;
            teSoXe = _data.FrmMain.Controls.Find("SoXe", true)[0] as TextEdit;
            teKmDau = _data.FrmMain.Controls.Find("KmDau", true)[0] as TextEdit;
            lcMain = _data.FrmMain.Controls.Find("lcMain", true)[0] as LayoutControl;
            gluLoai = _data.FrmMain.Controls.Find("Loai", true)[0] as GridLookUpEdit;

            teSoXe.Enter += new EventHandler(teSoXe_Enter);
            //thêm nút chọn DH từ hàng tồn
            SimpleButton btnChon = new SimpleButton();
            btnChon.Name = "btnChon";
            btnChon.Text = "Chọn hàng";
            LayoutControlItem lci = lcMain.AddItem("", btnChon);
            lci.Name = "cusChon";
            btnChon.Click += new EventHandler(btnChon_Click);

            //thêm nút chọn DH từ thành phẩm
            SimpleButton btnChon2 = new SimpleButton();
            btnChon2.Name = "btnChon2";
            btnChon2.Text = "Chọn phôi sóng";
            LayoutControlItem lci2 = lcMain.AddItem("", btnChon2);
            lci2.Name = "cusChon2";
            btnChon2.Click += new EventHandler(btnChon_Click);

            //thêm xem hàng đã chọn
            SimpleButton btnChon3 = new SimpleButton();
            btnChon3.Name = "btnChon3";
            btnChon3.Text = "Xem đơn hàng";
            LayoutControlItem lci3 = lcMain.AddItem("", btnChon3);
            lci3.Name = "cusChon3";
            btnChon3.Click += new EventHandler(btnChon_Click2);

            //Cho phép sửa đơn giá khi xuất hàng đặc biệt
            gvMain.ShownEditor += new EventHandler(gvMain_ShownEditor);
            _data.FrmMain.Shown += new EventHandler(FrmMain_Shown);
        }

        void FrmMain_Shown(object sender, EventArgs e)
        {
            string isAdmin = Config.GetValue("Admin").ToString();
            string menuid = _data.DrTable["sysMenuID"].ToString();
            
            //Kiem tra quyen admin
            if (!Boolean.Parse(isAdmin) && menuid.Equals("9507")) //9507 id of menu phieu ban hang
            {
                string serverTime = Config.GetValue("ServerTime").ToString();

                string timeStart = serverTime.Split('-')[0].Trim();
                string timeEnd = serverTime.Split('-')[1].Trim();

                Database db = Database.NewDataDatabase();
                string timesql = "SELECT CONVERT (time, SYSDATETIME()) as TIME";
                DataTable dttime = db.GetDataTable(timesql);
                TimeSpan now;
                if (dttime.Rows.Count > 0)
                {
                    now = TimeSpan.Parse(dttime.Rows[0]["TIME"].ToString());
                }
                else
                {
                    now = DateTime.Now.TimeOfDay;
                }

                TimeSpan tStart, tEnd;
                TimeSpan.TryParse(timeStart, out tStart);
                TimeSpan.TryParse(timeEnd, out tEnd);

                //Khong hien thi DonGia và ThanhTien khi tao phieu ngoai gio he thong.
                if (now < tStart || now > tEnd)
                {
                    GridView gvDetail = (_data.FrmMain.Controls.Find("gcMain", true)[0] as GridControl).MainView as GridView;
                    gvDetail.Columns["DonGia"].Visible = false;
                    gvDetail.Columns["DonGia"].OptionsColumn.ShowInCustomizationForm = false;
                    gvDetail.Columns["ThanhTien"].Visible = false;
                    gvDetail.Columns["ThanhTien"].OptionsColumn.ShowInCustomizationForm = false;

                    LayoutControlItem lci = lcMain.Items.FindByName("lciTongCong") as LayoutControlItem;
                    if (lci != null) lci.Visibility = LayoutVisibility.Never;
                }
            }
        }

        void teSoXe_Enter(object sender, EventArgs e)
        {
            switch (gluLoai.Text)
            {
                case "":
                    XtraMessageBox.Show("Vui lòng chọn loại xe trước!", Config.GetValue("PackageName").ToString());
                    gluLoai.Focus();
                    break;
                case "Phúc Hảo":
                case "Xe thuê":
                    teKmDau.Focus();
                    break;
            }
        }

        void gvMain_ShownEditor(object sender, EventArgs e)
        {
            
            if (deNgayCT.Properties.ReadOnly == true)
                return;
            //Bị nhảy dòng khi nhấn tab
            if (gvMain.FocusedRowHandle < 0)
            {
                int rowHandle = gvMain.RowCount;
                GridColumn colFF = gvMain.FocusedColumn;
                gvMain.AddNewRow();
                gvMain.UpdateCurrentRow();     
            }
            //
            foreach (GridColumn col in gvMain.Columns)
            {
                if (col.FieldName == "DonGia")
                {
                    object obj = gvMain.GetFocusedRowCellValue("SoDH");
                    if (obj == DBNull.Value)
                    {
                        //col.OptionsColumn.AllowEdit = true;
                    } else if (Boolean.Parse(gvMain.GetFocusedRowCellValue("isPS").ToString()))
                    {
                        col.OptionsColumn.AllowEdit = true;
                    }
                    else
                    {
                        //col.OptionsColumn.AllowEdit = false;
                        GridColumn colF = gvMain.FocusedColumn;
                        if(colF.FieldName == col.FieldName)
                            gvMain.FocusedColumn = gvMain.Columns.ColumnByFieldName("ThanhTien");
                    }
                }
            }
            
        }

        private void TinhSLQD()  //Công thêm ngày 25/5/2015 theo yêu cầu tính slqđ m2
        {
            object loai = gvMain.GetFocusedRowCellValue("Loai");
            if (loai == null || loai == DBNull.Value)
                return;

            object dtdhid = gvMain.GetFocusedRowCellValue("DTDHID");

            object osl = gvMain.GetFocusedRowCellValue("SoLuong");
            decimal sl = (osl == null || osl.ToString() == "") ? 0 : decimal.Parse(osl.ToString());
            object od = gvMain.GetFocusedRowCellValue("Dai");
            object or = gvMain.GetFocusedRowCellValue("Rong");
            object oc = gvMain.GetFocusedRowCellValue("Cao");
            decimal c = (oc == null || oc.ToString() == "") ? 0 : decimal.Parse(oc.ToString());
            decimal d = (od == null || od.ToString() == "") ? 0 : decimal.Parse(od.ToString());
            decimal r = (or == null || or.ToString() == "") ? 0 : decimal.Parse(or.ToString());
            object ol = gvMain.GetFocusedRowCellValue("Lop");
            int l = (ol == null || ol == DBNull.Value) ? 0 : Convert.ToInt32(ol);
            decimal qd;

            if (dtdhid != DBNull.Value)
            {
                string sql = string.Format("SELECT KCT, DienTich FROM DTDonHang WHERE DTDHID = '{0}'", dtdhid.ToString());
                DataTable dtDH = db.GetDataTable(sql);
                bool kct = Convert.ToBoolean(dtDH.Rows[0]["KCT"]);
                if (kct)
                {
                    object odt = dtDH.Rows[0]["DienTich"];
                    decimal dt = (odt == null || odt.ToString() == "") ? 0 : decimal.Parse(odt.ToString());
                    qd = dt * sl;
                }
                else
                {
                    qd = TinhQuyDoi(loai, d, r, c, sl, l);
                }
            }
            else
            {
                qd = TinhQuyDoi(loai, d, r, c, sl, l);
            }
            gvMain.SetFocusedRowCellValue(gvMain.Columns["QuyDoi"], Math.Round(qd, 0, MidpointRounding.AwayFromZero));
        }

        private decimal TinhQuyDoi(object loai, decimal d, decimal r, decimal c, decimal sl, int l)
        {
            decimal qd;
            if (loai != null && loai.ToString() == "Tấm")
                qd = d * r * sl / 10000;
            else
            {
                int t1 = (l == 3) ? 2 : 3;
                int t2 = (l == 3) ? 5 : 6;
                qd = (c + r + t1) * ((d + r) * 2 + t2) * sl / 10000;
            }
            return qd;
        }

        void gvMain_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            drCur = (_data.BsMain.Current as DataRowView).Row;
            if (e.Column.FieldName == "SoLuong")
                TinhSLQD();
            if (Boolean.Parse(drCur["XK"].ToString()))
            {
                //if (e.Column.FieldName == "SLTL" || e.Column.FieldName == "DGTL")
                if (e.Column.FieldName == "SoLuong" || e.Column.FieldName == "DonGia")
                {
                    object osl = gvMain.GetFocusedRowCellValue("SoLuong");
                    object odg = gvMain.GetFocusedRowCellValue("DonGia");
                    decimal sl = (osl == null || osl.ToString() == "") ? 0 : decimal.Parse(osl.ToString());
                    decimal dg = (odg == null || odg.ToString() == "") ? 0 : decimal.Parse(odg.ToString());
                    object l = gvMain.GetFocusedRowCellValue("Loai");
                    if (l != null && l.ToString() == "Tấm")
                    {
                        object od = gvMain.GetFocusedRowCellValue("Dai");
                        object or = gvMain.GetFocusedRowCellValue("Rong");
                        decimal d = (od == null || od.ToString() == "") ? 0 : decimal.Parse(od.ToString());
                        decimal r = (or == null || or.ToString() == "") ? 0 : decimal.Parse(or.ToString());
                        decimal qd = Math.Round((sl * d * r / 10000), 0, MidpointRounding.AwayFromZero);
                        decimal tt = qd * Math.Round(dg, 0, MidpointRounding.AwayFromZero);
                        gvMain.SetFocusedRowCellValue(gvMain.Columns["ThanhTien"], tt - (tt % 10));
                    }
                    else
                        gvMain.SetFocusedRowCellValue(gvMain.Columns["ThanhTien"], sl * dg);
                }
                if (e.Column.FieldName == "SLTL" || e.Column.FieldName == "DGTL")
                {
                    object osl = gvMain.GetFocusedRowCellValue("SLTL");
                    object odg = gvMain.GetFocusedRowCellValue("DGTL");
                    decimal sl = (osl == null || osl.ToString() == "") ? 0 : decimal.Parse(osl.ToString());
                    decimal dg = (odg == null || odg.ToString() == "") ? 0 : decimal.Parse(odg.ToString());
                    gvMain.SetFocusedRowCellValue(gvMain.Columns["TTTL"], sl * dg);
                }
            }
            else
            {
                if (e.Column.FieldName == "SoLuong" || e.Column.FieldName == "DonGia")
                {
                    object osl = gvMain.GetFocusedRowCellValue("SoLuong");
                    object odg = gvMain.GetFocusedRowCellValue("DonGia");
                    object od = gvMain.GetFocusedRowCellValue("Dai");
                    object or = gvMain.GetFocusedRowCellValue("Rong");
                    decimal sl = (osl == null || osl.ToString() == "") ? 0 : decimal.Parse(osl.ToString());
                    decimal dg = (odg == null || odg.ToString() == "") ? 0 : decimal.Parse(odg.ToString());
                    decimal d = (od == null || od.ToString() == "") ? 0 : decimal.Parse(od.ToString());
                    decimal r = (or == null || or.ToString() == "") ? 0 : decimal.Parse(or.ToString());
                    object l = gvMain.GetFocusedRowCellValue("Loai");
                    if (l != null && l.ToString() == "Tấm")
                    {
                        decimal qd = Math.Round((sl * d * r / 10000), 0, MidpointRounding.AwayFromZero);
                        gvMain.SetFocusedRowCellValue(gvMain.Columns["SL2"], qd);
                        decimal tt = qd * Math.Round(dg, 0, MidpointRounding.AwayFromZero);
                        gvMain.SetFocusedRowCellValue(gvMain.Columns["ThanhTien"], tt - (tt % 10));
                    }
                    else
                        gvMain.SetFocusedRowCellValue(gvMain.Columns["ThanhTien"], sl * dg);
                }
            }
        }

        void btnChon_Click(object sender, EventArgs e)
        {

            SimpleButton btn = (SimpleButton) sender;
            //btnChon2.Name = "btnChon2";

            drCur = (_data.BsMain.Current as DataRowView).Row;
            if (!gvMain.Editable)
            {
                XtraMessageBox.Show("Vui lòng chọn chế độ thêm hoặc sửa phiếu",
                    Config.GetValue("PackageName").ToString());
                return;
            }
            if (drCur["MaKH"] == DBNull.Value)
            {
                XtraMessageBox.Show("Vui lòng chọn khách hàng trước!",
                    Config.GetValue("PackageName").ToString());
                return;
            }
            //dùng cách này để truyền tham số vào report
            Config.NewKeyValue("@MaKH", drCur["MaKH"]);
            //dùng report 1514 trong sysReport
            if (btn.Name.Equals("btnChon"))
            {
                frmDS = FormFactory.FormFactory.Create(FormType.Report, "1516") as ReportPreview;
            } else
            {
                frmDS = FormFactory.FormFactory.Create(FormType.Report, "1542") as ReportPreview;
            }
            
            //định dạng thêm cho grid của report
            gvDS = (frmDS.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;
            //viết xử lý cho nút F4-Xử lý trong report
            SimpleButton btnXuLy = (frmDS.Controls.Find("btnXuLy", true)[0] as SimpleButton);
            btnXuLy.Text = btn.Name.Equals("btnChon") ? "Chọn đơn hàng" : "Chọn lệnh sản xuất";
            btnXuLy.Click += btn.Name.Equals("btnChon") ? new EventHandler(btnXuLy_Click) : new EventHandler(btnXuLy_Click2);
            frmDS.WindowState = FormWindowState.Maximized;
            frmDS.ShowDialog();
        }

        void btnChon_Click2(object sender, EventArgs e)
        {

            SimpleButton btn = (SimpleButton)sender;
            //btnChon2.Name = "btnChon2";

            drCur = (_data.BsMain.Current as DataRowView).Row;
            if (!gvMain.Editable)
            {
                XtraMessageBox.Show("Vui lòng chọn chế độ thêm hoặc sửa phiếu",
                    Config.GetValue("PackageName").ToString());
                return;
            }

            DataRow[] drs = (_data.BsMain.DataSource as DataSet).Tables[1].Select("MT32ID = '" + drCur["MT32ID"].ToString() + "'");
            List<string> data = new List<string>();
            foreach (DataRow row in drs)
            {
                if (row.RowState == DataRowState.Added)
                {
                    data.Add("''" + row["DTDHID"].ToString() + "''");
                }
            }

            var dataText =  "(" + string.Join(",", data.ToArray()) + ")";
            //dùng cách này để truyền tham số vào report
            Config.NewKeyValue("@DTDHIDs", dataText);
            frmDS = FormFactory.FormFactory.Create(FormType.Report, "1617") as ReportPreview;

            //định dạng thêm cho grid của report
            gvDS = (frmDS.Controls.Find("gridControlReport", true)[0] as GridControl).MainView as GridView;
            //viết xử lý cho nút F4-Xử lý trong report
            SimpleButton btnXuLy = (frmDS.Controls.Find("btnXuLy", true)[0] as SimpleButton);
            btnXuLy.Visible = false;
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
                XtraMessageBox.Show("Bạn chưa chọn đơn hàng", Config.GetValue("PackageName").ToString());
                return;
            }
            frmDS.Close();
            //add du lieu vao danh sach hang xuat
            DataTable dtDTKH = (_data.BsMain.DataSource as DataSet).Tables[1];
            foreach (DataRow dr in drs)
            {
                //kiểm tra tên hàng có dấu nháy
                string strTenH = "";
                string[] strTenHang = dr["TenHang"].ToString().Split('\'');
                for(int i = 0; i < strTenHang.Length; i++)
                {
                    if ((i == strTenHang.Length - 1) || strTenHang[i] == "")
                        strTenH += strTenHang[i];
                    else
                        strTenH += strTenHang[i] + "''"; 
                }
                if (dtDTKH.Select(  string.Format("MT32ID = '{0}' and DTDHID = '{1}' and Loi = {2} and TenHang = '{3}'" 
                                    , drCur["MT32ID"], dr["DTDHID"],dr["Loi"],strTenH)).Length > 0)
                    continue;
                addNewRow(dr, 0);
            }
        }

        void btnXuLy_Click2(object sender, EventArgs e)
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
            //add du lieu vao danh sach hang xuat
            DataTable dtDTKH = (_data.BsMain.DataSource as DataSet).Tables[1];

            List<string> listDtlsx = new List<string>();

            foreach (DataRow dr in drs)
            {
                listDtlsx.Add("'" + dr["DTDHID"].ToString() + "'");
            }

            string condition = string.Join(",", listDtlsx.ToArray());
            string sql = @"select mt.MaKH, kh.TenTat as TenKH, mt.SoDH, mt.NgayCT as NgayDH, dt.NgayGH, dt.Lop , dt.SoPOKH as SoPOKH, dt.SoLietKe as SoLietKe, dt.GhiChu,
		QuyCach = cast(cast(dt.Dai as float) as varchar)
				+ '*' + cast(cast(dt.Rong as float) as varchar)
				+ (case when dt.Cao is not null then
					'*' + cast(cast(dt.Cao as float) as varchar) else '' end)
				+ '_' + cast(dt.Lop as varchar) + 'L',
		dt.TenHang [TenHang], dt.DVT, Round(dt.GiaBan,0) as DonGia, dt.SoLuong as [SL đặt],
		[SL đã nhập] = isnull((	select	sum(d.SoLuong) from BLVT d  WITH  (NOLOCK) where d.MaCT <> 'NTA'
										and d.DTDHID = dt.DTDHID 										
										and d.Loi = 0),0),
		[SL đã xuất] = isnull((select sum(SoLuong) from DT32 where DT32.DTDHID = dt.DTDHID and dt32.Loi = 0),0),
		[SL hàng trả] = isnull((select sum(soluong) from dt23 where dt23.dtdhid = dt.dtdhid and dt23.loi = 0),0),
		[SLGiayPhe] = isnull((select sum(SLTonCuoi) from dt32 where dt32.dtdhid = dt.dtdhid and dt32.loi = 0 and dt32.isgp = 1),0)
		,mt.MTDHID, dt.DTDHID, dt.Dai, dt.Rong, dt.Cao,
		MaHH = mt.MaKH + '_'
				+ cast(cast(dt.Dai as float) as varchar)
				+ '*' + cast(cast(dt.Rong as float) as varchar)
				+ (case when dt.Cao is not null then
					'*' + cast(cast(dt.Cao as float) as varchar) else '' end)
				+ '_' + cast(dt.Lop as varchar) + 'L',dt.Loai,cast(0 as bit) [Loi]
		,vc.DGVC
        from	MTDonHang mt inner join DTDonHang dt on mt.MTDHID = dt.MTDHID
		        inner join DMKH kh on mt.MaKH = kh.MaKH
                left join DGVC vc on dt.THS = vc.THS
        WHERE dt.DTDHID in ({0})";
            Database db = Database.NewDataDatabase();
            DataTable dtDonHang = db.GetDataTable(string.Format(sql, condition));

            foreach (DataRow dr in dtDonHang.Rows)
            {
                //kiểm tra tên hàng có dấu nháy
                string strTenH = "";
                string[] strTenHang = dr["TenHang"].ToString().Split('\'');
                for (int i = 0; i < strTenHang.Length; i++)
                {
                    if ((i == strTenHang.Length - 1) || strTenHang[i] == "")
                        strTenH += strTenHang[i];
                    else
                        strTenH += strTenHang[i] + "''";
                }
                if (dtDTKH.Select(string.Format("MT32ID = '{0}' and DTDHID = '{1}' and Loi = {2} and TenHang = '{3}'"
                                    , drCur["MT32ID"], dr["DTDHID"], dr["Loi"], strTenH)).Length > 0)
                    continue;

                addNewRow(dr, 1);
            }
        }

        private void addNewRow(DataRow dr, int isPs)
        {
            gvMain.AddNewRow();
            gvMain.UpdateCurrentRow();

            gvMain.SetFocusedRowCellValue(gvMain.Columns["SoPOKH"], dr["SoPOKH"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["SoLietKe"], dr["SoLietKe"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["SoDH"], dr["SoDH"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["NgayDH"], dr["NgayDH"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["TenHang"], dr["TenHang"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["DVT"], dr["DVT"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["QuyCach"], dr["QuyCach"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["Loi"], dr["Loi"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["DonGia"], dr["DonGia"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["Dai"], dr["Dai"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["Rong"], dr["Rong"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["Cao"], dr["Cao"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["DTDHID"], dr["DTDHID"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["Loai"], dr["Loai"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["MaHH"], dr["MaHH"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["Lop"], dr["Lop"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["GhiChu"], dr["GhiChu"]);
            decimal slGiayPhe = isPs == 1 ? Convert.ToDecimal(dr["SLGiayPhe"]) : Convert.ToDecimal(dr["SL Giấy Phế"]);
            decimal slDangTon = Convert.ToDecimal(dr["SL đã nhập"]) - Convert.ToDecimal(dr["SL đã xuất"])
                + Convert.ToDecimal(dr["SL hàng trả"]) - slGiayPhe;
            gvMain.SetFocusedRowCellValue(gvMain.Columns["SLDangTon"], slDangTon);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["DGVC"], dr["DGVC"]);
            gvMain.SetFocusedRowCellValue(gvMain.Columns["isPS"], isPs);
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
