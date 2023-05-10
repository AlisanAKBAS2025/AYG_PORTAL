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

namespace GİO
{
    public partial class Form4 : Form
    {
        SqlDataAdapter da;
        DataSet ds;
        public static string SqlCon = "Data Source = DESKTOP-3BUBNE9\\SQLEXPRESS; Initial Catalog = GİO; Integrated Security = TRUE";
        SqlConnection con = new SqlConnection(SqlCon);

        public Form4()
        {
            InitializeComponent();
        }
        void Ürün()
        {
            con.Open();
            da = new SqlDataAdapter("select *from Ürünler", con);
            ds = new DataSet();
            da.Fill(ds, "Ürünler");
            dataGridView1.DataSource = ds.Tables["Ürünler"];
            con.Close();
        }       
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox7.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            Ürün();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update Ürünler set ürünAdı=@p1,ürünAFiyatı=@p2,ürünSFiyatı=@p3,stok=@p4,ürünAçıklama=@p5 where islemID=@p7", con);
            cmd.Parameters.AddWithValue("@p1", textBox1.Text);
            cmd.Parameters.AddWithValue("@p2", textBox2.Text);
            cmd.Parameters.AddWithValue("@p3", textBox3.Text);
            cmd.Parameters.AddWithValue("@p4", textBox4.Text);
            cmd.Parameters.AddWithValue("@p5", textBox5.Text);
            cmd.Parameters.AddWithValue("@p7", Convert.ToInt32(textBox7.Text));
            cmd.ExecuteNonQuery();
            con.Close();
            Ürün();
        }


        
        private void button3_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from Ürünler where ID= @p1", con);
            cmd.Parameters.AddWithValue("@p1", Convert.ToInt32(textBox7.Text));
            cmd.ExecuteNonQuery();
            con.Close();
            Ürün();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string kayit = "Select *from Ürünler Where ürünAdı=@ürünAdı";
            SqlCommand komut = new SqlCommand(kayit, con);
            komut.Parameters.AddWithValue("@ürünAdı", textBox8.Text);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
            if (textBox8.Text == "") Ürün();
        }
        private void button1_Click(object sender, EventArgs e)
        {
             con.Open();
            SqlCommand cmd = new SqlCommand("insert into Ürünler (ürünAdı,ürünAFiyatı,ürünSFiyatı,stok,ürünAçıklama) values (@p1,@p2,@p3,@p4,@p5)", con);
            cmd.Parameters.AddWithValue("@p1", textBox1.Text);
            cmd.Parameters.AddWithValue("@p2", textBox2.Text);
            cmd.Parameters.AddWithValue("@p3", textBox3.Text);
            cmd.Parameters.AddWithValue("@p4", textBox4.Text);
            cmd.Parameters.AddWithValue("@p5", textBox5.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            Ürün();
        }

        
    }

}




