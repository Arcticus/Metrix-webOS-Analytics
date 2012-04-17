<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Metrix - Delete App
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="../../Images/iconDelete128.png" alt="Invalid login" style="float: left; margin-right: 20px; margin-bottom: 40px;" />
    <p class="bigText">Whoa now...wait a minute. Deleting an app is serious stuff.</p>
    <p class="mediumText">
        Let's take a second and breathe...<br />
        There is no shame in turning around and forgetting we were ever here.
    </p>
    <p style="clear:both" class="mediumText">
        Are you absolutely, positively, 100% sure that you want to delete 
        <strong><%=ViewData["PID"] %></strong>?
    </p>
    
    <%Html.BeginForm("Delete", "Apps", FormMethod.Post, new { style = "float:right", name = "frmAction" }); %>   
    <%=Html.Hidden("hdnDelete") %>
    <a href="javascript://" onclick="document.frmCancel.submit();" class="button button_large" style="font-family:Helvetica; font-size:18pt; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">No, take me to a happy place.</a>
    <%Html.EndForm(); %>
    
     <%Html.BeginForm("List", "Apps", FormMethod.Post, new { style = "float:right", name = "frmCancel" }); %>
    <a href="javascript://" onclick="document.frmAction.submit();" class="button button_large" style="font-family:Helvetica; font-size:18pt; margin-right: 20px;" onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Yes! Do it.</a>
    <%Html.EndForm(); %>

   
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<link href="../../Content/jaxcore-button.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../Scripts/jaxscript.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-button.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-icon.js"></script>
</asp:Content>
