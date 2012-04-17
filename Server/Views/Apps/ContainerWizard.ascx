<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript" src="../../Scripts/jquery.simpletip-1.3.1.min.js" ></script>

<div style="min-height: 440px">
    <%  
        List<MongoCommand.AdNetwork> networks = (List<MongoCommand.AdNetwork>)ViewData["AdNetworks"];
        MongoCommand.AppApproval approval = (MongoCommand.AppApproval)ViewData["Approval"];
        List<MongoCommand.AdContainer> containers = (List<MongoCommand.AdContainer>)ViewData["Containers"];
        MongoCommand.AdContainer container = null;

        if (containers == null || containers.Count() == 0)
        {
            containers = new List<MongoCommand.AdContainer>();
            container = new MongoCommand.AdContainer();
        }
        else
            container = containers[0];

        int total = 0;

        ArrayList sizes = new ArrayList();
        sizes.Add("300 x 50");
        sizes.Add("300 x 250");

        var ddlSizes = new SelectList(sizes, container.ContainerSize);
    %>
    <%  Html.BeginForm("SaveContainer/" + approval.PackageID, "Apps", FormMethod.Post, new { name = "frmAction" }); %>
    
    <%=Html.Hidden("hdnOldName", container.ContainerName, new { id = "hdnOldName" })%>
    <div id="step1" style="position: absolute;">
        <p style="font-weight: bold">Name and Size</p>
        
        <p class="smallText">
            The name will be used in the code to identify the place to put the ads.<br />
            The size is the actual size of the ad. Keep this in mind when designing your app.
        </p>
        
        <div id="step1Error" class="errorBox" style="display: none; position: absolute; width: 300px; margin-left: 510px; margin-top: 12px; font-size: 12px;"></div>

        <div style="float: left; width: 180px">Container Name:</div>
        <%=Html.TextBox("txtName", container.ContainerName, new { @class = "bigTextbox", id = "txtName", onchange = "ValidateName(); CheckDup();" })%>
        <br style="clear: both" />
        <div style="float: left; width: 180px">Size (in pixels):</div>
        <%=Html.DropDownList("ddlSize", ddlSizes, new { @class="bigTextbox"}) %>
        
        <div style="clear: both; margin-top: 10px; margin-left: 360px; ">
            <a href="javascript://" onclick="Step2()" class="button button_small" style="font-family: Helvetica; font-size: 14pt; margin-right: 0px;" onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Next</a>
            <a href="/Apps/AdStorm/<%=approval.PackageID %>" class="button button_small" style="font-family: Helvetica; font-size: 14pt; margin-right: 0px;" onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Cancel</a>
        </div>
    </div>

     <div id="step2" style="position: absolute; display: none; width: 900px;">
        <p style="font-weight: bold">Networks</p>
        
        <p class="smallText">
            Enter the percentage you would like to show ads from a particular network. 
            The percentages should add up to 100%. If you put in 100% for one network, then
            only ads from that network will show up. If you put nothing in a network, we will
            assume you put 0% in there.
        </p>

        <div id="step2Error" class="errorBox" style="display: none; position: absolute; width: 280px; margin-left: 620px; margin-top: 12px; font-size: 12px;">test</div>
        
        <%  int count = 1;
            foreach (var item in networks)
            {
                MongoCommand.AdContainer c = containers.Where(i => i.AdNetworkID == item.Id).FirstOrDefault();
                if (c == null)
                    c = new MongoCommand.AdContainer();
                else
                    total += c.ShowPercentage; %>

                <div style="clear: both; float: left; margin-bottom: 10px;">
                    <div style="float: left" id="an<%=item.Id %>" >
                        <img style="float: left" src="<%=item.ImagePath %>" alt="" />
                        <div style="float: left; margin-top: -0px; margin-left: 10px;">
                            <%=Html.TextBox("txtPercent" + item.Id, c.ShowPercentage, new { id="txtPercent" + count, @class="bigTextbox", style="width: 50px", onkeyup="CalcTotal()"}) %>
                        </div>
                    </div>
                    <script type="text/javascript">
                        $("#an<%=item.Id %>").simpletip({
                            content: "<%=item.Description %>",
                            fixed: true,
                            position: [280, 100]
                        });
                    </script>
                </div>
        <%      count++;
            } %>

        <div style="float: left; margin-left: 140px; clear: both; font-size: 18pt; color: #000;">Total: <span style="margin-left: 10px" id="total"><%=total %></span></div>
        <div style="float: left; margin-left: 235px;">
            <a href="javascript://" onclick="BackStep1()" class="button button_small" style="font-family: Helvetica; font-size: 14pt;" onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Back</a> 
            <a href="javascript://" onclick="Save()" class="button button_small" style="font-family: Helvetica; font-size: 14pt;" onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Save</a>
        </div>

        <br style="clear: both" />
    </div>
   
    <%  Html.EndForm(); %>
