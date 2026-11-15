using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Practical8_Inventory.DataAccess;

namespace Practical8_Inventory
{
    public partial class InventoryManager : Page
    {
        private readonly InventoryDAL dal = new InventoryDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            gvProducts.DataSource = dal.GetAllProducts();
            gvProducts.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text) ||
                string.IsNullOrWhiteSpace(txtCategory.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "All fields are required.";
                return;
            }

            int id = Convert.ToInt32(hfProductId.Value);
            string name = txtProductName.Text.Trim();
            string category = txtCategory.Text.Trim();

            if (!int.TryParse(txtQuantity.Text.Trim(), out int qty) || qty < 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please enter a valid positive quantity.";
                return;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price) || price < 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please enter a valid price.";
                return;
            }

            if (id == 0)
            {
                // INSERT Operation
                bool success = dal.InsertProduct(name, category, qty, price);
                if (success)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Product added successfully.";
                }
            }
            else
            {
                // UPDATE Operation
                bool success = dal.UpdateProduct(id, name, category, qty, price);
                if (success)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Product updated successfully.";
                }
            }

            ClearForm();
            BindGrid();
        }

        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int id = Convert.ToInt32(gvProducts.DataKeys[e.NewEditIndex].Value);
            GridViewRow row = gvProducts.Rows[e.NewEditIndex];

            hfProductId.Value = id.ToString();
            txtProductName.Text = Server.HtmlDecode(row.Cells[1].Text);
            txtCategory.Text = Server.HtmlDecode(row.Cells[2].Text);
            txtQuantity.Text = Server.HtmlDecode(row.Cells[3].Text);
            txtPrice.Text = Server.HtmlDecode(row.Cells[4].Text);

            btnSave.Text = "Update Product";
            btnSave.CssClass = "btn btn-primary";
            lblMessage.Text = "";

            e.Cancel = true; // Prevent GridView from switching to inline textboxes
        }

        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);
            bool success = dal.DeleteProduct(id);

            if (success)
            {
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "Product deleted successfully.";
            }

            ClearForm();
            BindGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblMessage.Text = "";
        }

        private void ClearForm()
        {
            hfProductId.Value = "0";
            txtProductName.Text = string.Empty;
            txtCategory.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtPrice.Text = string.Empty;
            btnSave.Text = "Save Product";
        }
    }
}