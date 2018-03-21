using CDTDatabase;
using CDTLib;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace POSApp
{
    public partial class Add : Form
    {
        AppCon ac = new AppCon();
        Main mainFrm;
        MaCuon data;
        bool returnFrm;
        public Add(Main main, bool isReturn = false)
        {
            InitializeComponent();
            mainFrm = main;
           
            returnFrm = isReturn;

            if (isReturn)
            {
                simpleButton1.Visible = false;
                simpleButton2.Visible = false;
                simpleButton3.Visible = false;
            }
            else
            {
                simpleButton4.Visible = false;
            }
            textBox1.Focus();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            GetData(SoMay.May1);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            GetData(SoMay.May2);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            GetData(SoMay.May3);
        }
        private void GetData(SoMay may)
        {
            var mc = GetMaCuon(null);
            if (mc.SoKg == 0)
            {
                XtraMessageBox.Show("Mã cuộn này đã sử dụng hết", "POS Warning");
                return;
            }

            var order = mainFrm.GetOrder(mc, may);
            if (order == null || order.Rows.Count == 0)
            {
                XtraMessageBox.Show("Cuộn này không có trong kế hoạch sản xuất - Yêu cầu quản lý xác nhận", "POS Warning");
                CheckData(mc, may);
            }
            else
            {
                showResult(mc, may);
            }
        }

        private void CheckData(MaCuon mc, SoMay may)
        {
            if (!mainFrm.IsQuanLy())
            {
                Login frmLogin = new Login();
                frmLogin.StartPosition = FormStartPosition.CenterScreen;
                frmLogin.ShowDialog();

                //dang nhap lai quyen quan ly
                if (frmLogin.DialogResult != DialogResult.Cancel)
                {
                    mainFrm.UpdateLoginUser(frmLogin.drUser);
                    if (!mainFrm.IsQuanLy())
                    {
                        XtraMessageBox.Show("Chức năng này chỉ dành cho quản lý", "POS Warning");
                        this.Close();
                    }
                    else
                    {
                        showResult(mc, may);
                    }
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                showResult(mc, may);
            }
        }

        private void showResult(MaCuon mc, SoMay may)
        {
            Result frmRs = new Result(mc, may, mainFrm);
            this.Close();
            frmRs.ShowDialog();
        }

        public MaCuon GetMaCuon(string code)
        {
            MaCuon result = new MaCuon();
            string dataCnn = Config.GetValue("DataConnection").ToString();
            if (string.IsNullOrEmpty(dataCnn))
            {
                XtraMessageBox.Show("Không tìm thấy chuỗi kết nối database", Config.GetValue("PackageName").ToString());
                this.Close();
            } 
            dataCnn = dataCnn.Replace("POS", "HTCPH");

            Database hoaTieuDb = Database.NewCustomDatabase(dataCnn);

            string macuon = textBox1.Text;
            result.Macuon = textBox1.Text;

            if (!string.IsNullOrEmpty(code))
            {
                macuon = code;
                result.Macuon = code;
            }

            var soTon = hoaTieuDb.GetValue(string.Format("SELECT SoLuong FROM TonKhoNL WHERE MaCuon = '{0}'", macuon.Trim()));
            decimal soluongTon = 0;
            if (soTon != null)
            {
                soluongTon = Convert.ToDecimal(soTon.ToString());
            }
            result.SoKg = soluongTon;
            var manl = hoaTieuDb.GetValue(string.Format("SELECT MaNL FROM DT42 WHERE MaCuon = '{0}'", macuon.Trim()));

            if (manl != null)
            {
                result.MaNL = manl.ToString();
                DataTable dmNL = hoaTieuDb.GetDataTable(string.Format("SELECT KyHieu, Kho FROM wDMNL2 WHERE Ma = '{0}'", manl.ToString()));
                if (dmNL.Rows.Count > 0)
                {
                    result.KyHieu = dmNL.Rows[0]["KyHieu"].ToString();
                    result.Kho = dmNL.Rows[0]["Kho"].ToString();
                }

                var tileK = hoaTieuDb.GetValue(string.Format("SELECT TiLeK from DMNL WHERE Ma = '{0}'", manl.ToString()));
                if (tileK != null)
                {
                    result.TileK = Convert.ToDecimal( string.IsNullOrEmpty(tileK.ToString()) ? "0" : tileK.ToString());
                }
            }

            return result;
        }

        /// <summary>
        /// Code này để xử lý khi bị duplicate mã cuộn trong database
        /// normal_table là bảng tạo ra chứa những dt42id bị duplicate
        /// hiện tại không dùng
        /// </summary>
        /// <param name="db"></param>
        private void updatedup (Database db)
        {
            var dupId = db.GetDataTable("SELECT * FROM normal_table");
            if (dupId.Rows.Count > 0)
            {
                foreach (DataRow row in dupId.Rows)
                {
                    string update = @"UPDATE DT42 SET MaCuon = 
                                    (select SUBSTRING(Max(MaCuon), 0, 4) + RIGHT('00000'+CAST(CAST(SUBSTRING(Max(MaCuon), 4, 8) AS int) + 1 AS VARCHAR(5)),5)  as Max
                                    from DT42
                                    where MaCuon like 
                                    ( select SUBSTRING(macuon, 0, 4) + '%' from dt42 where dt42id= '{0}'))
                                    WHERE DT42ID  = '{0}'";
                    db.UpdateByNonQuery(string.Format(update, row["DT42ID"].ToString()));

                }
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            AddXuatKho();
        }

        private void AddXuatKho()
        {
            Input frm = new Input();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            var mc =   GetMaCuon(null);

            if (frm.DialogResult != DialogResult.Cancel)
            {
                mainFrm.RemoveMainGrid(mc.Macuon);
                mainFrm.AddToReturnGrid(mc, frm.duongkinh);
                mainFrm.SyncMainGrid();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
           
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

    }
}
