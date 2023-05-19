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
    public partial class AdminÜrünİşlemleri : Form
    {
        public AdminÜrünİşlemleri()
        {
            InitializeComponent();
        }

        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void LoadProducts()
        {
            SqlCommand commandList = new SqlCommand("Select ProductID,ProductName,ProductPrice,ProductBarcode,CategoryName from TableProduct inner join TableCategory on TableProduct.ProductCategory = TableCategory.CategoryID", SqlConnectionClass.connect);

            SqlConnectionClass.CheckConnection(SqlConnectionClass.connect);

            SqlDataAdapter da = new SqlDataAdapter(commandList);

            DataTable dt = new DataTable();

            da.Fill(dt);

            dgProduct.DataSource = dt;
        }

        public void LoadCategories()
        {
            SqlCommand commandLoadCategories = new SqlCommand("Select * form TableCategory", SqlConnectionClass.connect);

            SqlConnectionClass.CheckConnection(SqlConnectionClass.connect);          

            cmbBoxCategory.DisplayMember = "CategoryName";

            cmbBoxCategory.ValueMember = "CategoryID";

            SqlDataAdapter daLoadCategories = new SqlDataAdapter(commandLoadCategories);

            DataTable dtLoadCategories = new DataTable();

            daLoadCategories.Fill(dtLoadCategories);

            cmbBoxCategory.DataSource = dtLoadCategories;

        }
        public void loadCategories_new()
        {
            SqlCommand commandLoadCategories = new SqlCommand("Select * from TableCategory", SqlConnectionClass.connect);

            SqlConnectionClass.CheckConnection(SqlConnectionClass.connect);

            SqlDataReader dr = commandLoadCategories.ExecuteReader();

            while (dr.Read())
            {
                cmbBoxCategory.Items.Add(dr["CategoryName"].ToString());

            }

        }

        private void AdminÜrünİşlemleri_Load(object sender, EventArgs e)
        {
            LoadProducts();
            loadCategories_new();
        }

        private void btnMW_Click(object sender, EventArgs e)
        {
            this.Hide();
            adminPage newForm = new adminPage();
            newForm.Show();
        }

        private void bunifuGroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void bunifuLabel1_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlCommand commandAdd = new SqlCommand("Insert into TableProduct(ProductName,ProductCategory,ProductPrice,ProductBarcode) values (@pname,@pcategory,@pprice,@pbarcode)", SqlConnectionClass.connect);

            SqlConnectionClass.CheckConnection(SqlConnectionClass.connect);

            commandAdd.Parameters.AddWithValue("@pname", tboxProductName.Text);
            commandAdd.Parameters.AddWithValue("@pcategory",Convert.ToInt32(cmbBoxCategory.SelectedValue));
            commandAdd.Parameters.AddWithValue("@pprice", Convert.ToInt32(tboxProductPrice.Text));
            commandAdd.Parameters.AddWithValue("@pbarcode", tboxProductBarcode.Text);

            commandAdd.ExecuteNonQuery();

            IncreaseCategoryCount();

            LoadProducts();

            MessageBox.Show("Ürün başarıyla eklendi.");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlCommand commandDelete = new SqlCommand("Delete from TableProduct where ProductID=@pid", SqlConnectionClass.connect);

            SqlConnectionClass.CheckConnection(SqlConnectionClass.connect);

            commandDelete.Parameters.AddWithValue("@pid", Convert.ToInt32(Convert.ToInt32(SelectedID)));

            commandDelete.ExecuteNonQuery();

            DecreaseCategoryCount();

            LoadProducts();

            MessageBox.Show("Ürün başarıyla silindi.");
        }

        string SelectedID;

        private void dgProduct_SelectionChanged(object sender, EventArgs e)
        {
            if (dgProduct.CurrentRow==null)
            {

            }
            else
            {
                SelectedID = dgProduct.CurrentRow.Cells["ProductID"].Value.ToString();
                lblSelectedID.Text = SelectedID;
            }
            
        }

        private void IncreaseCategoryCount()
        {
            SqlCommand commandIncrease = new SqlCommand("Uptade TableCategory set CategoryCount+= where CategoryId=@pid");

            SqlConnectionClass.CheckConnection(SqlConnectionClass.connect);

            commandIncrease.Parameters.AddWithValue("@pid", Convert.ToInt32(cmbBoxCategory.SelectedValue));

            commandIncrease.ExecuteNonQuery();
        }

        private void DecreaseCategoryCount()
        {
            SqlCommand commandIncrease = new SqlCommand("Uptade TableCategory set CategoryCount-= where CategoryId=@pid");

            SqlConnectionClass.CheckConnection(SqlConnectionClass.connect);

            commandIncrease.Parameters.AddWithValue("@pid", Convert.ToInt32(cmbBoxCategory.SelectedValue));

            commandIncrease.ExecuteNonQuery();
        }
    }
}
