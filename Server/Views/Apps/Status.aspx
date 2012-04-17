<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Metrix - Check Status
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <div id="LeftContent">
        <img src="../../Images/headerStatus.png" alt="Status" /><br />
        <p class="smallText">
            To get the full benefits of Metrix, your app needs to be published on some sort of feed: App Catalog, Web, Beta, or even Homebrew.
            It doesn't matter to us which one, but it has to be out there. Now what if you have an app you aren't ready for the big time, but you
            want to see if Metrix is installed correctly? 
        </p>
        <p class="smallText">
            That is what this page is for. Simply type in the Package ID (i.e. com.awesome.app) and hit the check button. We will tell our gerbils to get
            to work and let you know if all things are working properly and data is ready to be collected.
        </p>
         <p>
                <%Html.BeginForm("Status", "Apps", FormMethod.Post, new { name = "frmAction" }); %>
                <%=Html.TextBox("txtPackage", null, new { @class="bigTextbox"}) %>
                <a href="javascript://" onclick="document.frmAction.submit();" class="button button_large" style="font-family:Helvetica; font-size:18pt; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Check Please!</a>
                <%Html.EndForm(); %>
        </p>
        <p class="status"><%=ViewData["Status"] %></p>
   </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<link href="../../Content/jaxcore-button.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../Scripts/jaxscript.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-button.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-icon.js"></script>
</asp:Content>
