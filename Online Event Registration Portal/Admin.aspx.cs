using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_Event_Registration_Portal
{
    public partial class Admin : System.Web.UI.Page
    {
        // Reads your connection string configured in Web.config
        private string ConnString => ConfigurationManager.ConnectionStrings["EventDbConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardMetrics();
                BindGrid();
            }
        }

        // Calculates top summary cards
        private void LoadDashboardMetrics()
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();

                using (SqlCommand cmdTotal = new SqlCommand("SELECT COUNT(1) FROM EventRegistrations", conn))
                {
                    lblTotalCount.Text = cmdTotal.ExecuteScalar().ToString();
                }

                using (SqlCommand cmdToday = new SqlCommand("SELECT COUNT(1) FROM EventRegistrations WHERE CAST(CreatedAt AS DATE) = CAST(GETDATE() AS DATE)", conn))
                {
                    lblTodayCount.Text = cmdToday.ExecuteScalar().ToString();
                }
            }
        }

        // Fetches and binds data to the GridView table
        private void BindGrid()
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                string query = @"
                    SELECT RegistrationId, RegistrationRef, FullName, Email, Age, EventTrack, CreatedAt 
                    FROM EventRegistrations 
                    WHERE (FullName LIKE @Search OR Email LIKE @Search OR RegistrationRef LIKE @Search)
                      AND (@Track = '' OR EventTrack LIKE '%' + @Track + '%')
                    ORDER BY CreatedAt DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    string search = "%" + txtSearch.Text.Trim() + "%";
                    cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 100).Value = search;
                    cmd.Parameters.Add("@Track", SqlDbType.NVarChar, 100).Value = ddlFilterTrack.SelectedValue;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvRegistrations.DataSource = dt;
                    gvRegistrations.DataBind();
                }
            }
        }

        // Triggers search & filter
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        // Resets filters back to default
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlFilterTrack.SelectedIndex = 0;
            BindGrid();
        }

        // Handles removing an attendee from SQL
        protected void gvRegistrations_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int regId = Convert.ToInt32(gvRegistrations.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                string deleteQuery = "DELETE FROM EventRegistrations WHERE RegistrationId = @Id";
                using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = regId;
                    cmd.ExecuteNonQuery();
                }
            }

            LoadDashboardMetrics();
            BindGrid();
        }

        // Exports active records to a downloadable CSV spreadsheet
        protected void btnExport_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                string query = "SELECT RegistrationRef, FullName, Email, Age, EventTrack, CreatedAt FROM EventRegistrations ORDER BY CreatedAt DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("ReferenceID,FullName,Email,Age,EventTrack,RegisteredAt");

                    foreach (DataRow row in dt.Rows)
                    {
                        sb.AppendLine($"\"{row["RegistrationRef"]}\",\"{row["FullName"]}\",\"{row["Email"]}\",{row["Age"]},\"{row["EventTrack"]}\",\"{Convert.ToDateTime(row["CreatedAt"]):yyyy-MM-dd HH:mm:ss}\"");
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=Event_Attendees_" + DateTime.Now.ToString("yyyyMMdd") + ".csv");
                    Response.Charset = "";
                    Response.ContentType = "text/csv";
                    Response.Output.Write(sb.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }
}