</div>
<script type="text/javascript">
    function ValidateName() {
        var name = $('#txtName').val();
        if (name == "") {
            $('#step1Error').html("Please enter a container name");
            $('#step1Error').fadeIn();
            setTimeout("$('#step1Error').fadeOut();", 5000);
            return false;
        }

        var exp = "^[^<>`~!/@\#} $%:;)(_^{&*=|'+]+$";
        if (!name.match(exp)) {
            $('#step1Error').html("Names cannot contain spaces or any special characters.");
            $('#step1Error').fadeIn();
            setTimeout("$('#step1Error').fadeOut();", 5000);
            return false;
        }

        return true;
    }

    function CheckDup() {
        var name = $('#txtName').val();
        var old = $('#hdnOldName').val();

        $.ajax({
            url: '/Apps/CheckName?old=' + old + '&name=' + name,
            success: function (data) {
                if (data == "true") {
                    $('#step1Error').html("You already have a container with that name");
                    $('#step1Error').fadeIn();
                    setTimeout("$('#step1Error').fadeOut();", 5000);
                    return false;
                }

                return true;
            }
        });
    }

    function CalcTotal() {
        var p1 = $('#txtPercent1').val();
        var p2 = $('#txtPercent2').val();
        var p3 = $('#txtPercent3').val();
        var p4 = $('#txtPercent4').val();
        var p5 = $('#txtPercent5').val();

        if (p1 == "")
            p1 = 0;

        if (p2 == "")
            p2 = 0;

        if (p3 == "")
            p3 = 0;

        if (p4 == "")
            p4 = 0;

        if (p5 == "")
            p5 = 0;

        if (isNaN(p1) || isNaN(p2) || isNaN(p3) || isNaN(p4) || isNaN(p5)) {
            $('#step2Error').html("Please enter a valid percentage (numbers only)");
            $('#step2Error').fadeIn();
            setTimeout("$('#step2Error').fadeOut();", 3000);

            $('#total').html("0");
        }

        $('#total').html(parseInt(p1) + parseInt(p2) + parseInt(p3) + parseInt(p4) + parseInt(p5));
    }

    function Save() {
        var total = $('#total').html();

        if (total != "100") {
            $('#step2Error').html("Total must equal 100%");
            $('#step2Error').fadeIn();
            setTimeout("$('#step2Error').fadeOut();", 3000);
            return;
        }
        
        document.frmAction.submit();
    }

    function BackStep1() {
        $('#step2').fadeOut();
        $('#step1').fadeIn('slow');
    }

    function Step2() {
        if (!ValidateName())
            return;

        var name = $('#txtName').val();
        var old = $('#hdnOldName').val();

        $.ajax({
            url: '/Apps/CheckName?old=' + old + '&name=' + name,
            success: function (data) {
                if (data == "true") {
                    $('#step1Error').html("You already have a container with that name");
                    $('#step1Error').fadeIn();
                    setTimeout("$('#step1Error').fadeOut();", 5000);
                    return;
                }


                $('#step1').fadeOut();
                $('#step2').fadeIn('slow');
            }
        });
    }
</script>
