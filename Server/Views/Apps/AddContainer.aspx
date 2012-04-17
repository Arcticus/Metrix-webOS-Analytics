<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register src="ContainerWizard.ascx" tagname="ContainerWizard" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add Container
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="../../Images/headerAdStorm.png" alt="AdStorm" /><br />  
    
    <p style="padding-bottom: 15px; margin-bottom: 15px; border-bottom: solid 2px #c00;">
        Ok, so you are ready to add a new container to put some yummy ads in. Great! Let's get started.
    </p>
    <uc1:ContainerWizard ID="ContainerWizard1" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.2.6/jquery.min.js"></script>
<script type="text/javascript" src="../../Scripts/jaxscript.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-button.js"></script>

</asp:Content>