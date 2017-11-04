using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using System.Drawing;
using System.Data;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;
using MyWebCam;

namespace CaptureHoaDon
{
    public class CaptureHoaDon : ICControl
    {
        private DataCustomFormControl _data;
        private InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        private PictureEdit pe;
        LayoutControl lc;

        #region ICControl Members

        public void AddEvent()
        {
            pe = _data.FrmMain.Controls.Find("HoaDon", true)[0] as PictureEdit;
            lc = _data.FrmMain.Controls.Find("lcMain", true)[0] as LayoutControl;

            SimpleButton btnCapture = new SimpleButton();
            btnCapture.Name = "btnCapture";   //phai co name cua control
            btnCapture.Text = "Capture";
            LayoutControlItem lci1 = lc.AddItem("", btnCapture);
            lci1.Name = "cusCapture"; //phai co name cua item, bat buoc phai co "cus" phai truoc
            btnCapture.Click += new EventHandler(btnCapture_Click);
        }

        void btnCapture_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (pe.Properties.ReadOnly)
            {
                flag = true;
                _data.FrmMain.Activate();
                SendKeys.SendWait("{F3}");
            }
            if (pe.Properties.ReadOnly)
                return;
            FrmMain frm = new FrmMain();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageConverter converter = new ImageConverter();
                (_data.BsMain.Current as DataRowView).Row["Hinh"] = converter.ConvertTo(frm.image, typeof(byte[]));
            }
            if (flag)
            {
                _data.FrmMain.Activate();
                SendKeys.SendWait("{F12}");
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
