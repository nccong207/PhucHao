using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using DevExpress.XtraEditors;
using System.Drawing;
using DevExpress.XtraLayout;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using CDTLib;
using XuLyBangIn;
using System.Data;

namespace XemHinhPhieuMH
{
    public class XemHinhPhieuMH : ICControl
    {
        DataCustomFormControl _data;
        InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        ZoomPictureEdit peHinhZoom = new ZoomPictureEdit();
        SimpleButton btnXemFile;
        Point _startPoint;
        PictureEdit peHinhGoc;

        public DataCustomFormControl Data
        {
            set { _data = value; }
        }

        public InfoCustomControl Info
        {
            get { return _info; }
        }

        public void AddEvent()
        {
            LayoutControl lcMain = _data.FrmMain.Controls.Find("lcMain", true)[0] as LayoutControl;
            peHinhGoc = _data.FrmMain.Controls.Find("HoaDon", true)[0] as PictureEdit;
            //thêm nút Xem file hình
            btnXemFile = new SimpleButton();
            btnXemFile.Name = "btnXemFile";
            btnXemFile.Text = "Xem hình ảnh";
            LayoutControlItem lci2 = lcMain.AddItem("", btnXemFile);
            lci2.Name = "cusXemFile";
            btnXemFile.Click += new EventHandler(btnXemFile_Click);

            //custom control cho hình ảnh
            peHinhZoom.Name = "peHinhZoom";
            LayoutControlItem lci = lcMain.AddItem("", peHinhZoom);
            lci.Name = "cusHinhZoom";
            peHinhZoom.Properties.ShowMenu = false;
            peHinhZoom.Properties.Scrollable = true;
            peHinhZoom.DataBindings.Add("EditValue", _data.BsMain, "HoaDon", true, DataSourceUpdateMode.OnValidation);
            peHinhZoom.EditValueChanged += new EventHandler(peHinhZoom_EditValueChanged);
            peHinhZoom.Properties.MouseDown += new MouseEventHandler(Properties_MouseDown);
            peHinhZoom.Properties.MouseMove += new MouseEventHandler(Properties_MouseMove);
            peHinhZoom.Properties.MouseUp += new MouseEventHandler(Properties_MouseUp);

            //thêm nút Rotate hình
            SimpleButton btnRotateLeft = new SimpleButton();
            btnRotateLeft.Name = "btnRotateLeft";
            btnRotateLeft.Text = "Xoay trái";
            LayoutControlItem lci3 = lcMain.AddItem("", btnRotateLeft);
            lci3.Name = "cusRotateLeft";
            btnRotateLeft.Click += BtnRotateLeft_Click;

            SimpleButton btnRotateRight = new SimpleButton();
            btnRotateRight.Name = "btnRotateRight";
            btnRotateRight.Text = "Xoay phải";
            LayoutControlItem lci4 = lcMain.AddItem("", btnRotateRight);
            lci4.Name = "cusRotateRight";
            btnRotateRight.Click += BtnRotateRight_Click;
        }

        private void BtnRotateRight_Click(object sender, EventArgs e)
        {
            var img = peHinhZoom.Image;
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var converter = new ImageConverter();
            (_data.BsMain.Current as DataRowView).Row["HoaDon"] = converter.ConvertTo(img, typeof(byte[]));
        }

        private void BtnRotateLeft_Click(object sender, EventArgs e)
        {
            var img = peHinhZoom.Image;
            img.RotateFlip(RotateFlipType.Rotate270FlipNone);
            var converter = new ImageConverter();
            (_data.BsMain.Current as DataRowView).Row["HoaDon"] = converter.ConvertTo(img, typeof(byte[]));
        }

        void btnXemFile_Click(object sender, EventArgs e)
        {
            if (peHinhZoom.Image != null)
            {
                try
                {
                    string extension = ".jpg";
                    string fileName = Path.GetRandomFileName();
                    fileName = Path.ChangeExtension(fileName, extension);
                    fileName = Path.Combine(Path.GetTempPath(), fileName);
                    peHinhZoom.Image.Save(fileName);
                    if (File.Exists(fileName))
                        Process.Start(fileName);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("Lỗi xem tập tin hình ảnh:\n" + ex.Message,
                        Config.GetValue("PackageName").ToString());
                }
            }
        }

        void Properties_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Cursor.Current == Cursors.Hand)
                Cursor.Current = Cursors.Default;
        }

        void Properties_MouseMove(object sender, MouseEventArgs e)
        {
            bool isScrollable = peHinhZoom.Properties.Scrollable &&
                (peHinhZoom.HScroll.Visible || peHinhZoom.VScroll.Visible);
            if (e.Button == MouseButtons.Left && isScrollable)
            {
                Point changePoint = new Point(e.Location.X - _startPoint.X,
                                              e.Location.Y - _startPoint.Y);
                if (peHinhZoom.HScroll.Visible)
                {
                    peHinhZoom.HScroll.Value -= changePoint.X;
                    peHinhZoom.Properties.XIndent = peHinhZoom.HScroll.Value;
                    peHinhZoom.Properties.MaximumXIndent = peHinhZoom.HScroll.Maximum;
                }
                if (peHinhZoom.VScroll.Visible)
                {
                    peHinhZoom.VScroll.Value -= changePoint.Y;
                    peHinhZoom.Properties.YIndent = peHinhZoom.VScroll.Value;
                    peHinhZoom.Properties.MaximumYIndent = peHinhZoom.VScroll.Maximum;
                }
                if (isScrollable)
                    peHinhZoom.Refresh();
            }
        }

        void Properties_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _startPoint = e.Location;
                Cursor.Current = Cursors.Hand;
            }
        }

        private void peHinhZoom_EditValueChanged(object sender, EventArgs e)
        {
            btnXemFile.Enabled = peHinhZoom.EditValue != null;
            peHinhZoom.Properties.ZoomFactor = 100;

            peHinhZoom.Properties.YIndent = peHinhZoom.Properties.XIndent = 0;
            peHinhZoom.Properties.MaximumYIndent = peHinhZoom.Properties.MaximumXIndent = 0;
            peHinhZoom.Refresh();
        }
    }
}
