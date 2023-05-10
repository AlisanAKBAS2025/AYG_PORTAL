using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GİO
{
    public partial class Form5 : Form
    {
        SqlDataAdapter da;
        DataSet ds;
        public static string SqlCon = "Data Source = DESKTOP-3BUBNE9\\SQLEXPRESS; Initial Catalog = GİO; Integrated Security = TRUE";
        SqlConnection con = new SqlConnection(SqlCon);
        
        public Form5()
        {
            InitializeComponent();
        }
        string Fiyat;
      
        double nFiyat;
        string SilAdet;
        string SilFiyat;
        void Ürün()
        {
            con.Open();
            da = new SqlDataAdapter("select *from Ürünler", con);
            ds = new DataSet();
            da.Fill(ds, "Ürünler");
            dataGridView2.DataSource = ds.Tables["Ürünler"];
            con.Close();
        }
        void Sepet()
        {
            con.Open();
            da = new SqlDataAdapter("select *from Sepet", con);
            ds = new DataSet();
            da.Fill(ds, "Sepet");
            dataGridView1.DataSource = ds.Tables["Sepet"];
            con.Close();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            textBox2.Text = "PERAKENDE";
            textBox3.Text = "0";



            con.Open();
            SqlCommand cmd = new SqlCommand("delete from Sepet ", con);
            cmd.ExecuteNonQuery();
            con.Close();
            label14.Text = " ";
            Ürün();
            Sepet();
            


        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox10.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            textBox6.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            Fiyat = dataGridView2.CurrentRow.Cells[3].Value.ToString();
            textBox7.Text = Fiyat.ToString();
            textBox8.Text = dataGridView2.CurrentRow.Cells[4].Value.ToString();
            textBox9.Text = dataGridView2.CurrentRow.Cells[5].Value.ToString();          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string kayit = "Select *from Ürünler Where ürünAdı LIKE '%' + @ürünAdı + '%' ";
            SqlCommand komut = new SqlCommand(kayit, con);
            komut.Parameters.AddWithValue("@ürünAdı", textBox4.Text);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            con.Close();
            if (textBox4.Text == "") Ürün();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Sepet (ID,Ürün,Fiyat,Adet,ToplamFiyat) values (@p1,@p2,@p3,@p4,@p5)", con);
            cmd.Parameters.AddWithValue("@p1", textBox10.Text);
            cmd.Parameters.AddWithValue("@p2", textBox6.Text);
            cmd.Parameters.AddWithValue("@p3", Convert.ToDouble(Fiyat));
            cmd.Parameters.AddWithValue("@p4", textBox5.Text);
            cmd.Parameters.AddWithValue("@p5", textBox7.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            

            int Adet = Convert.ToInt32(textBox5.Text);   
            nFiyat = Convert.ToDouble(textBox11.Text);
            double ara = 0;
            MessageBox.Show(Fiyat);
            ara = Adet * Convert.ToDouble(Fiyat);     
            nFiyat=ara+nFiyat;
            textBox11.Text = Convert.ToString(nFiyat);

            Sepet();
            Ürün();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            

            con.Open();
            
            SqlCommand cmd = new SqlCommand("delete from Sepet where ID= @p1", con);
            cmd.Parameters.AddWithValue("@p1", Convert.ToInt32(label14.Text));
            cmd.ExecuteNonQuery();
            con.Close();
            Ürün();
            Sepet() ;
            label14.Text=" ";

            double ara =Convert.ToDouble(SilFiyat)*Convert.ToDouble(SilAdet);
            nFiyat =( nFiyat - ara);
            textBox11.Text = Convert.ToString(nFiyat);


        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            label14.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            SilAdet = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            SilFiyat = dataGridView1.CurrentRow.Cells[3].Value.ToString();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from Sepet ", con);
            cmd.ExecuteNonQuery();
            con.Close();
            Ürün();
            Sepet();
            label14.Text = " ";
            nFiyat= 0;
            textBox11.Text = Convert.ToString(nFiyat);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();  
            form6.ShowDialog();
            
        }
        
        
    }
}
