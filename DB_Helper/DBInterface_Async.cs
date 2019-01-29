using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel=Microsoft.Office.Interop.Excel;

namespace DB_Helper {
    public enum TEST_TYPE { AFTER = 2, INITIAL = 1 };
    public enum TEST_AREA { CENTERA = 1, CENTERB = 2, CENTERC = 3, RIGHT = 4, TOP = 5, LEFT = 6 };

    class DBInterface_Async {

        private string server, uid, password, database, connectionString;

        public DBInterface_Async() {
            server = "172.20.4.20";
            uid = "aelmendorf";
            password = "Drizzle123!";
            database = "epi";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + "SslMode=none";
        }

        

        public async Task<DataTable> GetSpectrum(string waferID,TEST_TYPE typ) {
            DataTable tbl = new DataTable();
            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    await connect.OpenAsync();
                    string query = "get_spectrum";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@wafer", waferID);
                    cmd.Parameters.AddWithValue("@test", (int)typ);
                    //cmd.ExecuteNonQuery();cmd.ExecuteNonQueryAsync();
                    using(MySqlDataAdapter adp = new MySqlDataAdapter(cmd)) {
                       var t=await adp.FillAsync(tbl);
                    }
                }
                return tbl;
            } catch(MySqlException ex) {
                return null;
            }
        }
    }//End DBInterface_Async


    public class ExcelWrapper {
        public DataSet ds;
        public Excel.Application xlApp;
        public Excel.Workbook xlWB;
        public Excel.Worksheet xlWS;



        public ExcelWrapper() {
            this.ds = new DataSet("Wafer Spectrum");

        }

        public async Task<DataTable> GetWaferSpectrum(string wafer,TEST_TYPE type) {


            return null;
        }

        public void GetSpectrum(string[] wafers, TEST_TYPE[] types) {
            List<Task<DataTable>> tasks = new List<Task<DataTable>>();
            for(int i = 0; i < wafers.Length; i++) {
                
                tasks.Add(this.GetWaferSpectrum(wafers[i], types[i]));
            }

            Task.WaitAll(tasks.ToArray());

            foreach(Task<DataTable> task in tasks) {
                this.ds.Tables.Add(task.Result);
            }

        }

        public void WriteOut() {

        }




    }
}
