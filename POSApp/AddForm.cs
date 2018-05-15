using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CDTDatabase;
using CDTLib;

namespace POSApp
{
    public partial class AddForm : Form
    {
        Database db = Database.NewDataDatabase();
        PosMain mainFrm;
        public String softwareType;
        public String machineTable;
        public AddForm(PosMain posMain)
        {
            AppCon ac = new AppCon();
            softwareType = ac.GetValue("SoftwareType");
            machineTable = ac.GetValue("MachineTable");
            if (softwareType == "SV" && machineTable == "")
            { messageBox msg = new messageBox("Lỗi", "Lỗi cấu hình", "Phải nhập thông tin Machine Table"); msg.ShowDialog(); }
            InitializeComponent();
            mainFrm = posMain;


        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            switch (softwareType)
            {
                case "SV": //giao dien 1 may
                    may1Btn.Visible = false;
                    may2Btn.Visible = false;
                    may3Btn.Visible = false;
                    may4Btn.Visible = false;


                    break;
                case "MV": // giao dien nhieu may
                    matBtn.Visible = false;
                    songBtn.Visible = false;
                    break;

            }
        }

        private void nhapCuon(string xmacuon, string may, int vitri)
        {
            // buoc 1: Lay thong tin cuon
            MaCuon mc = new MaCuon();
            string dataCnn = Config.GetValue("DataConnection").ToString();
            dataCnn = dataCnn.Replace("POS", "HTCPH");
            Database hoaTieuDb = Database.NewCustomDatabase(dataCnn);
            mc.Macuon = xmacuon;
            //lay manvl + kho + ky hieu + ty le khoi
            var manl = hoaTieuDb.GetValue(string.Format("SELECT MaNL FROM DT42 WHERE MaCuon = '{0}'", xmacuon.Trim()));
            if (manl != null)
            {
                mc.MaNL = manl.ToString();
                DataTable dmNL = hoaTieuDb.GetDataTable(string.Format("SELECT KyHieu, Kho FROM wDMNL2 WHERE Ma = '{0}'", manl.ToString()));
                if (dmNL.Rows.Count > 0)
                {
                    mc.KyHieu = dmNL.Rows[0]["KyHieu"].ToString();
                    mc.Kho = dmNL.Rows[0]["Kho"].ToString();
                }

                var tileK = hoaTieuDb.GetValue(string.Format("SELECT TiLeK from DMNL WHERE Ma = '{0}'", manl.ToString()));
                if (tileK != null)
                {
                    mc.TileK = Convert.ToDecimal(string.IsNullOrEmpty(tileK.ToString()) ? "0" : tileK.ToString());
                }
            }
            // lay so kg 
            var soTon = hoaTieuDb.GetValue(string.Format("SELECT SoLuong FROM TonKhoNL WHERE MaCuon = '{0}'", xmacuon));
            decimal soluongTon = 0;
            if (soTon != null)
            {
                soluongTon = Convert.ToDecimal(soTon.ToString());
            }

            mc.SoKg = soluongTon;
            if (mc.SoKg == 0)
            {
                messageBox msg = new messageBox("PPEr", "Paper Err", "Cuộn vừa nhập đã sử dụng hết"); msg.ShowDialog();
                this.Close();
            }
            else
            {
                // check current list
                string dataCnn1 = Config.GetValue("DataConnection").ToString();
                if (string.IsNullOrEmpty(dataCnn1))
                {
                    messageBox msg = new messageBox("ConnErr", "Database Err", "Không tìm thấy chuỗi kết nối database");
                    msg.Show();
                    this.Close();
                }
                Database posDB = Database.NewCustomDatabase(dataCnn1);
                string machine = machineTable.Substring(0, 3).ToString();
                int ms1 = Convert.ToInt32(posDB.GetValue(string.Format("SELECT count(ID) FROM {0}_D WHERE [MaCuon] = '{1}'", machine, xmacuon.Trim())));
                int ms2 = Convert.ToInt32(posDB.GetValue(string.Format("SELECT count(ID) FROM {0}_E WHERE [MaCuon] = '{1}'", machine, xmacuon.Trim())));
                int ms3 = Convert.ToInt32(posDB.GetValue(string.Format("SELECT count(ID) FROM {0}_B WHERE [MaCuon] = '{1}'", machine, xmacuon.Trim())));
                int ms4 = Convert.ToInt32(posDB.GetValue(string.Format("SELECT count(ID) FROM {0}_C WHERE [MaCuon] = '{1}'", machine, xmacuon.Trim())));
                if ((ms1 + ms2 + ms3 + ms4) >= 1)
                {
                    messageBox msg = new messageBox("PPEr", "Paper Err", "Cuộn vừa nhập đang được sử dụng"); msg.ShowDialog();
                    this.Close();
                }
                else {
                    Database longwayDb = Database.NewStructDatabase();
                    int startNum = 0;
                    switch (may)
                    {
                        case "D": startNum = 0; break;
                        case "E":
                            switch (vitri) { case 1: startNum = 3; break; case 2: startNum = 5; break; }
                            break;
                        case "B":
                            switch (vitri) { case 1: startNum = 7; break; case 2: startNum = 9; break; }
                            break;
                        case "C":
                            switch (vitri) { case 1: startNum = 11; break; case 2: startNum = 13; break; }
                            break;
                        default:
                            break;
                    }
                    string query = @"SELECT PaperUse, ProduceWid, CutNum, SumSquare, OrderNo FROM LW_Order
                WHERE SUBSTRING(PaperUse, {0}, 2) = '{1}' and ProduceWid = {2} ";
                    DataTable order = longwayDb.GetDataTable(string.Format(query, startNum, mc.KyHieu, mc.Kho));
                    if (order == null || order.Rows.Count == 0)
                    {
                        messageBox msg = new messageBox("PPEr", "Paper Err", "Cuộn này không có trong kế hoạch sản xuất, Yêu cầu Admin Xác nhận"); msg.ShowDialog();
                        if (msg.DialogResult != DialogResult.Cancel)
                        {
                            if (!mainFrm.IsQuanLy())
                            {
                                //Login frmLogin = new Login();
                                //frmLogin.StartPosition = FormStartPosition.CenterScreen;
                                //frmLogin.ShowDialog();
                                AdminConfirm frmConfirm = new AdminConfirm();
                                frmConfirm.StartPosition = FormStartPosition.CenterScreen;
                                frmConfirm.ShowDialog();
                                //dang nhap lai quyen quan ly
                                if (frmConfirm.DialogResult != DialogResult.Cancel)
                                {

                                    if (!frmConfirm.confUser["Quyen"].ToString().Equals("Quản lý"))
                                    {
                                        MessageBox.Show("Chức năng này chỉ dành cho quản lý", "POS Warning");
                                        this.Close();
                                    }
                                    else
                                    {
                                        mc.Duyet = frmConfirm.confUser["HoTen"].ToString();
                                        LoadToData(mc, may, vitri);
                                    }
                                }
                                else
                                {
                                    this.Close();
                                }
                            }
                            else
                            {
                                mc.Duyet = mainFrm.loginUser["HoTen"].ToString();
                                LoadToData(mc, may, vitri);
                            }
                            
                        }
                        this.Close();
                    }
                    else { LoadToData(mc, may, vitri); }
                }// check longway 

            }
        }

