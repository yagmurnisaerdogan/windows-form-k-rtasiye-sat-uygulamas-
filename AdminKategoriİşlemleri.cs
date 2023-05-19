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

namespace Kirtasiye_Proje
{
    public partial class AdminKategoriİşlemleri : Form
    {
        public AdminKategoriİşlemleri()
        {
            InitializeComponent();
        }

        public void LoadCategories()
        {
            SqlCommand commandListCategories = new SqlCommand("Select * from TableCategory", (SqlConnection)SqlConnectionClass.connect);

            SqlConnectionClass.CheckConnection(SqlConnectionClass.connect);

            SqlDataAdapter da = new SqlDataAdapter(commandListCategories);

            DataTable dt = new DataTable();

            da.Fill(dt);

            bunifuDataGridView1.DataSource = dt;
        }

        private void AdminKategoriİşlemleri_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            SqlCommand commandAddCategory = new SqlCommand("Insert into TableCategory (CategoryName) values (@pname)", SqlConnectionClass.connect);

            SqlConnectionClass.CheckConnection(SqlConnectionClass.connect);

            commandAddCategory.Parameters.AddWithValue("@pname", tboxCategoryName.Text);

            commandAddCategory.ExecuteNonQuery();

            LoadCategories();

        }

        string SelectedID;
        private void btnCategoryDelete_Click(object sender, EventArgs e)
        {
            SqlCommand commandDelete = new SqlCommand("Delete from TableCategory where CategoryID=@pid", SqlConnectionClass.connect);

            SqlConnectionClass.CheckConnection(SqlConnectionClass.connect);

            commandDelete.Parameters.AddWithValue("@pid", Convert.ToInt32(SelectedID));

            commandDelete.ExecuteNonQuery();

            LoadCategories();

            MessageBox.Show("Veri başarıyla silindi.");
        }

        private void bunifuDataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            SelectedID = bunifuDataGridView1.CurrentRow.Cells["CategoryID"].Value.ToString();
            lblSelectedID.Text = SelectedID;
        }

        private void btnMW_Click(object sender, EventArgs e)
        {
            this.Hide();
            adminPage newForm = new adminPage();
            newForm.Show();
        }
    }
}
