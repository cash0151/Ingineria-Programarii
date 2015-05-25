<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="WebForms_Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
      
    <br/> <br/>
    <asp:Panel ID="Panel1" runat="server">
        <div style="float:left"> <asp:Button ID="HomeButton" runat="server" Text="Home" OnClick="HomeButton_Click" /></div>
        <div style="float:left"> <asp:Menu ID="menu" runat="server" ></asp:Menu></div>
    </asp:Panel>
   
    
</asp:Content>