        public void LoadToData(MaCuon macuon, string may, int vitri)
        {

            switch (softwareType)
            {
                case "SV":
                    string sql = @"INSERT INTO {0} (MaCuon, Vitri, SoKG, Duyet)
                            VALUES ('{1}',{2},{3},'{4}')";
                    decimal sokg = macuon.SoKg;
                    db.UpdateByNonQuery(string.Format(sql, machineTable, macuon.Macuon.ToString(), vitri, sokg, macuon.Duyet));
                    mainFrm.SyncMainGrid();
                    break;
                case "MV":
                    string machine = machineTable.Substring(0, 3).ToString();
                    string sql1 = @"INSERT INTO {0}_{1} (MaCuon, Vitri, SoKG,Duyet)
                    VALUES ('{2}',{3},{4},'{5}')";
                    decimal sokg1 = macuon.SoKg;
                    db.UpdateByNonQuery(string.Format(sql1, machineTable, may, macuon.Macuon.ToString(), vitri, sokg1, macuon.Duyet));
                    mainFrm.SyncMainGrid();
                    break;

            }
        }
        private Boolean checkAdmin()
        {
            if (!mainFrm.IsQuanLy())
            {
                //Login frmLogin = new Login();
                //frmLogin.StartPosition = FormStartPosition.CenterScreen;
                //frmLogin.ShowDialog();
                AdminConfirm frmConfirm = new AdminConfirm();
                frmConfirm.StartPosition = FormStartPosition.CenterScreen;
                frmConfirm.ShowDialog();
                //dang nhap lai quyen quan ly
                if (frmConfirm.DialogResult != DialogResult.Cancel)
                {

                    if (!frmConfirm.confUser["Quyen"].ToString().Equals("Quản lý"))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

        }


        //Single View

        private void songBtn_Click(object sender, EventArgs e)
        {
            string macuon = textBox1.Text;
            string machine = machineTable.Substring(4, 1).ToString();
            nhapCuon(macuon, machine, 1);

        }
        private void matBtn_Click(object sender, EventArgs e)
        {
            string macuon = textBox1.Text;
            string machine = machineTable.Substring(4, 1).ToString();
            nhapCuon(macuon, machine, 2);
        }
        // Multi View
        private void may1Btn_Click(object sender, EventArgs e)
        {
            string macuon = textBox1.Text;
            Add_Vitri vitri = new Add_Vitri();
            vitri.ShowDialog();
            if (vitri.DialogResult != DialogResult.Cancel)
            {
                nhapCuon(macuon, "D", vitri.vitri);
            }

        }

        private void may2Btn_Click(object sender, EventArgs e)
        {
            string macuon = textBox1.Text;
            Add_Vitri vitri = new Add_Vitri();
            vitri.ShowDialog();
            if (vitri.DialogResult != DialogResult.Cancel)
            {
                nhapCuon(macuon, "E", vitri.vitri);
            }
        }

        private void may3Btn_Click(object sender, EventArgs e)
        {
            string macuon = textBox1.Text;
            Add_Vitri vitri = new Add_Vitri();
            vitri.ShowDialog();
            if (vitri.DialogResult != DialogResult.Cancel)
            {
                nhapCuon(macuon, "B", vitri.vitri);
            }
        }

        private void may4Btn_Click(object sender, EventArgs e)
        {
            string macuon = textBox1.Text;
            Add_Vitri vitri = new Add_Vitri();
            vitri.ShowDialog();
            if (vitri.DialogResult != DialogResult.Cancel)
            {
                nhapCuon(macuon, "C", vitri.vitri);
            }
        }




    }
    public class MaCuon
    {
        public string KyHieu { get; set; }
        public string MaNL { get; set; }
        public decimal TileK { get; set; }
        public string Kho { get; set; }
        public string Macuon { get; set; }
        public decimal SoKg { get; set; }
        public string Duyet { get; set; }

    }
}
