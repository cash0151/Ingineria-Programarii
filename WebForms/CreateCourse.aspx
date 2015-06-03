<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateCourse.aspx.cs" Inherits="WebForms_CreateCourse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        #TextArea1 {
            height: 87px;
            width: 367px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div class="MarginClass">
    <p>
        <asp:Label ID="Label1" runat="server"></asp:Label>
    </p>
    <p>
        Nume Curs:<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    </p>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [Categorii_Cursuri]"></asp:SqlDataSource>
    <p>
        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="NumeCategorie" DataValueField="Id">
        </asp:DropDownList>
        
    </p>
    <p>
        Detalii curs:</p>
    <p>
        <asp:TextBox id="TextBox2" TextMode="multiline" Columns="50" Rows="5" runat="server" /></p>
    <p>
        Locul desfasurarii trainingului:</p>
    <p>
        <asp:TextBox id="TextBox3" TextMode="multiline" Columns="50" Rows="5" runat="server" Height="38px" Width="404px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
       
        
    </p>
    <p>
        Programul desfasurarii trainingului:
    </p>
    <p>
         <asp:TextBox id="TextBox4" TextMode="multiline" Columns="50" Rows="5" runat="server" Height="38px" Width="404px" />
    </p>
    <asp:Label ID="Label2" runat="server" Text="Oras:"></asp:Label>
&nbsp;&nbsp;
    <asp:Label ID="Label3" runat="server" Text="Bucuresti"></asp:Label>
    <p>
        <asp:Button ID="Button4" runat="server" Text="Creaza" OnClick="Button4_Click" />
    </p>
        </div>
</asp:Content>

