using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Data;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DBInterface {

    public enum TEST_TYPE { AFTER = 2, INITIAL = 1 };
    public enum TEST_AREA { CENTERA = 1, CENTERB = 2, CENTERC = 3, RIGHT = 4, TOP = 5, LEFT = 6 };

    public class LV_Interface : IDisposable {
        private string server, uid, password, database, connectionString;

        public LV_Interface() {
            server = "172.20.4.20";
            uid = "aelmendorf";
            password = "Drizzle123!";
            database = "epi";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + "SslMode=none";
        }

        public string NewWaferEntry(string wafer) {
            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "new_entry";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@wafer", wafer);
                    cmd.ExecuteNonQuery();
                }
                return "success";
            }catch(MySqlException ex) {
                return ex.ToString();
            }
        }

        public string NewSpectrumEntry(string wafer) {
            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "spectrum_initial_entry";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@wafer", wafer);
                    cmd.ExecuteNonQuery();
                }
                return "success";
            } catch(MySqlException ex) {
                return ex.ToString();
            }
        }

        public string DeleteEntry(string wafer) {
            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "delete_entry";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@wafer", wafer);
                    cmd.ExecuteNonQuery();
                }
                return "success";
            } catch(MySqlException ex) {
                return ex.ToString();
            }
        }

        public string MarkTested(string wafer, TEST_TYPE type, bool isTested) {
            int val = 0;
            if(isTested) {
                val = 1;
            }
            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "mark_tested";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@wafer", wafer);
                    cmd.Parameters.AddWithValue("@test", (int)type);
                    cmd.Parameters.AddWithValue("@isTested", val);
                    cmd.ExecuteNonQuery();
                }
                return "Success";
            } catch(MySqlException ex) {
                return ex.ToString();
            }
        }

        public int Exist(string wafer, TEST_TYPE type) {
            int retVal = 0;
            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "check";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@wafer", wafer);
                    cmd.Parameters.AddWithValue("@test", (int)type);
                    cmd.Parameters.AddWithValue("?isentry", MySqlDbType.Int32);
                    cmd.Parameters["?isentry"].Direction = System.Data.ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    retVal = (int)cmd.Parameters["?isentry"].Value;
                }
                return retVal;
            } catch(MySqlException ex) {
                return -1;
            }
        }

        public int Tested(string wafer, TEST_TYPE type) {
            int retVal = 0;
            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "check_tested";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@wafer", wafer);
                    cmd.Parameters.AddWithValue("@test", (int)type);
                    cmd.Parameters.AddWithValue("?isTested", MySqlDbType.Int32);
                    cmd.Parameters["?isTested"].Direction = System.Data.ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    retVal = (int)cmd.Parameters["?isTested"].Value;
                }
                return retVal;
            } catch(MySqlException ex) {
                return -1;
            }
        }

        public int UpdateSpectrum(string wafer,TEST_TYPE type,TEST_AREA area, double[] wl, double[] spect, double[] wl50mA, double[] spect50mA) {
            int retVal = 0;
            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "epi_update_spectrum";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@wafer", wafer);
                    cmd.Parameters.AddWithValue("@test", (int)type);
                    cmd.Parameters.AddWithValue("@area", (int)area);
                    var wlout = JsonConvert.SerializeObject(wl);
                    cmd.Parameters.AddWithValue("@wl", wlout);
                    var spectout = JsonConvert.SerializeObject(spect);
                    cmd.Parameters.AddWithValue("@spect", spectout);

                    var wlout50 = JsonConvert.SerializeObject(wl50mA);
                    cmd.Parameters.AddWithValue("@wl50mA", wlout50);
                    var spectout50 = JsonConvert.SerializeObject(spect50mA);
                    cmd.Parameters.AddWithValue("@spect50mA", spectout50);

                    cmd.ExecuteNonQuery();
                }
                return retVal;
            }catch(MySqlException ex) {
                return -1;
            }
        }

        public string LogData(string wafer, TEST_TYPE type, TEST_AREA area, double wl, double power, double voltage, double knee) {

            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "epi_update";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@wafer", wafer);
                    cmd.Parameters.AddWithValue("@test", (int)type);
                    cmd.Parameters.AddWithValue("@area", (int)area);
                    cmd.Parameters.AddWithValue("@wl", wl);
                    cmd.Parameters.AddWithValue("@power", power);
                    cmd.Parameters.AddWithValue("@voltage", voltage);
                    cmd.Parameters.AddWithValue("@knee", knee);
                    cmd.ExecuteNonQuery();
                }
                return "success";
            } catch(MySqlException ex) {
                return ex.ToString();
            }
        }

        public string LogData50mA(string wafer, TEST_TYPE type, TEST_AREA area, double wl, double power, double voltage, double knee) {

            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "epi_update_50ma";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@wafer", wafer);
                    cmd.Parameters.AddWithValue("@test", (int)type);
                    cmd.Parameters.AddWithValue("@area", (int)area);
                    cmd.Parameters.AddWithValue("@wl", wl);
                    cmd.Parameters.AddWithValue("@power", power);
                    cmd.Parameters.AddWithValue("@voltage", voltage);
                    cmd.Parameters.AddWithValue("@knee", knee);
                    cmd.ExecuteNonQuery();
                }
                return "success";
            } catch(MySqlException ex) {
                return ex.ToString();
            }
        }

        public int CopyNewName(string waferOld, string waferNew) {
            int retVal = 0;
            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "copy_newname";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@waferOld", waferOld);
                    cmd.Parameters.AddWithValue("@waferNew", waferNew);
                    cmd.Parameters.AddWithValue("?isError", MySqlDbType.Int32);
                    cmd.Parameters["?isError"].Direction = System.Data.ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    object value = cmd.Parameters["?isError"].Value;
                    if(!DBNull.Value.Equals(value)) {
                        retVal = (int)value;
                    } else {
                        retVal = 0;
                    }
                }
                return retVal;
            } catch(MySqlException ex) {
                return -1;
            }
        }//End

        public string[] GetWaferBySystem(string system, TEST_TYPE type) {
            List<string> wafers = new List<string>();
            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "get_waferlist";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@sys", system);
                    cmd.Parameters.AddWithValue("@test", (int)type);
                    cmd.ExecuteNonQuery();
                    var reader = cmd.ExecuteReader();
                    while(reader.Read()) {
                        wafers.Add((string)reader[0]);
                    }

                }
                return wafers.ToArray();
            } catch(MySqlException ex) {
                return null;
            }
        }

        public Table GetWaferData(string wafer, TEST_TYPE type) {
            DataTable tbl = new DataTable();
            Table dat = new Table();
            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "getWaferData";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@wafer",wafer );
                    cmd.Parameters.AddWithValue("@test", (int)type);
                    var reader = cmd.ExecuteReader();
                    if(reader.Read()) {
                        dat.Add(new TestData(TEST_AREA.CENTERA, (double)reader[0], (double)reader[1], (double)reader[2], (double)reader[3]));
                        dat.Add(new TestData(TEST_AREA.CENTERB, (double)reader[4], (double)reader[5], (double)reader[6], (double)reader[7]));
                        dat.Add(new TestData(TEST_AREA.CENTERC, (double)reader[8], (double)reader[9], (double)reader[10], (double)reader[11]));
                        dat.Add(new TestData(TEST_AREA.RIGHT, (double)reader[12], (double)reader[13], (double)reader[14], (double)reader[15]));
                        dat.Add(new TestData(TEST_AREA.TOP, (double)reader[16], (double)reader[17], (double)reader[18], (double)reader[19]));
                        dat.Add(new TestData(TEST_AREA.LEFT, (double)reader[20], (double)reader[21], (double)reader[22], (double)reader[23]));
                    }
                }
                return dat;
            } catch(MySqlException ex) {
                return null;
            }
        }

        public DataTable GetSpectrum(string wafer,TEST_TYPE typ) {
            DataTable tbl = new DataTable();
            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "get_spectrum";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@wafer", wafer);
                    cmd.Parameters.AddWithValue("@test", (int)typ);
                    cmd.ExecuteNonQuery();
                    using(MySqlDataAdapter adp=new MySqlDataAdapter(cmd)) {
                        adp.Fill(tbl);
                    }
                }
                return tbl;
            } catch(MySqlException ex) {
                return null;
            }
        }

        public string GetWaferList(string wafer) {
            try {
                using(MySqlConnection connect = new MySqlConnection(connectionString)) {
                    connect.Open();
                    string query = "epi_update";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@wafer", wafer);

                    cmd.ExecuteNonQuery();
                }
                return "success";
            } catch(MySqlException ex) {
                return ex.ToString();
            }
        }

        public void Dispose() {

        }
    }

    public class Table : IEnumerable {
        private List<TestData> rows;

        public Table() {
            this.rows =new List<TestData>();
        }

        public Table(DataTable tbl) {
            this.Fill(tbl);
        }

        private void Fill(DataTable tbl) {
            foreach(DataRow row in tbl.Rows) {
                this.rows.Add(new TestData(TEST_AREA.CENTERA, (double)row[0], (double)row[1], (double)row[2], (double)row[3]));
                this.rows.Add(new TestData(TEST_AREA.CENTERB, (double)row[4], (double)row[5], (double)row[6], (double)row[7]));
                this.rows.Add(new TestData(TEST_AREA.CENTERC, (double)row[8], (double)row[9], (double)row[10], (double)row[11]));
                this.rows.Add(new TestData(TEST_AREA.RIGHT, (double)row[12], (double)row[13], (double)row[14], (double)row[15]));
                this.rows.Add(new TestData(TEST_AREA.TOP, (double)row[16], (double)row[17], (double)row[18], (double)row[19]));
                this.rows.Add(new TestData(TEST_AREA.LEFT, (double)row[20], (double)row[21], (double)row[22], (double)row[23]));
            }
        }

        public int Count {
            get {
                return this.rows.Count();
            }
        }

        public void Add(TestData row) {
            this.rows.Add(row);
        }

        public TestData GetData(int index) {
            return this.rows[index];
        }

        public TestData this[int index] {
            get {
                return this.rows[index];
            }
        }

        public void Remove(TestData row) {
            this.rows.Remove(row);
        }

        public IEnumerator GetEnumerator() {
            return this.rows.GetEnumerator();
        }
    }


    public class TestData {
        private TEST_AREA area;
        private double power, wl, voltage, knee;

        public TEST_AREA Area { get => this.area; set => this.area = value; }
        public double WL { get => this.wl; set => this.wl = value; }
        public double Voltage { get => this.voltage; set => this.voltage = value; }
        public double Knee { get => this.knee; set => this.knee = value; }
        public double Power { get => this.power; set => this.power = value; }

        public TestData() {
            this.power = 0;
            this.wl = 0;
            this.knee = 0;
            this.voltage = 0;
            this.area = TEST_AREA.CENTERA;
        }

        public TestData(TEST_AREA a, double wl,double power, double voltage, double knee) {
            this.area = a;
            this.power = power;
            this.wl = wl;
            this.voltage = voltage;
            this.knee = knee;
        }

        public void Set(TEST_AREA a, double power, double wl, double voltage, double knee) {
            this.area = a;
            this.power = power;
            this.wl = wl;
            this.voltage = voltage;
            this.knee = knee;
        }

        public override string ToString() {

            return this.area.ToString() + ": " + this.Power.ToString() + " " + this.Voltage.ToString()+" "+this.WL.ToString();
        }
    }
}
