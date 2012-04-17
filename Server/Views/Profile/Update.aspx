<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%  CommandLibrary.User u = (CommandLibrary.User)ViewData["User"]; %>
    
    <img src="../../Images/headerUpdateProfile.png" alt="Update Profile" /><br />     
    
    <div class="bigText">
        <%Html.BeginForm("Update", "Profile", FormMethod.Post, new { name = "frmAction" }); %>
        <p><div style="float: left; width: 180px">User Name:</div> <%=u.Login %></p>
        <p><div style="float: left; width: 180px">First Name:</div> <%=Html.TextBox("txtName", u.FirstName, new { @class="bigTextbox"}) %></p>
        <p><div style="float: left; width: 180px">Last Name:</div> <%=Html.TextBox("txtLast", u.LastName, new { @class="bigTextbox"}) %></p>
        <p><div style="float: left; width: 180px">Company:</div> <%=Html.TextBox("txtCompany", u.CompanyName, new { @class="bigTextbox"}) %></p>
        <p><div style="float: left; width: 180px">Email:</div> <%=Html.TextBox("txtEmail", u.Email, new { @class="bigTextbox"}) %></p>
        <p style="font-size: 12pt">If you want to change your password, enter the new one twice.</p>
        <p><div style="float: left; width: 180px">Password:</div> <%=Html.Password("txtPassword", null, new { @class="bigTextbox"}) %></p>
        <p><div style="float: left; width: 180px">Confirm:</div> <%=Html.Password("txtConfirm", null, new { @class = "bigTextbox" })%></p>
        
        <%Html.EndForm(); %>
        <a href="javascript://" onclick="document.frmAction.submit();" class="button button_large" style="margin-left: 330px; font-family:Helvetica; font-size:18pt; margin-right: 20px; " onmouseover="Button.rollover(this,'#ddd','');" onmouseout="Button.rollout(this);">Change Me!</a>
        <div style="position: absolute; font-size: 14pt; color:Red; top: 180px; margin-left: 610px; width: 300px;">
            <%=ViewData["Status"] %>
        </div>
        
        
        
        <!-- Begin MailChimp Signup Form -->
<!--[if IE]>
<style type="text/css" media="screen">
	#mc_embed_signup fieldset {position: relative;}
	#mc_embed_signup legend {position: absolute; top: -1em; left: .2em;}
</style>
<![endif]--> 
<!--[if IE 7]>
<style type="text/css" media="screen">
	.mc-field-group {overflow:visible;}
</style>
<![endif]-->
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.2.6/jquery.min.js"></script>
<script type="text/javascript" src="http://downloads.mailchimp.com/js/jquery.validate.js"></script>
<script type="text/javascript" src="http://downloads.mailchimp.com/js/jquery.form.js"></script>

<div id="mc_embed_signup">
<form action="http://webosroundup.us1.list-manage.com/subscribe/post?u=907be7cc03f815a681aa61e86&amp;id=854cba84af" method="post" id="mc-embedded-subscribe-form" name="mc-embedded-subscribe-form" class="validate" target="_blank" style="font: normal 100% Arial;font-size: 12px;">
	<fieldset style="-moz-border-radius: 4px;border-radius: 4px;-webkit-border-radius: 4px;border: 1px solid #000000;padding-top: 1.5em;margin: .5em 0;background-color: #ffffff;color: #333333;text-align: left;">
	<legend style="text-transform: capitalize;font-weight: bold;color: #ffffff;background: #000000;padding: .5em 1em;border: 1px solid #000000;-moz-border-radius: 4px;border-radius: 4px;-webkit-border-radius: 4px;font-size: 1.2em;">Subscribe to the Metrix Newsletter</legend>
<div class="indicate-required" style="text-align: right;font-style: italic;overflow: hidden;color: #333333;margin: 0 9% 0 0;">* indicates required</div>
<div class="mc-field-group" style="margin: 1.3em 5%;clear: both;overflow: hidden;">
<label for="mce-EMAIL" style="display: block;margin: .3em 0;line-height: 1em;font-weight: bold;">Email Address <strong class="note-required">*</strong>
</label>
<input type="text" value="" name="EMAIL" class="required email" id="mce-EMAIL" style="margin-right: 1.5em;padding: .2em .3em;width: 90%;float: left;z-index: 999;">
</div>
		<div id="mce-responses" style="float: left;top: -1.4em;padding: 0em .5em 0em .5em;overflow: hidden;width: 90%;margin: 0 5%;clear: both;">
			<div class="response" id="mce-error-response" style="display: none;margin: 1em 0;padding: 1em .5em .5em 0;font-weight: bold;float: left;top: -1.5em;z-index: 1;width: 80%;background: #FFEEEE;color: #FF0000;"></div>
			<div class="response" id="mce-success-response" style="display: none;margin: 1em 0;padding: 1em .5em .5em 0;font-weight: bold;float: left;top: -1.5em;z-index: 1;width: 80%;background: #;color: #529214;"></div>
		</div>
		<div><input type="submit" value="Subscribe" name="subscribe" id="mc-embedded-subscribe" class="btn" style="clear: both;width: auto;display: block;margin: 1em 0 1em 5%;"></div>
	</fieldset>	
	<a href="#" id="mc_embed_close" class="mc_embed_close" style="display: none;">Close</a>
