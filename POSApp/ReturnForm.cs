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
    public partial class ReturnForm : Form
    {
        Database db = Database.NewDataDatabase();
        PosMain mainFrm;
        public String softwareType;
        public String machineTable;
        public ReturnForm(PosMain posmain)
        {
            AppCon ac = new AppCon();
            softwareType = ac.GetValue("SoftwareType");
            machineTable = ac.GetValue("MachineTable");
            mainFrm = posmain;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch(softwareType) {
                case "SV" :
                    string macuon = textBox1.Text;
                    ReturnData(macuon);
                    break;
                case "MV" :
                    string macuon2 = textBox1.Text;
                    ReturnData(macuon2);
                    break;

            }
        }
        private void ReturnData (string xmacuon) {
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
            if (ms1 == 1) { UpdateData(xmacuon,"D"); }
            else if (ms2 == 1) { UpdateData(xmacuon, "E"); }
            else if (ms3 == 1) { UpdateData(xmacuon, "B"); }
            else if (ms4 == 1) { UpdateData(xmacuon, "C"); }
            else
            {
                messageBox msg = new messageBox("PaperErr", "Paper Err", "Không tìm thấy cuộn này trong bảng sử dụng");
                msg.Show();
                this.Close();
            }
        } 
        private void UpdateData(string macuon, string may) {
            string ngay = DateTime.Now.ToString();
            MaCuon mc = new MaCuon();
            string dataCnn = Config.GetValue("DataConnection").ToString();
            dataCnn = dataCnn.Replace("POS", "HTCPH");
            Database hoaTieuDb = Database.NewCustomDatabase(dataCnn);
            mc.Macuon = macuon;
            //lay manvl + kho + ky hieu + ty le khoi
            var manl = hoaTieuDb.GetValue(string.Format("SELECT MaNL FROM DT42 WHERE MaCuon = '{0}'", macuon.Trim()));
            string cVitri = "";
            string msch = machineTable.Substring(0, 3).ToString();
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
                
                DataTable dbo = db.GetDataTable(string.Format("SELECT Duyet,SoKg,ViTri from {0}_{1} WHERE MaCuon = '{2}'", msch,may,mc.Macuon));
                if (dbo.Rows.Count > 0)
                {
                    mc.Duyet = dbo.Rows[0]["Duyet"].ToString();
                    mc.SoKg =Convert.ToDecimal(dbo.Rows[0]["SoKg"].ToString());
                    cVitri = dbo.Rows[0]["ViTri"].ToString();
                }
        
            }
            string _machine = machineTable.Substring(0, 3).ToString();
            decimal sosokgBd = mc.SoKg;
            decimal duongkinh = 0;
            Input dkFrm = new Input();
            dkFrm.ShowDialog();
            if (dkFrm.DialogResult != DialogResult.Cancel) { 
            duongkinh = dkFrm.duongkinh;
            } 
            decimal soluongCL = (duongkinh / 1000) * Convert.ToDecimal(mc.Kho) * Convert.ToDecimal("3.14") * mc.TileK;
            decimal soluongSD = mc.SoKg - soluongCL;
            
            string sql = @"INSERT INTO YeuCauXuatKho (Ngay, MaCuon, SoLuongBD, SoLuongSD, SoLuongCL, NguoiDuyet, LSX, Duyet, NguoiLap,ViTri)
                            VALUES ('{0}','{1}',{2},{3},{4},'{5}', '{6}',1, '{7}', '{8}_{9}_{10}')";
            db.UpdateByNonQuery(string.Format(sql, ngay, mc.Macuon, mc.SoKg, soluongSD, soluongCL,mc.Duyet, "LSX", mainFrm.loginUser["Ma"].ToString(),msch,may,cVitri));

            string sql2 = @"DELETE FROM {0}_{1} WHERE MaCuon = '{2}'";
            db.UpdateByNonQuery(string.Format(sql2, _machine,may, mc.Macuon));            
            mainFrm.SyncMainGrid();
            this.Close();
        } 
    }
}
