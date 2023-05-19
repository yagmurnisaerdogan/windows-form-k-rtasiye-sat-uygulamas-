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
    public partial class fSatis : Form
    {
        private readonly BarcodeSalesDbEntities db = new BarcodeSalesDbEntities();
        fIsemBeklet fIsemBeklet = new fIsemBeklet();
        public fSatis()
        {
            InitializeComponent();
        }

        private void fSatis_Load(object sender, EventArgs e)
        {
            btn5TL.Text = 5.ToString("C2");
            btn10TL.Text = 10.ToString("C2");
            btn20TL.Text = 20.ToString("C2");
            btn50TL.Text = 50.ToString("C2");
            btn100TL.Text = 100.ToString("C2");
            btn200TL.Text = 200.ToString("C2");
            HizliButonDoldur();

            using (var db = new BarcodeSalesDbEntities())
            {
                var sabit = db.Sabits.FirstOrDefault();
                checkBoxYazdırmaDurumu.Checked = Convert.ToBoolean(sabit.Yazici);
            }
        }

        private void ListeyeUrunGetir(Urun urun, string barkod, double miktar)
        {
            int satirSayisi = dataGridViewSatisListesi.Rows.Count;
            bool eklenmismi = false;
            if (satirSayisi > 0)
            {
                for (int i = 0; i < satirSayisi; i++)
                {
                    if (dataGridViewSatisListesi.Rows[i].Cells["gvBarkod"].Value.ToString() == barkod)
                    {
                        dataGridViewSatisListesi.Rows[i].Cells["gvMiktar"].Value = miktar + Convert.ToDouble(dataGridViewSatisListesi.Rows[i].Cells["gvMiktar"].Value);

                        dataGridViewSatisListesi.Rows[i].Cells["gvToplam"].Value = Math.Round(Convert.ToDouble(dataGridViewSatisListesi.Rows[i].Cells["gvMiktar"].Value) * Convert.ToDouble(dataGridViewSatisListesi.Rows[i].Cells["gvFiyat"].Value), 2);

                        double dblKdvTutari = (double)urun.KdvTutari;

                        dataGridViewSatisListesi.Rows[i].Cells["gvKdvTutari"].Value = Convert.ToDouble(dataGridViewSatisListesi.Rows[i].Cells["gvMiktar"].Value) * dblKdvTutari;

                        eklenmismi = true;
                    }
                }
            }
            if (!eklenmismi)
            {
                dataGridViewSatisListesi.Rows.Add();
                dataGridViewSatisListesi.Rows[satirSayisi].Cells["gvBarkod"].Value = barkod;
                dataGridViewSatisListesi.Rows[satirSayisi].Cells["gvUrunAdi"].Value = urun.UrunAdi;
                dataGridViewSatisListesi.Rows[satirSayisi].Cells["gvMiktar"].Value = miktar;
                dataGridViewSatisListesi.Rows[satirSayisi].Cells["gvFiyat"].Value = urun.SatisFiyat;
                dataGridViewSatisListesi.Rows[satirSayisi].Cells["gvToplam"].Value = Math.Round(miktar * (double)urun.SatisFiyat, 2);
                dataGridViewSatisListesi.Rows[satirSayisi].Cells["gvKdvTutari"].Value = urun.SatisFiyat * urun.KdvOrani / 100;
                dataGridViewSatisListesi.Rows[satirSayisi].Cells["gvBirim"].Value = urun.Birim;
                dataGridViewSatisListesi.Rows[satirSayisi].Cells["gvUrunGrup"].Value = urun.UrunGrup;
                dataGridViewSatisListesi.Rows[satirSayisi].Cells["gvAlisFiyati"].Value = urun.AlisFiyat;
            }
        }

        private void GenelToplam()
        {
            double genelToplam = 0;
            for (int i = 0; i < dataGridViewSatisListesi.Rows.Count; i++)
            {
                genelToplam += Convert.ToDouble(dataGridViewSatisListesi.Rows[i].Cells["gvToplam"].Value);
            }
            txtGenelToplam.Text = genelToplam.ToString("C2");
            txtMiktar.Text = "1";
            txtBarkod.Clear();
            txtBarkod.Focus();
        }

        public void SatisYap(string odemeSekli)
        {
            int satirSayisi = dataGridViewSatisListesi.Rows.Count;
            bool satisIade = chSatisIadeIslemi.Checked;
            double alisFiyatToplam = 0;
            if (satirSayisi > 0)
            {
                int? islemNo = db.Islems.First().IslemNo;
                Sati satis = new Sati();
                for (int i = 0; i < satirSayisi; i++)
                {
                    satis.IslemNoId = islemNo;
                    satis.UrunAdi = dataGridViewSatisListesi.Rows[i].Cells["gvUrunAdi"].Value.ToString();
                    satis.UrunGrup = dataGridViewSatisListesi.Rows[i].Cells["gvUrunGrup"].Value.ToString();
                    satis.Barkod = dataGridViewSatisListesi.Rows[i].Cells["gvBarkod"].Value.ToString();
                    satis.Birim = dataGridViewSatisListesi.Rows[i].Cells["gvBirim"].Value.ToString();
                    satis.AlisFiyat = Islemler.DoubleYap(dataGridViewSatisListesi.Rows[i].Cells["gvAlisFiyati"].Value.ToString());
                    satis.SatisFiyat = Islemler.DoubleYap(dataGridViewSatisListesi.Rows[i].Cells["gvFiyat"].Value.ToString());
                    satis.Miktar = Islemler.DoubleYap(dataGridViewSatisListesi.Rows[i].Cells["gvMiktar"].Value.ToString());
                    satis.Toplam = Islemler.DoubleYap(dataGridViewSatisListesi.Rows[i].Cells["gvToplam"].Value.ToString());
                    satis.KdvTutar = Islemler.DoubleYap(dataGridViewSatisListesi.Rows[i].Cells["gvKdvTutari"].Value.ToString()) * Islemler.DoubleYap(dataGridViewSatisListesi.Rows[i].Cells["gvMiktar"].Value.ToString());
                    satis.OdemeSekli = odemeSekli;
                    satis.Iade = satisIade;
                    satis.Tarih = DateTime.Now;
                    satis.Kullanici = lblKullanici.Text;

                    db.Satis.Add(satis);
                    db.SaveChanges();

                    if (!satisIade)
                    {
                        Islemler.StokAzalt(dataGridViewSatisListesi.Rows[i].Cells["gvBarkod"].Value.ToString(), Islemler.DoubleYap(dataGridViewSatisListesi.Rows[i].Cells["gvMiktar"].Value.ToString()));
                    }
                    else
                    {
                        Islemler.StokArttir(dataGridViewSatisListesi.Rows[i].Cells["gvBarkod"].Value.ToString(), Islemler.DoubleYap(dataGridViewSatisListesi.Rows[i].Cells["gvMiktar"].Value.ToString()));
                    }

                    alisFiyatToplam += Islemler.DoubleYap(dataGridViewSatisListesi.Rows[i].Cells["gvAlisFiyati"].Value.ToString()) * Islemler.DoubleYap(dataGridViewSatisListesi.Rows[i].Cells["gvMiktar"].Value.ToString());
                }

                IslemOzet islemOzet = new IslemOzet();
                islemOzet.IslemNoId = islemNo;
                islemOzet.Iade = satisIade;
                islemOzet.AlisFiyatToplam = alisFiyatToplam;
                islemOzet.Gelir = false;
                islemOzet.Gider = false;
                if (!satisIade)
                {
                    islemOzet.Aciklama = odemeSekli + " " + "Satış";
                }
                else
                {
                    islemOzet.Aciklama = "İade İşlemi (" + odemeSekli + ")";
                }
                islemOzet.OdemeSekli = odemeSekli;
                islemOzet.Kullanici = lblKullanici.Text;
                islemOzet.Tarih = DateTime.Now;
                switch (odemeSekli)
                {
                    case "Nakit":
                        islemOzet.Nakit = Islemler.DoubleYap(txtGenelToplam.Text);
                        islemOzet.KrediKarti = 0;
                        break;
                    case "Kredi Kartı":
                        islemOzet.KrediKarti = Islemler.DoubleYap(txtGenelToplam.Text);
                        islemOzet.Nakit = 0;
                        break;
                    case "Nakit Kart":
                        islemOzet.Nakit = Islemler.DoubleYap(lblNakit.Text);
                        islemOzet.KrediKarti = Islemler.DoubleYap(lblKart.Text);
                        break;
                }

                db.IslemOzets.Add(islemOzet);
                db.SaveChanges();

                var islemNoArttir = db.Islems.First();
                islemNoArttir.IslemNo += 1;
                db.SaveChanges();

                if (checkBoxYazdırmaDurumu.Checked)
                {
                    Yazdir yazdir = new Yazdir(islemNo);
                    yazdir.Yazdirmaİslemi();
                }
                Temizle();
            }
        }

        private void Temizle()
        {
            txtMiktar.Text = 1.ToString();
            txtBarkod.Clear();
            txtParaUstu.Clear();
            txtOdenen.Clear();
            txtGenelToplam.Clear();
            txtTusTakimiNumarator.Clear();
            dataGridViewSatisListesi.Rows.Clear();
            txtBarkod.Clear();
            txtBarkod.Focus();
            chSatisIadeIslemi.Checked = false;
        }

        private void btnOdenen_Click(object sender, EventArgs e)
        {
            if (txtTusTakimiNumarator.Text != "")
            {
                double paraUstuSonuc = Islemler.DoubleYap(txtGenelToplam.Text) - Islemler.DoubleYap(txtTusTakimiNumarator.Text);
                txtParaUstu.Text = paraUstuSonuc.ToString("C2");
                txtOdenen.Text = Islemler.DoubleYap(txtTusTakimiNumarator.Text).ToString("C2");
                txtTusTakimiNumarator.Clear();
                txtBarkod.Focus();
            }
        }

        private void btnBarkod_Click(object sender, EventArgs e)
        {
            if (txtTusTakimiNumarator.Text != "")
            {
                if (db.Uruns.Any(x => x.Barkod == txtTusTakimiNumarator.Text))
                {
                    var urun = db.Uruns.Where(x => x.Barkod == txtTusTakimiNumarator.Text).FirstOrDefault();
                    ListeyeUrunGetir(urun, txtTusTakimiNumarator.Text, Convert.ToDouble(txtMiktar.Text));
                    txtTusTakimiNumarator.Clear();
                    txtBarkod.Focus();
                }
                else
                {

                }
            }
        }

        private void ParaUstuHesapla_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            double paraUstuSonuc = Islemler.DoubleYap(txtGenelToplam.Text) - Islemler.DoubleYap(btn.Text);
            txtParaUstu.Text = paraUstuSonuc.ToString("C2");
            txtOdenen.Text = Islemler.DoubleYap(btn.Text).ToString("C2");
        }



    }
}
