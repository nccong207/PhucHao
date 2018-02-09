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
        public SoMay LoaiMay;
        Main mainFrm;
        MaCuon macuonData;
        public Result(MaCuon macuon, SoMay May, Main main)
        {
            InitializeComponent();
            label6.Text = macuon.KyHieu;
            label7.Text = macuon.Kho;
            label8.Text = macuon.Macuon;
            label9.Text = macuon.SoKg.ToString("###,###");
            LoaiMay = May;
            mainFrm = main;
            macuonData = macuon;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            mainFrm.LoadToGrid(macuonData, LoaiMay);
            this.Close();
        }
    }
    public enum SoMay
    {
        May1,
        May2,
        May3
    };

}
