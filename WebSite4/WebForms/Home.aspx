<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="WebForms_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <span>MAIN</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
&nbsp;
    <asp:Panel ID="Panel1" runat="server" Height="153px" Width="470px">
        &nbsp; <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" 
            style="margin-left: 0px" Text="Creaza Grup" />
        <br />
        &nbsp; <asp:Label ID="Label1" runat="server"></asp:Label>
    &nbsp;<br />
        <br />
        &nbsp;<br />
        <br />
    </asp:Panel>
&nbsp;
</asp:Content>

