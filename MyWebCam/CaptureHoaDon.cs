using System;
using Plugins;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Data;
using System.Runtime.InteropServices;
using CDTLib;
using XuLyBangIn;
using System.Reflection;

namespace CaptureHoaDon
{
    public class CaptureHoaDon : ICControl
    {
        #region Windows API
        const int SW_RESTORE = 9;
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion

        #region Free2X Webcam Environment
        string fileName = @"\Free2X\Webcam Recorder\WebcamRecorder.exe";
        string pro86Path = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
        string proPath = Environment.GetEnvironmentVariable("ProgramFiles");
        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        #endregion

        private DataCustomFormControl _data;
        private InfoCustomControl _info = new InfoCustomControl(IDataType.MasterDetailDt);
        private PictureEdit peHinhGoc;
        private ZoomPictureEdit peHinhZoom;
        LayoutControl lc;

        #region ICControl Members

        public void AddEvent()
        {
            peHinhGoc = _data.FrmMain.Controls.Find("HoaDon", true)[0] as PictureEdit;
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
            if (peHinhGoc.Properties.ReadOnly)
            {
                XtraMessageBox.Show("Vui lòng thực hiện chức năng này ở chế độ Tạo mới/Cập nhật phiếu!",
                    Config.GetValue("PackageName").ToString());
                return;
            }

            OpenWebcamCapture();

            //thiết lập lắng nghe thư mục hình ảnh của phần mềm webcam để capture file được tạo
            var fileWatcher = new FileSystemWatcher();
            fileWatcher.Path = docPath + @"\Free2x\Webcam Recorder";
            fileWatcher.Filter = "*.jpg";
            fileWatcher.Created += FileWatcher_Created;
            fileWatcher.EnableRaisingEvents = true;
        }

        private void OpenWebcamCapture()
        {
            IntPtr handle = FindWindow("Free2X Webcam Recorder", "Free2X Webcam Recorder");
            if (handle != IntPtr.Zero)
            {
                //phần mềm Webcam đã mở sẵn, thực hiện active lên màn hình
                ShowWindow(handle, SW_RESTORE);
                SetForegroundWindow(handle);
                //tự động capture theo phím tắt mặc định của Free2X
                SendKeys.SendWait("{F12}");
            }
            else
            {
                string file = pro86Path + fileName;
                if (!File.Exists(file))
                    file = proPath + fileName;

                if (!File.Exists(file))
                {
                    XtraMessageBox.Show("Không tìm thấy phần mềm Webcam ở đường dẫn mặc định \n" + file,
                        Config.GetValue("PackageName").ToString());
                    return;
                }
                XtraMessageBox.Show("Phần mềm Webcam chưa mở sẵn, hệ thống sẽ tự động mở phần mềm." +
                    "\nSau khi phần mềm đã mở, phải nhấn Connect để kết nối Webcam trước khi sử dụng!",
                    Config.GetValue("PackageName").ToString());
                var prc = new Process();
                prc.StartInfo.FileName = file;
                prc.Start();
            }
        }

        private void FileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (peHinhGoc.Properties.ReadOnly)
            {
                return;
            }
            try
            {
                FileInfo fi = new FileInfo(e.FullPath);
                while (IsFileLocked(fi)) { }

                var img = Image.FromFile(e.FullPath);
                if (img == null)
                    return;
                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                var converter = new ImageConverter();
                if (peHinhZoom == null)
                    peHinhZoom = _data.FrmMain.Controls.Find("peHinhZoom", true)[0] as ZoomPictureEdit;
                SetControlPropertyThreadSafe(peHinhZoom, "EditValue", converter.ConvertTo(img, typeof(byte[])));

                fi.Delete();
            }
            catch (Exception)
            {
            }
        }

        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        private delegate void SetControlPropertyThreadSafeDelegate(
           Control control,
           string propertyName,
           object propertyValue);

        public static void SetControlPropertyThreadSafe(
            Control control,
            string propertyName,
            object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate
                (SetControlPropertyThreadSafe),
                new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(
                    propertyName,
                    BindingFlags.SetProperty,
                    null,
                    control,
                    new object[] { propertyValue });
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
