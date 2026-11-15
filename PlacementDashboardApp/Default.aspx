<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PlacementDashboardApp.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Placement Management Dashboard</title>
    <style>
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 30px; background-color: #f1f5f9; color: #1e293b; }
        .card { background: #ffffff; padding: 25px; border-radius: 8px; box-shadow: 0 4px 6px -1px rgba(0,0,0,0.1); margin-bottom: 25px; }
        h2, h3 { margin-top: 0; color: #0f172a; }
        .filter-bar { margin-bottom: 20px; display: flex; align-items: center; gap: 15px; }
        .form-select, .form-input { padding: 8px 12px; border: 1px solid #cbd5e1; border-radius: 6px; font-size: 14px; }
        .btn { padding: 9px 18px; border: none; border-radius: 6px; cursor: pointer; font-weight: 600; font-size: 14px; text-decoration: none; display: inline-block; }
        .btn-primary { background-color: #2563eb; color: #ffffff; }
        .btn-primary:hover { background-color: #1d4ed8; }
        .btn-report { background-color: #0284c7; color: #ffffff; }
        
        .custom-grid { width: 100%; border-collapse: collapse; margin-top: 15px; font-size: 14px; }
        .custom-grid th { background-color: #1e293b; color: white; padding: 12px; text-align: left; }
        .custom-grid td { padding: 12px; border-bottom: 1px solid #e2e8f0; }
        .custom-grid tr:hover { background-color: #f8fafc; }
        .badge { background: #dcfce7; color: #15803d; padding: 4px 10px; border-radius: 12px; font-size: 12px; font-weight: 600; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Top Section: Placement Records Table (GridView Data Control) -->
        <div class="card">
            <h2>Campus Placement Dashboard</h2>
            <p style="color: #64748b; margin-bottom: 20px;">Review, filter, and track registered student placement records.</p>
            
            <div class="filter-bar">
                <asp:Label ID="lblFilter" runat="server" Text="Filter Branch:" Font-Bold="true"></asp:Label>
                <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true" CssClass="form-select" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                    <asp:ListItem Text="-- All Departments --" Value="ALL" />
                    <asp:ListItem Text="CSE" Value="CSE" />
                    <asp:ListItem Text="ECE" Value="ECE" />
                    <asp:ListItem Text="MECH" Value="MECH" />
                </asp:DropDownList>

                <asp:Button ID="btnOpenReport" runat="server" Text="📊 Generate Official Report" CssClass="btn btn-report" OnClick="btnOpenReport_Click" />
            </div>

            <!-- ASP.NET Data Control: GridView -->
            <asp:GridView ID="gvPlacements" runat="server" AutoGenerateColumns="False" 
                          DataKeyNames="PlacementID" AllowPaging="True" PageSize="5"
                          OnPageIndexChanging="gvPlacements_PageIndexChanging"
                          CssClass="custom-grid" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="StudentRollNo" HeaderText="Roll No" />
                    <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                    <asp:BoundField DataField="Branch" HeaderText="Branch" />
                    <asp:BoundField DataField="CompanyName" HeaderText="Company" />
                    <asp:BoundField DataField="PackageLPA" HeaderText="Package (LPA)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="PlacementDate" HeaderText="Selection Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class="badge"><%# Eval("Status") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- Bottom Section: Record Entry Form (DetailsView Data Control) -->
        <div class="card">
            <h3>Add New Placement Record</h3>
            <p style="color: #64748b; font-size: 13px;">Use this form to insert an entry directly into SQL Server.</p>

            <asp:DetailsView ID="dvPlacementInsert" runat="server" Height="50px" Width="500px" 
                             AutoGenerateRows="False" DefaultMode="Insert"
                             OnItemInserting="dvPlacementInsert_ItemInserting"
                             GridLines="None" CellPadding="5">
                <Fields>
                    <asp:TemplateField HeaderText="Roll Number:">
                        <InsertItemTemplate>
                            <asp:TextBox ID="txtRollNo" runat="server" CssClass="form-input" Width="90%"></asp:TextBox>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Full Name:">
                        <InsertItemTemplate>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-input" Width="90%"></asp:TextBox>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Branch:">
                        <InsertItemTemplate>
                            <asp:DropDownList ID="ddlInsertBranch" runat="server" CssClass="form-select" Width="93%">
                                <asp:ListItem>CSE</asp:ListItem>
                                <asp:ListItem>ECE</asp:ListItem>
                                <asp:ListItem>MECH</asp:ListItem>
                            </asp:DropDownList>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company:">
                        <InsertItemTemplate>
                            <asp:TextBox ID="txtCompany" runat="server" CssClass="form-input" Width="90%"></asp:TextBox>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Package (LPA):">
                        <InsertItemTemplate>
                            <asp:TextBox ID="txtPackage" runat="server" CssClass="form-input" Width="90%"></asp:TextBox>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowInsertButton="True" ButtonType="Button" ControlStyle-CssClass="btn btn-primary" />
                </Fields>
            </asp:DetailsView>

            <div style="margin-top: 10px;">
                <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>