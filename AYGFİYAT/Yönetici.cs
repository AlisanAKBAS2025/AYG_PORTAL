using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AYGFİYAT
{
    public partial class Yönetici : Form
    {
        public Yönetici()
        {
            InitializeComponent();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            TopluDüzenle topluDüzenle = new TopluDüzenle();
            topluDüzenle.ShowDialog();
        }

        private void Yönetici_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Personelislem personelislem = new Personelislem();
            personelislem.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AnaPanel anaPanel = new AnaPanel();
            anaPanel.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FiyatPaneli fiyatPaneli = new FiyatPaneli();    
            fiyatPaneli.ShowDialog();
        }
    }
}
