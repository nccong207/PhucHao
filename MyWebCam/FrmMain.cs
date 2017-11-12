using System;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using Camera_NET;



namespace MyWebCam
{

    public partial class FrmMain : DevExpress.XtraEditors.XtraForm
    {
        public Image image;
        private CameraChoice _CameraChoice = new CameraChoice();

        public FrmMain()
        {
            InitializeComponent();
        }
        private void mainWinForm_Load(object sender, EventArgs e)
        {
            //webcam = new WebCam();
            //webcam.InitializeWebCam(ref imgVideo);

            FillCameraList();

            // Select the first one
            if (comboBoxCameraList.Items.Count > 0)
            {
                comboBoxCameraList.SelectedIndex = 0;
            }

            // Fill camera list combobox with available resolutions
            FillResolutionList();
        }
        #region Camera and resolution selection

        private void FillCameraList()
        {
            comboBoxCameraList.Items.Clear();

            _CameraChoice.UpdateDeviceList();

            foreach (var camera_device in _CameraChoice.Devices)
            {
                comboBoxCameraList.Items.Add(camera_device.Name);
            }
        }

        private void FillResolutionList()
        {
            comboBoxResolutionList.Items.Clear();

            if (!cameraControl.CameraCreated)
                return;

            ResolutionList resolutions = Camera.GetResolutionList(cameraControl.Moniker);

            if (resolutions == null)
                return;

            int index_to_select = -1;

            for (int index = 0; index < resolutions.Count; index++)
            {
                comboBoxResolutionList.Items.Add(resolutions[index].ToString());

                if (resolutions[index].CompareTo(cameraControl.Resolution) == 0)
                {
                    index_to_select = index;
                }
            }

            // select current resolution
            if (index_to_select >= 0)
            {
                comboBoxResolutionList.SelectedIndex = index_to_select;
            }
        }

        private void comboBoxCameraList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCameraList.SelectedIndex < 0)
            {
                cameraControl.CloseCamera();
            }
            else
            {
                // Set camera
                cameraControl.SetCamera(_CameraChoice.Devices[comboBoxCameraList.SelectedIndex].Mon, null);
                //SetCamera(_CameraChoice.Devices[ comboBoxCameraList.SelectedIndex ].Mon, null);
            }

            FillResolutionList();
        }

        private void comboBoxResolutionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cameraControl.CameraCreated)
                return;

            int comboBoxResolutionIndex = comboBoxResolutionList.SelectedIndex;
            if (comboBoxResolutionIndex < 0)
            {
                return;
            }
            ResolutionList resolutions = Camera.GetResolutionList(cameraControl.Moniker);

            if (resolutions == null)
                return;

            if (comboBoxResolutionIndex >= resolutions.Count)
                return; // throw

            if (0 == resolutions[comboBoxResolutionIndex].CompareTo(cameraControl.Resolution))
            {
                // this resolution is already selected
                return;
            }

            // Recreate camera
            //SetCamera(_Camera.Moniker, resolutions[comboBoxResolutionIndex]);
            cameraControl.SetCamera(cameraControl.Moniker, resolutions[comboBoxResolutionIndex]);

        }

        #endregion

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

        private void bntStart_Click(object sender, EventArgs e)
        {
            cameraControl.Camera.RunGraph();
        }

        private void bntStop_Click(object sender, EventArgs e)
        {
            cameraControl.Camera.StopGraph();
        }

        private void bntCapture_Click(object sender, EventArgs e)
        {
            imgCapture.Image = cameraControl.SnapshotOutputImage();
            imgCapture.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }
        
        private void bntSave_Click(object sender, EventArgs e)
        {
            if (imgCapture.Image == null)
            {
                XtraMessageBox.Show("Vui lòng chọn Capture để lấy hình trước!");
                return;
            }
            var stream = new System.IO.MemoryStream();
            imgCapture.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            image = Image.FromStream(stream);

            this.DialogResult = DialogResult.OK;
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            cameraControl.CloseCamera();
        }

        private void FrmMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
                bntCapture_Click(bntCapture, new EventArgs());
            if (e.KeyCode == Keys.F12)
                bntSave_Click(bntSave, new EventArgs());
        }
    }
}
