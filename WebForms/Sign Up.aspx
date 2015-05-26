<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Sign Up.aspx.cs" Inherits="WebForms_Sign_Up" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">


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
            <td>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="TextBox2" ControlToValidate="TextBox3" ErrorMessage="Parolele nu se potrivesc"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td>Confirmare Parola</td>
            <td>
                <asp:TextBox ID="TextBox3" runat="server" TextMode="password"></asp:TextBox>
            </td>

        </tr>
        <tr>
            <td>Profesor</td>
            <td>
                <asp:CheckBox ID="CheckBox1" runat="server" />
            </td>

        </tr>
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Sign Up" />
            </td>
        </tr>
    </table>

</asp:Content>

