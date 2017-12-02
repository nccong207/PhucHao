using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace XuLyPBHMi
{
    public partial class LyDoFrm : Form
    {
        public string LyDo;

        public LyDoFrm()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            LyDo = textEdit1.Text;
            this.Close();
        }

        private void LyDoFrm_Load(object sender, EventArgs e)
        {
            textEdit1.Text = LyDo;
        }
    }
}
