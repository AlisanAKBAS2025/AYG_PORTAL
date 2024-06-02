using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AYGFİYAT
{
    public partial class TopluDüzenle : Form
    {
        static string serverName = System.Environment.MachineName;
        public static string SqlCon = "Data Source = " + serverName + "; Initial Catalog = AYGPORTAL; Integrated Security = TRUE";
        SqlConnection con = new SqlConnection(SqlCon);

        private int pageSize = 8500;
        private int currentPage = 1;

        public TopluDüzenle()
        {
            InitializeComponent();
        }
        
        
        private void TopluDüzenle_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            con.Open();
            string query = "SELECT * FROM Resim1"; // Sorgunuzu burada ayarlayın.
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            dataGridView1.DataSource = dt; // DataGridView'e verileri yükleme
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            con.Open();
            string[] keywords = textBox1.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string ad = "SELECT * FROM Resim1 WHERE 1=1";
            SqlCommand komut = new SqlCommand(ad, con);

            for (int i = 0; i < keywords.Length; i++)
            {
                string parameterName = "@Keyword" + i;
                string keyword = keywords[i];

                ad += $" AND (Ürün LIKE '%' + {parameterName} + '%'  or Marka LIKE '%' + {parameterName} + '%' )";
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


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(textBox2.Text, out double yuzdeArtis))
            {
                MessageBox.Show("Geçerli bir yüzde artış miktarı girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double artisMiktari = yuzdeArtis / 100;
            List<string> ayrıntılar = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                int id = Convert.ToInt32(row.Cells["id"].Value);

                if (checkBox1.Checked)
                {
                    if (double.TryParse(row.Cells["Fiyat"].Value.ToString(), out double fiyat))
                    {
                        double yeniFiyat = fiyat + (fiyat * artisMiktari);
                        ayrıntılar.Add($"Ürün: {row.Cells[2].Value}, Yeni Fiyat: {yeniFiyat:F2}");
                        UpdateFiyat(id, yeniFiyat);
                    }
                }

                if (checkBox2.Checked)
                {
                    if (double.TryParse(row.Cells["Fiyat_KDV"].Value.ToString(), out double fiyat))
                    {
                        double yeniFiyatKDV = fiyat + (fiyat * artisMiktari);
                        ayrıntılar.Add($"Ürün: {row.Cells[2].Value}, Yeni Fiyat KDV: {yeniFiyatKDV:F2}");
                        UpdateFiyatKDV(id, yeniFiyatKDV);
                    }
                }
            }

            if (ayrıntılar.Count > 0)
            {
                AyrıntılarForm ayrıntılarForm = new AyrıntılarForm(ayrıntılar);
                DialogResult result = ayrıntılarForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    LoadData();
                    MessageBox.Show("Kayıtlar Güncellendi", "Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result == DialogResult.Cancel)
                {
                    MessageBox.Show("İptal Edildi", "Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Değişiklik yok veya hatalı girişler tespit edildi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ... Diğer fonksiyonlar ve olay işleyiciler

        private void UpdateFiyat(int id, double yeniFiyat)
        {
            using (SqlConnection updateCon = new SqlConnection(SqlCon))
            {
                updateCon.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Resim1 SET Fiyat = @p1 WHERE id = @p2", updateCon);
                string formatliFiyat = string.Format("{0:F2}", yeniFiyat); // Dönüşümü burada yapın
                cmd.Parameters.AddWithValue("@p1",Convert.ToDouble( formatliFiyat)); // Formatlı değeri kullanın
                cmd.Parameters.AddWithValue("@p2", id);
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateFiyatKDV(int id, double yeniFiyatKDV)
        {
            using (SqlConnection updateCon = new SqlConnection(SqlCon))
            {
                updateCon.Open();
                string formatliFiyat = string.Format("{0:F2}", yeniFiyatKDV); // Dönüşümü burada yapın
                SqlCommand cmd = new SqlCommand("UPDATE Resim1 SET Fiyat_KDV = @p1 WHERE id = @p2", updateCon);
                cmd.Parameters.AddWithValue("@p1", Convert.ToDouble( formatliFiyat));
                cmd.Parameters.AddWithValue("@p2", id);
                cmd.ExecuteNonQuery();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox1.Text.Length >= 3)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    LoadData();
                }
            }
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
