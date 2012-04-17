<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="../../Images/badLogin.jpg" alt="Invalid login" style="float: left; margin-right: 20px; margin-bottom: 100px;" />
    <div class="bigText">
        <p>Excuse me.</p>
        <p>Sorry to bother you, but...umm...there is a problem.</p>
    </div>
    <p class="smallText">
        So I don't know what is going on, but I do know you shouldn't be <em>here</em>...
    </p>
    <p class="smallText">
        Maybe your session expired? Or perhaps you gave us some funky login credentials? Why don't you try logging in again
        and see if that fixes things for ya.
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
