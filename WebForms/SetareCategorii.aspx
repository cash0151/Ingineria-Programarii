<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SetareCategorii.aspx.cs" Inherits="WebForms_SetareCategorii" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">

    <div class="MarginClass">

    <asp:Table ID="TabelPreferinteSelectate" runat="server">
           <asp:TableHeaderRow BackColor=Aqua>
                    <asp:TableHeaderCell> Preferinte Selecate </asp:TableHeaderCell>
                    <asp:TableHeaderCell>  </asp:TableHeaderCell>                                        
             </asp:TableHeaderRow>
        </asp:Table>
    <br/>
    <asp:Table ID="TabelPreferintePosibile" runat="server">
           <asp:TableHeaderRow BackColor=Aqua>
                    <asp:TableHeaderCell> Alte Categorii </asp:TableHeaderCell>
                    <asp:TableHeaderCell>  </asp:TableHeaderCell>                                        
             </asp:TableHeaderRow>
        </asp:Table>
        </div>
</asp:Content>