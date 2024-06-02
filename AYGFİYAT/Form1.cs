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
    public partial class Login : Form
    {
        static string serverName = System.Environment.MachineName;
        public static string SqlCon = "Data Source = " + serverName + "; Initial Catalog = AYGPORTAL; Integrated Security = TRUE";
        SqlConnection con = new SqlConnection(SqlCon);

        SqlDataReader dr;
        SqlCommand com;
        bool move;
        int mouse_x;
        int mouse_y;

        public Login()
        {
            InitializeComponent();
        }
        public static string metin;

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);
            }
        }

        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (textBox2.PasswordChar == '*')
            { textBox2.PasswordChar = '\0'; }
            else { textBox2.PasswordChar = '*'; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Focus(); 
            string kullanici = textBox1.Text;
            string sifre = textBox2.Text;
            bool yönetici = radioButton2.Checked;
            com = new SqlCommand();
            con.Open();
            com.Connection = con;
            com.CommandText = "Select *From Login where kullanici='" + textBox1.Text + "' And sifre='" + textBox2.Text + "' And yönetici = '" + radioButton2.Checked + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                

                if (radioButton1.Checked)
                {
                    metin = kullanici;
                    AnaPanel AnaPanel = new AnaPanel();
                    AnaPanel.Show();
                    this.Hide();
                }
                if (radioButton2.Checked)
                {
                    metin = kullanici;
                    Yönetici Yönetici = new Yönetici();
                    Yönetici.Show();
                    this.Hide();
                }

                con.Close();

            }
            else
            {
                MessageBox.Show("Kullanıcı Adı veya Şifre Hatalı");
            }
            con.Close();

        }

        private void Login_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }
    }
}
