using System;
using System.Collections.Generic;
using System.Text;
using CDTLib;
using CDTDatabase;
using System.IO;
using System.Data;

namespace UpdatePOResult
{
    class Program
    {
        static DataTable dtResultFormat;
        const string lsxName = "OrderNumber";
        const string slName = "CuttingQTY";
        static Database db;
        //static AppCon ac = new AppCon();
        static void Main(string[] args)
        {
            string cnn = @"Server =113.161.95.123\HOATIEU2K8R2,1436; database = HTCPH; user = sa; pwd = Makiut123";
            //db = Database.NewCustomDatabase(Security.DeCode(ac.GetValue("Connection")));
            db = Database.NewCustomDatabase(cnn);
            if (dtResultFormat == null)
                dtResultFormat = GetResultFormat();
            string fileName = CopyDataFile();
            if (fileName == string.Empty)
                return;
            Dictionary<string, object> results = GetDataFromFile(fileName, 0);
        }

        static string CopyDataFile()
        {
            string dataPath = "D:\\";
            DateTime dtToDay = DateTime.Today;
            //Sample file: D:\2016.Y\03.Mon\1.20160306.txt
            string sourceFileName = string.Format(@"{0}\{1}.Y\{2}.Mon\1.{1}{2}{3}.txt", dataPath, //ac.GetValue("DataPath"),
                dtToDay.Year, dtToDay.Month.ToString("D2"), dtToDay.Day.ToString("D2"));
            if (!File.Exists(sourceFileName))
            {
                Console.WriteLine("The data file does not exist today");
                return string.Empty;
            }
            FileInfo fi = new FileInfo(sourceFileName);
            string desFileName = Path.Combine(Environment.CurrentDirectory, fi.Name);
            try
            {
                File.Copy(sourceFileName, desFileName, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot copy data file\n" + ex.Message);
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
            return db.GetDataTable("select * from POResultLog where 1 = 0");
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
                    Console.WriteLine("There is wrong format data in file: \n" + rawData);
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
                        drNew[dc.ColumnName] = allDataItems[dc.ColumnName];
                dtResultLog.Rows.Add(drNew);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when add result log: \n" + ex.Message);
            }
        }

        static Dictionary<string, object> GetDataFromFile(string fileName, int fromLineNo)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Cannot find copied data file");
                return null;
            }
            try
            {
                string[] dataLines = File.ReadAllLines(fileName);
                if (fromLineNo > dataLines.Length)
                {
                    Console.WriteLine("The number of data file lines less than the line number need to read");
                    return null;
                }
                
                int linesToRead = dataLines.Length - fromLineNo;
                Dictionary<string, object> dicData = new Dictionary<string, object>(linesToRead);
                DataTable dtResultLog = GetResultLog();
                for (int i = fromLineNo; i < linesToRead; i++)
                {
                    string rawData = dataLines[i];
                    Dictionary<string, object> allDataItems = ParseData(dtResultFormat, rawData);
                    if (allDataItems.ContainsKey(lsxName) && allDataItems.ContainsKey(slName))
                        dicData.Add(allDataItems[lsxName].ToString(), allDataItems[slName]);
                    AddResultLog(dtResultLog, allDataItems);
                }
                return dicData;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot read data file\n" + ex.Message);
                return null;
            }
        }
    }
}
