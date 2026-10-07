<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AcademicLeaveManagement.Login" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Academic ERP - Sign In</title>
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    <style>
        :root { --primary: #3b82f6; --primary-dark: #1d4ed8; --bg: #0f172a; }
        * { box-sizing: border-box; font-family: 'Inter', sans-serif; margin: 0; padding: 0; }
        body { background: radial-gradient(circle at 10% 20%, #1e293b 0%, #0f172a 90%); min-height: 100vh; display: flex; align-items: center; justify-content: center; padding: 20px; }
        .login-card { background: rgba(30, 41, 59, 0.7); backdrop-filter: blur(12px); border: 1px solid rgba(255, 255, 255, 0.1); border-radius: 16px; padding: 36px; width: 100%; max-width: 420px; box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.5); color: #fff; }
        .brand { font-size: 24px; font-weight: 700; color: #60a5fa; margin-bottom: 6px; text-align: center; }
        .subtext { font-size: 13px; color: #94a3b8; text-align: center; margin-bottom: 20px; }
        .field { margin-bottom: 16px; }
        .field label { display: block; font-size: 13px; font-weight: 500; color: #cbd5e1; margin-bottom: 6px; }
        .field input[type="text"], .field select { width: 100%; padding: 11px 14px; background: #0f172a; border: 1px solid #334155; border-radius: 8px; color: #f8fafc; font-size: 14px; outline: none; transition: 0.2s; }
        .field input[type="text"]:focus, .field select:focus { border-color: var(--primary); box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.25); }
        .btn-submit { width: 100%; padding: 12px; background: linear-gradient(135deg, #3b82f6, #2563eb); border: none; border-radius: 8px; color: #fff; font-weight: 600; cursor: pointer; transition: 0.2s; margin-top: 10px; font-size: 15px; }
        .btn-submit:hover { opacity: 0.95; transform: translateY(-1px); }
        .alert { background: rgba(239, 68, 68, 0.15); border: 1px solid #ef4444; color: #fca5a5; padding: 12px; border-radius: 8px; font-size: 13px; margin-bottom: 18px; }
        .alert ul { padding-left: 18px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-card">
            <div class="brand">Academic Leave Form</div>
            <div class="subtext">Academic Calendar & Leave Management Portal</div>

            <!-- Validation Summary -->
            <asp:ValidationSummary ID="ValSummaryLogin" runat="server" ForeColor="#fca5a5" DisplayMode="BulletList" CssClass="alert" HeaderText="Please address the following:" />

            <!-- User ID Field -->
            <div class="field">
                <label>Roll Number / Faculty ID</label>
                <asp:TextBox ID="txtUserId" runat="server" placeholder="e.g. 924001"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUserId" runat="server" ControlToValidate="txtUserId"
                    ErrorMessage="User ID / Roll Number is required." Display="Dynamic" ForeColor="#f87171" Text="*" />
            </div>

            <!-- Full Name Field -->
            <div class="field">
                <label>Full Name</label>
                <asp:TextBox ID="txtName" runat="server" placeholder="e.g. Nani"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                    ErrorMessage="Full Name is required." Display="Dynamic" ForeColor="#f87171" Text="*" />
            </div>

            <!-- Role Selector -->
            <div class="field">
                <label>Role / Designation</label>
                <asp:DropDownList ID="ddlRole" runat="server">
                    <asp:ListItem Value="Faculty">Assistant Professor / Faculty</asp:ListItem>
                    <asp:ListItem Value="Student" Selected="True">Undergraduate Student</asp:ListItem>
                    <asp:ListItem Value="Admin">Department Admin</asp:ListItem>
                </asp:DropDownList>
            </div>

            <!-- Workspace Theme Cookie Preference -->
            <div class="field">
                <label>Workspace Theme (Stored in Persistent Cookie)</label>
                <asp:DropDownList ID="ddlTheme" runat="server">
                    <asp:ListItem Value="Dark" Selected="True">Modern Midnight (Dark)</asp:ListItem>
                    <asp:ListItem Value="Light">Clean Canvas (Light)</asp:ListItem>
                </asp:DropDownList>
            </div>

            <!-- Remember Me Checkbox (7-day cookie) -->
            <div class="field" style="display:flex; align-items:center; gap:8px;">
                <asp:CheckBox ID="chkRememberMe" runat="server" />
                <label for="chkRememberMe" style="margin:0; cursor:pointer; font-size:13px; color:#cbd5e1;">Remember ID for 7 days</label>
            </div>

            <!-- Submit Button -->
            <asp:Button ID="btnLogin" runat="server" Text="Sign In to Portal" CssClass="btn-submit" OnClick="btnLogin_Click" />
            <asp:Label ID="lblMsg" runat="server" CssClass="alert" Visible="false" style="display:block; margin-top:14px; text-align:center;"></asp:Label>
        </div>
    </form>
</body>
</html>