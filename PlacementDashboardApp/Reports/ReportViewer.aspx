<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="PlacementDashboardApp.Reports.ReportViewer" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Campus Placement Official Report</title>
    <style>
        body { font-family: 'Segoe UI', Arial, sans-serif; background-color: #f8fafc; margin: 0; padding: 20px; }
        .no-print { margin-bottom: 20px; display: flex; gap: 10px; }
        .btn { padding: 8px 16px; border: none; border-radius: 4px; cursor: pointer; font-weight: 600; text-decoration: none; font-size: 14px; }
        .btn-back { background-color: #64748b; color: white; }
        .btn-print { background-color: #0284c7; color: white; }
        .btn-excel { background-color: #16a34a; color: white; }
        
        .report-sheet { background: white; padding: 40px; max-width: 850px; margin: 0 auto; box-shadow: 0 4px 12px rgba(0,0,0,0.1); border-radius: 4px; }
        .header { text-align: center; border-bottom: 2px solid #0f172a; padding-bottom: 15px; margin-bottom: 25px; }
        .header h1 { margin: 0; font-size: 24px; color: #0f172a; text-transform: uppercase; }
        .header p { margin: 5px 0 0 0; color: #475569; font-size: 14px; }
        
        .meta-table { width: 100%; margin-bottom: 20px; font-size: 14px; }
        .meta-table td { padding: 4px 0; }
        
        .report-grid { width: 100%; border-collapse: collapse; margin-top: 15px; font-size: 13px; }
        .report-grid th { background-color: #f1f5f9; color: #0f172a; border: 1px solid #cbd5e1; padding: 10px; text-align: left; }
        .report-grid td { border: 1px solid #cbd5e1; padding: 9px 10px; }
        .report-footer { margin-top: 40px; display: flex; justify-content: space-between; font-size: 13px; color: #334155; }

        @media print {
            .no-print { display: none !important; }
            body { background: white; padding: 0; }
            .report-sheet { box-shadow: none; border: none; padding: 0; width: 100%; max-width: 100%; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Action Buttons (Hidden when printing/saving to PDF) -->
        <div class="no-print">
            <a href="../Default.aspx" class="btn btn-back">&larr; Return to Dashboard</a>
            <button type="button" class="btn btn-print" onclick="window.print()">🖨️ Print / Save as PDF</button>
            <asp:Button ID="btnExportExcel" runat="server" Text="📥 Export to Excel" CssClass="btn btn-excel" OnClick="btnExportExcel_Click" />
        </div>

        <!-- Printable Document Area -->
        <div class="report-sheet">
            <div class="header">
                <h1>Department Placement Report</h1>
                <p>Training & Placement Cell | Academic Year 2026</p>
            </div>

            <table class="meta-table">
                <tr>
                    <td><strong>Department Filter:</strong> <asp:Label ID="lblDepartment" runat="server" Text="All Departments"></asp:Label></td>
                    <td style="text-align: right;"><strong>Generated On:</strong> <asp:Label ID="lblGeneratedDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td><strong>Total Selected Students:</strong> <asp:Label ID="lblTotalCount" runat="server" Text="0"></asp:Label></td>
                    <td style="text-align: right;"><strong>Status:</strong> Verified Final Records</td>
                </tr>
            </table>

            <!-- Report Grid bound dynamically to PlacementDataSet -->
            <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" CssClass="report-grid" GridLines="Both">
                <Columns>
                    <asp:BoundField DataField="StudentRollNo" HeaderText="Roll Number" />
                    <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                    <asp:BoundField DataField="Branch" HeaderText="Branch" />
                    <asp:BoundField DataField="CompanyName" HeaderText="Company" />
                    <asp:BoundField DataField="PackageLPA" HeaderText="Package (LPA)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="PlacementDate" HeaderText="Date of Selection" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                </Columns>
            </asp:GridView>

            <div class="report-footer">
                <div>Prepared By: <strong>Placement Coordinator</strong></div>
                <div>Authorized Signatory: ________________________</div>
            </div>
        </div>
    </form>
</body>
</html>