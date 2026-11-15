using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using PlacementDashboardApp.Models;

namespace PlacementDashboardApp.Reports
{
    public partial class ReportViewer : Page
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["PlacementDBConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblGeneratedDate.Text = DateTime.Now.ToString("dd MMM yyyy, hh:mm tt");
                LoadReportData();
            }
        }

        private PlacementDataSet.PlacementRecordsDataTable GetReportData()
        {
            string selectedBranch = Request.QueryString["branch"] ?? "ALL";
            lblDepartment.Text = selectedBranch == "ALL" ? "All Departments" : selectedBranch;

            var table = new PlacementDataSet.PlacementRecordsDataTable();

            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "SELECT StudentRollNo, StudentName, Branch, CompanyName, PackageLPA, PlacementDate, Status FROM Placements";
                if (selectedBranch != "ALL")
                {
                    query += " WHERE Branch = @Branch";
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (selectedBranch != "ALL")
                    {
                        cmd.Parameters.AddWithValue("@Branch", selectedBranch);
                    }
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(table);
                    }
                }
            }

            return table;
        }

        private void LoadReportData()
        {
            var dataTable = GetReportData();
            lblTotalCount.Text = dataTable.Rows.Count.ToString();
            gvReport.DataSource = dataTable;
            gvReport.DataBind();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Campus_Placement_Report.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    gvReport.RenderControl(hw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Required by ASP.NET for GridView rendering during Excel export
        }
    }
}