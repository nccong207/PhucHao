using CDTDatabase;
using CDTLib;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Data;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Drawing;

namespace POSApp
{
    public partial class PosMain : Form
    {
        Database db = Database.NewDataDatabase();
        public DataRow loginUser;
        public String softwareType;
        public String machineTable;
        public PosMain(DataRow drUser)
        {
            AppCon ac = new AppCon();
            softwareType = ac.GetValue("SoftwareType");
            machineTable = ac.GetValue("MachineTable");
            if (softwareType == "SV" && machineTable == "")
            { messageBox msg = new messageBox("Lỗi", "Lỗi cấu hình", "Phải nhập thông tin Machine Table"); msg.ShowDialog(); }
            InitializeComponent();
            m1.Visible = false;
            m2.Visible = false;
            m3.Visible = false;
            m4.Visible = false;
            m0_1.Visible = false;
            m0_2.Visible = false;
            header.Visible = false;
            body.Visible = false;
            body_1.Visible = false;
            body_2.Visible = false;
            footer.Visible = false;
            loginUser = drUser;
            casx.Text = loginUser["Ca"].ToString();
            usrname.Text = loginUser["HoTen"].ToString();
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer2.Enabled = true;
            timer2.Interval = 10000;
            timer3.Enabled = true;
            timer3.Interval = 10000;

        }
        public void singleView()
        {
            body.Location = new Point(0, header.Height);
            body.Width = ClientSize.Width;
            body.Height = (ClientSize.Height * 80) / 100;

            body_0.Location = new Point(0, 0);
            body_0.Width = body.Width;
            body_0.Height = (body.Height * 10) / 100;

            body_1.Location = new Point(0, body_0.Height);
            body_1.Width = body.Width;
            body_1.Height = (body.Height * 60) / 100;

            body_2.Location = new Point(0, body_0.Height + body_1.Height);
            body_2.Width = body.Width;
            body_2.Height = (body.Height * 30) / 100;

            m0_1.Width = body_1.Width / 2;
            m0_1.Height = body_1.Height;
            m0_1.Location = new Point(0, 0);
            m0_2.Width = body_1.Width / 2;
            m0_2.Height = body_1.Height;
            m0_2.Location = new Point(m0_1.Width, 0);
            lw_0_1.Location = new Point(0, 0);
            lw_0_1.Width = body_0.Width / 2;
            lw_0_1.Height = body_0.Height;

            lw_0_2.Location = new Point(lw_0_1.Width, 0);
            lw_0_2.Width = body_0.Width / 2;
            lw_0_2.Height = body_0.Height;
            m0_1.Visible = true;
            m0_2.Visible = true;
            m1.Visible = false;
            m2.Visible = false;
            m3.Visible = false;
            m4.Visible = false;

            th.Width = body_2.Width;
            th.Height = body_2.Height;
            th.Location = new Point(0, 0);

            header.Visible = true;
            body.Visible = true;
            body_0.Visible = true;
            body_1.Visible = true;
            body_2.Visible = true;

            footer.Location = new Point(0, header.Height + body.Height);
            footer.Width = ClientSize.Width;
            footer.Height = (ClientSize.Height * 10) / 100;
            thoatBtn.Height = ((footer.Height * 80) / 100);
            thoatBtn.Location = new Point(footer.Width - thoatBtn.Width + 10, (footer.Height / 2) - (thoatBtn.Height / 2));

            footer.Visible = true;
            SyncMainGrid();

        }

