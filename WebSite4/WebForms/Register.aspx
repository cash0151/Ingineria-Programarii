<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="WebForms_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
Register
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<div id="RegisterTableContainer">
<table  id="RegisterTable">
     <tr>
        <td>
           
        </td>
        <td>
            
            <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
            
        </td>
        <td class="style1">

            &nbsp;</td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="Utilizator"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        </td>
        <td class="style1">
            &nbsp;</td>
    </tr>
        <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Parola"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TextBox2" runat="server" 
                TextMode="Password"></asp:TextBox>
        </td>
        <td class="style1">
            &nbsp;</td>
        <td>
            
            <asp:CompareValidator ID="CompareValidator1" runat="server" 
                ControlToCompare="TextBox2" ErrorMessage="Parolele nu se potrivesc" 
                ControlToValidate="TextBox3"></asp:CompareValidator>
           
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label4" runat="server" Text="Confirmare parola"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TextBox3" runat="server" 
                TextMode="Password"></asp:TextBox>
        </td>
        <td class="style1">
            &nbsp;</td>
    </tr>
     <tr>
        <td>
           
            &nbsp;</td>
        <td>
            <asp:Button ID="Button1" runat="server" Text="Register" onclick="Button1_Click" />
                   
        </td>
        <td class="style1">

            &nbsp;</td>
    </tr>
</table>
</div>
</asp:Content>
