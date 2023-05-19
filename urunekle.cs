using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kirtasiye_Proje
{
    public partial class urunekle : Form
    {
        public urunekle()
        {
            InitializeComponent();
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminKategoriİşlemleri categoryPage = new AdminKategoriİşlemleri();
            categoryPage.Show();
        }
    }
}
