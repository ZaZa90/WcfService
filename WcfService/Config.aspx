<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="WcfService.Config" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
<style>
p, h1{
    font-family: "Palatino Linotype", "Book Antiqua", Palatino, serif;
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
    width: auto;
    background-color: #4CAF50;
    color: white;
    padding: 14px 20px;
    margin: 8px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

input[type=submit]:hover {
    background-color: #45a049;
}
</style>

</head>
<body>
    <h1>Kiva Car: Configuration</h1>
    <form id="form1" runat="server">
        <table>
            <tbody>
                <tr>
                    <td>Rest Speed:</td>
                    <td><asp:Label ID="Label1" runat="server"></asp:Label></td>
                    <td>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Cruise Speed:</td>
                    <td><asp:Label ID="Label2" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td># of Lines:</td>
                    <td><asp:Label ID="Label3" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Turn Deviation:</td>
                    <td><asp:Label ID="Label4" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Button1" runat="server" OnClick="UpdateConfig" Text="Update" />
                    </td>
                </tr>
            </tbody>
        </table>
        <p>
            Grid Dimension:<asp:Label ID="Label5" runat="server"></asp:Label><asp:TextBox ID="TextBox5" runat="server"></asp:TextBox><asp:Button ID="ButtonDim" runat="server" OnClick="Set_Grid_Dim" Text="Set" />
        </p>
        <p>
            MANUAL DRIVE ~ Test Mode</p>
        <p>
            <asp:Button ID="ButtonManualStop" runat="server" OnClick="Stop_Click" Text="STOP" />
            <asp:Button ID="ButtonManualForward" runat="server" OnClick="Forward_Click" Text="FORWARD" />
            <asp:Button ID="ButtonManualBackward" runat="server" OnClick="Backward_Click" Text="BACKWARD" />
            <asp:Button ID="ButtonManualRight" runat="server" OnClick="Right_Click" Text="RIGHT" />
            <asp:Button ID="ButtonManualLeft" runat="server" OnClick="Left_Click" Text="LEFT" />
            <asp:Button ID="ButtonPicture" runat="server" OnClick="Picture_Click" Text="TAKE PICTURE" />
        </p>
        <p>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="WebForm1.aspx">Back</asp:HyperLink>
        </p>
    </form>
</body>
</html>
