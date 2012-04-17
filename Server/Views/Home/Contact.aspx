<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Contact
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <img src="../../Images/iconContact128.png" alt="Contact" style="float: left; margin-right: 20px; margin-bottom: 20px;" />
    <img src="../../Images/headerContact.png" alt="Contact" /><br /> 
    <p class="smallText>
        Comments, concerns, snide remark? We want to hear them all. Heck, drop us an email just to say hi
        if you want. We are big fans of webOS devs and consumers so we would love to chat.
    </p>
    <p class="smallText">
        Just fill out the little form below and we will get back to you as soon as we can.
    </p>
    <br style="clear:both" />
     <%Html.BeginForm("Contact", "Home", FormMethod.Post, new { name = "frmAction" }); %>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">Name:</span> <%=Html.TextBox("txtName", null, new { @class = "bigTextbox" })%></p>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">Email:</span> <%=Html.TextBox("txtEmail", null, new { @class = "bigTextbox" })%></p>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">Message:</span> <%=Html.TextArea("txtMessage", null, new { @class = "bigTextbox", rows="4" })%></p>
        <a href="javascript://" onclick="document.frmAction.submit();" class="button button_large" style="margin-left: 355px; font-family:Helvetica; font-size:18pt; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Send It!</a>
    <%Html.EndForm(); %>
    
    <div style="position: absolute; margin-left: 500px; top: 320px; width: 400px;" class="mediumText status"><%=ViewData["Status"] %></div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<link href="../../Content/jaxcore-button.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../Scripts/jaxscript.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-button.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-icon.js"></script>
</asp:Content>