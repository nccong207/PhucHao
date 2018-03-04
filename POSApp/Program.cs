using CDTLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace POSApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //tuy theo moi soft co productName khac nhau
            string siteCode = "HTC"; //giá trị mặc định
            if (args.Length > 0)
                siteCode = args[0];
            Config.NewKeyValue("SiteCode", siteCode);

            InitApp();
            SetEnvironment(siteCode);
            Login frmLogin = new Login();
            frmLogin.StartPosition = FormStartPosition.CenterScreen;
            frmLogin.ShowDialog();

            var mainFrm = new Main(frmLogin.drUser);
            mainFrm.StartPosition = FormStartPosition.CenterScreen;
            //dang nhap thanh cong, bat dau su dung chuong trinh
            if (frmLogin.DialogResult != DialogResult.Cancel)
                Application.Run(mainFrm);

        }

        private static void InitApp()
        {
        }

        private static void SetEnvironment(string siteCode)
        {
            CultureInfo CultureInfo = Application.CurrentCulture.Clone() as CultureInfo;
            CultureInfo = new CultureInfo("en-US");
            DateTimeFormatInfo dtInfo = new DateTimeFormatInfo();
            dtInfo.LongDatePattern = "MM/dd/yyyy h:mm:ss tt";
            dtInfo.ShortDatePattern = "MM/dd/yyyy";
            CultureInfo.DateTimeFormat = dtInfo;
            Application.CurrentCulture = CultureInfo;
            Config.NewKeyValue("PackageName", "Phần mềm Quản lý Giấy cuộn tại xưởng");
            //lay chuoi ket noi
            AppCon ac = new AppCon();
            string posDb = ac.GetValue("POSDb");
            posDb = Security.DeCode(posDb);
            Config.NewKeyValue("DataConnection", posDb);

            string longwayDb = ac.GetValue("LongwayDb");
            longwayDb = Security.DeCode(longwayDb);
            Config.NewKeyValue("StructConnection", longwayDb);
        }
    }
}
