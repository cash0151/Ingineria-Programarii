<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="WebForms_Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
      

    <div class="MarginClass">
         <h1  >Cele mai populare Cursuri</h1>

     <asp:Table ID="Clasament" runat="server">
         <asp:TableHeaderRow BackColor=Aqua>
                    <asp:TableHeaderCell>Locul </asp:TableHeaderCell>   
             <asp:TableHeaderCell>Cursul </asp:TableHeaderCell>    
             <asp:TableHeaderCell>Numarul de participanti </asp:TableHeaderCell>                                                    
             </asp:TableHeaderRow>
         </asp:Table>
        </div>
   <p id="p1" runat="server"></p>
    
</asp:Content>

