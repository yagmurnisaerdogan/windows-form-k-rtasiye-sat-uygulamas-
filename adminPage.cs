using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Kirtasiye_Proje.Classes;
using System.IO;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec;

namespace Kirtasiye_Proje
{
    public partial class adminPage : Form
    {
        public adminPage()
        {
            InitializeComponent();
        }

        FilterInfoCollection Cihazlar;  //bilgisayara bağlı olan cihazları görmeye yarar
        VideoCaptureDevice kameram;     //kameraya erişim

        private void adminPage_Load(object sender, EventArgs e)
        {
            Cihazlar = new FilterInfoCollection(FilterCategory.VideoInputDevice);  //tüm video kaynaklarını al
            //Filterinfo cihazdaki görüntü yakalama cihazları hakkında bilgi tutar

            foreach (FilterInfo cihaz in Cihazlar)
            {
                cmbKamera.Items.Add(cihaz.Name);
            }

            cmbKamera.SelectedIndex = 0;
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminKategoriİşlemleri categoryPage = new AdminKategoriİşlemleri();
            categoryPage.Show();
        }

        

        private void cam_NewCam(object sender,NewFrameEventArgs eventArgs)
        {
            PictureBox1.Image = ((Bitmap)eventArgs.Frame.Clone());
        }

        private void bunifuPanel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminÜrünİşlemleri productPage = new AdminÜrünİşlemleri();
            productPage.Show();
        }

        private void btnBaslat_Click(object sender, EventArgs e)
        {
            kameram = new VideoCaptureDevice(Cihazlar[cmbKamera.SelectedIndex].MonikerString);

            kameram.NewFrame += new NewFrameEventHandler(cam_NewCam);
            kameram.Start();
        }

       

        private void adminPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (kameram != null)
            {
                if (kameram.IsRunning == true)
                {
                    kameram.Stop();
                }
            }
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            if (kameram != null)
            {
                if (kameram.IsRunning)
                {
                    kameram.Stop();
                }
            }
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            BarcodeReader barkod = new BarcodeReader();
            if (PictureBox1.Image != null)
            {
                Result res = barkod.Decode((Bitmap)PictureBox1.Image);
                try
                {
                    string dec = res.ToString().Trim();
                    if (dec !="")
                    {
                        timer1.Stop();
                        txtBarcode.Text = dec;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void QRekle_Click(object sender, EventArgs e)
        {
            qr ekle = new qr();
            ekle.Show();
            this.Hide();
        }
    }
}
