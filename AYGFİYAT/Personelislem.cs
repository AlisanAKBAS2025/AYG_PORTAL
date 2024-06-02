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

namespace AYGFİYAT
{
    public partial class Personelislem : Form
    {
        public Personelislem()
        {
            InitializeComponent();
        }
        public int yönetici;
        SqlDataAdapter da;
        DataSet ds;
        static string serverName = System.Environment.MachineName;
        public static string SqlCon = "Data Source = " + serverName + "; Initial Catalog = AYGPORTAL; Integrated Security = TRUE";
        SqlConnection con = new SqlConnection(SqlCon);
        void Login()
        {
            con.Open();
            da = new SqlDataAdapter("select *from Login", con);
            ds = new DataSet();
            da.Fill(ds, "Login");
            dataGridView1.DataSource = ds.Tables["Login"];
            con.Close();
        }

        private void Personelislem_Load(object sender, EventArgs e)
        {
           
            Login();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtNo.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtKullanici.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtSifre.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            radioButton1.Checked = dataGridView1.CurrentRow.Cells[3].Value.Equals(false);
            radioButton2.Checked = dataGridView1.CurrentRow.Cells[3].Value.Equals(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Login (kullanici, sifre, yönetici) values (@p1,@p2,@p4)", con);
            cmd.Parameters.AddWithValue("@p1", txtKullanici.Text);
            cmd.Parameters.AddWithValue("@p2", txtSifre.Text);
            cmd.Parameters.AddWithValue("@p4", radioButton2.Checked);
            cmd.ExecuteNonQuery();
            con.Close();
            Login();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from Login where kID= @p1", con);
            cmd.Parameters.AddWithValue("@p1", Convert.ToInt32(txtNo.Text));
            cmd.ExecuteNonQuery();
            con.Close();
            Login();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update Login set kullanici=@p1, sifre=@p2,yönetici=@p4 where no=@p3", con);
            cmd.Parameters.AddWithValue("@p1", txtKullanici.Text);
            cmd.Parameters.AddWithValue("@p2", txtSifre.Text);
            cmd.Parameters.AddWithValue("@p3", Convert.ToInt32(txtNo.Text));
            cmd.Parameters.AddWithValue("@p4", radioButton2.Checked);
            cmd.ExecuteNonQuery();
            con.Close();
            Login();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string kayit = "Select *from Login Where kullanici=@kullanici";
            SqlCommand komut = new SqlCommand(kayit, con);
            komut.Parameters.AddWithValue("@kullanici", textBox1.Text);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
            if (textBox1.Text == "")
            Login();
        }
    }
}
