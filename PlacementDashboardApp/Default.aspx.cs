using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PlacementDashboardApp
{
    public partial class Default : Page
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["PlacementDBConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPlacementGrid();
            }
        }

        private void BindPlacementGrid()
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "SELECT PlacementID, StudentRollNo, StudentName, Branch, CompanyName, PackageLPA, PlacementDate, Status FROM Placements";
                if (ddlBranch.SelectedValue != "ALL")
                {
                    query += " WHERE Branch = @Branch";
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (ddlBranch.SelectedValue != "ALL")
                    {
                        cmd.Parameters.AddWithValue("@Branch", ddlBranch.SelectedValue);
                    }

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvPlacements.DataSource = dt;
                        gvPlacements.DataBind();
                    }
                }
            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPlacements.PageIndex = 0;
            BindPlacementGrid();
        }

        protected void gvPlacements_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPlacements.PageIndex = e.NewPageIndex;
            BindPlacementGrid();
        }

        protected void dvPlacementInsert_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            TextBox txtRoll = (TextBox)dvPlacementInsert.FindControl("txtRollNo");
            TextBox txtName = (TextBox)dvPlacementInsert.FindControl("txtName");
            DropDownList ddlBranchInsert = (DropDownList)dvPlacementInsert.FindControl("ddlInsertBranch");
            TextBox txtCompany = (TextBox)dvPlacementInsert.FindControl("txtCompany");
            TextBox txtPackage = (TextBox)dvPlacementInsert.FindControl("txtPackage");

            if (string.IsNullOrWhiteSpace(txtRoll.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please fill in Roll Number and Student Name.";
                return;
            }

            decimal packageVal = 0;
            decimal.TryParse(txtPackage.Text.Trim(), out packageVal);

            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = @"INSERT INTO Placements (StudentRollNo, StudentName, Branch, CompanyName, PackageLPA, PlacementDate, Status) 
                                 VALUES (@Roll, @Name, @Branch, @Company, @Package, @Date, 'Selected')";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Roll", txtRoll.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Branch", ddlBranchInsert.SelectedValue);
                    cmd.Parameters.AddWithValue("@Company", txtCompany.Text.Trim());
                    cmd.Parameters.AddWithValue("@Package", packageVal);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Record added successfully!";
            BindPlacementGrid();
        }

        protected void btnOpenReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Reports/ReportViewer.aspx?branch=" + Server.UrlEncode(ddlBranch.SelectedValue));
        }
    }
}