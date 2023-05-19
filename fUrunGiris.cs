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
    public partial class fUrunGiris : Form
    {
        public fUrunGiris()
        {
            InitializeComponent();
        }

        private void btnBarkodOlustur_Click(object sender, EventArgs e)
        {
            var barkodNo = db.Barkods.First();
            int barkodKarakter = barkodNo.BarkodNo.ToString().Length;
            string sifirler = string.Empty;

            for (int i = 0; i < 8 - barkodKarakter; i++)
            {
                sifirler = sifirler + "0";

            }
            string olusanBarkodNo = sifirler + barkodNo.BarkodNo.ToString();
            txtBarkod.Text = olusanBarkodNo;
            txtUrunAdi.Focus();
        }

        private void txtBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barkod = txtBarkod.Text.Trim();
                if (db.Uruns.Any(x => x.Barkod == barkod))
                {
                    var urun = db.Uruns.Where(x => x.Barkod == barkod).SingleOrDefault();
                    txtBarkod.Text = urun.Barkod;
                    txtUrunAdi.Text = urun.UrunAdi;
                    comboBoxUrunGrubu.Text = urun.UrunGrup;
                    txtAlisFiyati.Text = urun.AlisFiyat.ToString();
                    txtSatisFiyati.Text = urun.SatisFiyat.ToString();
                    txtMiktar.Text = urun.Miktar.ToString();
                    txtKdvOrani.Text = urun.KdvOrani.ToString();
                    if (urun.Birim == Convert.ToString(BirimTipi.Kg))
                    {
                        chBarkodluUrunTipi.Checked = true;
                    }
                    else
                    {
                        chBarkodluUrunTipi.Checked = false;
                    }
                }
                else
                {
                    MessageBox.Show("Ürün kayıtlı değil");
                }
            }
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewUrunGiris.Rows.Count > 0)
            {
                txtBarkod.Text = dataGridViewUrunGiris.CurrentRow.Cells["Barkod"].Value.ToString();
                txtUrunAdi.Text = dataGridViewUrunGiris.CurrentRow.Cells["UrunAdi"].Value.ToString();
                txtUrunAdi.Text = dataGridViewUrunGiris.CurrentRow.Cells["UrunAdi"].Value.ToString();
                comboBoxUrunGrubu.Text = dataGridViewUrunGiris.CurrentRow.Cells["UrunGrup"].Value.ToString();
                txtAlisFiyati.Text = dataGridViewUrunGiris.CurrentRow.Cells["AlisFiyat"].Value.ToString();
                txtSatisFiyati.Text = dataGridViewUrunGiris.CurrentRow.Cells["SatisFiyat"].Value.ToString();
                txtKdvOrani.Text = dataGridViewUrunGiris.CurrentRow.Cells["KdvOrani"].Value.ToString();
                txtMiktar.Text = dataGridViewUrunGiris.CurrentRow.Cells["Miktar"].Value.ToString();
                string birim = dataGridViewUrunGiris.CurrentRow.Cells["Birim"].Value.ToString();
                if (birim == Convert.ToString(BirimTipi.Kg))
                {
                    chBarkodluUrunTipi.Checked = true;
                }
                else
                {
                    chBarkodluUrunTipi.Checked = false;
                }
            }
        }

        private void Temizle()
        {
            txtBarkod.Clear();
            txtUrunAdi.Clear();
            comboBoxUrunGrubu.Text = "";
            txtAlisFiyati.Text = 0.ToString();
            txtSatisFiyati.Text = 0.ToString();
            txtMiktar.Text = 0.ToString();
            txtKdvOrani.Text = 8.ToString();
            txtBarkod.Focus();
            chBarkodluUrunTipi.Checked = false;
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (txtBarkod.Text != "" &&
                txtUrunAdi.Text != "" &&
                comboBoxUrunGrubu.Text != "" &&
                txtAlisFiyati.Text != "" &&
                txtSatisFiyati.Text != "" &&
                txtMiktar.Text != "" &&
                txtKdvOrani.Text != "")
            {
                if (db.Uruns.Any(x => x.Barkod == txtBarkod.Text))
                {
                    var urunGüncelle = db.Uruns.Where(x => x.Barkod == txtBarkod.Text).SingleOrDefault();
                    urunGüncelle.Barkod = txtBarkod.Text;
                    urunGüncelle.UrunAdi = txtUrunAdi.Text;
                    urunGüncelle.UrunGrup = comboBoxUrunGrubu.Text;
                    urunGüncelle.AlisFiyat = Convert.ToDouble(txtAlisFiyati.Text);
                    urunGüncelle.SatisFiyat = Convert.ToDouble(txtSatisFiyati.Text);
                    urunGüncelle.KdvOrani = Convert.ToInt32(txtKdvOrani.Text);
                    urunGüncelle.KdvTutari = Islemler.DoubleYap(txtSatisFiyati.Text) * Convert.ToInt32(txtKdvOrani.Text) / 100;
                    urunGüncelle.Miktar += Convert.ToDouble(txtMiktar.Text);
                    if (chBarkodluUrunTipi.Checked)
                    {
                        urunGüncelle.Birim = Convert.ToString(BirimTipi.Kg);
                    }
                    else
                    {
                        urunGüncelle.Birim = Convert.ToString(BirimTipi.Adet);
                    }
                    urunGüncelle.Tarih = DateTime.Now;
                    urunGüncelle.Kullanici = lblKullanici.Text;

                    db.SaveChanges();
                }
                else
                {
                    Urun urun = new Urun();
                    urun.Barkod = txtBarkod.Text;
                    urun.UrunAdi = txtUrunAdi.Text;
                    urun.UrunGrup = comboBoxUrunGrubu.Text;
                    urun.AlisFiyat = Convert.ToDouble(txtAlisFiyati.Text);
                    urun.SatisFiyat = Convert.ToDouble(txtSatisFiyati.Text);
                    urun.KdvOrani = Convert.ToInt32(txtKdvOrani.Text);
                    urun.KdvTutari = Islemler.DoubleYap(txtSatisFiyati.Text) * Convert.ToInt32(txtKdvOrani.Text) / 100;
                    urun.Miktar = Convert.ToDouble(txtMiktar.Text);
                    if (chBarkodluUrunTipi.Checked)
                    {
                        urun.Birim = Convert.ToString(BirimTipi.Kg);
                    }
                    else
                    {
                        urun.Birim = Convert.ToString(BirimTipi.Adet);
                    }
                    urun.Tarih = DateTime.Now;
                    urun.Kullanici = lblKullanici.Text;

                    db.Uruns.Add(urun);
                    db.SaveChanges();

                    if (txtBarkod.Text.Length == 8)
                    {
                        var ozelBarkod = db.Barkods.First();
                        ozelBarkod.BarkodNo += 1;
                        db.SaveChanges();
                    }
                }
                Islemler.StokHaraketGiris(txtBarkod.Text, txtUrunAdi.Text, Convert.ToString(BirimTipi.Adet), Convert.ToDouble(txtMiktar.Text), comboBoxUrunGrubu.Text, lblKullanici.Text);
            }
            else
            {
                MessageBox.Show("Eksik alanları doldurunuz.");
                txtBarkod.Focus();
            }
            dataGridViewUrunGiris.DataSource = db.Uruns.OrderByDescending(x => x.UrunId).Take(12).ToList();
            Islemler.DataGridViewDüzenle(dataGridViewUrunGiris);
            Temizle();
        }
    }
}
