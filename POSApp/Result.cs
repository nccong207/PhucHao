using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace POSApp
{
    public partial class Result : Form
    {
        public string machine;
        public int xvitri;
        PosMain posMainFrm;
        MaCuon macuonData;
        public Result(MaCuon macuon, string May, PosMain posMain,int vitri)
        {
            InitializeComponent();
            label6.Text = macuon.KyHieu;
            label7.Text = macuon.Kho;
            macuonl.Text = macuon.Macuon;
            label10.Text = macuon.SoKg.ToString("###,###");
            machine = May;
            xvitri = vitri;
            posMainFrm = posMain;
            macuonData = macuon;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }


}
