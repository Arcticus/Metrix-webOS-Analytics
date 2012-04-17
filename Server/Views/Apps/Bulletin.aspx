<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Metrix - Bulletins
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%  List<CommandLibrary.MetrixBulletinBoard> bbList = (List<CommandLibrary.MetrixBulletinBoard>)ViewData["Bulletins"]; %>
    <%  CommandLibrary.MetrixBulletinBoard version = (CommandLibrary.MetrixBulletinBoard)ViewData["Version"]; %>
    <%  string pid = (string)ViewData["PID"]; %>
    <div id="LeftContent">
        <img src="../../Images/headerAppBulletins.png" alt="Apps" /><br />       
        <p class="smallText">
            Creating a bulletin for an app will publish it to all users the next time they load your application.
            Users will be able to "snooze", which will cause the bulletin to go away, but come back on next load, or
            they can close it and not see it again.
        </p>
        <p class="smallText">
            You can send out a bulletin at anytime without updating your app. Also worth noting is that a bulletin
            is capable of full HTML output, including images and text formatting. If you want to fine tune the
            HTML, just click the Source button.        
        </p>
        <%Html.BeginForm("Bulletin", "Apps", FormMethod.Post, new { name = "frmAction" }); %>
        <p class="mediumText">Version</p>
        <p class="smallText" style="margin-top: -15px">
            The version is important. When a user closes a bulletin, they will not see another one until
            you update the version. Once the version is updated, then all the bulletins will be
            visible by the user (with the newest showing first). Be sure to read the support docs for more details.<br />
            <%=Html.TextBox("txtUpdateVersion", version.BulletinBoardVersion, new { style = "width: 50px" })%>
            <a href="javascript://" onclick="document.frmAction.submit();" class="button button_small" style="font-family:Helvetica; font-size:12pt; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Update Version</a>
            <span class="status" style="font-size: 12pt"><%=ViewData["VersionStatus"] %></span>
        </p>
        <%Html.EndForm(); %>
        
        <p class="smallText" style="float:right">
            <a href="/Apps/AddBulletin/<%=pid%>">Pssst...wanna add a new bulletin?</a>
        </p>
        <br style="clear: both" />
        <%Html.BeginForm(); %>
        <%  foreach (CommandLibrary.MetrixBulletinBoard bb in bbList)
            {
                var list = bb.MetrixBulletinBoardAnnouncements.OrderByDescending(i => i.ID);
                
                foreach (CommandLibrary.MetrixBulletinBoardAnnouncement a in list)
	            {   %>
	                <div class="bulletin"> 
	                    <p class="smallText">
	                        <strong><%=a.Title %></strong>
	                        <a href="/Apps/MaintainBulletin/<%=a.ID %>">Edit</a>
	                        <a onclick="return confirm('Are you sure you want to delete this bulletin?');" href="/Apps/DeleteBulletin/<%=a.ID %>">Delete</a>
	                    </p>
                    
		                <%  string message = a.Message;
                            int maxLength = 500;
                    
                            if (message.Length > maxLength)
                                message = message.Substring(0, maxLength);

                            Response.Write(message); %>
	                </div>                     
	    <%      }
            } %>
        
        <%Html.EndForm(); %>
    </div>

  
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<link href="../../Content/jaxcore-button.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../Scripts/jaxscript.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-button.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-icon.js"></script>
</asp:Content>
