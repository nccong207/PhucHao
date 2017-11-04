using System;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.XtraEditors;



namespace MyWebCam
{

    public partial class FrmMain : DevExpress.XtraEditors.XtraForm
    {
        WebCam webcam;
        public Image image;

        public FrmMain()
        {
            InitializeComponent();
        }
        private void mainWinForm_Load(object sender, EventArgs e)
        {
            webcam = new WebCam();
            webcam.InitializeWebCam(ref imgVideo);
        }

        private void bntStart_Click(object sender, EventArgs e)
        {
            webcam.Start();
        }

        private void bntStop_Click(object sender, EventArgs e)
        {
            webcam.Stop();
        }

        private void bntContinue_Click(object sender, EventArgs e)
        {
            webcam.Continue();
        }

        private void bntCapture_Click(object sender, EventArgs e)
        {
            imgCapture.Image = imgVideo.Image;
        }

        private Image ResizeImage(Image imgToResize, Size destinationSize)
        {
            int originalWidth = imgToResize.Width;
            int originalHeight = imgToResize.Height;

            //how many units are there to make the original length
            float hRatio = (float)originalHeight / destinationSize.Height;
            float wRatio = (float)originalWidth / destinationSize.Width;

            //get the shorter side
            float ratio = Math.Min(hRatio, wRatio);

            int hScale = Convert.ToInt32(destinationSize.Height * ratio);
            int wScale = Convert.ToInt32(destinationSize.Width * ratio);

            //start cropping from the center
            int startX = (originalWidth - wScale) / 2;
            int startY = (originalHeight - hScale) / 2;

            //crop the image from the specified location and size
            Rectangle sourceRectangle = new Rectangle(startX, startY, wScale, hScale);

            //the future size of the image
            Bitmap bitmap = new Bitmap(destinationSize.Width, destinationSize.Height);

            //fill-in the whole bitmap
            Rectangle destinationRectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            //generate the new image
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel);
            }

            return (Image)bitmap;

        }

        private void bntSave_Click(object sender, EventArgs e)
        {
            if (imgCapture.Image == null)
            {
                XtraMessageBox.Show("Vui lòng chọn Capture để lấy hình trước!");
                return;
            }
            image = ResizeImage(imgCapture.Image, imgCapture.Size);
            this.DialogResult = DialogResult.OK;
        }

        private void bntVideoFormat_Click(object sender, EventArgs e)
        {
            webcam.ResolutionSetting();
        }

        private void bntVideoSource_Click(object sender, EventArgs e)
        {
            webcam.AdvanceSetting();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            webcam.Stop();
            webcam.Dispose();
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            webcam.Start();
        }

        
    }
}