</form>
</div>
<script type="text/javascript">
    var fnames = new Array(); var ftypes = new Array(); fnames[0] = 'EMAIL'; ftypes[0] = 'email'; fnames[1] = 'FNAME'; ftypes[1] = 'text'; fnames[2] = 'LNAME'; ftypes[2] = 'text'; var err_style = '';
    try {
        err_style = mc_custom_error_style;
    } catch (e) {
        err_style = 'margin: 1em 0 0 0; padding: 1em 0.5em 0.5em 0.5em; background: FFEEEE none repeat scroll 0% 0%; font-weight: bold; float: left; z-index: 1; width: 80%; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial; color: FF0000;';
    }
    var mce_jQuery = jQuery.noConflict();
    mce_jQuery(document).ready(function($) {
        var options = { errorClass: 'mce_inline_error', errorElement: 'div', errorStyle: err_style, onkeyup: function() { }, onfocusout: function() { }, onblur: function() { } };
        var mce_validator = mce_jQuery("#mc-embedded-subscribe-form").validate(options);
        options = { url: 'http://webosroundup.us1.list-manage.com/subscribe/post-json?u=907be7cc03f815a681aa61e86&id=854cba84af&c=?', type: 'GET', dataType: 'json', contentType: "application/json; charset=utf-8",
            beforeSubmit: function() {
                mce_jQuery('#mce_tmp_error_msg').remove();
                mce_jQuery('.datefield', '#mc_embed_signup').each(
                        function() {
                            var txt = 'filled';
                            var fields = new Array();
                            var i = 0;
                            mce_jQuery(':text', this).each(
                                function() {
                                    fields[i] = this;
                                    i++;
                                });
                            mce_jQuery(':hidden', this).each(
                                function() {
                                    if (fields[0].value == 'MM' && fields[1].value == 'DD' && fields[2].value == 'YYYY') {
                                        this.value = '';
                                    } else if (fields[0].value == '' && fields[1].value == '' && fields[2].value == '') {
                                        this.value = '';
                                    } else {
                                        this.value = fields[0].value + '/' + fields[1].value + '/' + fields[2].value;
                                    }
                                });
                        });
                return mce_validator.form();
            },
            success: mce_success_cb
        };
        mce_jQuery('#mc-embedded-subscribe-form').ajaxForm(options);

    });
    function mce_success_cb(resp) {
        mce_jQuery('#mce-success-response').hide();
        mce_jQuery('#mce-error-response').hide();
        if (resp.result == "success") {
            mce_jQuery('#mce-' + resp.result + '-response').show();
            mce_jQuery('#mce-' + resp.result + '-response').html(resp.msg);
            mce_jQuery('#mc-embedded-subscribe-form').each(function() {
                this.reset();
            });
        } else {
            var index = -1;
            var msg;
            try {
                var parts = resp.msg.split(' - ', 2);
                if (parts[1] == undefined) {
                    msg = resp.msg;
                } else {
                    i = parseInt(parts[0]);
                    if (i.toString() == parts[0]) {
                        index = parts[0];
                        msg = parts[1];
                    } else {
                        index = -1;
                        msg = resp.msg;
                    }
                }
            } catch (e) {
                index = -1;
                msg = resp.msg;
            }
            try {
                if (index == -1) {
                    mce_jQuery('#mce-' + resp.result + '-response').show();
                    mce_jQuery('#mce-' + resp.result + '-response').html(msg);
                } else {
                    err_id = 'mce_tmp_error_msg';
                    html = '<div id="' + err_id + '" style="' + err_style + '"> ' + msg + '</div>';

                    var input_id = '#mc_embed_signup';
                    var f = mce_jQuery(input_id);
                    if (ftypes[index] == 'address') {
                        input_id = '#mce-' + fnames[index] + '-addr1';
                        f = mce_jQuery(input_id).parent().parent().get(0);
                    } else if (ftypes[index] == 'date') {
                        input_id = '#mce-' + fnames[index] + '-month';
                        f = mce_jQuery(input_id).parent().parent().get(0);
                    } else {
                        input_id = '#mce-' + fnames[index];
                        f = mce_jQuery().parent(input_id).get(0);
                    }
                    if (f) {
                        mce_jQuery(f).append(html);
                        mce_jQuery(input_id).focus();
                    } else {
                        mce_jQuery('#mce-' + resp.result + '-response').show();
                        mce_jQuery('#mce-' + resp.result + '-response').html(msg);
                    }
                }
            } catch (e) {
                mce_jQuery('#mce-' + resp.result + '-response').show();
                mce_jQuery('#mce-' + resp.result + '-response').html(msg);
            }
        }
    }
</script>
<!--End mc_embed_signup-->
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<link href="../../Content/jaxcore-button.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../Scripts/jaxscript.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-button.js"></script>
<script type="text/javascript" src="../../Scripts/jaxcore-icon.js"></script>
</asp:Content>
