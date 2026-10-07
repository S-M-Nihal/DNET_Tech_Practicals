using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AcademicLeaveManagement
{
    public partial class LeaveDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    Response.Redirect("Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                ApplyThemeAndCookieInfo();
                InitializeMasterDataTable();

                if (!IsPostBack)
                {
                    ViewState["SortExpression"] = "AppliedDate";
                    ViewState["SortDirection"] = "DESC";

                    RefreshMetrics();
                    BindHistoryGrid();

                    string role = Session["Role"]?.ToString();
                    if (role == "Faculty" || role == "Admin")
                    {
                        pnlApprovalDesk.Visible = true;
                        BindApprovalGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowToast("Unexpected error loading dashboard: " + ex.Message, false);
            }
        }

        private void InitializeMasterDataTable()
        {
            if (Session["MasterLeaveRepo"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("LeaveID", typeof(string));
                dt.Columns.Add("Applicant", typeof(string));
                dt.Columns.Add("AppliedDate", typeof(DateTime));
                dt.Columns.Add("StartDate", typeof(DateTime));
                dt.Columns.Add("EndDate", typeof(DateTime));
                dt.Columns.Add("LeaveRange", typeof(string));
                dt.Columns.Add("DaysCount", typeof(int));
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("Reason", typeof(string));
                dt.Columns.Add("ProofDoc", typeof(string));
                dt.Columns.Add("Status", typeof(string));
                Session["MasterLeaveRepo"] = dt;
            }
        }

        private void ApplyThemeAndCookieInfo()
        {
            string theme = "Dark";
            string lastLogin = "First Login";

            if (Request.Cookies["AcadixUserPrefs"] != null)
            {
                HttpCookie c = Request.Cookies["AcadixUserPrefs"];
                theme = c["Theme"] ?? "Dark";
                lastLogin = c["LastLoginTime"] ?? "Recent";
            }

            lblUserStatus.Text = $"{Session["UserName"]} ({Session["Role"]}) | User: {Session["UserID"]} | Last Access: {lastLogin}";

            if (theme == "Dark")
            {
                themeStylesheet.InnerHtml = @"
                    :root {
                        --bg: #0b1329; --card-bg: #111c44; --border-color: #1e293b;
                        --text-main: #f8fafc; --text-muted: #94a3b8;
                        --input-bg: #0b1329; --table-th: #1b2559;
                    }
                    body { background: var(--bg); color: var(--text-main); }";
            }
            else
            {
                themeStylesheet.InnerHtml = @"
                    :root {
                        --bg: #f8fafc; --card-bg: #ffffff; --border-color: #e2e8f0;
                        --text-main: #0f172a; --text-muted: #64748b;
                        --input-bg: #ffffff; --table-th: #f1f5f9;
                    }
                    body { background: var(--bg); color: var(--text-main); }";
            }
        }

        private void RefreshMetrics()
        {
            lblCL.Text = (Session["CL_Balance"] ?? 0) + " d";
            lblML.Text = (Session["ML_Balance"] ?? 0) + " d";
            lblOD.Text = (Session["OD_Balance"] ?? 0) + " d";

            DataTable dt = Session["MasterLeaveRepo"] as DataTable;
            string uid = (Session["UserID"] ?? string.Empty).ToString();

            int approvedThisMonth = 0;
            int totalLeaves = 0;
            int pending = 0;

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;

                    string applicant = row["Applicant"]?.ToString();
                    if (applicant == uid)
                    {
                        totalLeaves += Convert.ToInt32(row["DaysCount"]);
                        DateTime sDate = Convert.ToDateTime(row["StartDate"]);
                        if (row["Status"]?.ToString() == "Approved" && sDate.Month == DateTime.Now.Month && sDate.Year == DateTime.Now.Year)
                        {
                            approvedThisMonth += Convert.ToInt32(row["DaysCount"]);
                        }
                    }
                    if (row["Status"]?.ToString() == "Pending")
                    {
                        pending++;
                    }
                }
            }

            lblMonthStats.Text = $"{approvedThisMonth} / {totalLeaves}";
            lblPendingCount.Text = pending.ToString();
        }

        // ==========================================
        // 2-CLICK DATE RANGE ENGINE ON CALENDAR
        // ==========================================
        protected void calAcademic_SelectionChanged(object sender, EventArgs e)
        {
            DateTime clicked = calAcademic.SelectedDate;
            pnlToast.Visible = false;

            // First click: sets Start Date
            if (ViewState["RangeStart"] == null || (ViewState["RangeStart"] != null && ViewState["RangeEnd"] != null))
            {
                ViewState["RangeStart"] = clicked;
                ViewState["RangeEnd"] = null;

                txtFromDate.Text = clicked.ToString("yyyy-MM-dd");
                txtToDate.Text = clicked.ToString("yyyy-MM-dd");
                lblSelectionPrompt.Text = "Start Date set to " + clicked.ToString("dd-MMM-yyyy") + ". Now click your End Date.";
                lblTotalDays.Text = CalculateWorkingDays(clicked, clicked).ToString();
            }
            // Second click: sets End Date
            else
            {
                DateTime start = (DateTime)ViewState["RangeStart"];
                DateTime end = clicked;

                // Swap if user clicked an earlier date as second click
                if (end < start)
                {
                    DateTime temp = start;
                    start = end;
                    end = temp;
                }

                ViewState["RangeStart"] = start;
                ViewState["RangeEnd"] = end;

                txtFromDate.Text = start.ToString("yyyy-MM-dd");
                txtToDate.Text = end.ToString("yyyy-MM-dd");

                int days = CalculateWorkingDays(start, end);
                lblTotalDays.Text = days.ToString();
                lblSelectionPrompt.Text = $"Selected Range: {start:dd-MMM} to {end:dd-MMM-yyyy} ({days} working days).";
            }
        }

        protected void btnResetRange_Click(object sender, EventArgs e)
        {
            ViewState["RangeStart"] = null;
            ViewState["RangeEnd"] = null;
            txtFromDate.Text = "";
            txtToDate.Text = "";
            lblTotalDays.Text = "0";
            lblSelectionPrompt.Text = "Click any date on the calendar to begin selection.";
        }

        private int CalculateWorkingDays(DateTime start, DateTime end)
        {
            int workingDays = 0;
            for (DateTime d = start.Date; d <= end.Date; d = d.AddDays(1))
            {
                if (d.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }
            }
            return workingDays;
        }

        // ==========================================
        // CALENDAR DAYRENDER (Range Tinting & Badges)
        // ==========================================
        protected void calAcademic_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime date = e.Day.Date;

            // Highlight Today
            if (date.Date == DateTime.Today)
            {
                e.Cell.CssClass += " cell-today";
                e.Cell.ToolTip = "Today";
            }

            // Highlight Selected Range (From -> To)
            if (ViewState["RangeStart"] != null)
            {
                DateTime rStart = (DateTime)ViewState["RangeStart"];
                DateTime rEnd = ViewState["RangeEnd"] != null ? (DateTime)ViewState["RangeEnd"] : rStart;

                if (date >= rStart && date <= rEnd)
                {
                    e.Cell.CssClass += " cell-range";
                }
            }

            // Sundays
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                e.Cell.CssClass += " cell-disabled";
                e.Day.IsSelectable = false;
                e.Cell.ToolTip = "Weekend (Sunday)";
                return;
            }

            // Gazetted Holidays (15th or 2nd Saturday)
            bool isSecondSat = (date.DayOfWeek == DayOfWeek.Saturday && date.Day >= 8 && date.Day <= 14);
            if (date.Day == 15 || isSecondSat)
            {
                e.Cell.Controls.Add(new LiteralControl("<span class='event-badge ev-holiday'>Holiday</span>"));
                e.Cell.ToolTip = "Academic Holiday";
                return;
            }

            // Midterm Exams (20th - 23rd)
            if (date.Day >= 20 && date.Day <= 23)
            {
                e.Cell.Controls.Add(new LiteralControl("<span class='event-badge ev-exam'>Midterm</span>"));
                e.Cell.ToolTip = "Midterm Examination";
                return;
            }

            // Syndicate Meeting (5th)
            if (date.Day == 5)
            {
                e.Cell.Controls.Add(new LiteralControl("<span class='event-badge ev-meeting'>Meeting</span>"));
                e.Cell.ToolTip = "Faculty Syndicate Meeting";
                return;
            }

            // Badges for user's applied leaves
            DataTable dt = Session["MasterLeaveRepo"] as DataTable;
            if (dt != null)
            {
                string uid = Session["UserID"]?.ToString();
                foreach (DataRow row in dt.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;

                    if (row["Applicant"]?.ToString() == uid)
                    {
                        DateTime s = Convert.ToDateTime(row["StartDate"]);
                        DateTime ed = Convert.ToDateTime(row["EndDate"]);

                        if (date >= s && date <= ed)
                        {
                            string status = row["Status"]?.ToString() ?? "Pending";
                            string badgeClass = status == "Approved" ? "ev-approved" : (status == "Pending" ? "ev-pending" : "ev-rejected");
                            e.Cell.Controls.Add(new LiteralControl($"<span class='event-badge {badgeClass}'>{status}</span>"));
                            e.Cell.ToolTip = $"Leave: {status} ({row["Type"]})";
                            break;
                        }
                    }
                }
            }
        }

        // ==========================================
        // BUSINESS RULES VALIDATION
        // ==========================================
        protected void cvDateRules_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime start, end;
            if (!DateTime.TryParse(txtFromDate.Text.Trim(), out start) || !DateTime.TryParse(txtToDate.Text.Trim(), out end))
            {
                cvDateRules.ErrorMessage = "Please select both a valid From Date and To Date.";
                args.IsValid = false;
                return;
            }

            if (end < start)
            {
                cvDateRules.ErrorMessage = "End Date cannot be earlier than Start Date.";
                args.IsValid = false;
                return;
            }

            // Check each day in the selected span
            for (DateTime d = start.Date; d <= end.Date; d = d.AddDays(1))
            {
                // Rule 1: Gazetted Holiday collision
                bool isSecondSat = (d.DayOfWeek == DayOfWeek.Saturday && d.Day >= 8 && d.Day <= 14);
                if (d.Day == 15 || isSecondSat)
                {
                    cvDateRules.ErrorMessage = $"Leave span overlaps with a Holiday on {d:dd-MMM-yyyy}.";
                    args.IsValid = false;
                    return;
                }

                // Rule 2: Exam collision
                if (d.Day >= 20 && d.Day <= 23)
                {
                    cvDateRules.ErrorMessage = $"Leave span overlaps with Midterm Exam days ({d:dd-MMM-yyyy}).";
                    args.IsValid = false;
                    return;
                }
            }

            // Rule 3: Duplicate Range Check
            DataTable dt = Session["MasterLeaveRepo"] as DataTable;
            string uid = Session["UserID"]?.ToString() ?? string.Empty;

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;

                    if (row["Applicant"]?.ToString() == uid && row["Status"]?.ToString() != "Rejected")
                    {
                        DateTime exStart = Convert.ToDateTime(row["StartDate"]).Date;
                        DateTime exEnd = Convert.ToDateTime(row["EndDate"]).Date;

                        // Check for date range overlap
                        if (start.Date <= exEnd && end.Date >= exStart)
                        {
                            cvDateRules.ErrorMessage = $"Selected dates conflict with an existing request ({exStart:dd-MMM} to {exEnd:dd-MMM}).";
                            args.IsValid = false;
                            return;
                        }
                    }
                }
            }

            // Rule 4: Quota Check
            int requiredDays = CalculateWorkingDays(start, end);
            string cat = ddlLeaveType.SelectedValue;
            int balance = 0;
            if (Session[cat + "_Balance"] != null)
            {
                int.TryParse(Session[cat + "_Balance"].ToString(), out balance);
            }

            if (balance < requiredDays)
            {
                cvDateRules.ErrorMessage = $"Insufficient balance for {cat}. Required: {requiredDays}, Available: {balance}.";
                args.IsValid = false;
                return;
            }

            args.IsValid = true;
        }

        protected void cvFileValidation_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (fuProof.HasFile)
            {
                string ext = Path.GetExtension(fuProof.FileName).ToLower();
                if (ext != ".pdf" && ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    cvFileValidation.ErrorMessage = "Attachment must be a PDF or JPG/PNG image.";
                    args.IsValid = false;
                    return;
                }

                if (fuProof.PostedFile.ContentLength > 2 * 1024 * 1024)
                {
                    cvFileValidation.ErrorMessage = "Attached document exceeds the 2MB size limit.";
                    args.IsValid = false;
                    return;
                }
            }
            args.IsValid = true;
        }

        // ==========================================
        // SUBMIT APPLICATION
        // ==========================================
        protected void btnApplyLeave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            DateTime start = DateTime.Parse(txtFromDate.Text.Trim());
            DateTime end = DateTime.Parse(txtToDate.Text.Trim());
            int days = CalculateWorkingDays(start, end);

            string savedFileName = "None";
            if (fuProof.HasFile)
            {
                string uploadFolder = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

                savedFileName = $"{Guid.NewGuid().ToString().Substring(0, 6)}_{Path.GetFileName(fuProof.FileName)}";
                fuProof.SaveAs(Path.Combine(uploadFolder, savedFileName));
            }

            string cat = ddlLeaveType.SelectedValue;
            string role = Session["Role"]?.ToString() ?? "Student";
            string initialStatus = (role == "Faculty") ? "Approved" : "Pending";

            // Deduct required working days
            int currentBal = Convert.ToInt32(Session[cat + "_Balance"] ?? 0);
            Session[cat + "_Balance"] = Math.Max(0, currentBal - days);

            // Record into Master Session Repo
            DataTable dt = (DataTable)Session["MasterLeaveRepo"];
            DataRow newRow = dt.NewRow();
            newRow["LeaveID"] = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            newRow["Applicant"] = Session["UserID"].ToString();
            newRow["AppliedDate"] = DateTime.Now;
            newRow["StartDate"] = start;
            newRow["EndDate"] = end;
            newRow["LeaveRange"] = (start == end) ? start.ToString("dd-MMM-yyyy") : $"{start:dd-MMM} to {end:dd-MMM-yyyy}";
            newRow["DaysCount"] = days;
            newRow["Type"] = cat;
            newRow["Reason"] = txtReason.Text.Trim();
            newRow["ProofDoc"] = savedFileName;
            newRow["Status"] = initialStatus;
            dt.Rows.Add(newRow);

            // Reset range picker state
            ViewState["RangeStart"] = null;
            ViewState["RangeEnd"] = null;
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtReason.Text = "";
            lblTotalDays.Text = "0";
            lblSelectionPrompt.Text = "Leave submitted successfully! Select dates on calendar for a new request.";

            RefreshMetrics();
            BindHistoryGrid();
            if (pnlApprovalDesk.Visible) BindApprovalGrid();

            ShowToast($"Leave application submitted for {days} working day(s). Status: {initialStatus}", true);
        }

        // ==========================================
        // GRIDVIEW BINDING, SORTING, FILTERING & ACTIONS
        // ==========================================
        private void BindHistoryGrid()
        {
            DataTable dt = Session["MasterLeaveRepo"] as DataTable;
            if (dt == null) return;

            string uid = Session["UserID"]?.ToString() ?? string.Empty;

            DataView dv = new DataView(dt);
            string filter = $"Applicant = '{uid}'";
            if (ddlStatusFilter.SelectedValue != "ALL")
            {
                filter += $" AND Status = '{ddlStatusFilter.SelectedValue}'";
            }
            dv.RowFilter = filter;

            string sortExp = ViewState["SortExpression"]?.ToString() ?? "AppliedDate";
            string sortDir = ViewState["SortDirection"]?.ToString() ?? "DESC";
            dv.Sort = $"{sortExp} {sortDir}";

            gvRecords.DataSource = dv;
            gvRecords.DataBind();
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvRecords.PageIndex = 0;
            BindHistoryGrid();
        }

        protected void gvRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRecords.PageIndex = e.NewPageIndex;
            BindHistoryGrid();
        }

        protected void gvRecords_Sorting(object sender, GridViewSortEventArgs e)
        {
            string currentSortExp = ViewState["SortExpression"]?.ToString();
            string currentSortDir = ViewState["SortDirection"]?.ToString();

            if (currentSortExp == e.SortExpression)
            {
                ViewState["SortDirection"] = currentSortDir == "ASC" ? "DESC" : "ASC";
            }
            else
            {
                ViewState["SortExpression"] = e.SortExpression;
                ViewState["SortDirection"] = "ASC";
            }
            BindHistoryGrid();
        }

        protected void gvRecords_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelLeave")
            {
                string leaveId = e.CommandArgument.ToString();
                DataTable dt = Session["MasterLeaveRepo"] as DataTable;

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted) continue;

                        if (row["LeaveID"].ToString() == leaveId && row["Status"].ToString() == "Pending")
                        {
                            string cat = row["Type"].ToString();
                            int daysToRefund = Convert.ToInt32(row["DaysCount"]);
                            int bal = Convert.ToInt32(Session[cat + "_Balance"] ?? 0);
                            Session[cat + "_Balance"] = bal + daysToRefund;

                            row.Delete();
                            break;
                        }
                    }
                    dt.AcceptChanges();
                    RefreshMetrics();
                    BindHistoryGrid();
                    if (pnlApprovalDesk.Visible) BindApprovalGrid();
                    ShowToast("Pending request cancelled and quota refunded.", true);
                }
            }
        }

        private void BindApprovalGrid()
        {
            DataTable dt = Session["MasterLeaveRepo"] as DataTable;
            if (dt == null) return;

            DataView dv = new DataView(dt);
            dv.RowFilter = "Status = 'Pending'";
            gvPendingReview.DataSource = dv;
            gvPendingReview.DataBind();
        }

        protected void gvPendingReview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string leaveId = e.CommandArgument.ToString();
            DataTable dt = Session["MasterLeaveRepo"] as DataTable;

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;

                    if (row["LeaveID"].ToString() == leaveId)
                    {
                        if (e.CommandName == "ApproveLeave")
                        {
                            row["Status"] = "Approved";
                            ShowToast($"Application {leaveId} marked as Approved.", true);
                        }
                        else if (e.CommandName == "RejectLeave")
                        {
                            row["Status"] = "Rejected";
                            string cat = row["Type"].ToString();
                            int daysToRefund = Convert.ToInt32(row["DaysCount"]);
                            int bal = Convert.ToInt32(Session[cat + "_Balance"] ?? 0);
                            Session[cat + "_Balance"] = bal + daysToRefund;
                            ShowToast($"Application {leaveId} rejected and balance restored.", false);
                        }
                        break;
                    }
                }

                RefreshMetrics();
                BindHistoryGrid();
                BindApprovalGrid();
            }
        }

        protected void btnExportCSV_Click(object sender, EventArgs e)
        {
            DataTable dt = Session["MasterLeaveRepo"] as DataTable;
            string uid = Session["UserID"]?.ToString() ?? string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("LeaveID,Applicant,AppliedDate,StartDate,EndDate,Duration,Days,Category,Reason,ProofDocument,Status");

            if (dt != null)
            {
                foreach (DataRow r in dt.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;

                    if (r["Applicant"].ToString() == uid)
                    {
                        sb.AppendLine($"\"{r["LeaveID"]}\",\"{r["Applicant"]}\",\"{Convert.ToDateTime(r["AppliedDate"]):yyyy-MM-dd HH:mm}\",\"{Convert.ToDateTime(r["StartDate"]):yyyy-MM-dd}\",\"{Convert.ToDateTime(r["EndDate"]):yyyy-MM-dd}\",\"{r["LeaveRange"]}\",\"{r["DaysCount"]}\",\"{r["Type"]}\",\"{r["Reason"]}\",\"{r["ProofDoc"]}\",\"{r["Status"]}\"");
                    }
                }
            }

            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", $"attachment;filename=LeaveHistory_{uid}.csv");
                Response.Charset = "";
                Response.ContentType = "text/csv";
                Response.Output.Write(sb.ToString());
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ShowToast("Failed to export CSV: " + ex.Message, false);
            }
        }

        public bool CanCancel(object status)
        {
            return status != null && status.ToString() == "Pending";
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            if (Request.Cookies["AcadixUserPrefs"] != null)
            {
                HttpCookie c = new HttpCookie("AcadixUserPrefs") { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(c);
            }

            Response.Redirect("Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        public string GetStatusPillClass(string status)
        {
            switch (status)
            {
                case "Approved": return "pill-approved";
                case "Pending": return "pill-pending";
                default: return "pill-rejected";
            }
        }

        private void ShowToast(string message, bool isSuccess)
        {
            pnlToast.Visible = true;
            pnlToast.CssClass = "toast-msg " + (isSuccess ? "toast-success" : "toast-error");
            lblToast.Text = message;
        }
    }
}