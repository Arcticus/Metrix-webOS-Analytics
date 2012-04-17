<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Metrix - Add Bulletin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <img src="../../Images/headerAddBulletin.png" alt="Add Bulletin" /><br />           
    
    <%Html.BeginForm("AddBulletin", "Apps", FormMethod.Post, new { name = "frmAction" }); %>   
    <p>Title: <%=Html.TextBox("txtCreateTitle", "New Bulletin") %></p>
    <textarea name="editorCreate"></textarea>
    <%Html.EndForm(); %>
    
    <a href="/Apps/Bulletin/<%=ViewData["PackageID"] %>" class="button button_large" style="float:right; margin-top: 5px; font-family:Helvetica; font-size:18pt; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Cancel</a>
    <a href="javascript://" onclick="document.frmAction.submit();" class="button button_large" style="float:right; margin-top: 5px;font-family:Helvetica; margin-right: 20px; font-size:18pt; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Save!!</a>
    <span class="error"><%=ViewData["CreateStatus"] %></span>
   
    <br style="clear: both" />
    <script type="text/javascript">
        CKEDITOR.replace('editorCreate');
    </script>
    
    <script type="text/javascript">
        window.onload = function() {
        CKEDITOR.replace('editorCreate');
        };
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
<link href="../../Content/jaxcore-button.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../Scripts/jaxscript.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-button.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-icon.js"></script>
</asp:Content>