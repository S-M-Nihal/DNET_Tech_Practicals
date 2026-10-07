<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Online_Event_Registration_Portal.Admin" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Admin Dashboard &bull; Event Attendees</title>
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700;800&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />

    <style>
        :root {
            --primary: #4f46e5;
            --primary-hover: #4338ca;
            --bg-canvas: #0f172a;
            --card-bg: #ffffff;
            --text-dark: #0f172a;
            --text-muted: #64748b;
            --border-color: #e2e8f0;
            --radius-lg: 16px;
            --radius-md: 8px;
        }

        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
        }

        body {
            background-color: #f8fafc;
            color: var(--text-dark);
            min-height: 100vh;
            padding: 30px 20px;
        }

        .container {
            max-width: 1200px;
            margin: 0 auto;
        }

        .header-bar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 24px;
            flex-wrap: wrap;
            gap: 16px;
        }

        .header-bar h1 {
            font-size: 24px;
            font-weight: 800;
            color: #0f172a;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .header-bar .btn-nav {
            text-decoration: none;
            background: #ffffff;
            border: 1px solid var(--border-color);
            color: #334155;
            padding: 8px 16px;
            border-radius: var(--radius-md);
            font-weight: 600;
            font-size: 13px;
            display: inline-flex;
            align-items: center;
            gap: 6px;
            transition: all 0.2s;
        }

        .header-bar .btn-nav:hover {
            border-color: var(--primary);
            color: var(--primary);
        }

        /* Metric Cards */
        .metrics-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
            gap: 16px;
            margin-bottom: 24px;
        }

        .metric-card {
            background: var(--card-bg);
            border: 1px solid var(--border-color);
            border-radius: var(--radius-lg);
            padding: 20px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
        }

        .metric-card h3 {
            font-size: 28px;
            font-weight: 800;
            color: #0f172a;
        }

        .metric-card p {
            font-size: 13px;
            color: var(--text-muted);
            margin-top: 2px;
        }

        .metric-icon {
            width: 48px;
            height: 48px;
            border-radius: 12px;
            background: #e0e7ff;
            color: var(--primary);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 20px;
        }

        /* Filter Bar */
        .controls-card {
            background: #ffffff;
            border: 1px solid var(--border-color);
            border-radius: var(--radius-lg);
            padding: 18px 20px;
            margin-bottom: 20px;
            display: flex;
            gap: 12px;
            flex-wrap: wrap;
            align-items: center;
            justify-content: space-between;
        }

        .search-box {
            display: flex;
            gap: 10px;
            flex: 1;
            min-width: 280px;
        }

        .input-control {
            height: 40px;
            padding: 0 12px;
            border: 1.5px solid var(--border-color);
            border-radius: var(--radius-md);
            font-size: 14px;
            outline: none;
            background: #f8fafc;
            transition: all 0.2s;
        }

        .input-control:focus {
            border-color: var(--primary);
            background: #ffffff;
        }

        .btn-action {
            height: 40px;
            padding: 0 16px;
            background: var(--primary);
            color: #ffffff;
            border: none;
            border-radius: var(--radius-md);
            font-weight: 600;
            font-size: 13px;
            cursor: pointer;
            display: inline-flex;
            align-items: center;
            gap: 6px;
            transition: background 0.2s;
        }

        .btn-action:hover {
            background: var(--primary-hover);
        }

        .btn-export {
            background: #059669;
        }

        .btn-export:hover {
            background: #047857;
        }

        /* GridView / Table styling */
        .table-card {
            background: #ffffff;
            border: 1px solid var(--border-color);
            border-radius: var(--radius-lg);
            overflow: hidden;
            box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
        }

        .table-responsive {
            width: 100%;
            overflow-x: auto;
        }

        .custom-grid {
            width: 100%;
            border-collapse: collapse;
            text-align: left;
            font-size: 13px;
        }

        .custom-grid th {
            background: #f1f5f9;
            color: #475569;
            padding: 14px 16px;
            font-weight: 700;
            border-bottom: 1px solid var(--border-color);
            white-space: nowrap;
        }

        .custom-grid td {
            padding: 14px 16px;
            border-bottom: 1px solid #f1f5f9;
            color: #1e293b;
            vertical-align: middle;
        }

        .custom-grid tr:hover {
            background: #f8fafc;
        }

        .tag-ref {
            font-family: monospace;
            background: #e2e8f0;
            padding: 2px 6px;
            border-radius: 4px;
            font-weight: 600;
            color: #0f172a;
            font-size: 12px;
        }

        .tag-track {
            display: inline-block;
            padding: 3px 10px;
            border-radius: 9999px;
            font-size: 12px;
            font-weight: 600;
            background: #e0e7ff;
            color: #3730a3;
        }

        .btn-delete {
            background: #fee2e2;
            color: #dc2626;
            border: none;
            padding: 6px 12px;
            border-radius: 6px;
            font-weight: 600;
            font-size: 12px;
            cursor: pointer;
            transition: all 0.2s;
        }

        .btn-delete:hover {
            background: #fca5a5;
        }

        .empty-state {
            padding: 40px 20px;
            text-align: center;
            color: #94a3b8;
        }

        .empty-state i {
            font-size: 40px;
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            
            <div class="header-bar">
                <div>
                    <h1><i class="fa-solid fa-users-gear" style="color: #4f46e5;"></i> Registrations Control Room</h1>
                    <p style="color: #64748b; font-size: 14px; margin-top: 4px;">Live overview of all participant applications</p>
                </div>
                <div>
                    <a href="Register.aspx" class="btn-nav"><i class="fa-solid fa-arrow-left"></i> Open Registration Form</a>
                </div>
            </div>

            <!-- Top Highlights -->
            <div class="metrics-grid">
                <div class="metric-card">
                    <div>
                        <asp:Label ID="lblTotalCount" runat="server" Text="0" Font-Bold="true" Font-Size="26px"></asp:Label>
                        <p>Total Registered</p>
                    </div>
                    <div class="metric-icon"><i class="fa-solid fa-address-card"></i></div>
                </div>

                <div class="metric-card">
                    <div>
                        <asp:Label ID="lblTodayCount" runat="server" Text="0" Font-Bold="true" Font-Size="26px"></asp:Label>
                        <p>Applied Today</p>
                    </div>
                    <div class="metric-icon" style="background: #dcfce7; color: #16a34a;"><i class="fa-solid fa-calendar-check"></i></div>
                </div>
            </div>

            <!-- Search, Filter & Export -->
            <div class="controls-card">
                <div class="search-box">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="input-control" placeholder="Search by name, email or Ref ID..."></asp:TextBox>
                    <asp:DropDownList ID="ddlFilterTrack" runat="server" CssClass="input-control">
                        <asp:ListItem Text="All Tracks" Value="" />
                        <asp:ListItem Text="Coding Hackathon" Value="Coding" />
                        <asp:ListItem Text="Web Development Bootcamp" Value="WebDev" />
                        <asp:ListItem Text="AI, ML & Robotics" Value="AI" />
                        <asp:ListItem Text="Cybersecurity" Value="Security" />
                    </asp:DropDownList>
                    <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn-action" OnClick="btnFilter_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Reset" CssClass="btn-action" Style="background:#64748b;" OnClick="btnClear_Click" />
                </div>
                <div>
                    <asp:Button ID="btnExport" runat="server" Text="Export CSV" CssClass="btn-action btn-export" OnClick="btnExport_Click" />
                </div>
            </div>

            <!-- Registrations Grid -->
            <div class="table-card">
                <div class="table-responsive">
                    <asp:GridView ID="gvRegistrations" runat="server" AutoGenerateColumns="False" 
                        CssClass="custom-grid" GridLines="None" DataKeyNames="RegistrationId"
                        OnRowDeleting="gvRegistrations_RowDeleting" EmptyDataText="No participants registered yet.">
                        <Columns>
                            <asp:BoundField DataField="RegistrationId" HeaderText="#" ReadOnly="True" />
                            
                            <asp:TemplateField HeaderText="Reference ID">
                                <ItemTemplate>
                                    <span class="tag-ref"><%# Eval("RegistrationRef") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="FullName" HeaderText="Participant Name" />
                            <asp:BoundField DataField="Email" HeaderText="Email Address" />
                            <asp:BoundField DataField="Age" HeaderText="Age" />

                            <asp:TemplateField HeaderText="Event Track">
                                <ItemTemplate>
                                    <span class="tag-track"><%# Eval("EventTrack") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="CreatedAt" HeaderText="Registered On" DataFormatString="{0:dd MMM yyyy, hh:mm tt}" />

                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="Remove" CssClass="btn-delete"
                                        OnClientClick="return confirm('Are you sure you want to remove this registration?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </div>
    </form>
</body>
</html>