        public void multiView()
        {
            body.Location = new Point(0, header.Height);
            body.Width = ClientSize.Width;
            body.Height = (ClientSize.Height * 80) / 100;

            body_0.Location = new Point(0, 0);
            body_0.Width = body.Width;
            body_0.Height = (body.Height * 10) / 100;

            body_1.Location = new Point(0, body_0.Height);
            body_1.Width = body.Width;
            body_1.Height = (body.Height * 60) / 100;

            body_2.Location = new Point(0, body_0.Height + body_1.Height);
            body_2.Width = body.Width;
            body_2.Height = (body.Height * 30) / 100;

            m1.Width = body_1.Width / 4;
            m1.Height = body_1.Height;
            m1.Location = new Point(0, 0);

            m2.Width = body_1.Width / 4;
            m2.Height = body_1.Height;
            m2.Location = new Point(m1.Width, 0);

            m3.Width = body_1.Width / 4;
            m3.Height = body_1.Height;
            m3.Location = new Point(m1.Width+m2.Width, 0);

            m4.Width = body_1.Width / 4;
            m4.Height = body_1.Height;
            m4.Location = new Point(m1.Width + m2.Width + m3.Width, 0);

            lw_1.Location = new Point(0, 0);
            lw_1.Width = body_0.Width / 4;
            lw_1.Height = body_0.Height;

            lw_2.Location = new Point(lw_1.Width, 0);
            lw_2.Width = body_0.Width / 4;
            lw_2.Height = body_0.Height;

            lw_3.Location = new Point(lw_1.Width + lw_2.Width, 0);
            lw_3.Width = body_0.Width / 4;
            lw_3.Height = body_0.Height;

            lw_4.Location = new Point(lw_1.Width + lw_2.Width + lw_3.Width, 0);
            lw_4.Width = body_0.Width / 4;
            lw_4.Height = body_0.Height;

            th.Width = body_2.Width;
            th.Height = body_2.Height;
            th.Location = new Point(0, 0);

            m1.Visible = true;
            m2.Visible = true;
            m3.Visible = true;
            m4.Visible = true;
            lw_1.Visible = true;
            lw_2.Visible = true;
            lw_3.Visible = true;
            lw_4.Visible = true;
            header.Visible = true;
            body.Visible = true;
            body_0.Visible = true;
            body_1.Visible = true;
            body_2.Visible = true;

            footer.Location = new Point(0, header.Height + body.Height);
            footer.Width = ClientSize.Width;
            footer.Height = (ClientSize.Height * 10) / 100;
            thoatBtn.Height = ((footer.Height * 80) / 100);
            thoatBtn.Location = new Point(footer.Width - thoatBtn.Width + 10, (footer.Height / 2) - (thoatBtn.Height / 2));

            footer.Visible = true;
            SyncMainGrid();

        }

        private void PosMain_Load(object sender, EventArgs e)
        {

            header.Width = ClientSize.Width;
            header.Height = (ClientSize.Height * 10) / 100;
            header.Location = new Point(0, 0);
            traBtn.Height = ((header.Height * 90) / 100);
            traBtn.Location = new Point(header.Width - traBtn.Width - 10, (header.Height / 2) - (traBtn.Height / 2));
            themBtn.Height = ((header.Height * 90) / 100);
            themBtn.Location = new Point(header.Width - themBtn.Width - traBtn.Width - 20, (header.Height / 2) - (themBtn.Height / 2));

           

           



            switch (softwareType)
            {
                case "SV":
                    singleView();
                    break;
                case "MV":
                    multiView();
                    break;

            }

        }
        
