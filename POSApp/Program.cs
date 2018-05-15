using CDTLib;
using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Threading;
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
            
            
            
            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Length > 1)
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //tuy theo moi soft co productName khac nhau
            string siteCode = "HTC"; //giá trị mặc định
            if (args.Length > 0)
                siteCode = args[0];
            Config.NewKeyValue("SiteCode", siteCode);
            
            InitApp();
            SetEnvironment(siteCode);
            
            RunSplashScreen();

            Login frmLogin = new Login();
            frmLogin.StartPosition = FormStartPosition.CenterScreen;
            frmLogin.ShowDialog();

            Menufrm frmMenu = new Menufrm();
            frmMenu.StartPosition = FormStartPosition.CenterScreen;

            //var main = new Main(frmLogin.drUser);
            //main.StartPosition = FormStartPosition.CenterScreen;
           
            var posMain = new PosMain(frmLogin.drUser);
            posMain.StartPosition = FormStartPosition.CenterScreen;
            int choice = 0;
            DataRow currUser = frmLogin.drUser;
            //dang nhap thanh cong, bat dau su dung chuong trinh
            frmLogin.DialogResult = DialogResult.OK;
            if (frmLogin.DialogResult != DialogResult.Cancel)
            {
            do{
            frmMenu.ShowDialog();
            choice = frmMenu.menuChoice;
            switch (choice) { 
                case 1:
                   posMain.ShowDialog();
                    if (posMain.DialogResult == DialogResult.Cancel) { choice = 0; }
                    break;
                case 4:
                    
                    posMain.ShowDialog();
                    if (posMain.DialogResult == DialogResult.Cancel) { choice = 0; }
                    break;
                case 5: 
                    Settings settings = new Settings();
                    settings.ShowDialog();
                    break;
                case 6:
                    Application.Exit();
                    break;
            }
            }
            while ( choice == 0) ;
                    }

        }
        
        public static void RunSplashScreen()
        {
            Thread newThread = new Thread(Splashx);
            newThread.Start();
            Thread.Sleep(5000);
            newThread.Abort();
        }

        public static void Splashx ()
        {
            Application.Run(new Splash1());
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
