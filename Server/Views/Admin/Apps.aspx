<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Apps
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%  MongoCommand.MetrixApp app = (MongoCommand.MetrixApp)ViewData["App"];
        List<MongoCommand.MApp> appList = (List<MongoCommand.MApp>)ViewData["AppList"];
        string pid = null;
        if (app != null)
            pid = app.PackageID;
        var ddlList = new SelectList(appList, "PackageID", "Name", pid);
        %>
         <div class="mediumText">
        <%Html.BeginForm(); %>
        <p>
            App: <%=Html.DropDownList("ddlApp", ddlList) %> 
            <button type="submit" name="btnEdit" class="smallButton">Go</button>
        </p>
        <%Html.EndForm(); %>
        </div>
        
       <%   if (app != null)
             Html.RenderPartial("Graphs", app); %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
