<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="WebForms_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<title>Login</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    LOGIN
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<div id="LoginTableContainer">
<table  id="LoginTable">
     <tr>
        <td>
           
        </td>
        <td>
            
            <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
            
        </td>
        <td>

        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="Utilizator"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ErrorMessage="*" ControlToValidate="TextBox1"></asp:RequiredFieldValidator>
        </td>
    </tr>
        <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Parola"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TextBox2" runat="server" ontextchanged="TextBox2_TextChanged" 
                TextMode="Password"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="*" ControlToValidate="TextBox2"></asp:RequiredFieldValidator>
        </td>
    </tr>
     <tr>
        <td>
           
        </td>
        <td>
            <asp:Button ID="Button1" runat="server" Text="Log In" onclick="Button1_Click" />
                   
        </td>
        <td>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
                onselecting="SqlDataSource1_Selecting" SelectCommand="SELECT * FROM [Login]">
            </asp:SqlDataSource>

        </td>
    </tr>
</table>
</div>
</asp:Content>