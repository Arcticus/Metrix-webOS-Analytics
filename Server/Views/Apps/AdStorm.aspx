<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register src="ContainerWizard.ascx" tagname="ContainerWizard" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Metrix - AdStorm
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%  MongoCommand.AppApproval approval = (MongoCommand.AppApproval)ViewData["Approval"]; %>

    <img src="../../Images/headerAdStorm.png" alt="AdStorm" /><br />  

    <%  if (approval == null)
        { %>
            <p>Hey! So I see here that you haven't signed up for AdStorm for this app. No problem, but we have to get all the paperwork signed, crossed, and dotted.</p>
            <p>
                Here is how it works. For each app you want to use with AdStorm you need to get approved. This process take a bit of time because we have to get everything
                setup with the various networks. Usually it takes between 1 and 3 business days. Once you are approved though, you will be off and running.
            </p>
            <p>
                So what do you have to do to get approved? Well first there is some legal mumbo jumbo that you have to agree to. Essentially it says that your app is not malicious,
                doesn't break any laws, and is not offensive in some way. If your app is good on those fronts odds are pretty good you will be approved. Once you agree to the terms
                then we will start the process of getting you on all the networks. We will shoot you an email to let you know when your apps are ready.
            </p>

            <p class="smallText">
                TOS goes here...
            </p>
            <%Html.BeginForm("AdStorm", "Apps", FormMethod.Post, new { name = "frmAction" }); %>   
            <%=Html.Hidden("hdnAgree") %>
            <%Html.EndForm(); %>
            <a href="javascript://" onclick="document.frmAction.submit();" class="button button_large" style="float:right; margin-top: 5px;font-family:Helvetica; font-size:18pt; margin-right: 20px; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">I agree!!</a>
    <%  }
        else if (!approval.IsApproved)
        { %>
            <p>
                Ok, so your app is currently in the approval process. This typically takes 1 - 3 business days, but once you are approved you are good to go. We will email you at 
                the address we have on file for you as soon as you are ready to go.
            </p>
            <p>Thanks for using AdStorm!</p>
    <%  } 
        else
        {
            List<MongoCommand.AdContainer> containers = (List<MongoCommand.AdContainer>)ViewData["Containers"]; %>

            <p>Below are your containers. You can change any of the properties including the network mix, or you can remove it permanently.</p>
            <%  string alt = string.Empty;
                foreach (var item in containers)
                {   
                    if (alt == string.Empty)
                        alt = "alt";
                    else
                        alt = string.Empty;%>

                    <div id="row<%=item.Id %>" class="row smallText <%=alt %>" style="clear:both; float: left;">
                        <div style="float: left; width: 400px"><%=item.ContainerName %></div>
                        <a style="margin-right: 15px;" href="/Apps/EditContainer/<%=approval.PackageID %>?container=<%=item.ContainerName%>">Edit</a>  
                        <a href="javascript://" onclick="DeleteContainer('<%=item.Id %>', '<%=item.ContainerName%>')" style="color: red">Delete</a>  
                    </div>
            <%  }

            if (containers == null || containers.Count() == 0)
            {   %>
                <p>Oh look! It appears you don't have any containers. How sad.</p>
                <p>Well it's hard to make any moolah without them so why not click the link below to create your first one?</p>
        <%  }   %>
            
            <br style="clear: both" />
            <p style="margin-top: 30px; margin-bottom: 15px;"><a href="/Apps/AddContainer/<%=approval.PackageID %>">Add a new container!</a></p>
            <p class="smallText">
                Once you have your containers created, be sure top check the Metrix documentation to see how top get it up and running in your app.
            </p>
            <p class="smallText">
                As always, if you have any questions you can visit our <a href="http://forums.webosroundup.com/categories/metrix">support forums</a> or <a href="/Contact">send us an email</a>.
            </p>
    <%  } %>

   <span class="error"><%=ViewData["Status"] %></span>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.2.6/jquery.min.js"></script>
<script type="text/javascript" src="../../Scripts/jaxscript.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-button.js"></script>

<style type="text/css">
    .row { padding: 5px; }
    .cell { float: left; width: 400px; }
    .alt { background-color: #f5f5f5; }
</style>

<script type="text/javascript">
    function DeleteContainer(id, name) {
        if (!confirm("Are you sure you want to delete the container '" + name + "'?"))
            return;

        $.ajax({
            url: '/Apps/DeleteContainer?container=' + name,
            success: function (data) {
                $('#row' + id).fadeOut();
            }
        });
    }

</script>
</asp:Content>
