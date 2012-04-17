<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AdStorm
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%  List<CommandLibrary.App> unapproved = (List<CommandLibrary.App>)ViewData["Unapproved"]; %>

    <p>Ok, here are all the apps that currently need to be processed and approved by the different networks.</p>
    <p>Once they have been added to all of the networks, click Approve next to their name to notify the developer so they can start creating containers.</p>

    <div class="smallText row" style="float: left">
        <div class="cell header">App</div>
        <div class="cell header">Package</div>
    </div>

    <%  string alt = string.Empty;
        foreach (var item in unapproved)
        {   
            if (alt == string.Empty)
                alt = "alt";
            else
                alt = string.Empty;%>

            <div id="app<%=item.ID %>" class="row smallText <%=alt %>" style="clear:both; float: left;">
                <div class="cell"><%=item.Name %></div>
                <div class="cell"><%=item.PackageID %></div>
                <div style="float: left"><a href="javascript://" onclick="Approve('<%=item.ID %>', '<%=item.PackageID %>')">Approve</a></div>
            </div>
    <%  } %>

    <br style="clear: both" /><br />
    <p>Testing, clicking this button will reset all apps to unapproved.</p>
    <a href="/Admin/Reset" onclick="Reset()">Do it.</a>


    <script type="text/javascript">
        function Approve(id, package) {
            $.ajax({
                url: '/Admin/ApproveApp/' + package,
                success: function (data) {
                    $('#app' + id).fadeOut();
                }
            });
        }

    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
    .row { padding: 5px; }
    .cell { float: left; width: 400px; }
    .header { font-weight: bold }
    .alt { background-color: #f5f5f5; }
</style>

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
</asp:Content>
