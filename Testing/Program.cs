using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInterface;
using System.Diagnostics;
using System.IO;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

namespace Testing {
    class Program {
        public static void Main(string[] args) {
            LV_Interface db = new LV_Interface();
            string server, uid, password, database, connectionString;


            server = "172.20.4.20";
            uid = "aelmendorf";
            password = "Drizzle123!";
            database = "epi";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + "SslMode=none";

            //db.NewWaferEntry("AE-abcd-02");
            //db.LogData50mA("AE-abcd-02", TEST_TYPE.INITIAL, TEST_AREA.CENTERA, 275.65, 900.45, 52.25, 2.45);
            //db.LogData50mA("AE-abcd-02", TEST_TYPE.INITIAL, TEST_AREA.LEFT, 275.65, 900.45, 52.25, 2.45);
            //db.LogData50mA("AE-abcd-02", TEST_TYPE.INITIAL, TEST_AREA.RIGHT, 275.65, 900.45, 52.25, 2.45);

            //db.LogData50mA("AE-abcd-02", TEST_TYPE.AFTER, TEST_AREA.CENTERA, 275.65, 900.45, 52.25, 2.45);
            //db.LogData50mA("AE-abcd-02", TEST_TYPE.AFTER, TEST_AREA.LEFT, 275.65, 900.45, 52.25, 2.45);
            //db.LogData50mA("AE-abcd-02", TEST_TYPE.AFTER, TEST_AREA.RIGHT, 275.65, 900.45, 52.25, 2.45);
            //Console.WriteLine(db.DeleteEntry("Andrew_50mA_Test"));
            //Console.WriteLine(db.DeleteEntry("AndrewTest123"));
            //DataTable tbl = new DataTable();
            //Table dat = new Table();
            //using(MySqlConnection connect = new MySqlConnection(connectionString)) {

            //    connect.Open();
            //    string query = "getWaferData_LB";
            //    MySqlCommand cmd = new MySqlCommand(query, connect);
            //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //    cmd.Prepare();
            //    cmd.Parameters.AddWithValue("@wafer", "B01-1485-05");
            //    cmd.Parameters.AddWithValue("@test", 1);
            //    var reader = cmd.ExecuteReader();
            //    if(reader.Read()) {
            //        dat.Add(new TestData(TEST_AREA.CENTERA, (double)reader[0], (double)reader[1], (double)reader[2], (double)reader[3]));
            //        dat.Add(new TestData(TEST_AREA.CENTERB, (double)reader[4], (double)reader[5], (double)reader[6], (double)reader[7]));
            //        dat.Add(new TestData(TEST_AREA.CENTERC, (double)reader[8], (double)reader[9], (double)reader[10], (double)reader[11]));
            //        dat.Add(new TestData(TEST_AREA.RIGHT, (double)reader[12], (double)reader[13], (double)reader[14], (double)reader[15]));
            //        dat.Add(new TestData(TEST_AREA.TOP, (double)reader[16], (double)reader[17], (double)reader[18], (double)reader[19]));
            //        dat.Add(new TestData(TEST_AREA.LEFT, (double)reader[20], (double)reader[21], (double)reader[22], (double)reader[23]));
            //    }
            //}
            //foreach(TestData row in dat) {
            //    Console.WriteLine("Power: {0}", row.Power);
            //}


            Table table = db.GetWaferData("B01-1485-05", TEST_TYPE.INITIAL);

            Console.WriteLine("Initial Data");
            foreach(TestData row in table) {
                Console.WriteLine(row.ToString());
            }


            //int okay = db.CopyNewName("B02-0152-10", "B05-222-555");

            //if(okay == 0) {
            //    Table init = db.GetWaferData("B05-222-555", TEST_TYPE.INITIAL);
            //    Table after = db.GetWaferData("B05-222-555", TEST_TYPE.AFTER);

            //    Console.WriteLine("Initial Data");
            //    foreach(TestData row in init) {
            //        Console.WriteLine(row.ToString());
            //    }

            //    Console.WriteLine("After Data");
            //    foreach(TestData row in after) {
            //        Console.WriteLine(row.ToString());
            //    }
            //} else {
            //    string error=(okay<0) ? "MYSQL Exception":"Already exist";
            //    Console.WriteLine("Error: "+error);
            //}



            Console.WriteLine("Press any key to exit");
            Console.Read();
        }

        public static void GetSpectrum() {
            LV_Interface test = new LV_Interface();
            DataTable tbl= test.GetSpectrum("BO2-0116-05", TEST_TYPE.INITIAL);

            Dictionary<string, double[]> values = new Dictionary<string, double[]>();
            List<double> wlArrs = new List<double>();
            List<double> spectArrs = new List<double>();

            DataTable tblNew = new DataTable();


            foreach(DataRow row in tbl.Rows) {
                foreach(DataColumn col in tbl.Columns) {
                    tblNew.Columns.Add(col.ColumnName);
                    if(!DBNull.Value.Equals(row[col])) {
                        values.Add(col.ColumnName, JsonConvert.DeserializeObject<double[]>((string)row[col]));
                        Console.WriteLine(row[col]);
                    } else {
                        values.Add(col.ColumnName, new double[2048]);
                    }

                }
            }

            foreach(KeyValuePair<string,double[]> item in values) {
                for(int i = 0; i < item.Value.Length; i++) {
                    tblNew.Rows.Add();
                }
            }


            for(int y=0;y<tblNew.Columns.Count;y++) {
                for(int i = 0; i < values[tblNew.Columns[y].ColumnName].Length; i++) {
                    tblNew.Rows[i][y] = values[tblNew.Columns[y].ColumnName][i];
                }
            }


            tblNew.ExportToExcel(@"C:\dtToExcel4");
            Console.ReadKey();
        }

