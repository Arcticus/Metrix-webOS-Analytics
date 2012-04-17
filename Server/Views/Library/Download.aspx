<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Download
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="../../Images/headerDownload.png" alt="Download" /><br />
    <p class="mediumText">
        Download Ready!
    </p>
    <p class="smallText">
        Ok, the library is queued up and ready to go. Inside the zip you will find the code to insert into your app
        and instructions how to do everything.
    </p>
    <p>
        If you ever run into any issues, please don't hesitate to email us at <a href="mailto:metrix@webosroundup.com">metrix@webosroundup.com</a>.
    </p>
    <%Html.BeginForm("Download", "Library", FormMethod.Post, new { style = "float:right", name = "frmAction" }); %>
    <%=Html.Hidden("hdnHiThere") %>
    
    <a href="javascript://" onclick="document.frmAction.submit();" class="button button_large" style="font-family:Helvetica; font-size:18pt; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Download Now!</a>
    
    <%Html.EndForm(); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<link href="../../Content/jaxcore-button.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../Scripts/jaxscript.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-button.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-icon.js"></script>
</asp:Content>
