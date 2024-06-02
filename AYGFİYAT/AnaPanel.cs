using AYGFİYAT.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AYGFİYAT
{
    public partial class AnaPanel : Form
    {

        public AnaPanel()
        {
            InitializeComponent();
        }

            
        static string serverName = System.Environment.MachineName;
        public static string SqlCon = "Data Source = " + serverName + "; Initial Catalog = AYGPORTAL; Integrated Security = TRUE";
        SqlConnection con = new SqlConnection(SqlCon);

        decimal Ytoplam;
        public  void Sepet()
        {
            con.Open();
            SqlCommand command = new SqlCommand("SELECT id,Ürün,Marka,Fiyat,Fiyat_KDV,Döviz,Adet  FROM Sepet ", con);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            con.Close();
        }
        decimal dolar ;
        decimal euro;
        private void AnaPanel_Load(object sender, EventArgs e)
        {
            TemizleSepet();
            Sepet();
            GetExchangeRates();

        }
         
        private void GetExchangeRates()
        {
            string bugun = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldosya = new XmlDocument();
            xmldosya.Load(bugun);

            string dolar = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;

            label8.Text = dolar;

            string euro = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;

            label7.Text = euro;

            string cro = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/CrossRateOther").InnerXml;

            label6.Text = cro;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetExchangeRates();

            ÜrünArama ürünArama = new ÜrünArama();
            ürünArama.SepetUpdated += ÜrünArama_SepetUpdated;
            ürünArama.Show();
        }
        private void ÜrünArama_SepetUpdated(object sender, EventArgs e)
        {
            Sepet();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Cells[0].Value != null)
            {
                // Kullanıcıya silme işlemini onaylaması için bir mesaj göster
                DialogResult result = MessageBox.Show("Bu satırı silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Eğer kullanıcı "Evet" butonuna tıklarsa, silme işlemini gerçekleştir
                if (result == DialogResult.Yes)
                {
                    string connectionString = SqlCon; // Bağlantı dizesini güncelleyin
                    string deleteQuery = "DELETE FROM Sepet WHERE id = @p1";

                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                            {
                                cmd.Parameters.AddWithValue("@p1", dataGridView1.CurrentRow.Cells[0].Value.ToString());
                                cmd.ExecuteNonQuery();
                            }
                        }

                        Sepet();
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

        void fiyat()
        {
            decimal toplam = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (decimal.TryParse(row.Cells["Fiyat"].Value?.ToString(), out decimal fiyat) &&
                    int.TryParse(row.Cells["Adet"].Value?.ToString(), out int adet))
                {
                    decimal carpan = 1;
                    string doviz = row.Cells["Döviz"].Value?.ToString()?.Trim();

                    if (doviz == "$")
                    {
                        carpan = dolar;
                    }

                    else if (doviz == "€")
                    {
                        carpan = euro;
                    }

                    toplam += (fiyat * carpan) * adet;
                }
            }

            label1.Text = "Fiyat " + toplam.ToString("C");
        }
        void fiyatKDV()
        {
            decimal toplam = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (decimal.TryParse(row.Cells["Fiyat_KDV"].Value?.ToString(), out decimal fiyat) &&
                    int.TryParse(row.Cells["Adet"].Value?.ToString(), out int adet))
                {
                    decimal carpan = 1;
                    string doviz = row.Cells["Döviz"].Value?.ToString()?.Trim();

                    if (doviz == "$")
                    {
                        carpan = dolar;
                    }

                    else if (doviz == "€")
                    {
                        carpan = euro;
                    }

                    toplam += (fiyat * carpan) * adet;
                    Ytoplam = toplam;   

                }
            }

            label2.Text = "KDV DAHİL " + toplam.ToString("C");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            GetExchangeRates();
        }
        private void TemizleSepet()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Sepet", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Verileri temizleme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool printingCompleted = false;
        private void button5_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(this.printDocument1_PrintPage);

            // Sadece "ZJ-58" isimli yazıcıyı kullanmak için PrintDocument nesnesinin PrinterSettings'ını ayarlayın
            //printDocument.PrinterSettings.PrinterName = "ZJ-58";

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;

            // PrintPreviewControl özelliğini kullanarak önizleme yakınlaştırmasını ayarlayın
            printPreviewDialog.PrintPreviewControl.Zoom = 1.5; // %150 yakınlaştırma
            printPreviewDialog.StartPosition = FormStartPosition.CenterScreen;

            printPreviewDialog.ShowDialog();
            if (printingCompleted)
            {
                // Yazdırma işlemi tamamlandıysa sepeti temizle
                TemizleSepet();
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            fiyat();
            fiyatKDV();
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            fiyat();
            fiyatKDV();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GetExchangeRates();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Font font = new Font("Arial", 10, FontStyle.Bold);
                Font font1 = new Font("Arial", 5);
                Font font2 = new Font("Arial", 8);
                Font font3 = new Font("Arial", 6, FontStyle.Bold);
                

                SolidBrush firca = new SolidBrush(Color.Black);
                Pen pen = new Pen(Color.Black);

                Image image = Image.FromFile("C:\\Users\\alisa\\OneDrive\\Masaüstü\\Üretim\\ayg\\AYGPORTAL\\AYGFİYAT\\Resources\\ayg.png");
            

            Rectangle destRect = new Rectangle(30, 1, 125, 100);

                e.Graphics.DrawImage(image, destRect);
                e.Graphics.DrawString($"AYGÜNLER YAPI MARKET", font, firca, 6, 100);
                e.Graphics.DrawString($"Tarih = {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}", font2, firca, 60, 130);

                e.Graphics.DrawString($"ÜRÜN", font2, firca, 3, 155);
                e.Graphics.DrawString($"FİYAT", font2, firca, 145, 155);
                e.Graphics.DrawString($"Ad.", font2, firca, 125, 155);
                e.Graphics.DrawString($"--------------------------------------", font, firca, 3, 160);


                int y = 180;


                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string urunAdi = row.Cells["Ürün"].Value.ToString();
                        string Döviz = row.Cells["Döviz"].Value.ToString();
                        decimal fiyat = Convert.ToDecimal(row.Cells["Fiyat_KDV"].Value);
                        decimal adet = Convert.ToDecimal(row.Cells["Adet"].Value);


                        if (urunAdi.Length > 30)
                        {
                            int maxChars = 28; // İlk satırda yazdırılacak karakter sayısı
                            int remainingChars = urunAdi.Length - maxChars;
                            string urunAdiPart1 = urunAdi.Substring(0, maxChars) ;
                            string urunAdiPart2 = urunAdi.Substring(maxChars, remainingChars) + "...";

                            e.Graphics.DrawString(urunAdiPart1, font1, firca, 3, y);
                            e.Graphics.DrawString(adet.ToString(), font3, firca, 128, y);
                            fiyat = fiyat * adet;
                            e.Graphics.DrawString(fiyat.ToString() +Döviz, font3, firca, 146, y);

                            y += 7;

                            // İkinci satırı çiz sadece eğer geri kalan karakterler varsa
                            if (!string.IsNullOrEmpty(urunAdiPart2))
                            {
                                e.Graphics.DrawString(urunAdiPart2, font1, firca, 3, y);
                                e.Graphics.DrawString("", font1, firca, 137, y);
                                e.Graphics.DrawString("", font1, firca, 155, y);
                            }

                            y += 20;
                        }
                        else
                        {
                            e.Graphics.DrawString(urunAdi, font1, firca, 3, y);
                            e.Graphics.DrawString(adet.ToString(), font3, firca, 128, y);
                            fiyat = fiyat * adet;
                            e.Graphics.DrawString(fiyat.ToString()+ Döviz, font3, firca, 146, y);

                            y += 20;
                        }
                    }
                }
                e.Graphics.DrawString($"--------------------------------------", font, firca, 3, 160);

                e.Graphics.DrawString($"TOPLAM={Ytoplam.ToString("C")}", font, firca, 15, y);

            }
            catch (Exception)
            {
                MessageBox.Show("ANANA");
            }

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            TemizleSepet();
            Sepet();
            Ytoplam=0; 
        }

        private void printDocument1_EndPrint(object sender, PrintEventArgs e)
        {
            printingCompleted = true;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }
    }
}
