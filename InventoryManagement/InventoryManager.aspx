<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventoryManager.aspx.cs" Inherits="Practical8_Inventory.InventoryManager" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventory Management Module</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f4f6f9;
            margin: 0;
            padding: 25px;
        }
        .container {
            max-width: 900px;
            margin: auto;
            background: #ffffff;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }
        h2 {
            color: #333;
            border-bottom: 2px solid #007bff;
            padding-bottom: 8px;
            margin-top: 0;
        }
        .form-row {
            display: flex;
            gap: 15px;
            margin-bottom: 12px;
        }
        .form-col {
            flex: 1;
        }
        label {
            display: block;
            font-weight: 600;
            margin-bottom: 4px;
            color: #495057;
        }
        .form-control {
            width: 100%;
            padding: 8px;
            border: 1px solid #ced4da;
            border-radius: 4px;
            box-sizing: border-box;
        }
        .btn {
            padding: 8px 18px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-weight: 600;
        }
        .btn-primary { background-color: #007bff; color: white; }
        .btn-secondary { background-color: #6c757d; color: white; }
        .btn-edit { background-color: #ffc107; color: #212529; border: none; padding: 4px 10px; border-radius: 3px; cursor: pointer; }
        .btn-delete { background-color: #dc3545; color: white; border: none; padding: 4px 10px; border-radius: 3px; cursor: pointer; }
        .alert {
            display: block;
            margin-top: 12px;
            font-weight: bold;
        }
        .table-responsive {
            margin-top: 25px;
        }
        .gridview {
            width: 100%;
            border-collapse: collapse;
        }
        .gridview th {
            background-color: #007bff;
            color: white;
            text-align: left;
            padding: 10px;
        }
        .gridview td {
            border: 1px solid #dee2e6;
            padding: 8px;
        }
        .gridview tr:nth-child(even) {
            background-color: #f8f9fa;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Inventory Management Module (ADO.NET CRUD)</h2>
            
            <asp:HiddenField ID="hfProductId" runat="server" Value="0" />

            <div class="form-row">
                <div class="form-col">
                    <label>Product Name</label>
                    <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" placeholder="e.g. Wireless Mouse"></asp:TextBox>
                </div>
                <div class="form-col">
                    <label>Category</label>
                    <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control" placeholder="e.g. Electronics"></asp:TextBox>
                </div>
            </div>

            <div class="form-row">
                <div class="form-col">
                    <label>Quantity</label>
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TextMode="Number" placeholder="e.g. 50"></asp:TextBox>
                </div>
                <div class="form-col">
                    <label>Unit Price (INR)</label>
                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="e.g. 799.00"></asp:TextBox>
                </div>
            </div>

            <div style="margin-top: 15px;">
                <asp:Button ID="btnSave" runat="server" Text="Save Product" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" />
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="alert"></asp:Label>

            <div class="table-responsive">
                <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="ProductId" CssClass="gridview"
                    OnRowEditing="gvProducts_RowEditing" 
                    OnRowDeleting="gvProducts_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="ProductId" HeaderText="ID" ReadOnly="True" ItemStyle-Width="60px" />
                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                        <asp:BoundField DataField="Category" HeaderText="Category" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-Width="90px" />
                        <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price (INR)" DataFormatString="{0:F2}" ItemStyle-Width="120px" />
                        <asp:ButtonField ButtonType="Button" CommandName="Edit" Text="Edit" ControlStyle-CssClass="btn-edit" ItemStyle-Width="70px" />
                        <asp:ButtonField ButtonType="Button" CommandName="Delete" Text="Delete" ControlStyle-CssClass="btn-delete" ItemStyle-Width="70px" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>