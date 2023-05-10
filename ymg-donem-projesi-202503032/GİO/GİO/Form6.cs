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

namespace GİO
{
    public partial class Form6 : Form
    {
        SqlDataAdapter da;
        DataSet ds;
        public static string SqlCon = "Data Source = DESKTOP-3BUBNE9\\SQLEXPRESS; Initial Catalog = GİO; Integrated Security = TRUE";
        SqlConnection con = new SqlConnection(SqlCon);



       

        public Form6()
        {
            InitializeComponent();
        }
       

        void Müşteri()
        {
            con.Open();
            da = new SqlDataAdapter("select *from Müşteri", con);
            ds = new DataSet();
            da.Fill(ds, "Müşteri");
            dataGridView1.DataSource = ds.Tables["Müşteri"];
            con.Close();
        }
        private void Form6_Load(object sender, EventArgs e)
        {
            
            Müşteri();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kayit = "SELECT * FROM Müşteri WHERE Ad LIKE '%' + @Ad + '%' ";
            SqlCommand komut = new SqlCommand(kayit, con);
            komut.Parameters.AddWithValue("@Ad", textBox1.Text);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
            if (textBox1.Text == "") Müşteri();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Form5 form5 = (Form5)Application.OpenForms["Form5"];
            form5.textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            form5.textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            this.Close();

        }

    }
}
