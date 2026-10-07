<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Online_Event_Registration_Portal.Register" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>EventHub &bull; Online Registration Portal</title>
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700;800&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />

    <style>
        :root {
            --primary: #4f46e5;
            --primary-hover: #4338ca;
            --primary-glow: rgba(79, 70, 229, 0.25);
            --bg-canvas: #0f172a;
            --card-bg: rgba(255, 255, 255, 0.96);
            --text-dark: #0f172a;
            --text-muted: #64748b;
            --border-color: #e2e8f0;
            --border-focus: #6366f1;
            --danger: #ef4444;
            --danger-bg: #fef2f2;
            --danger-border: #fecaca;
            --success: #10b981;
            --success-bg: #ecfdf5;
            --radius-lg: 18px;
            --radius-md: 10px;
            --shadow-card: 0 25px 50px -12px rgba(15, 23, 42, 0.25), 0 0 0 1px rgba(255, 255, 255, 0.1);
        }

        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            -webkit-font-smoothing: antialiased;
        }

        body {
            background-color: var(--bg-canvas);
            background-image: 
                radial-gradient(at 0% 0%, rgba(99, 102, 241, 0.3) 0px, transparent 50%),
                radial-gradient(at 100% 100%, rgba(236, 72, 153, 0.2) 0px, transparent 50%),
                radial-gradient(at 50% 50%, rgba(15, 23, 42, 0.8) 0px, transparent 100%);
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 40px 16px;
            color: var(--text-dark);
        }

        .portal-wrapper {
            width: 100%;
            max-width: 680px;
            background: var(--card-bg);
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-card);
            overflow: hidden;
            animation: fadeIn 0.4s ease-out;
        }

        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(12px); }
            to { opacity: 1; transform: translateY(0); }
        }

        .portal-header {
            background: linear-gradient(135deg, #4f46e5 0%, #3730a3 100%);
            color: #ffffff;
            padding: 36px 32px;
            text-align: center;
            position: relative;
        }

        .portal-header .badge {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            font-size: 11px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 0.08em;
            background: rgba(255, 255, 255, 0.18);
            backdrop-filter: blur(8px);
            padding: 4px 12px;
            border-radius: 9999px;
            margin-bottom: 12px;
        }

        .portal-header h1 {
            font-size: 26px;
            font-weight: 800;
            letter-spacing: -0.02em;
            margin-bottom: 6px;
        }

        .portal-header p {
            color: #c7d2fe;
            font-size: 14px;
            max-width: 440px;
            margin: 0 auto;
        }

        .portal-body {
            padding: 32px;
        }

        .val-summary {
            background: var(--danger-bg);
            border: 1px solid var(--danger-border);
            border-radius: var(--radius-md);
            padding: 14px 18px;
            margin-bottom: 24px;
            color: #991b1b;
            font-size: 13px;
        }

        .val-summary ul {
            margin: 8px 0 0 16px;
        }

        .val-summary li {
            margin-bottom: 3px;
        }

        .form-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 18px;
        }

        .col-span-2 {
            grid-column: span 2;
        }

        @media (max-width: 600px) {
            .form-grid {
                grid-template-columns: 1fr;
            }
            .col-span-2 {
                grid-column: span 1;
            }
            .portal-body {
                padding: 24px 20px;
            }
        }

        .field-group {
            display: flex;
            flex-direction: column;
        }

        .field-label {
            font-size: 13px;
            font-weight: 600;
            color: #334155;
            margin-bottom: 6px;
            display: flex;
            align-items: center;
            gap: 6px;
        }

        .input-box {
            position: relative;
            display: flex;
            align-items: center;
        }

        .input-box > i:first-child {
            position: absolute;
            left: 14px;
            color: #94a3b8;
            font-size: 14px;
            pointer-events: none;
            transition: color 0.2s;
        }

        .input-box .toggle-password {
            position: absolute;
            right: 14px;
            left: auto;
            cursor: pointer;
            color: #94a3b8;
            font-size: 14px;
            pointer-events: auto;
            transition: color 0.2s;
            z-index: 2;
        }

        .input-box .toggle-password:hover {
            color: var(--primary);
        }

        .form-control {
            width: 100%;
            height: 44px;
            padding: 0 14px 0 40px;
            border: 1.5px solid var(--border-color);
            background: #f8fafc;
            border-radius: var(--radius-md);
            font-size: 14px;
            color: var(--text-dark);
            transition: all 0.2s ease-in-out;
        }

        .form-control.has-toggle {
            padding-right: 42px;
        }

        .form-control:focus {
            outline: none;
            background: #ffffff;
            border-color: var(--border-focus);
            box-shadow: 0 0 0 4px var(--primary-glow);
        }

        .form-control:focus ~ i:first-child {
            color: var(--primary);
        }

        .val-error {
            color: var(--danger);
            font-size: 12px;
            font-weight: 500;
            margin-top: 5px;
            display: flex;
            align-items: center;
            gap: 4px;
        }

        .terms-card {
            display: flex;
            align-items: flex-start;
            gap: 10px;
            padding: 12px 14px;
            background: #f8fafc;
            border: 1px solid var(--border-color);
            border-radius: var(--radius-md);
            margin-top: 6px;
        }

        .terms-card input[type="checkbox"] {
            margin-top: 3px;
            width: 16px;
            height: 16px;
            accent-color: var(--primary);
            cursor: pointer;
        }

        .terms-label {
            font-size: 13px;
            color: #475569;
            cursor: pointer;
            line-height: 1.4;
        }

        .btn-stack {
            display: flex;
            gap: 12px;
            margin-top: 26px;
        }

        .btn {
            height: 46px;
            border-radius: var(--radius-md);
            font-size: 14px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s ease;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            border: none;
        }

        .btn-primary {
            flex: 2;
            background: var(--primary);
            color: #ffffff;
            box-shadow: 0 4px 14px var(--primary-glow);
        }

        .btn-primary:hover {
            background: var(--primary-hover);
            transform: translateY(-1px);
        }

        .btn-secondary {
            flex: 1;
            background: #f1f5f9;
            color: #475569;
        }

        .btn-secondary:hover {
            background: #e2e8f0;
            color: #1e293b;
        }

        .result-container {
            margin-top: 24px;
            animation: fadeIn 0.3s ease;
        }

        .ticket-card {
            background: #ffffff;
            border: 1.5px dashed #cbd5e1;
            border-radius: var(--radius-md);
            padding: 20px;
            display: flex;
            gap: 18px;
            position: relative;
            box-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.05);
        }

        .ticket-badge {
            background: var(--success-bg);
            color: var(--success);
            width: 48px;
            height: 48px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 20px;
            flex-shrink: 0;
        }

        .ticket-info h4 {
            font-size: 16px;
            font-weight: 700;
            color: #0f172a;
            margin-bottom: 4px;
        }

        .ticket-info p {
            font-size: 13px;
            color: #64748b;
            line-height: 1.5;
        }

        .ticket-meta {
            margin-top: 10px;
            font-size: 12px;
            font-weight: 600;
            color: var(--primary);
            display: flex;
            gap: 14px;
            flex-wrap: wrap;
        }

        .ticket-meta span {
            display: inline-flex;
            align-items: center;
            gap: 5px;
        }

        .alert-error {
            background: var(--danger-bg);
            border: 1px solid var(--danger-border);
            color: #991b1b;
            padding: 14px;
            border-radius: var(--radius-md);
            font-size: 13px;
            display: flex;
            align-items: center;
            gap: 8px;
        }
    </style>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <script type="text/javascript">
    // @ts-nocheck
    function validateTerms(source, args) {
        var chk = document.getElementById('<%= chkTerms.ClientID %>');
        if (chk) {
            args.IsValid = chk.checked;
        } else {
            args.IsValid = false;
        }
    }

    function togglePasswordVisibility(inputId, iconId) {
        var input = document.getElementById(inputId);
        var icon = document.getElementById(iconId);
        if (input && icon) {
            if (input.getAttribute('type') === 'password') {
                input.setAttribute('type', 'text');
                icon.classList.remove('fa-eye');
                icon.classList.add('fa-eye-slash');
            } else {
                input.setAttribute('type', 'password');
                icon.classList.remove('fa-eye-slash');
                icon.classList.add('fa-eye');
            }
        }
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="portal-wrapper">
            
            <div class="portal-header">
                <div class="badge"><i class="fa-solid fa-sparkles"></i> 2026 Summit</div>
                <h1>Online Event Registration</h1>
                <p>Register your pass for the annual technical and engineering tracks</p>
            </div>

            <div class="portal-body">
                
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="val-summary" 
                    HeaderText="Please fix the following issues before continuing:" />

                <div class="form-grid">
                    
                    <!-- Full Name -->
                    <div class="field-group col-span-2">
                        <label class="field-label" for="txtName"><i class="fa-regular fa-user"></i> Full Name</label>
                        <div class="input-box">
                            <i class="fa-solid fa-signature"></i>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Jane Doe" autocomplete="name"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                            ErrorMessage="Full Name is required" CssClass="val-error" Display="Dynamic">
                            <i class="fa-solid fa-circle-exclamation"></i> Full Name is required
                        </asp:RequiredFieldValidator>
                    </div>

                    <!-- Email Address -->
                    <div class="field-group">
                        <label class="field-label" for="txtEmail"><i class="fa-regular fa-envelope"></i> Email Address</label>
                        <div class="input-box">
                            <i class="fa-solid fa-at"></i>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="jane@example.com" autocomplete="email"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="Email Address is required" CssClass="val-error" Display="Dynamic">
                            <i class="fa-solid fa-circle-exclamation"></i> Email is required
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                            ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"
                            ErrorMessage="Valid email format required" CssClass="val-error" Display="Dynamic">
                            <i class="fa-solid fa-circle-exclamation"></i> Enter a valid email
                        </asp:RegularExpressionValidator>
                    </div>

                    <!-- Age -->
                    <div class="field-group">
                        <label class="field-label" for="txtAge"><i class="fa-regular fa-calendar"></i> Age</label>
                        <div class="input-box">
                            <i class="fa-solid fa-hashtag"></i>
                            <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" placeholder="18 - 60"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvAge" runat="server" ControlToValidate="txtAge"
                            ErrorMessage="Age is required" CssClass="val-error" Display="Dynamic">
                            <i class="fa-solid fa-circle-exclamation"></i> Age is required
                        </asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvAge" runat="server" ControlToValidate="txtAge" MinimumValue="18" MaximumValue="60" Type="Integer"
                            ErrorMessage="Age must be between 18 and 60" CssClass="val-error" Display="Dynamic">
                            <i class="fa-solid fa-circle-exclamation"></i> Must be 18 to 60
                        </asp:RangeValidator>
                    </div>

                    <!-- Password -->
                    <div class="field-group">
                        <label class="field-label" for="txtPass"><i class="fa-solid fa-lock"></i> Password</label>
                        <div class="input-box">
                            <i class="fa-solid fa-key"></i>
                            <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="form-control has-toggle" placeholder="••••••••"></asp:TextBox>
                            <i id="eyePass" class="fa-solid fa-eye toggle-password" 
                               onclick="togglePasswordVisibility('<%= txtPass.ClientID %>', 'eyePass')"></i>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvPass" runat="server" ControlToValidate="txtPass"
                            ErrorMessage="Password is required" CssClass="val-error" Display="Dynamic">
                            <i class="fa-solid fa-circle-exclamation"></i> Password is required
                        </asp:RequiredFieldValidator>
                    </div>

                    <!-- Confirm Password -->
                    <div class="field-group">
                        <label class="field-label" for="txtConfirmPass"><i class="fa-solid fa-shield-halved"></i> Confirm Password</label>
                        <div class="input-box">
                            <i class="fa-solid fa-check-double"></i>
                            <asp:TextBox ID="txtConfirmPass" runat="server" TextMode="Password" CssClass="form-control has-toggle" placeholder="••••••••"></asp:TextBox>
                            <i id="eyeConfirm" class="fa-solid fa-eye toggle-password" 
                               onclick="togglePasswordVisibility('<%= txtConfirmPass.ClientID %>', 'eyeConfirm')"></i>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvConfirmPass" runat="server" ControlToValidate="txtConfirmPass"
                            ErrorMessage="Confirm Password is required" CssClass="val-error" Display="Dynamic">
                            <i class="fa-solid fa-circle-exclamation"></i> Confirm Password is required
                        </asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPass" ControlToCompare="txtPass"
                            ErrorMessage="Passwords do not match" CssClass="val-error" Display="Dynamic">
                            <i class="fa-solid fa-circle-exclamation"></i> Passwords do not match
                        </asp:CompareValidator>
                    </div>

                    <!-- Event Dropdown -->
                    <div class="field-group col-span-2">
                        <label class="field-label" for="ddlEvent"><i class="fa-solid fa-cubes"></i> Select Event Track</label>
                        <div class="input-box">
                            <i class="fa-solid fa-layer-group"></i>
                            <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-control">
                                <asp:ListItem Text="-- Choose an Event Track --" Value="" />
                                <asp:ListItem Text="Technical Coding Hackathon" Value="Coding" />
                                <asp:ListItem Text="Full-Stack Web Development Bootcamp" Value="WebDev" />
                                <asp:ListItem Text="AI, ML & Robotics Conference" Value="AI" />
                                <asp:ListItem Text="Cybersecurity Red-Team Workshop" Value="Security" />
                            </asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvEvent" runat="server" ControlToValidate="ddlEvent" InitialValue=""
                            ErrorMessage="Select an event track" CssClass="val-error" Display="Dynamic">
                            <i class="fa-solid fa-circle-exclamation"></i> Please choose an event track
                        </asp:RequiredFieldValidator>
                    </div>

                    <!-- Terms Agreement -->
                    <div class="field-group col-span-2">
                        <div class="terms-card">
                            <asp:CheckBox ID="chkTerms" runat="server" />
                            <label for="<%= chkTerms.ClientID %>" class="terms-label">
                                I confirm that all submitted details are accurate and agree to follow the <strong>Summit Code of Conduct</strong> and health protocols.
                            </label>
                        </div>
                        <asp:CustomValidator ID="cvTerms" runat="server" ClientValidationFunction="validateTerms" OnServerValidate="cvTerms_ServerValidate"
                            ErrorMessage="Terms must be accepted" CssClass="val-error" Display="Dynamic">
                            <i class="fa-solid fa-circle-exclamation"></i> You must accept the terms before registering
                        </asp:CustomValidator>
                    </div>

                </div>

                <!-- Form Controls -->
                <div class="btn-stack">
                    <asp:Button ID="btnSubmit" runat="server" Text="Confirm & Register" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset Form" CssClass="btn btn-secondary" OnClick="btnReset_Click" CausesValidation="false" />
                </div>

            </div>
        </div>
    </form>
</body>
</html>