﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Courses.aspx.cs" Inherits="WebForms_Courses" %>

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
    <div runat="server" ID="divContent1" > 
    </div>

    <div runat="server" id="registeredUsers">
        Useri inregistrati la acest curs:<br />
    </div>

    <asp:Button ID="registerToThisCourse" runat="server" Text="Register to this course" onClick="registerToThisCourseAction"/>
    <asp:Button ID="deleteFromThisCourse" runat="server" Text="Delete from this course" onClick="deleteFromThisCourseAction"/>

</asp:Content>

