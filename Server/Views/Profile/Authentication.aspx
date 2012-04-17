<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Authentication
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <img src="../../Images/headerAuthentication.png" alt="Authentication" /><br />
    <p class="smallText">
        Ok, almost done. You should have gotten an email that had a riduculously long code in there.
        Just copy and paste that big ole thing into the textbox below and hit go and you will
        be ready to download Metrix for your apps.
    </p>
    <span class="status"><%=ViewData["Status"] %></span>
    
    <%Html.BeginForm("Authentication", "Profile", FormMethod.Post, new { name = "frmAction" }); %>
        <p class="mediumText">
            <span style="float:left; display:block; margin-top: 8px;">Authentication Key </span>&nbsp; <%=Html.TextBox("txtKey", null, new { @class="bigTextbox"})%>
            <a href="javascript://" onclick="document.frmAction.submit();" id="btnActivate" class="button button_large" style="font-family:Helvetica; font-size:18pt; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Go!</a>
        </p>
        
    <%Html.EndForm(); %>
    
    <p class="smallText">
        If you didn't get the email, you have a couple of options.<br />
        <%Html.BeginForm("Authentication", "Profile", FormMethod.Post, new { name = "frmResend" }); %>
        1. You can resend it to <strong><%=ViewData["Email"] %></strong>. <a href="javascript://" id="btnResend" onclick="document.frmResend.submit();" class="button button_small" style="font-family:Helvetica; font-size:12pt; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Resend</a><br />
        <%=Html.Hidden("hdnResend") %>
        <%Html.EndForm(); %>
        
        <%Html.BeginForm("Authentication", "Profile", FormMethod.Post, new { name = "frmChange" }); %>
        2. You can give us another email address and send it there.<br />
        <%=Html.TextBox("txtEmail", ViewData["Email"], new { @class = "smallTextbox" })%>
        <a href="javascript://" onclick="document.frmChange.submit();" class="button button_small" id="btnChange" style="font-family:Helvetica; font-size:12pt; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Change and Resend</a><br />
        <%Html.EndForm(); %>
    </p>

    </p>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<link href="../../Content/jaxcore-button.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../Scripts/jaxscript.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-button.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-icon.js"></script>
</asp:Content>
