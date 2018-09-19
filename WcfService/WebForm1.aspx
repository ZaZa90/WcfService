﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WcfService.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
<body>
    <form id="form1" runat="server">
    <h1>Kiva Car: Manage your robot</h1>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <asp:Timer ID="Timer1" runat="server" Enabled="true" Interval="500">
        </asp:Timer>

        <asp:UpdatePanel ID="StockPricePanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" />
            </Triggers>
            <ContentTemplate>
                <p>
                    Connection status:<asp:TextBox ID="TextBox1" runat="server" ReadOnly="true"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; IP<asp:TextBox ID="TextBox2" runat="server" ReadOnly="true"></asp:TextBox>
                    <asp:Button ID="ButtonReload" runat="server" OnClick="Reload_Click" Text="Reload" />
                    <asp:Button ID="ButtonReset" runat="server" OnClick="Reset_Click" Text="Reset" />
                </p>
                <p>
                    Current operation:<asp:TextBox ID="TextBox3" runat="server" ReadOnly="true"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    Status:<asp:TextBox ID="StatusBox" runat="server" ReadOnly="true" Width="300px"></asp:TextBox>
                </p>
                <p>
                    &nbsp;Next operation list:<asp:TextBox ID="TextBox4" runat="server" CssClass="opList" ReadOnly="true"></asp:TextBox>
                </p>
                <p>
                    Last Picture: </p>
                <p>
                    <asp:Image ID="Image1" runat="server"/>
                    <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
                </p>
                <p>
                    <asp:Label ID="Label2" runat="server" Text="Picture Code:"></asp:Label>
                    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                </p>  
            </ContentTemplate>
        </asp:UpdatePanel>
        <p>
            Target:<asp:DropDownList ID="ddlDim" runat="server" AppendDataBoundItems="true"></asp:DropDownList>
        </p>
        <p>
            Current Position:<asp:DropDownList ID="Curr_pos" runat="server"></asp:DropDownList>
            Current Direction:<asp:DropDownList ID="Curr_dir" runat="server">
                                    <asp:ListItem Text="NORTH" Value="NORTH"></asp:ListItem>
                                    <asp:ListItem Text="EAST" Value="NORTH"></asp:ListItem>
                                    <asp:ListItem Text="SOUTH" Value="NORTH"></asp:ListItem>
                                    <asp:ListItem Text="WEST" Value="NORTH"></asp:ListItem>
                              </asp:DropDownList>
        </p>
        <p>
            Binding CheckPoints:
        </p>
        <p>
            <asp:Panel ID="panel1" runat="server"></asp:Panel>
        </p>
        <p>
            <asp:CheckBoxList ID="chkList" runat="server" AutoPostBack="true" RepeatDirection ="Horizontal"></asp:CheckBoxList>
        </p>
        <p>
            <asp:Button ID="ButtonSetTarget" runat="server" OnClick="Set_Target" Text="Start" />
        </p>
        <p>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="Config.aspx">Car&#39;s Configuration & Test</asp:HyperLink>
        </p>
    </form>
    </body>
</html>
