<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Metrix - Signup
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="../../Images/iconSignup128.png" alt="Thanks" style="float: left; margin-right: 20px; margin-bottom: 20px;" />
    <img src="../../Images/headerSignup.png" alt="Ready to get started?" /><br />
    <p class="smallText">
        Great! Glad to hear it. The first thing you need to do is create yourself a profile. Nothing fancy, but we need
        to know a bit about you.
    </p>
    <br style="clear: both" />
    <%Html.BeginForm("Signup", "Home", FormMethod.Post, new { name = "frmAction" }); %>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">User Name:</span> <%=Html.TextBox("txtName", ViewData["Name"], new { @class="bigTextbox"})%></p>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">First Name:</span> <%=Html.TextBox("txtFirst", ViewData["First"], new { @class = "bigTextbox" })%></p>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">Last Name:</span> <%=Html.TextBox("txtLast", ViewData["Last"], new { @class = "bigTextbox" })%></p>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">Company:</span> <%=Html.TextBox("txtCompany", ViewData["Company"], new { @class = "bigTextbox" })%></p>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">Email:</span> <%=Html.TextBox("txtEmail", ViewData["Email"], new { @class = "bigTextbox" })%></p>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">Password:</span> <%=Html.Password("txtPassword", null, new { @class = "bigTextbox" })%></p>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">Confirm:</span> <%=Html.Password("txtConfirm", null, new { @class = "bigTextbox" })%></p>
       
        <a href="javascript://" onclick="document.frmAction.submit();" class="button button_large" style="margin-left: 355px; font-family:Helvetica; font-size:18pt; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Signup!</a>
    <%Html.EndForm(); %>
    
    <div style="position: absolute; margin-left: 500px; top: 260px; width: 400px;" class="mediumText status"><%=ViewData["Status"] %></div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<link href="../../Content/jaxcore-button.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../Scripts/jaxscript.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-button.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-icon.js"></script>
</asp:Content>
