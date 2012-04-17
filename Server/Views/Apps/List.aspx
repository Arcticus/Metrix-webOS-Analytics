<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Metrix - Apps
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%  List<string> apps = (List<string>)ViewData["Apps"]; %>
    
    <img src="../../Images/headerApps.png" alt="Apps" /><br />
    <div class="mediumText">
        <%  if (apps.Count > 0)
            {   %>
                <div style="margin-bottom: 30px">  
        <%          foreach (string pid in apps)
                    {%>
                        <p>
                            <div style="float:left; margin-top: -0px;"><%=pid%> </div>
                            <a href="../Apps/Analytics/<%=pid%>"><img src="../../Images/iconAnalytics32.png" title="Analytics" style="margin-left: 15px" alt="Analytics" /></a>
                            <a href="../Apps/Bulletin/<%=pid%>"><img src="../../Images/iconBulletins32.png" title="Bulletins" alt="Bulletins" /></a>
                            <%--<a href="../Apps/AdStorm/<%=app.PackageID %>"><img src="../../Images/iconAdStorm32.png" title="adStorm" alt="adStorm" /></a>--%>
                            <a href="../Apps/Delete/<%=pid%>"><img src="../../Images/iconDelete32.png" title="Delete app from Metrix" alt="Delete" /></a>
                        </p>
        <%          }%>
                </div>
        <%  }
            else
            {%>
                <p><strong>Hmmm...don't see any apps around here.</strong></p>
                <p class="smallText">Since you are new around these parts, don't forget to <a href="/Library/License">download the Metrix libary</a>.</p>
       <%   } %>
        <p style="margin-top: 20px">
            Why not claim an app right this instant?<br />
            <span class="smallText">Just type in your Package ID from HP (i.e. com.webosroundup.awesomeapp)</span>
        </p>
        <p>
            <%Html.BeginForm("List", "Apps", FormMethod.Post, new { name = "frmAction" }); %>
            <%=Html.TextBox("txtPackage", null, new { @class="bigTextbox"}) %>
            <a href="javascript://" onclick="document.frmAction.submit();" class="button button_large" style="font-family:Helvetica; font-size:18pt; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Claim it!</a>
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
