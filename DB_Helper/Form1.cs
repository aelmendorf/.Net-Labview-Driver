using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System.Windows.Forms;


namespace DB_Helper {
    public partial class Form1 : Form {
        private List<string[]> waferList;

        public Form1() {
            InitializeComponent();
            this.waferList = new List<string[]>();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void add_Click(object sender, EventArgs e) {

        }
    }
}
