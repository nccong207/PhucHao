using System;
using System.Collections.Generic;
using System.Text;
using CDTDatabase;
using System.IO;
using System.Data;
using CDTLib;
using System.Timers;
using System.Globalization;

namespace ProductionResult
{
    class Program
    {
        static int minInterval = 10;
        static DataTable dtResultFormat;
        const string lsxName = "OrderNumber";
        const string slName = "FinishJobQTY";
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

            db = Database.NewCustomDatabase(Security.DeCode(ac.GetValue("Connection")));
            dtResultFormat = GetResultFormat();
            return true;
        }

        static void exTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 59)
                timer_Elapsed(timer, null);
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime dtNow = DateTime.Now;

            //read file 1:
            string path = ac.GetValue("DataPath");
            string format = @"{0}\{1}.Y\{2}.Mon\1.{1}{2}{3}.txt";
            ExcuteData(dtNow, format, path);

            //read file 2: 

            string path2 = ac.GetValue("DataPath2");
            string format2 = @"{0}\{1}.Y\{2}.Mon\2.{1}{2}{3}.txt";
            ExcuteData(dtNow, format2, path2);
        }

        public static void ExcuteData(DateTime dtNow, string format, string path)
        {
            string fileName = CopyDataFile(dtNow, format, path);
            if (fileName == string.Empty)
                return;
            int fromLineNo = GetUpdatedRecords(dtNow);
            //int fromLineNo = 0;
            using (DataTable dtResultLog = GetDataFromFile(fileName, fromLineNo))
            {
                if (dtResultLog == null)
                    return;
                if (UpdateResultLog(dtResultLog))
                {
                    WriteLog(LogType.Info, string.Format("{0}: Write result log successfully", DateTime.Now));
                    if (db.UpdateByNonQuery("exec PostPOResult"))
                        WriteLog(LogType.Info, string.Format("{0}: Posting into inventory book successfully", DateTime.Now));
                }
            }
        }

        static string CopyDataFile(DateTime dtToDay, string format, string path)
        {
            //DateTime dtToDay = DateTime.Today;
            //Sample file: D:\2016.Y\03.Mon\1.20160306.txt
            string sourceFileName = string.Format(format, path, dtToDay.Year, dtToDay.Month.ToString("D2"), dtToDay.Day.ToString("D2"));

            if (!File.Exists(sourceFileName))
            {
                WriteLog(LogType.Warning, string.Format("{0}: No data file exists today", DateTime.Now));
                return string.Empty;
            }
            FileInfo fi = new FileInfo(sourceFileName);
            string desFileName = Path.Combine(dataDir, fi.Name);
            try
            {
                File.Copy(sourceFileName, desFileName, true);
            }
            catch (Exception ex)
            {
                WriteLog(LogType.Error, "Cannot copy data file\n" + ex.Message);
                return string.Empty;
            }
            return desFileName;
        }

        static DataTable GetResultFormat()
        {
            return db.GetDataTable("select * from POResultFormat order by OrderNo");
        }

        static DataTable GetResultLog()
        {
            DataTable dt = db.GetDataTable("select * from POResultLog where 1 = 0");
            return dt;
        }

        static bool UpdateResultLog(DataTable dtResultLog)
        {
            return db.UpdateDataTable("select * from POResultLog where 1 = 0", dtResultLog);
        }

        static int GetUpdatedRecords(DateTime dtNow)
        {
            object o = db.GetValue(string.Format("select COUNT(*) from POResultLog where DATEDIFF(dd, UpdatedDate, '{0}') = 0", dtNow));
            return (o == null || o.ToString() == string.Empty) ? 0 : Convert.ToInt32(o);
        }

        static Dictionary<string, object> ParseData(DataTable dtFormat, string rawData)
        {
            Dictionary<string, object> temp = new Dictionary<string, object>();
            int seek = 0;
            foreach (DataRow drFormat in dtFormat.Rows)
            {
                string name = drFormat["Name"].ToString();
                int length = Convert.ToInt32(drFormat["Length"]);
                if (rawData.Length < seek + length)
                    WriteLog(LogType.Error, "There is wrong format data in file: \n" + rawData);
                else
                    temp.Add(name, rawData.Substring(seek, length).Trim());
                seek += length;
            }
            return temp;
        }

        static void AddResultLog(DataTable dtResultLog, Dictionary<string, object> allDataItems)
        {
            try
            {
                DataRow drNew = dtResultLog.NewRow();
                foreach (DataColumn dc in dtResultLog.Columns)
                    if (allDataItems.ContainsKey(dc.ColumnName))
                        drNew[dc.ColumnName] = allDataItems[dc.ColumnName].ToString() == string.Empty ? DBNull.Value : allDataItems[dc.ColumnName];
                drNew["TotalQTY"] = drNew[slName];
                dtResultLog.Rows.Add(drNew);
            }
            catch (Exception ex)
            {
                WriteLog(LogType.Error, "Error when add result log: \n" + ex.Message);
            }
        }

        static DataTable GetDataFromFile(string fileName, int fromLineNo)
        {
            if (!File.Exists(fileName))
            {
                WriteLog(LogType.Error, "Cannot find copied data file");
                return null;
            }
            try
            {
                DataTable dtResultLog = GetResultLog();
                string[] dataLines = File.ReadAllLines(fileName);
                if (fromLineNo >= dataLines.Length)
                {
                    WriteLog(LogType.Warning, string.Format("{0}: No new Orders to update!", DateTime.Now));
                    return null;
                }
                List<string> lstOrders = new List<string>();
                WriteLog(LogType.Info, string.Format("{0}: Start to update production result.", DateTime.Now));
                for (int i = fromLineNo; i < dataLines.Length; i++)
                {
                    string rawData = dataLines[i];
                    Dictionary<string, object> allDataItems = ParseData(dtResultFormat, rawData);
                    if (allDataItems.ContainsKey(lsxName) && allDataItems.ContainsKey(slName))
                    {
                        string soLSX = allDataItems[lsxName].ToString();
                        string strSLSX = allDataItems[slName].ToString();
                        int slSX = 0;
                        //if (lstOrders.Contains(soLSX))
                        //    WriteLog(LogType.Warning, "There is duplicated Order number: " + soLSX);
                        //else
                        if (!Int32.TryParse(strSLSX, out slSX))
                            WriteLog(LogType.Error, "Wrong format of Finish Job Quantity: " + strSLSX);
                        else
                        {
                            lstOrders.Add(soLSX);
                            WriteLog(LogType.Info, string.Format("Finish Job Quantity of Order number {0} is {1}", soLSX, slSX));
                        }
                    }
                    AddResultLog(dtResultLog, allDataItems);
                }
                WriteLog(LogType.Info, string.Format("{0}: Update production result successfully.", DateTime.Now));
                return dtResultLog;
            }
            catch (Exception ex)
            {
                WriteLog(LogType.Error, "Cannot read data file\n" + ex.Message);
                return null;
            }
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