        public static void InsertWl() {
                LV_Interface test = new LV_Interface();
                test.NewSpectrumEntry("11-4501");
                Stopwatch sw = new Stopwatch();
                TEST_AREA[] area = { TEST_AREA.CENTERA, TEST_AREA.CENTERB, TEST_AREA.CENTERC, TEST_AREA.RIGHT, TEST_AREA.TOP, TEST_AREA.LEFT };
                string[] files = { "CenterA_20sp_Initial.dat", "CenterB_20sp_Initial.dat", "CenterC_20sp_Initial.dat", "Right_20sp_Initial.dat", "Top_20sp_Initial.dat", "Left_20sp_Initial.dat" };
                long[] times = new long[files.Length];
                for(int i = 0; i < files.Length; i++) {
                    string path = @"\\172.20.4.11\Data\Characterization Raw Data\Quick EL Test\B01-0997-15\" + files[i];
                    FileInfo info = new FileInfo(path);
                    if(info.Exists && info.Length != 0) {
                        string[] lines = System.IO.File.ReadAllLines(@"\\172.20.4.11\Data\Characterization Raw Data\Quick EL Test\B01-0997-15\" + files[i]);
                        double[] wl = new double[lines.Length];
                        double[] spect = new double[lines.Length];
                        for(int x = 0; x < lines.Length; x++) {
                            var col = lines[x].Split('\t');
                            if(col.Length == 2) {
                                wl[x] = Convert.ToDouble(col[0]);
                                spect[x] = Convert.ToDouble(col[1]);
                            }
                        }
                        sw.Reset();
                        sw.Start();
                        test.UpdateSpectrum("11-4500",TEST_TYPE.INITIAL,area[i], wl, spect,wl,spect);
                        sw.Stop();
                        times[i] = sw.ElapsedMilliseconds;
                        Console.WriteLine(files[i] + " done");
                    } else {
                        times[i] = 0;
                    }
                }

                for(int i = 0; i < times.Length; i++) {
                    Console.WriteLine("Time: " + times[i].ToString());
                }

                Console.WriteLine("Total: " + times.Sum());

            Console.WriteLine("Press any ket to export to excel");
            Console.ReadKey();

        }




        static int Allocate() {
            // Compute total count of digits in strings.
            int size = 0;
            for(int z = 0; z < 100; z++) {
                for(int i = 0; i < 1000000; i++) {
                    string value = i.ToString();
                    if(value == null) {
                        return 0;
                    }
                    size += value.Length;
                }
            }
            return size;
        }


    }



   /* public class AsyncDB {
        public async Task InsertFromText() {
            LV_Interface test = new LV_Interface();
            TEST_AREA[] area = { TEST_AREA.CENTERA, TEST_AREA.CENTERB, TEST_AREA.CENTERC, TEST_AREA.RIGHT, TEST_AREA.TOP, TEST_AREA.LEFT };
            string[] lines = File.ReadAllLines(@"C:\DB_DATA.txt");
            int[] offsets = { 2, 6, 10, 14, 18, 22};
            List<int> resp = new List<int>();
            foreach(string line in lines) {
                string[] cols = line.Split('\t');
                //test.NewWaferEntry(cols[0]);
                for(int i = 0; i < 6; i++) {
                    await test.Update(cols[0], TEST_TYPE.AFTER, area[i], cols[1], true, Convert.ToDouble(cols[offsets[i]]),
                        Convert.ToDouble(cols[offsets[i] + 1]), 
                        Convert.ToDouble(cols[offsets[i] + 2]),
                        Convert.ToDouble(cols[offsets[i] + 3]));
                }
            }
        }
    }*/

    public static class My_DataTable_Extensions {
        /// <summary>
        /// Export DataTable to Excel file
        /// </summary>
        /// <param name="DataTable">Source DataTable</param>
        /// <param name="ExcelFilePath">Path to result file name</param>
        public static void ExportToExcel(this System.Data.DataTable DataTable, string ExcelFilePath = null) {
            try {
                int ColumnsCount;

                if(DataTable == null || (ColumnsCount = DataTable.Columns.Count) == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");

                // load excel, and create a new workbook
                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbooks.Add();

                // single worksheet
                Microsoft.Office.Interop.Excel._Worksheet Worksheet = Excel.ActiveSheet;

                object[] Header = new object[ColumnsCount];

                // column headings               
                for(int i = 0; i < ColumnsCount; i++)
                    Header[i] = DataTable.Columns[i].ColumnName;

                Microsoft.Office.Interop.Excel.Range HeaderRange = Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, ColumnsCount]));
                HeaderRange.Value = Header;

                HeaderRange.Font.Bold = true;

                // DataCells
                int RowsCount = DataTable.Rows.Count;
                object[,] Cells = new object[RowsCount, ColumnsCount];

                for(int j = 0; j < RowsCount; j++)
                    for(int i = 0; i < ColumnsCount; i++)
                        Cells[j, i] = DataTable.Rows[j][i];

                Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[2, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[RowsCount + 1, ColumnsCount])).Value = Cells;

                // check fielpath
                if(ExcelFilePath != null && ExcelFilePath != "") {
                    try {
                        Worksheet.SaveAs(ExcelFilePath);
                        Excel.Quit();
                    } catch(Exception ex) {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                            + ex.Message);
                    }
                } else    // no filepath is given
                  {
                    Excel.Visible = true;
                }
            } catch(Exception ex) {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }
    }
}
