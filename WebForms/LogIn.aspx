<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="LogIn.aspx.cs" Inherits="WebForms_LogIn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server" >


   

                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [Categorii_Cursuri]"></asp:SqlDataSource>
                    <asp:Label ID="Label1" runat="server"></asp:Label>
       <table class="MarginClass2">
            <tr>
                <td>Nume</td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Parola</td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server" TextMode="password"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Log In" />
                </td>
            </tr>
        </table>


</asp:Content>

