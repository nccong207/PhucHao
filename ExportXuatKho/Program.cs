using CDTDatabase;
using CDTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Timers;

namespace ExportXuatKho
{
    class Program
    {
        static int minInterval = 15;
        static DataTable dtResultFormat;
        //const string lsxName = "OrderNumber";
        //const string slName = "FinishJobQTY";
        static Database db;
        static Timer timer;
        static AppCon ac = new AppCon();
        static string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        static string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
        enum LogType { Info, Warning, Error };

        static void Main(string[] args)
        {
            try
            {
                if (!InitConfiguration())
                {
                    Console.ReadLine();
                    return;
                }

                timer = new Timer();
                timer.AutoReset = true;
                timer.Interval = Convert.ToDouble(ac.GetValue("Interval")) * 1000 * 60;
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                timer.Enabled = true;
                timer.Start();
                timer_Elapsed(timer, null);

                //the second timer to run at the end of day (23:59:00)
                Timer exTimer = new Timer();
                exTimer.AutoReset = true;
                exTimer.Interval = 1000 * 60;
                exTimer.Elapsed += new ElapsedEventHandler(exTimer_Elapsed);
                exTimer.Enabled = true;
                exTimer.Start();
            }
            catch (Exception ex)
            {
                WriteLog(LogType.Error, string.Format("{0}: Unhandled error occurs: {1}\n{2}",
                    DateTime.Now, ex.Message, ex.StackTrace));
            }
            Console.ReadLine();
        }

        static void exTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 59)
                timer_Elapsed(timer, null);
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Quét qua database POS để lấy yêu cầu xuất kho,
            string getYeuCau = @"SELECT MaCuon, sum(SoLuongBD) as SoLuongBD, sum(SoLuongSD) as SoLuongSD, sum(SoLuongCL) as SoLuongCL
                                    FROM YeuCauXuatKho WHERE Duyet = 1 AND GioXuatKho is NULL AND MaCTXuatKho is NULL GROUP BY MaCuon";

            DataTable dtYeuCau = db.GetDataTable(getYeuCau);
            if (dtYeuCau.Rows.Count == 0) return;
            var mtid = Guid.NewGuid().ToString();
            foreach (DataRow row in dtYeuCau.Rows)
            {
                  
                string macuon = row["MaCuon"].ToString();
                if (string.IsNullOrEmpty(macuon)) continue;

                //Sau khi hoàn thành, sẽ cập nhật số tồn kho tức thời trong HT (bảng TonKhoNL) 
                var mact = "XSX";
                var NgayCT = DateTime.Now;
                var soct = GetSoCT(NgayCT);
                var makho = "KNVL01";
                var DienGiai = "Xuất tự động";
                db = Database.NewCustomDatabase(Security.DeCode(ac.GetValue("Connection")));
                DataTable DT42IDtb = db.GetDataTable(string.Format("SELECT TOP 1 * FROM DT42 WHERE MaCuon = '{0}'", macuon));
                if (DT42IDtb.Rows.Count == 0) continue;  
                      
                var DT42ID = DT42IDtb.Rows[0]["DT42ID"].ToString();
                string manl = DT42IDtb.Rows[0]["MaNL"].ToString();
                var ThanhTien = Convert.ToDecimal(DT42IDtb.Rows[0]["SoLuong"]) * Convert.ToDecimal(DT42IDtb.Rows[0]["DonGia"]);
                var soluong_x = row["SoLuongSD"].ToString(); //TODO

                //insert into table BLNL
                db = Database.NewCustomDatabase(Security.DeCode(ac.GetValue("Connection")));
                string insertSQL = @"INSERT INTO BLNL(MaCT, MTID, SoCT, NgayCT, DienGiai, PsNo, PsCo, NhomDk, DT42ID, SoLuong, Soluong_x, MTIDDT, MaNL, KyHieu, MaKho)
                                              VALUES('XSX', '{0}', '{1}', '{2}', N'{3}',     0,  '{4}','XSX1', '{5}',   '0',      '{6}',   '{7}' , '{8}', NULL, '{9}')";
                db.UpdateByNonQuery(string.Format(insertSQL, mtid, soct, NgayCT, DienGiai, ThanhTien, DT42ID, soluong_x, Guid.NewGuid().ToString(), manl, makho));

                db = Database.NewCustomDatabase(Security.DeCode(ac.GetValue("ConnectionPOS")));
                string updateSQl = "UPDATE YeuCauXuatKho SET GioXuatKho = '{0}', MaCTXuatKho = '{1}' WHERE MaCuon = '{2}'";
                db.UpdateByNonQuery(string.Format(updateSQl, NgayCT, soct, macuon));
            }
        }

        private static string GetSoCT ( DateTime ngayct)
        {
            db = Database.NewCustomDatabase(Security.DeCode(ac.GetValue("Connection")));
            string newSoctBlnl = "";
            var Thang = ngayct.Month.ToString();
            var Nam = ngayct.Year.ToString();

            if (Thang.Length == 1)
                Thang = "0" + Thang;
            Nam = Nam.Substring(2, 2);

            string suffix = "-" + Thang + Nam;

            string sql = string.Format(@"SELECT Top 1 SoCT FROM BLNL WHERE SoCT LIKE '{0}%{1}' ORDER BY SoCT DESC", "XSX-", suffix);
            object newsoct = db.GetValue(sql);
            if (newsoct == null)
            {
                newSoctBlnl = "XSX-0001" + suffix;
            } else
            {
                string currentNum = newsoct.ToString().Substring(4, 4);
                newSoctBlnl = "XSX-" + (Convert.ToInt32(currentNum) + 1).ToString("D4") + suffix;
            }
            return newSoctBlnl;
        }

        static bool InitConfiguration()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            if (Convert.ToDouble(ac.GetValue("Interval")) < minInterval)
            {
                WriteLog(LogType.Error, string.Format("Cannot set interval less than {0} minutes", minInterval));
                return false;
            }

            if (!Directory.Exists(dataDir))
                Directory.CreateDirectory(dataDir);

            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            db = Database.NewCustomDatabase(Security.DeCode(ac.GetValue("ConnectionPOS")));
            return true;
        }

        static void WriteLog(LogType logType, string content)
        {
            if (logType == LogType.Error)
                Console.ForegroundColor = ConsoleColor.Red;
            if (logType == LogType.Warning)
                Console.ForegroundColor = ConsoleColor.Yellow;
            if (logType == LogType.Info)
                Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(content);
            string logFile = Path.Combine(logDir, DateTime.Today.ToString("yyyyMMdd") + ".txt");
            File.AppendAllText(logFile, string.Format("[{0}] {1}\r\n", logType, content));
        }
    }
}
