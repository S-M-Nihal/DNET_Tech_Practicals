<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaveDashboard.aspx.cs" Inherits="AcademicLeaveManagement.LeaveDashboard" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Academic ERP - Leave & Event Desk</title>
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    <style id="themeStylesheet" runat="server"></style>
    <style>
        * { box-sizing: border-box; font-family: 'Inter', sans-serif; margin: 0; padding: 0; }
        .wrapper { max-width: 1320px; margin: 0 auto; padding: 20px; }
        
        .navbar { display: flex; justify-content: space-between; align-items: center; padding: 16px 20px; border-radius: 12px; margin-bottom: 20px; border: 1px solid var(--border-color); background: var(--card-bg); flex-wrap: wrap; gap: 10px; }
        .session-timer { background: rgba(239, 68, 68, 0.15); color: #f87171; border: 1px solid #ef4444; padding: 4px 10px; border-radius: 20px; font-size: 12px; font-weight: 700; }
        .btn-ghost { padding: 8px 14px; border-radius: 6px; font-size: 13px; font-weight: 500; border: 1px solid #ef4444; background: rgba(239, 68, 68, 0.1); color: #ef4444; cursor: pointer; transition: 0.2s; }
        .btn-ghost:hover { background: rgba(239, 68, 68, 0.2); }
        .btn-export { background: #059669; color: #fff; border: none; padding: 8px 14px; border-radius: 6px; cursor: pointer; font-size: 12px; font-weight: 600; }

        .stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(180px, 1fr)); gap: 14px; margin-bottom: 20px; }
        .stat-card { background: var(--card-bg); border: 1px solid var(--border-color); border-radius: 10px; padding: 14px 18px; }
        .stat-card .title { font-size: 11px; font-weight: 600; text-transform: uppercase; color: var(--text-muted); }
        .stat-card .val { font-size: 22px; font-weight: 700; margin-top: 4px; color: var(--text-main); }
        
        .workspace { display: grid; grid-template-columns: 1.15fr 0.85fr; gap: 20px; }
        @media(max-width: 960px) { .workspace { grid-template-columns: 1fr; } }
        
        .box { background: var(--card-bg); border: 1px solid var(--border-color); border-radius: 12px; padding: 20px; margin-bottom: 20px; }
        .box-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 12px; }
        .box-header h3 { font-size: 15px; font-weight: 600; color: var(--text-main); margin: 0; }
        
        /* Calendar Styling */
        .custom-calendar { width: 100% !important; border: 1px solid var(--border-color) !important; border-collapse: separate !important; border-spacing: 4px !important; border-radius: 10px; background: transparent !important; }
        .custom-calendar tr:first-child a, .custom-calendar tr:first-child span { color: var(--text-main) !important; font-size: 14px !important; font-weight: 700 !important; text-decoration: none !important; }
        .custom-calendar th { padding: 8px 0 !important; font-size: 11px !important; font-weight: 700 !important; text-transform: uppercase !important; color: var(--text-muted) !important; background: transparent !important; border: none; }
        .custom-calendar td { text-align: center !important; height: 50px !important; border-radius: 6px !important; background: rgba(255, 255, 255, 0.03); border: 1px solid var(--border-color) !important; position: relative; cursor: pointer; transition: 0.15s; }
        .custom-calendar td a, .custom-calendar td span { color: var(--text-main) !important; text-decoration: none !important; font-weight: 600 !important; font-size: 13px !important; pointer-events: none; }
        .custom-calendar td:hover:not(.cell-disabled) { background: rgba(59, 130, 246, 0.2) !important; border-color: #3b82f6 !important; }
        
        /* Range Selection Highlighter */
        .cell-range { background: rgba(59, 130, 246, 0.35) !important; border: 1px solid #3b82f6 !important; }
        .cell-range a, .cell-range span { color: #ffffff !important; font-weight: 700 !important; }
        
        /* Dynamic Badges in DayRender */
        .event-badge { font-size: 9px !important; font-weight: 700 !important; display: block !important; padding: 2px 4px !important; border-radius: 3px !important; margin-top: 2px; }
        .ev-exam { background: #f59e0b !important; color: #000 !important; }
        .ev-holiday { background: #ef4444 !important; color: #fff !important; }
        .ev-meeting { background: #6366f1 !important; color: #fff !important; }
        .ev-approved { background: #10b981 !important; color: #fff !important; }
        .ev-pending { background: #f97316 !important; color: #fff !important; }
        .ev-rejected { background: #64748b !important; color: #fff !important; }
        .cell-today { border: 2px solid #3b82f6 !important; }
        .cell-disabled { background: rgba(0, 0, 0, 0.3) !important; opacity: 0.35; cursor: not-allowed !important; }
        
        .form-row { margin-bottom: 14px; }
        .form-row label { display: block; font-size: 12px; font-weight: 500; color: var(--text-muted); margin-bottom: 4px; }
        .ctrl { width: 100%; padding: 9px; border-radius: 6px; border: 1px solid var(--border-color); background: var(--input-bg); color: var(--text-main); font-size: 13px; outline: none; }
        .ctrl:focus { border-color: #3b82f6; }
        .btn-primary { width: 100%; padding: 11px; background: #2563eb; color: #fff; border: none; border-radius: 6px; font-weight: 600; cursor: pointer; transition: 0.2s; }
        .btn-primary:hover { background: #1d4ed8; }

        .date-range-container { display: flex; gap: 10px; }
        .date-range-container > div { flex: 1; }

        .modern-grid { width: 100%; border-collapse: collapse; margin-top: 10px; }
        .modern-grid th { background: var(--table-th); color: var(--text-muted); padding: 10px; font-size: 11px; text-transform: uppercase; border-bottom: 2px solid var(--border-color); text-align: left; }
        .modern-grid td { padding: 10px; border-bottom: 1px solid var(--border-color); font-size: 12px; color: var(--text-main); }
        .status-pill { padding: 3px 8px; border-radius: 12px; font-size: 10px; font-weight: 700; text-transform: uppercase; }
        .pill-pending { background: rgba(249, 115, 22, 0.2); color: #fb923c; border: 1px solid #f97316; }
        .pill-approved { background: rgba(34, 197, 94, 0.2); color: #4ade80; border: 1px solid #22c55e; }
        .pill-rejected { background: rgba(239, 68, 68, 0.2); color: #f87171; border: 1px solid #ef4444; }

        .btn-table-action { border: none; padding: 4px 8px; border-radius: 4px; font-size: 11px; font-weight: bold; cursor: pointer; }
        .btn-approve { background: #059669; color: white; }
        .btn-reject { background: #dc2626; color: white; }
        .btn-cancel { background: #475569; color: white; }
        
        .toast-msg { padding: 12px; border-radius: 6px; font-size: 13px; margin-bottom: 14px; font-weight: 500; }
        .toast-success { background: rgba(34, 197, 94, 0.15); border: 1px solid #22c55e; color: #4ade80; }
        .toast-error { background: rgba(239, 68, 68, 0.15); border: 1px solid #ef4444; color: #f87171; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">
            <!-- App Bar with Session Countdown -->
            <div class="navbar">
                <div>
                    <h2 style="font-size:17px; color:var(--text-main); margin-bottom: 4px;">Academic Calendar & Leave Management</h2>
                    <asp:Label ID="lblUserStatus" runat="server" style="font-size:12px; color:var(--text-muted);"></asp:Label>
                </div>
                <div style="display:flex; align-items:center; gap:12px;">
                    <span class="session-timer">Session Expiring: <span id="timerText">10:00</span></span>
                    <asp:Button ID="btnLogout" runat="server" Text="Sign Out" CssClass="btn-ghost" CausesValidation="false" OnClick="btnLogout_Click" />
                </div>
            </div>

            <!-- Toast Alert Box -->
            <asp:Panel ID="pnlToast" runat="server" Visible="false">
                <asp:Label ID="lblToast" runat="server"></asp:Label>
            </asp:Panel>

            <!-- Leave Stat Metrics -->
            <div class="stats-grid">
                <div class="stat-card">
                    <div class="title">Casual Leave (CL)</div>
                    <div class="val"><asp:Label ID="lblCL" runat="server"></asp:Label></div>
                </div>
                <div class="stat-card">
                    <div class="title">Medical Leave (ML)</div>
                    <div class="val"><asp:Label ID="lblML" runat="server"></asp:Label></div>
                </div>
                <div class="stat-card">
                    <div class="title">On-Duty Quota (OD)</div>
                    <div class="val"><asp:Label ID="lblOD" runat="server"></asp:Label></div>
                </div>
                <div class="stat-card">
                    <div class="title">Leaves Month/Total</div>
                    <div class="val"><asp:Label ID="lblMonthStats" runat="server">0 / 0</asp:Label></div>
                </div>
                <div class="stat-card">
                    <div class="title">Pending Review</div>
                    <div class="val"><asp:Label ID="lblPendingCount" runat="server">0</asp:Label></div>
                </div>
            </div>

            <div class="workspace">
                <!-- Left: Interactive Range Calendar (Click 1: From, Click 2: To) -->
                <div class="box">
                    <div class="box-header">
                        <h3>Academic Calendar</h3>
                        <span style="font-size:11px; color:var(--text-muted);">Click 1: Start Date | Click 2: End Date</span>
                    </div>
                    
                    <asp:Calendar ID="calAcademic" runat="server" CssClass="custom-calendar"
                        SelectionMode="Day" DayNameFormat="Short"
                        CausesValidation="false"
                        OnDayRender="calAcademic_DayRender" 
                        OnSelectionChanged="calAcademic_SelectionChanged">
                        <TitleStyle BackColor="Transparent" Height="36px" />
                    </asp:Calendar>

                    <div style="display:flex; justify-content:space-between; align-items:center; margin-top:10px;">
                        <asp:Label ID="lblSelectionPrompt" runat="server" ClientIDMode="Static" ForeColor="#60a5fa" Font-Size="12px" Font-Bold="true">
                            Click any date on the calendar to begin selection.
                        </asp:Label>
                        <asp:Button ID="btnResetRange" runat="server" Text="Reset Dates" CausesValidation="false" 
                            OnClientClick="resetRangeSelection(); return false;" 
                            OnClick="btnResetRange_Click" style="background:transparent; border:1px solid var(--border-color); color:var(--text-muted); padding:3px 8px; border-radius:4px; font-size:11px; cursor:pointer;" />
                    </div>

                    <!-- Legend -->
                    <div style="display:flex; gap:10px; margin-top:12px; font-size:10px; flex-wrap:wrap;">
                        <span><strong style="color:#f59e0b;">● Yellow:</strong> Exam (Midterm)</span>
                        <span><strong style="color:#ef4444;">● Red:</strong> Holiday</span>
                        <span><strong style="color:#6366f1;">● Indigo:</strong> Meeting</span>
                        <span><strong style="color:#10b981;">● Green:</strong> Approved</span>
                        <span><strong style="color:#f97316;">● Orange:</strong> Pending</span>
                        <span><strong style="color:#3b82f6;">■ Blue Fill:</strong> Selected Range</span>
                    </div>
                </div>

                <!-- Right: Leave Application Form (From Date -> To Date) -->
                <div class="box">
                    <div class="box-header">
                        <h3>Apply for Leave</h3>
                    </div>

                    <asp:ValidationSummary ID="valSummaryLeave" runat="server" DisplayMode="BulletList" CssClass="toast-msg toast-error" ValidationGroup="LeaveGroup" />

                    <!-- Date Range Inputs -->
                    <div class="form-row">
                        <div class="date-range-container">
                            <div>
                                <label>From Date</label>
                                <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" CssClass="ctrl" placeholder="YYYY-MM-DD"></asp:TextBox>
                            </div>
                            <div>
                                <label>To Date</label>
                                <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" CssClass="ctrl" placeholder="YYYY-MM-DD"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin-top:5px; font-size:12px; color:var(--text-muted);">
                            Duration: <asp:Label ID="lblTotalDays" runat="server" ClientIDMode="Static" Font-Bold="true" ForeColor="#38bdf8">0</asp:Label> working day(s)
                        </div>
                        <asp:RequiredFieldValidator ID="rfvFrom" runat="server" ControlToValidate="txtFromDate"
                            ErrorMessage="Please select a Start Date." Display="None" ValidationGroup="LeaveGroup" />
                        <asp:RequiredFieldValidator ID="rfvTo" runat="server" ControlToValidate="txtToDate"
                            ErrorMessage="Please select an End Date." Display="None" ValidationGroup="LeaveGroup" />
                        <asp:CustomValidator ID="cvDateRules" runat="server" ControlToValidate="txtFromDate"
                            OnServerValidate="cvDateRules_ServerValidate" Display="None" ValidationGroup="LeaveGroup" />
                    </div>

                    <div class="form-row">
                        <label>Leave Category</label>
                        <asp:DropDownList ID="ddlLeaveType" runat="server" CssClass="ctrl">
                            <asp:ListItem Value="CL">Casual Leave (CL)</asp:ListItem>
                            <asp:ListItem Value="ML">Medical Leave (ML)</asp:ListItem>
                            <asp:ListItem Value="OD">On-Duty / OD</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-row">
                        <label>Reason / Explanation</label>
                        <asp:TextBox ID="txtReason" runat="server" CssClass="ctrl" TextMode="MultiLine" Rows="2" placeholder="State reason for absence"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason"
                            ErrorMessage="Reason is required." Display="None" ValidationGroup="LeaveGroup" />
                    </div>

                    <div class="form-row">
                        <label>Proof Document (PDF/JPG, Max 2MB)</label>
                        <asp:FileUpload ID="fuProof" runat="server" CssClass="ctrl" />
                        <asp:CustomValidator ID="cvFileValidation" runat="server" ControlToValidate="fuProof"
                            OnServerValidate="cvFileValidation_ServerValidate" Display="None" ValidationGroup="LeaveGroup" />
                    </div>

                    <asp:Button ID="btnApplyLeave" runat="server" Text="Submit Application" CssClass="btn-primary" 
                        ValidationGroup="LeaveGroup"
                        OnClientClick="if(Page_ClientValidate('LeaveGroup')) { return confirm('Are you sure you want to submit this leave request?'); }" 
                        OnClick="btnApplyLeave_Click" />
                </div>
            </div>

            <!-- Role-Based: Faculty / HOD Approval Desk -->
            <asp:Panel ID="pnlApprovalDesk" runat="server" CssClass="box" Visible="false">
                <div class="box-header">
                    <h3>Department Review Portal (Faculty/HOD Mode)</h3>
                </div>
                <asp:GridView ID="gvPendingReview" runat="server" CssClass="modern-grid" AutoGenerateColumns="False" 
                    DataKeyNames="LeaveID" OnRowCommand="gvPendingReview_RowCommand" EmptyDataText="No pending applications awaiting decision.">
                    <Columns>
                        <asp:BoundField DataField="Applicant" HeaderText="Applicant" />
                        <asp:BoundField DataField="LeaveRange" HeaderText="Leave Duration" />
                        <asp:BoundField DataField="DaysCount" HeaderText="Days" />
                        <asp:BoundField DataField="Type" HeaderText="Category" />
                        <asp:BoundField DataField="Reason" HeaderText="Reason" />
                        <asp:BoundField DataField="ProofDoc" HeaderText="Attachment" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn-table-action btn-approve"
                                    CommandName="ApproveLeave" CommandArgument='<%# Eval("LeaveID") %>' CausesValidation="false" />
                                <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn-table-action btn-reject"
                                    CommandName="RejectLeave" CommandArgument='<%# Eval("LeaveID") %>' CausesValidation="false" style="margin-left:6px;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>

            <!-- Leave Audit Grid with Sorting, Paging, and CSV Export -->
            <div class="box">
                <div class="box-header">
                    <h3>My Leave History</h3>
                    <div style="display:flex; gap:10px; align-items:center;">
                        <asp:DropDownList ID="ddlStatusFilter" runat="server" AutoPostBack="true" CssClass="ctrl" style="width:130px; padding:4px;" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                            <asp:ListItem Value="ALL">All Status</asp:ListItem>
                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                            <asp:ListItem Value="Approved">Approved</asp:ListItem>
                            <asp:ListItem Value="Rejected">Rejected</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button ID="btnExportCSV" runat="server" Text="Export CSV" CssClass="btn-export" CausesValidation="false" OnClick="btnExportCSV_Click" />
                    </div>
                </div>

                <asp:GridView ID="gvRecords" runat="server" CssClass="modern-grid" AutoGenerateColumns="False" 
                    AllowPaging="True" PageSize="5" AllowSorting="True" 
                    DataKeyNames="LeaveID"
                    OnPageIndexChanging="gvRecords_PageIndexChanging" 
                    OnSorting="gvRecords_Sorting"
                    OnRowCommand="gvRecords_RowCommand"
                    EmptyDataText="No matching leave applications found.">
                    <Columns>
                        <asp:BoundField DataField="AppliedDate" HeaderText="Applied On" SortExpression="AppliedDate" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                        <asp:BoundField DataField="LeaveRange" HeaderText="Leave Duration" SortExpression="LeaveRange" />
                        <asp:BoundField DataField="DaysCount" HeaderText="Days" SortExpression="DaysCount" />
                        <asp:BoundField DataField="Type" HeaderText="Category" SortExpression="Type" />
                        <asp:BoundField DataField="Reason" HeaderText="Reason" />
                        <asp:BoundField DataField="ProofDoc" HeaderText="Proof" />
                        <asp:TemplateField HeaderText="Status" SortExpression="Status">
                            <ItemTemplate>
                                <span class='status-pill <%# GetStatusPillClass(Eval("Status").ToString()) %>'>
                                    <%# Eval("Status") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-table-action btn-cancel"
                                    CommandName="CancelLeave" CommandArgument='<%# Eval("LeaveID") %>' 
                                    Visible='<%# CanCancel(Eval("Status")) %>' CausesValidation="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>

    <!-- Interactive Calendar Range Selection & Session Timer Script -->
    <script>
        let rangeStart = null;
        let rangeEnd = null;

        function initCalendarRangePicker() {
            const cal = document.querySelector('.custom-calendar');
            if (!cal) return;

            // Extract displayed Month and Year from Calendar Header
            const headerCell = cal.querySelector('tr:first-child td[colspan], tr:first-child');
            let calMonth = 9; // Default to October (0-indexed: 9)
            let calYear = 2026;

            if (headerCell) {
                const headerText = headerCell.innerText || '';
                const months = ['january','february','march','april','may','june','july','august','september','october','november','december'];
                const match = headerText.match(/([A-Za-z]+)\s+(\d{4})/);
                if (match) {
                    const mIdx = months.indexOf(match[1].toLowerCase());
                    if (mIdx !== -1) calMonth = mIdx;
                    calYear = parseInt(match[2], 10);
                }
            }

            // Bind click handler to each calendar date cell
            const cells = cal.querySelectorAll('td');
            cells.forEach(td => {
                if (td.classList.contains('cell-disabled')) return;
                
                // Get the day number
                const rawText = td.innerText.trim();
                const dayMatch = rawText.match(/^\d+/);
                if (!dayMatch) return;

                const dayNum = parseInt(dayMatch[0], 10);
                if (isNaN(dayNum) || dayNum < 1 || dayNum > 31) return;

                // Create date instance
                const cellDate = new Date(calYear, calMonth, dayNum);
                const dateIso = formatDateIso(cellDate);
                td.setAttribute('data-date', dateIso);

                td.onclick = function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    handleDateSelect(cellDate, dateIso);
                };
            });
        }

        function handleDateSelect(dateObj, dateIso) {
            const txtFrom = document.getElementById('txtFromDate');
            const txtTo = document.getElementById('txtToDate');
            const prompt = document.getElementById('lblSelectionPrompt');

            // Click 1: Start Date
            if (!rangeStart || (rangeStart && rangeEnd)) {
                rangeStart = dateObj;
                rangeEnd = null;
                txtFrom.value = dateIso;
                txtTo.value = dateIso;
                if (prompt) prompt.innerText = "Start Date: " + dateIso + ". Now click your End Date.";
                renderRangeHighlight(rangeStart, rangeStart);
            } 
            // Click 2: End Date
            else {
                if (dateObj < rangeStart) {
                    rangeEnd = rangeStart;
                    rangeStart = dateObj;
                } else {
                    rangeEnd = dateObj;
                }

                const startIso = formatDateIso(rangeStart);
                const endIso = formatDateIso(rangeEnd);
                txtFrom.value = startIso;
                txtTo.value = endIso;

                const count = renderRangeHighlight(rangeStart, rangeEnd);
                if (prompt) prompt.innerText = "Selected: " + startIso + " to " + endIso + " (" + count + " working days).";
            }
        }

        function renderRangeHighlight(start, end) {
            const cal = document.querySelector('.custom-calendar');
            if (!cal) return 0;

            let workingDays = 0;
            const cells = cal.querySelectorAll('td[data-date]');

            cells.forEach(td => {
                const cur = new Date(td.getAttribute('data-date'));
                td.classList.remove('cell-range');

                if (cur >= start && cur <= end) {
                    td.classList.add('cell-range');
                    if (cur.getDay() !== 0) { // Exclude Sundays
                        workingDays++;
                    }
                }
            });

            const lblDays = document.getElementById('lblTotalDays');
            if (lblDays) lblDays.innerText = workingDays;
            return workingDays;
        }

        function resetRangeSelection() {
            rangeStart = null;
            rangeEnd = null;
            const txtFrom = document.getElementById('txtFromDate');
            const txtTo = document.getElementById('txtToDate');
            const lblDays = document.getElementById('lblTotalDays');
            const prompt = document.getElementById('lblSelectionPrompt');

            if (txtFrom) txtFrom.value = '';
            if (txtTo) txtTo.value = '';
            if (lblDays) lblDays.innerText = '0';
            if (prompt) prompt.innerText = 'Click any date on the calendar to begin selection.';

            document.querySelectorAll('.custom-calendar td.cell-range').forEach(td => {
                td.classList.remove('cell-range');
            });
        }

        function formatDateIso(d) {
            const year = d.getFullYear();
            const month = String(d.getMonth() + 1).padStart(2, '0');
            const day = String(d.getDate()).padStart(2, '0');
            return `${year}-${month}-${day}`;
        }

        // Initialize immediately
        document.addEventListener('DOMContentLoaded', initCalendarRangePicker);

        // 10-Minute Session Countdown
        let duration = 10 * 60;
        const timerDisplay = document.getElementById('timerText');
        const interval = setInterval(function () {
            let minutes = parseInt(duration / 60, 10);
            let seconds = parseInt(duration % 60, 10);

            minutes = minutes < 10 ? "0" + minutes : minutes;
            seconds = seconds < 10 ? "0" + seconds : seconds;

            if (timerDisplay) {
                timerDisplay.textContent = minutes + ":" + seconds;
            }

            if (--duration < 0) {
                clearInterval(interval);
                alert("Session expired due to inactivity. Redirecting to login.");
                window.location.href = "Login.aspx";
            }
        }, 1000);
    </script>
</body>
</html>