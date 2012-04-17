<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Metrix - Support
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="../../Images/iconSupport128.png" alt="Support" style="float: left; margin-right: 20px; margin-bottom: 20px;" />
    <img src="../../Images/headerSupport.png" alt="Support" /><br /> 
    <p class="smallText>
        Got questions about Metrix? Whether you need some answers before taking the plunge, or you need
        some help getting setup, we are here to help.
    </p>
    <p class="smallText">
        Depending on your mood, we have a few ways to get in touch with support. First, you could 
        <a href="http://forums.webosroundup.com/forumdisplay.php?23-Metrix">visit
        our forums</a> and ask a question. We check it all the time so we will get back to you ASAP.
    </p>
    <p class="smallText">
        If that doesn't work for ya, you can fill out this handy form right here and we will get in
        touch as soon as we can.
    </p>
    <br style="clear:both" />
     <%Html.BeginForm("Support", "Home", FormMethod.Post, new { name = "frmAction" }); %>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">Name:</span> <%=Html.TextBox("txtName", null, new { @class = "bigTextbox" })%></p>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">Company:</span> <%=Html.TextBox("txtCompany", null, new { @class = "bigTextbox" })%></p>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">Email:</span> <%=Html.TextBox("txtEmail", null, new { @class = "bigTextbox" })%></p>
        <p class="mediumText"><span style="float:left; display:block; width:160px;">Issue:</span> <%=Html.TextArea("txtReason", null, new { @class = "bigTextbox", rows="4" })%></p>
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
