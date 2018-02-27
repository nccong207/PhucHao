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
        public Add(Main main, bool isReturn = false)
        {
            InitializeComponent();
            mainFrm = main;
            if (isReturn)
            {
                simpleButton1.Visible = false;
                simpleButton2.Visible = false;
                simpleButton3.Visible = false;
            } else
            {
                simpleButton4.Visible = false;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var mc = GetMaCuon();
            Result frmRs = new Result(mc, SoMay.May1, mainFrm);
            this.Close();
            frmRs.ShowDialog();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            var mc = GetMaCuon();
            Result frmRs = new Result(mc, SoMay.May2, mainFrm);
            this.Close();
            frmRs.ShowDialog();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            var mc = GetMaCuon();
            Result frmRs = new Result(mc, SoMay.May3, mainFrm);
            this.Close();
            frmRs.ShowDialog();
        }

        private MaCuon GetMaCuon()
        {
            MaCuon result = new MaCuon();
            string StructConnection = ac.GetValue("StructDb");
            if (string.IsNullOrEmpty(StructConnection))
            {
                XtraMessageBox.Show("Không tìm thấy chuỗi kết nối database", Config.GetValue("PackageName").ToString());
                this.Close();
            }
            StructConnection = Security.DeCode(StructConnection);
            StructConnection = StructConnection.Replace("POS", "HTCPH");

            Database db = Database.NewCustomDatabase(StructConnection);
            result.Macuon = textBox1.Text;
            string macuon = textBox1.Text;
            var soTon = db.GetValue(string.Format("SELECT SoLuong FROM TonKhoNL WHERE MaCuon = '{0}'", macuon.Trim()));
            decimal soluongTon = 0;
            if (soTon != null)
            {
                soluongTon  = Convert.ToDecimal(soTon.ToString());
            }
            result.SoKg = soluongTon;
           var manl = db.GetValue(string.Format("SELECT MaNL FROM DT42 WHERE MaCuon = '{0}'", macuon.Trim()));
            string kyhieu = "", kho = "";
            if (manl != null)
            {
                DataTable dmNL = db.GetDataTable(string.Format("SELECT KyHieu, Kho FROM wDMNL2 WHERE Ma = '{0}'", manl.ToString()));
                if (dmNL.Rows.Count > 0)
                {
                    result.KyHieu = dmNL.Rows[0]["KyHieu"].ToString();
                    result.Kho = dmNL.Rows[0]["Kho"].ToString();
                }
            }

            return result;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string Macuon = textBox1.Text;
            if (mainFrm.IsExisted(Macuon) == null)
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
                            this.Close();
                        }else
                        {
                            //next
                            AddXuatKho();
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                } else
                {
                    //next
                    AddXuatKho();
                }
            } else
            {
                //next
                AddXuatKho();
            }
        }

        private void AddXuatKho() {
            Input frm = new Input();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();

            if (frm.DialogResult != DialogResult.Cancel)
            {
                mainFrm.AddToReturnGrid(textBox1.Text, frm.duongkinh);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }

    public class MaCuon
    {
        public string KyHieu { get; set; }
        public string Kho { get; set; }
        public string Macuon { get; set; }
        public decimal SoKg { get; set; }

    }
}
