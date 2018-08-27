<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationForm.aspx.cs" Inherits="WcfService.RegistrationForm" %>

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

input[type=text], input[type=password], select {
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
    <h1>Registration: New User</h1>
    <form id="form1" runat="server">  
        <div>  
        <asp:Label ID="lb1" runat="server" Font-Bold="True" ForeColor="#FF3300"></asp:Label><br />

            <table class="auto-style1">  
                <tr>  
                    <td>Name :</td>  
                    <td>  
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>  
                    </td>  
  
               </tr>  
                <tr>  
                    <td>Password</td>  
                     <td> <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox></td>  
                </tr>  
                <tr>  
                    <td>Confirm Password</td>  
                    <td>  
                        <asp:TextBox ID="TextBox3" runat="server" TextMode="Password"></asp:TextBox>  
                    </td>  
                </tr>  
                <tr>  
                    <td>  
                        <asp:Button ID="Button1" runat="server" Text="Register"  onclick="Button1_Click" />  
                    </td>  
                </tr>  
            </table>  
        </div>  
    </form>  
</body>  
</html>
