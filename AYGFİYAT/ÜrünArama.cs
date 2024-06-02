using System;
using System.CodeDom;
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
    public partial class ÜrünArama : Form
    {

        static string serverName = System.Environment.MachineName;
        public static string SqlCon = "Data Source = " + serverName + "; Initial Catalog = AYGPORTAL; Integrated Security = TRUE";
        SqlConnection con = new SqlConnection(SqlCon);

        private int pageSize = 50; 
        private int currentPage = 1;
        private Timer loadingTimer;

        public AnaPanel AnaPanel = new AnaPanel();
        public event EventHandler SepetUpdated;
        private void OnSepetUpdated()
        {
            SepetUpdated?.Invoke(this, EventArgs.Empty);
        }


        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }

        public ÜrünArama()
        {
            InitializeComponent();
            textBox1.KeyPress += textBox1_KeyPress;
            textBox1.Focus();
            loadingTimer = new Timer();
            loadingTimer.Interval = 2000; // İki saniye bekleyecek (2000 ms)
            loadingTimer.Tick += timer1_Tick;

        }

        private void ÜrünArama_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            PerformSearch();

        }
        private void PerformSearch()
        {

            loadingTimer.Start();

            con.Open();
            string[] keywords = textBox1.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string ad = "SELECT * FROM Resim1 WHERE 1=1";
            SqlCommand komut = new SqlCommand(ad, con);

            for (int i = 0; i < keywords.Length; i++)
            {
                string parameterName = "@Keyword" + i;
                string keyword = keywords[i];

                ad += $" AND (Ürün LIKE '%' + {parameterName} + '%'  )";
                komut.Parameters.AddWithValue(parameterName, keyword);
            }

            int offset = (currentPage - 1) * pageSize;
            ad += $" ORDER BY id OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            komut.CommandText = ad;
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();


            dataGridView1.Columns["Resim_Yolu"].Visible = false;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["KDV"].Visible = false;

            // Resimleri yükle
            int rowCount = dataGridView1.Rows.Count;
            int maxRows = Math.Min(rowCount, 1000); // Maksimum 30 satır için kontrol

            for (int i = 0; i < maxRows; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                string resimYolu = row.Cells["Resim_Yolu"].Value?.ToString();

                if (!string.IsNullOrEmpty(resimYolu))
                {
                    try
                    {
                        // Resim yolundan dosya okuma
                        Image originalImage = Image.FromFile(resimYolu);
                        int desiredWidth = 200; // Hedef genişlik değeri
                        int desiredHeight = 100; // Hedef yükseklik değeri
                        Image resizedImage = ResizeImage(originalImage, desiredWidth, desiredHeight);

                        row.Cells["Resim"].Value = resizedImage;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            textBox1.Focus();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox1.Text.Length >= 3)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    PerformSearch();
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.Visible = true;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {

                dataGridView1.Focus();
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
                }
                else
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.NewRowIndex].Cells[0];
                }
            }
        }
        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {                
                con.Open();
                SqlCommand cmd = new SqlCommand("insert into Sepet ( Ürün, Marka, Fiyat, Fiyat_KDV, Döviz,Adet) values ( @p2, @p3, @p4, @p5,@p6,@p7)", con);
                cmd.Parameters.AddWithValue("@p2", dataGridView1.CurrentRow.Cells[3].Value.ToString());
                cmd.Parameters.AddWithValue("@p3", dataGridView1.CurrentRow.Cells[4].Value.ToString());
                cmd.Parameters.AddWithValue("@p4", dataGridView1.CurrentRow.Cells[5].Value.ToString());
                cmd.Parameters.AddWithValue("@p5", dataGridView1.CurrentRow.Cells[6].Value.ToString());
                cmd.Parameters.AddWithValue("@p6", dataGridView1.CurrentRow.Cells[7].Value.ToString());
                Adet Adet = new Adet();
                Adet.ShowDialog();
                cmd.Parameters.AddWithValue("@p7", Convert.ToInt32(Adet.textBox1.Text));               
                this.Close();
                cmd.ExecuteNonQuery();
                con.Close();
                OnSepetUpdated();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

    }
}
