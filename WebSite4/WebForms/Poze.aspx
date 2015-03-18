<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Poze.aspx.cs" Inherits="WebForms_Poze" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="../Script/Script.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<span>Poze</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<img src="../Images/x.png" id="xIcon" class="invizibil"/>
    <asp:FileUpload ID="FileUpload1" runat="server" />
<asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Adauga poza" />
<asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
    <br />
    <br />
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    <asp:Button ID="Button3" runat="server" Text="Adauga Album" 
        onclick="Button3_Click" />
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
<br/>
<asp:Panel ID="Panel2"  runat="server" Height="500px">
</asp:Panel>
    <br />
</asp:Content>

