using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace Kirtasiye_Proje
{
    public partial class fHizliButonUrunEkle : Form
    {
        BarcodeSalesDbEntities db = new BarcodeSalesDbEntities();

        public fHizliButonUrunEkle()
        {
            InitializeComponent();
        }

        private void txtUrunAra_TextChanged(object sender, EventArgs e)
        {
            if (txtUrunAra.Text != "")
            {
                var urunAdi = txtUrunAra.Text;
                var urunler = db.Uruns.Where(x => x.UrunAdi.Contains(urunAdi)).ToList();
                dataGridViewUrunEkleListesi.DataSource = urunler;
                Islemler.DataGridViewDüzenle(dataGridViewUrunEkleListesi);
            }
        }

        private void dataGridViewUrunEkleListesi_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewUrunEkleListesi.Rows.Count > 0)
            {
                string barkod = dataGridViewUrunEkleListesi.CurrentRow.Cells["Barkod"].Value.ToString();
                string urunAdi = dataGridViewUrunEkleListesi.CurrentRow.Cells["UrunAdi"].Value.ToString();
                double fiyat = Convert.ToDouble(dataGridViewUrunEkleListesi.CurrentRow.Cells["SatisFiyat"].Value.ToString());
                int id = Convert.ToInt16(lblButonId.Text);

                var guncellenekSatır = db.HizliUruns.Find(id);
                guncellenekSatır.Barkod = barkod;
                guncellenekSatır.UrunAdi = urunAdi;
                guncellenekSatır.Fiyat = fiyat;
                db.SaveChanges();

                MessageBox.Show("Buton tanımlaması yapıldı");

                fSatis fSatis = (fSatis)Application.OpenForms["fSatis"];
                if (fSatis != null)
                {
                    Button btn = fSatis.Controls.Find("btnHizli" + id, true).FirstOrDefault() as Button;
                    btn.Text = urunAdi + "\n" + fiyat.ToString("C2");
                }
            }
        }

        private void checkBoxTumunuGoster_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTumunuGoster.Checked)
            {
                dataGridViewUrunEkleListesi.DataSource = db.Uruns.ToList();
                Islemler.DataGridViewDüzenle(dataGridViewUrunEkleListesi);
            }
            else
            {
                dataGridViewUrunEkleListesi.DataSource = null;
            }
        }
    }
}
