using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AYGFİYAT
{
    public partial class Adet : Form
    {
        public Adet()
        {
            InitializeComponent();
        }

        static string serverName = System.Environment.MachineName;
        public static string SqlCon = "Data Source = " + serverName + "; Initial Catalog = AYGPORTAL; Integrated Security = TRUE";
        SqlConnection con = new SqlConnection(SqlCon);

        public AnaPanel panel = new AnaPanel();

        private void Adet_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Space)
            {
                panel.Sepet();
                this.Close();
            }
        }     

        private void button1_Click(object sender, EventArgs e)
        {
            panel.dataGridView1.Refresh();
            this.Close();
        }
    }
}
