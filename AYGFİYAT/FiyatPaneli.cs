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
using System.IO;
using System.Net;
using System.Drawing.Printing;

namespace AYGFİYAT
{
    public partial class FiyatPaneli : Form
    {
        public FiyatPaneli()
        {
            InitializeComponent();            
        }      
        static string serverName = System.Environment.MachineName ;
        public static string SqlCon = "Data Source = " + serverName + "; Initial Catalog = AYGPORTAL; Integrated Security = TRUE";
        SqlConnection con = new SqlConnection(SqlCon);

        private int pageSize = 200;
        private int currentPage = 1;
        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }
        private void FiyatPaneli_Load(object sender, EventArgs e)
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
        private string resimYolu;
        private string resimYolu1;

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog1 = new OpenFileDialog();
            dialog1.Title = "Resim Seç";
            dialog1.Filter = "Resim Dosyaları (*.png;*.jpg;*.jpeg;*.gif;*.tif)|*.png;*.jpg;*.jpeg;*.gif;*.tif";

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                fotoPictureBox.Image = Image.FromFile(dialog1.FileName);
                resimYolu = dialog1.FileName; // Seçilen resim yolunu kaydet     
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(resimYolu))
            {
                MessageBox.Show("Lütfen önce bir resim seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Image orijinalResim = Image.FromFile(resimYolu);

            float oran = 0.4f; // Küçültme oranı
            int yeniEn = (int)(orijinalResim.Width * oran);
            int yeniBoy = (int)(orijinalResim.Height * oran);

            Image kucukResim = new Bitmap(yeniEn, yeniBoy);
            using (Graphics grafik = Graphics.FromImage(kucukResim))
            {
                grafik.DrawImage(orijinalResim, 0, 0, yeniEn, yeniBoy);
            }

            

            byte[] kucukResimBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                kucukResim.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg); // Resim formatını belirtin
                kucukResimBytes = memoryStream.ToArray();
            }

            con.Open();
            SqlCommand cmd;

            cmd = new SqlCommand("INSERT INTO Resim1 (Resim_Yolu, Ürün, Marka, Kategori, Döviz, Fiyat, KDV, Fiyat_KDV) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8)", con);


            cmd.Parameters.AddWithValue("@p1", resimYolu);
            cmd.Parameters.AddWithValue("@p2", ürünTextBox.Text);
            cmd.Parameters.AddWithValue("@p3", markaTextBox.Text);
            cmd.Parameters.AddWithValue("@p4", comboBox1.Text);
            cmd.Parameters.AddWithValue("@p5", dövizComboBox.Text);
            cmd.Parameters.AddWithValue("@p6", Convert.ToDouble(kDV_sizTextBox.Text));
            cmd.Parameters.AddWithValue("@p7", Convert.ToDouble(kDV_oranıTextBox.Text));
            cmd.Parameters.AddWithValue("@p8", Convert.ToDouble(kDV_liTextBox.Text));

            cmd.ExecuteNonQuery();

            
            MessageBox.Show("Yeni Kayıt Eklendi", "Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
          

            con.Close();
            PerformSearch();

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            idLabel2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            ürünTextBox.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            markaTextBox.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            kDV_sizTextBox.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            kDV_liTextBox.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            dövizComboBox.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            kDV_oranıTextBox.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            fotoPictureBox.ImageLocation= dataGridView1.CurrentRow.Cells[9].Value.ToString();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (resimYolu == null)
            {
                resimYolu = resimYolu1;
            }


            Image orijinalResim = Image.FromFile(resimYolu);

            // Resmi küçültme işlemi (yukarıdaki kodu kullanıyoruz)
            float oran = 0.4f; // Küçültme oranı
            int yeniEn = (int)(orijinalResim.Width * oran);
            int yeniBoy = (int)(orijinalResim.Height * oran);

            Image kucukResim = new Bitmap(yeniEn, yeniBoy);
            using (Graphics grafik = Graphics.FromImage(kucukResim))
            {
                grafik.DrawImage(orijinalResim, 0, 0, yeniEn, yeniBoy);
            }


            byte[] kucukResimBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                kucukResim.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg); // Resim formatını belirtin
                kucukResimBytes = memoryStream.ToArray();
            }

            // Veritabanına güncel verileri kaydetme işlemi (yukarıdaki kodu kullanıyoruz)
            con.Open();
            SqlCommand cmd = new SqlCommand("update Resim1 set Resim_Yolu=@p1, Ürün=@p2, Marka=@p3, Kategori=@p4, Döviz=@p6, Fiyat=@p7, KDV=@p8, Fiyat_KDV=@p9 where id=@p10", con);
            cmd.Parameters.AddWithValue("@p1", resimYolu);
            cmd.Parameters.AddWithValue("@p2", ürünTextBox.Text);
            cmd.Parameters.AddWithValue("@p3", markaTextBox.Text);
            cmd.Parameters.AddWithValue("@p4", comboBox1.Text);
            cmd.Parameters.AddWithValue("@p6", dövizComboBox.Text);
            cmd.Parameters.AddWithValue("@p7", Convert.ToDouble(kDV_sizTextBox.Text));
            cmd.Parameters.AddWithValue("@p8", Convert.ToDouble(kDV_oranıTextBox.Text));
            cmd.Parameters.AddWithValue("@p9", Convert.ToDouble(kDV_liTextBox.Text));
            cmd.Parameters.AddWithValue("@p10", idLabel2.Text);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Kayıt Güncellendi", "Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            con.Close();
            PerformSearch();

        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Cells[0].Value != null)
            {
                // Kullanıcıya silme işlemini onaylaması için bir mesaj göster
                DialogResult result = MessageBox.Show("Bu satırı silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Eğer kullanıcı "Evet" butonuna tıklarsa, silme işlemini gerçekleştir
                if (result == DialogResult.Yes)
                {
                    string connectionString = SqlCon; // Bağlantı dizesini güncelleyin
                    string deleteQuery = "DELETE FROM Resim1 WHERE id = @p1";

                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                            {
                                cmd.Parameters.AddWithValue("@p1", dataGridView1.CurrentRow.Cells[1].Value.ToString());
                                cmd.ExecuteNonQuery();
                            }
                            MessageBox.Show("Kayıt Silindi", "Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            con.Close();
                            PerformSearch();
                        }

                    }
                    catch (Exception ex)
                    {
                        // Silme işlemi sırasında bir hata oluştuysa hata mesajını göster
                        MessageBox.Show("Silme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };
            
        }
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            idLabel2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            ürünTextBox.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            markaTextBox.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            kDV_sizTextBox.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            kDV_liTextBox.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            dövizComboBox.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            kDV_oranıTextBox.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            fotoPictureBox.ImageLocation = dataGridView1.CurrentRow.Cells[9].Value.ToString();
            resimYolu1= dataGridView1.CurrentRow.Cells[9].Value.ToString();
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

        private void button6_Click(object sender, EventArgs e)
        {
            TopluDüzenle topluDüzenle = new TopluDüzenle();
            topluDüzenle.ShowDialog();
        }

        
    }
}
