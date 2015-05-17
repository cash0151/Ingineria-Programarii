<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProfilePage.aspx.cs" Inherits="WebForms_ProfilePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <script>
        function ClearRating() {
            document.getElementById("ContentPlaceHolder2_1").src = '../Images/steaalba.png';
            document.getElementById("ContentPlaceHolder2_2").src = '../Images/steaalba.png';
            document.getElementById("ContentPlaceHolder2_3").src = '../Images/steaalba.png';
            document.getElementById("ContentPlaceHolder2_4").src = '../Images/steaalba.png';
            document.getElementById("ContentPlaceHolder2_5").src = '../Images/steaalba.png';
        }

        function change1() {           
            document.getElementById("ContentPlaceHolder2_1").src = '../Images/stea.png';
        }
        function change2() {
            document.getElementById("ContentPlaceHolder2_1").src = '../Images/stea.png';
            document.getElementById("ContentPlaceHolder2_2").src = '../Images/stea.png';
        }
        function change3() {
            document.getElementById("ContentPlaceHolder2_1").src = '../Images/stea.png';
            document.getElementById("ContentPlaceHolder2_2").src = '../Images/stea.png';
            document.getElementById("ContentPlaceHolder2_3").src = '../Images/stea.png';
        }
        function change4() {
            document.getElementById("ContentPlaceHolder2_1").src = '../Images/stea.png';
            document.getElementById("ContentPlaceHolder2_2").src = '../Images/stea.png';
            document.getElementById("ContentPlaceHolder2_3").src = '../Images/stea.png';
            document.getElementById("ContentPlaceHolder2_4").src = '../Images/stea.png';
        }
        function change5() {
            document.getElementById("ContentPlaceHolder2_1").src = '../Images/stea.png';
            document.getElementById("ContentPlaceHolder2_2").src = '../Images/stea.png';
            document.getElementById("ContentPlaceHolder2_3").src = '../Images/stea.png';
            document.getElementById("ContentPlaceHolder2_4").src = '../Images/stea.png';
            document.getElementById("ContentPlaceHolder2_5").src = '../Images/stea.png';
        }

   </script>
     <asp:Table ID="TabelCursuri" runat="server">
           <asp:TableHeaderRow BackColor=Aqua>
                    <asp:TableHeaderCell> Traininguri </asp:TableHeaderCell>                                       
             </asp:TableHeaderRow>
        </asp:Table>

     <br />
     <br />

     <asp:Panel ID="PanelRating" runat="server">
         &nbsp;&nbsp;&nbsp; &nbsp;Review
         <br />
         &nbsp;&nbsp;&nbsp;
         <br />
         &nbsp;&nbsp;&nbsp;
         <asp:TextBox ID="TextBox1" runat="server" Height="130px" TextMode="MultiLine" Width="954px"></asp:TextBox>
         &nbsp;<br /> &nbsp;<br /> &nbsp;&nbsp;&nbsp;
         <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="Add Review" />
         <br />
         &nbsp;&nbsp;&nbsp;&nbsp;
         <br />
         <br />
     </asp:Panel>

    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <br/>
    <asp:Button ID="Button5" runat="server" OnClick="Button5_Click" Text="Add Training" />

    <!-- *********-->

    <p>
        <asp:Button ID="selectPreferinta" runat="server" Text="Setare Categorii Preferinte" OnClick="selectPreferinta_Click" Width="224px" />
    </p>

    <p> Nume cursurilor ce fac parte din categoriile selectate ce deja au o nota si la care ati putea particicpa: </p>
     <asp:Table ID="TableCursuri" runat="server">
           <asp:TableHeaderRow BackColor=Aqua>
                    <asp:TableHeaderCell> Nume Curs </asp:TableHeaderCell>
                    <asp:TableHeaderCell> Valoare Medie Review </asp:TableHeaderCell>                                        
             </asp:TableHeaderRow>
        </asp:Table>

    <p> Nume de profesori ordonati dupa review ce predau Traninguri de interes: </p>
     <asp:Table ID="TabelProfCursuriSelectate" runat="server">
           <asp:TableHeaderRow BackColor=Aqua>
                    <asp:TableHeaderCell> Nume Profesor </asp:TableHeaderCell>
                    <asp:TableHeaderCell> Valoare Medie </asp:TableHeaderCell>                                        
             </asp:TableHeaderRow>
        </asp:Table>

    <p> Perechile curs profesor la care nu ai participat si care au fost vodate drept clele mai bune de comuitate: </p>
     <asp:Table ID="TablePerechi" runat="server">
           <asp:TableHeaderRow BackColor=Aqua>
                    <asp:TableHeaderCell> Nume Curs </asp:TableHeaderCell>
                    <asp:TableHeaderCell> Nume Profesor </asp:TableHeaderCell>                                        
             </asp:TableHeaderRow>
        </asp:Table>
  <!--  <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label> -->
    <!-- *********-->

</asp:Content>

