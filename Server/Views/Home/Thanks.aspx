<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Thanks
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="../../Images/iconCake128.png" alt="Thanks" style="float: left; margin-right: 20px; margin-bottom: 20px;" />
    <img src="../../Images/headerThanks.png" alt="Thanks" /><br /> 
    <p class="mediumText">
        You win! Have some cake!
    </p>
    <p class="smallText">
        In all seriousness, thanks for taking a moment to get in touch. We will get back to you
        as soon as we can.
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
