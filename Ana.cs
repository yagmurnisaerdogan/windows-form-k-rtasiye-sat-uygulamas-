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
    public partial class Ana : Form
    {
        public Ana()
        {
            InitializeComponent();
        }
        public partial class fBaslangic : Form
        {
            public fBaslangic()
            {
                InitializeComponent();
            }

            private void btnSatisIslemi_Click(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;

                fSatis satis = new fSatis();
                satis.lblKullanici.Text = lblKullanici.Text;
                satis.ShowDialog();

                Cursor.Current = Cursors.Default;
            }

            private void btnRaporlar_Click(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;

                fRapor rapor = new fRapor();
                rapor.lblKullanici.Text = lblKullanici.Text;
                rapor.ShowDialog();

                Cursor.Current = Cursors.Default;
            }

            private void btnStok_Click(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;

                fStokIslemleri stokIslemleri = new fStokIslemleri();
                stokIslemleri.lblKullanici.Text = lblKullanici.Text;
                stokIslemleri.ShowDialog();

                Cursor.Current = Cursors.Default;
            }

            private void btnUrunGiris_Click(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;

                fUrunGiris urunGiris = new fUrunGiris();
                urunGiris.lblKullanici.Text = lblKullanici.Text;
                urunGiris.ShowDialog();

                Cursor.Current = Cursors.Default;
            }

            private void btnFiyatGuncelle_Click(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;
                fFiyatGuncelle guncelle = new fFiyatGuncelle();
                guncelle.ShowDialog();
                Cursor.Current = Cursors.Default;
            }

            private void btnAyarlar_Click(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;
                fAyarlar ayarlar = new fAyarlar();
                ayarlar.lblKullanici.Text = lblKullanici.Text;
                ayarlar.ShowDialog();
                Cursor.Current = Cursors.Default;
            }

            private void btnBackup_Click(object sender, EventArgs e)
            {
                Islemler.Backup();
            }

            private void btnKullaniciDegistir_Click(object sender, EventArgs e)
            {
                fLogin login = new fLogin();
                login.Show();
                this.Hide();
            }

            private void btnCikis_Click(object sender, EventArgs e)
            {
                Application.Exit();
            }
        }
        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
