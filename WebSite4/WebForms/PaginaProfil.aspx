<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PaginaProfil.aspx.cs" Inherits="WebForms_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../Script/Script.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Pagina Profil
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <p>
        <asp:Label ID="Label1" runat="server" Font-Size="Large" Font-Strikeout="False"></asp:Label>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:Button ID="Button5" runat="server" onclick="Button5_Click" Text="Albume" />
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" 
            Text="Adauga la Prieteni" />&nbsp;<asp:Button ID="Button4" runat="server" 
            onclick="Button4_Click" Text="Alaturate grupului" />
    </p>
    <p>
        &nbsp;&nbsp;<asp:Label ID="Label2" runat="server"></asp:Label>
        &nbsp;&nbsp;</p>
    <p>
        &nbsp;<asp:RadioButton ID="Public" runat="server" Text="Public" GroupName="tipProfil">
        </asp:RadioButton>
         <asp:RadioButton ID="Privat" runat="server" Text="Privat" GroupName="tipProfil">
        </asp:RadioButton>

        <asp:Button ID="Button3" runat="server" onclick="Button3_Click" 
            Text="Schimba Tipul profilului" />

    </p>

    <p>
        <asp:TextBox ID="TextBox1" runat="server" Height="89px" Width="322px"></asp:TextBox>
    </p>
    <p>
&nbsp;
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            Text="Posteaza" />
    </p>
    <asp:Panel ID="Panel1" runat="server">
        <br />
        <br />
        <br />
    </asp:Panel>
</asp:Content>

