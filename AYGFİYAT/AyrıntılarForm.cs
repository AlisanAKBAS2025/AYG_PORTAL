using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AYGFİYAT
{
    public partial class AyrıntılarForm : Form
    {
        public TopluDüzenle topluDüzenle = new TopluDüzenle();
        public AyrıntılarForm(List<string> ayrıntılar)
        {
            InitializeComponent();

            foreach (string ayrıntı in ayrıntılar)
            {
                listBox1.Items.Add(ayrıntı);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
            


        }
    }
}
