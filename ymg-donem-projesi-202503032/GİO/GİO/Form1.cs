using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;



namespace GİO
{
    public partial class Form1 : Form

    {
        SqlConnection con;
        SqlDataReader dr;
        SqlCommand com;
        bool move;
        int mouse_x;
        int mouse_y;

        public Form1()
        {
            InitializeComponent();
        }
        public static string metin;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.PasswordChar == '*')
            { textBox2.PasswordChar = '\0'; }

            else { textBox2.PasswordChar = '*'; }
        }



        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);
            }

        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            VeriTabani.MD5Sifrele(textBox2.Text);
            string kullanici = textBox1.Text;
            string sifre = textBox2.Text;
            bool yönetici = radioButton2.Checked;
            con = new SqlConnection("Data Source = DESKTOP-3BUBNE9\\SQLEXPRESS; Initial Catalog = GİO; Integrated Security = TRUE");
            com = new SqlCommand();
            con.Open();
            com.Connection = con;
            com.CommandText = "Select *From Login where kullanici='" + textBox1.Text + "' And sifre='" + VeriTabani.MD5Sifrele(textBox2.Text) + "' And yönetici = '" + radioButton2.Checked + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("Giriş Yapıldı");

                if (radioButton1.Checked)
                {
                    metin = kullanici;
                    Form5 form5 = new Form5();
                    form5.Show();
                    this.Hide();


                }
                if (radioButton2.Checked)
                {
                    metin = kullanici;
                    Form2 form2 = new Form2();
                    form2.Show();
                    this.Hide();
                    
                }
               
            con.Close();

            }
            else
            {
                MessageBox.Show("Kullanıcı Adı veya Şifre Hatalı");
            }


        }

        
    }
}


    