        public void SyncMainGrid()
        {
            DataTable d0 = new DataTable(null) ;
            //m0_1.DataSource = d0;
            m0_2.DataSource = d0;
            m1.DataSource = d0;
            m2.DataSource = d0;
            m3.DataSource = d0;
            m4.DataSource = d0;
            
            switch (softwareType)
            {
                    
                    
                case "SV":
                    string sql1 = string.Format("SELECT b.MaCuon as m0_1_MaCuon ,b.SoKg as m0_1_SoKG,w.KyHieu as m0_1_KyHieu FROM [POS].[dbo].[{0}] b inner join HTCPH.dbo.DT42 d on d.MaCuon = b.MaCuon left join HTCPH.dbo.wDMNL2 w on w.Ma = d.MaNL WHERE b.ViTri = 1 ORDER BY ID", machineTable);
                    
                    DataTable dt1 = db.GetDataTable(sql1);
                    if (dt1.Rows.Count > 0)
                    {
                        m0_1.DataSource = dt1;
                    }
                    string sql2 = string.Format("SELECT b.MaCuon as m0_2_MaCuon ,b.SoKg as m0_2_SoKG,w.KyHieu as m0_2_KyHieu FROM [POS].[dbo].[{0}] b inner join HTCPH.dbo.DT42 d on d.MaCuon = b.MaCuon left join HTCPH.dbo.wDMNL2 w on w.Ma = d.MaNL WHERE b.ViTri = 2 ORDER BY ID", machineTable);
                    DataTable dt2 = db.GetDataTable(sql2);
                    if (dt2.Rows.Count > 0)
                    {
                        m0_2.DataSource = dt2;
                    }
                    string _mstable = machineTable.Substring(0, 5).ToString();
                    string sql3 = string.Format("SELECT[Ngay],[MaCuon],[SoLuongBD],[SoLuongSD],[SoLuongCL],[NguoiDuyet],[ViTri] FROM [POS].[dbo].[YeuCauXuatKho] WHERE SUBSTRING([ViTri],1,5) = '{0}'", _mstable);
                    DataTable dt3 = db.GetDataTable(sql3);
                    if (dt3.Rows.Count > 0)
                    {
                        th.DataSource = dt3;
                    }
                    break;
                case "MV":
                    //load May 1
                    string ms1 = string.Format("SELECT b.MaCuon as m1_MaCuon ,b.SoKg as m1_SoKg,w.KyHieu as m1_KyHieu, b.ViTri as m1_ViTri FROM [POS].[dbo].[{0}_D] b inner join HTCPH.dbo.DT42 d on d.MaCuon = b.MaCuon left join HTCPH.dbo.wDMNL2 w on w.Ma = d.MaNL ORDER BY ID", machineTable);
                    DataTable dtb1 = db.GetDataTable(ms1);
                    if (dtb1.Rows.Count > 0)
                    {
                        m1.DataSource = dtb1;
                    }
                    //load May 2
                    string ms2 = string.Format("SELECT b.MaCuon as m2_MaCuon ,b.SoKg as m2_SoKg,w.KyHieu as m2_KyHieu, b.ViTri as m2_ViTri FROM [POS].[dbo].[{0}_E] b inner join HTCPH.dbo.DT42 d on d.MaCuon = b.MaCuon left join HTCPH.dbo.wDMNL2 w on w.Ma = d.MaNL ORDER BY ID", machineTable);
                    DataTable dtb2 = db.GetDataTable(ms2);
                    if (dtb2.Rows.Count > 0)
                    {
                        m2.DataSource = dtb2;
                    }
                    //load May 3
                    string ms3 = string.Format("SELECT b.MaCuon as m3_MaCuon ,b.SoKg as m3_SoKg,w.KyHieu as m3_KyHieu, b.ViTri as m3_ViTri FROM [POS].[dbo].[{0}_B] b inner join HTCPH.dbo.DT42 d on d.MaCuon = b.MaCuon left join HTCPH.dbo.wDMNL2 w on w.Ma = d.MaNL ORDER BY ID", machineTable);
                    DataTable dtb3 = db.GetDataTable(ms3);
                    if (dtb3.Rows.Count > 0)
                    {
                        m3.DataSource = dtb3;
                    }
                    //load May 4
                    string ms4 = string.Format("SELECT b.MaCuon as m4_MaCuon ,b.SoKg as m4_SoKg ,w.KyHieu as m4_KyHieu, b.ViTri as m4_ViTri FROM [POS].[dbo].[{0}_C] b inner join HTCPH.dbo.DT42 d on d.MaCuon = b.MaCuon left join HTCPH.dbo.wDMNL2 w on w.Ma = d.MaNL ORDER BY ID", machineTable);
                    DataTable dtb4 = db.GetDataTable(ms4);
                    if (dtb4.Rows.Count > 0)
                    {
                        m4.DataSource = dtb4;
                    }
                    string _mstable2 = machineTable.Substring(0, 3).ToString();
                    string sql5 = string.Format("SELECT[Ngay],[MaCuon],[SoLuongBD],[SoLuongSD],[SoLuongCL],[NguoiDuyet],[ViTri] FROM [POS].[dbo].[YeuCauXuatKho] WHERE SUBSTRING([ViTri],1,3) = '{0}'", _mstable2);
                    DataTable dt5= db.GetDataTable(sql5);
                    if (dt5.Rows.Count > 0)
                    {
                        th.DataSource = dt5;
                    }
                    
                    break;

            }
        }
        
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            day.Text = string.Format("{0:dd/mm/yy - HH:MM:ss}", DateTime.Now);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            var ping = new Ping();

            PingReply result = null;
            IPStatus status;
            try
            {
                result = ping.Send("8.8.8.8");
                status = result.Status;
            }
            catch (Exception ex)
            {
                status = IPStatus.DestinationHostUnreachable;
            }

            if (status != IPStatus.DestinationHostUnreachable)
            {
                longway.BackColor = Color.Green;
            }
            else
            {
                longway.BackColor = Color.Red;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            var ping = new Ping();

            PingReply result = null;
            IPStatus status;
            try
            {
                result = ping.Send("8.8.8.8");
                status = result.Status;
            }
            catch (Exception ex)
            {
                status = IPStatus.DestinationHostUnreachable;
            }

            if (status != IPStatus.DestinationHostUnreachable)
            {
                hoatieu.BackColor = Color.Green;
            }
            else
            {
                hoatieu.BackColor = Color.Red;
            }
        }

        private void themBtn_Click(object sender, EventArgs e)
        {
            AddForm frm = new AddForm(this);
            frm.ShowDialog();
           
        }

        private void traBtn_Click(object sender, EventArgs e)
        {
            ReturnForm rtn = new ReturnForm(this);
            rtn.Show();
        }


