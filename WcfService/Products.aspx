﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="WcfService.Products" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        p, h1{
    font-family: "Palatino Linotype", "Book Antiqua", Palatino, serif;
            width: 900px;
        }

.opList{
    width: 100% !important;
    margin: 8px 0 !important;
}

input[type=text], select {
    width: auto;
    padding: 12px 20px;
    margin: 8px;
    display: inline-block;
    border: 1px solid #ccc;
    border-radius: 4px;
    box-sizing: border-box;
}

input[type=submit] {
        border-style: none;
            border-color: inherit;
            border-width: medium;
            background-color: #4CAF50;
            color: white;
            padding: 14px 20px;
            margin: 8px;
            border-radius: 4px;
            cursor: pointer;
}

input[type=submit]:hover {
    background-color: #45a049;
}
</style>
</head>
<body style="height: 467px; width: 1072px;">
    <form id="form1" runat="server">
        <div style="height: 315px; width: 892px">
    <h1>Kiva Car: Add a new product</h1>
        <p>
            Position:<asp:DropDownList ID="DropDownList1" runat="server" Height="77px" style="margin-top: 0px; margin-bottom: 20px" Width="109px">
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp; New product:<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            <asp:Button ID="ButtonAdd" runat="server" OnClick="Add_Click" Text="Add" />
        </p>
            <p id="lb1">
                <asp:Label ID="Label1" runat="server"></asp:Label>
        </p>
            <div style="width: 211px; height: 309px; margin-left: 895px; margin-top: -150px; margin-bottom: 1px; top: 0px; z-index: 1;">
                <asp:Button ID="Home" runat="server" Text="Home" onclick="Home_Click" Width="164px" />
                <asp:Button ID="Logout" runat="server" Text="Logout" onclick="Logout_Click" Width="164px" />
            </div>
        </div>
        </form>
</body>
</html>
