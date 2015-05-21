<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Courses.aspx.cs" Inherits="WebForms_Courses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <p><asp:Label ID="titluCurs" runat="server" Text="Label"></asp:Label></p>
    <p>
        
        Detalii curs:</p>
    <div runat="server" id="divContent1">
         
    </div>
     <p>Locatie:</p>
        <div runat="server" id="divContent2">
              
    </div>
      <p>Program:</p>
            <div runat="server" id="divContent3">
                 
    </div>
    <div runat="server" id="registeredUsers">
        Useri inregistrati la acest curs:<br />
    </div>

    <asp:Button ID="registerToThisCourse" runat="server" Text="Register to this course" onClick="registerToThisCourseAction"/>
    <asp:Button ID="deleteFromThisCourse" runat="server" Text="Delete from this course" onClick="deleteFromThisCourseAction"/>

</asp:Content>