        public bool IsQuanLy()
        {
            return loginUser["Quyen"].ToString().Equals("Quản lý");
        }
        public void RemoveMainGrid(string macuon)
        {

            for (int i = 0; i < m1_view.DataRowCount; i++)
            {
                if (!string.IsNullOrEmpty(m1_view.GetRowCellValue(i, "MS1").ToString()))
                {
                    var data = m1_view.GetRowCellValue(i, "MS1").ToString();
                    if (data.Contains(macuon))
                    {
                        m1_view.SetRowCellValue(i, "MS1", "");
                        return;
                    }

                    var data2 = m1_view.GetRowCellValue(i, "MS2").ToString();
                    if (data2.Contains(macuon))
                    {
                        m1_view.SetRowCellValue(i, "MS2", "");
                        return;
                    }

                    var data3 = m1_view.GetRowCellValue(i, "MS3").ToString();
                    if (data3.Contains(macuon))
                    {
                        m1_view.SetRowCellValue(i, "MS3", "");
                        return;
                    }

                }
            }
        }

        private string GetListOder(decimal soluongSD, DataTable orderSelected)
        {
            string listOrderNo = "";
            DataView dv = orderSelected.DefaultView;
            dv.Sort = "CutNum asc";
            DataTable sortedDT = dv.ToTable();
            decimal total = 0;
            foreach (DataRow row in sortedDT.Rows)
            {
                total += Convert.ToDecimal(row["CutNum"]);
                listOrderNo += row["OrderNo"] + ",";
                if (total >= soluongSD)
                {
                    return listOrderNo;
                }
            }
            return listOrderNo;
        }
        public void ReloadReturnGrid()
        {
            string sql = "SELECT * FROM YeuCauXuatKho Order by Ngay DESC";
            DataTable dt = db.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                th.DataSource = dt;
            }
        }

        public DataTable GetOrder(MaCuon macuon, int may)
        {
            // kiểm tra cuộn giấy đó có trong đơn hàng sản xuất 
            Database longwayDb = Database.NewStructDatabase();

            //string connect = "Server =LINH-PC\\HOATIEU; database = CPMS; user = sa; pwd = ht";
            //Database longwayDb = Database.NewCustomDatabase(connect);

            int startNum = 0, endNum = 0;

            switch (may)
            {
                case 1: startNum = 3; endNum = 5; break;
                case 2: startNum = 7; endNum = 9; break;
                case 3: startNum = 11; endNum = 13; break;
                default:
                    break;
            }

            string query = @"SELECT PaperUse, ProduceWid, CutNum, SumSquare, OrderNo FROM LW_Order
			WHERE SUBSTRING(PaperUse, {0}, 2) = '{1}' OR SUBSTRING(PaperUse, {2}, 2) = '{3}'";

            DataTable order = longwayDb.GetDataTable(string.Format(query, startNum, macuon.KyHieu, endNum, macuon.KyHieu));
            return order;
        }

        private void m0_1_view_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                string colCaption = info.Column == null ? "N/A" : info.Column.GetCaption();
                string macuonx = view.GetRowCellValue(info.RowHandle, view.Columns["m0_1_MaCuon"]).ToString();
                string kyhieu = view.GetRowCellValue(info.RowHandle, view.Columns["m0_1_KyHieu"]).ToString();
                string sokg = view.GetRowCellValue(info.RowHandle, view.Columns["m0_1_SoKG"]).ToString();
                string text = macuonx + "\n" + kyhieu + "\n" + sokg;
                messageBox x = new messageBox("Thông tin", "Cuộn giấy đang thao tác", text);
                x.ShowDialog();
                if (x.DialogResult != DialogResult.Cancel)
                {


                }
            }
        }
        public void AddToReturnGrid(int may, MaCuon macuon, decimal duongkinh)
        {
            string ngay = DateTime.Now.ToString();
            decimal soluongBD = Convert.ToDecimal(macuon.SoKg);
            decimal soluongCL = (duongkinh / 1000) * Convert.ToDecimal(macuon.Kho) * Convert.ToDecimal("3.14") * macuon.TileK;
            decimal soluongSD = soluongBD - soluongCL;
            string list = "";
            // tính số lượng đơn hàng sử dụng


            string nguoiduyet = loginUser["HoTen"].ToString();
            string sql = @"INSERT INTO YeuCauXuatKho (Ngay, MaCuon, SoLuongBD, SoLuongSD, SoLuongCL, NguoiDuyet, LSX, Duyet, NguoiLap)
							VALUES ('{0}','{1}',{2},{3},{4},'{5}', '{6}',1, '{7}')";
            db.UpdateByNonQuery(string.Format(sql, ngay, macuon.Macuon, soluongBD, soluongSD, soluongCL, nguoiduyet, list, loginUser["Ma"].ToString()));
            ReloadReturnGrid();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SyncMainGrid();
        }
   
       
       
    }
}
