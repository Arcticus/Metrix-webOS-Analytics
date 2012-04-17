<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Metrix - Features
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <img src="../../Images/headerFeatures.png" alt="Features" /><br />           
     <p class="smallText">
        Metrix is really packed with features, and we really are just getting started. Take a gander
        at what is available now, and what is coming just around the corner.
     </p>
     <div id="content">
        <div id="slider"> 
			<ul>				
				<li>
				    <div class="sliderText">
				        <img src="../../Images/iconAnalytics32.png" /> 
				        <img src="../../Images/headerAnalytics.png" alt="Analytics" /><br />
				        <div style="float:left; width: 490px;">
				            <p class="smallText" style="margin-top: 10px">
				                Let's say you have built the best app in the world. You know that people are
				                downloading it, but once the user has it, you have no idea how they are using it
				                or anything about the people who buy your app.
				            </p>
				            <p class="smallText">
                                Imagine being able to answer the following questions:
                            </p>    
                            <div class="bulletList"><img src="../../Images/bullet.png" /> How many people have downloaded my app?</div>
                            <div class="bulletList"><img src="../../Images/bullet.png" /> When was the last time people actually used my app?</div>
                            <div class="bulletList"><img src="../../Images/bullet.png" /> What version of my app are users running?</div>
                            <div class="bulletList"><img src="../../Images/bullet.png" /> Which mobile Palm device do my users have?</div>
                            
                            <p class="smallText"  style="margin-top: 10px">
                                Right "out of the box" Metrix lets you answer these, plus many more. In future versions you will be
                                able to create your own questions to track. For instance, if you have a game you will be able to 
                                track how many times people got to level 5, or 50. You will be able to see how many tasks were
                                created in your ToDo app. The list is really endless and it all begins when you 
                                <a href="/Signup">signup for Metrix</a>.
                            </p>
                        </div>
                        <div class="analyticGraphs">
                            <a class="zoom" href="#graph1">
                                <img src="../../Images/AnalyticsGraph1Small.png" alt="Total Downloads" title="Total Downloads" />
                            </a>
                            <a class="zoom" href="#graph2">
                                <img src="../../Images/AnalyticsGraph2Small.png" alt="User Activity" title="User Activity" />
                            </a><br />
                            <a class="zoom" href="#graph3">
                                <img src="../../Images/AnalyticsGraph3Small.png" alt="Carrier Distribution" title="Carrier Distribution" />
                            </a>
                            <a class="zoom" href="#graph4">
                                <img src="../../Images/AnalyticsGraph4Small.png" alt="Locale Distribution" title="Locale Distribution" />
                            </a>
                        </div>
                        
                        <div id="graph1"> 
				            <img src="../../Images/AnalyticsGraph1.png" alt="Total Downloads" title="Total Downloads" />
			            </div> 
			            <div id="graph2">
			                 <img src="../../Images/AnalyticsGraph2.png" alt="User Activity" title="User Activity" />
			            </div>
			            <div id="graph3">
			                 <img src="../../Images/AnalyticsGraph3.png" alt="Carrier Distribution" title="Carrier Distribution" />
			            </div>
			            <div id="graph4">
			                <img src="../../Images/AnalyticsGraph4Small.png" alt="Locale Distribution" title="Locale Distribution" />
			            </div>
			        </div>
				</li> 
				<li>
				    <div class="sliderText">
				        <img src="../../Images/iconBulletins32.png" /> 
				        <img src="../../Images/headerBulletins.png" alt="Bulletin" /><br />
    				    
				        <div style="float:left; width: 490px;">
				            <p class="smallText" style="margin-top: 10px">
				                So, you have a new app coming out. You want to get the word out, but you aren't
				                sure exactly how...
				            </p>
				            <p class="smallText" >
				                Wouldn't it be great if you could tap into the 10,000 people
				                who downloaded your first app? Let them all know that you have another amazing
				                app coming down the pipe?
				            </p>
				            <p class="smallText" >
				                Now, wouldn't it be even better if you could let all 10,000 of them know
				                without having to issue an update that they may never download?
				            </p>
				            <p class="smallText" >
				                With Metrix you can create full HTML bulletins (using a rich text editor) that
				                will be displayed to all of your users next time they launch your app. The
				                bulleting is unobtrusive and is designed to not annoy your users. It is simply
				                the best way to keep all of your users up to date with what is happening.
				            </p>
				        </div>
				        <img src="../../Images/bulletinScreenshot.jpg" style="float:right" alt="" />
				    </div>
				</li> 
				<li>
				    <div class="sliderText">
				        <img src="../../Images/iconRemoteDebugging32.png" /> &nbsp;
				        <img src="../../Images/headerRemoteDebugging.png" alt="Remote Debugging" /> 
				        <span class="mediumText" style="margin-left: 20px; color: #999">(coming soon)</span><br />
				        <p class="smallText" style="margin-top: 10px">
			                Remote debugging is one of those tools that you don't need, until you <em>need it</em>.
			                Once you understand the usefulness, it is really hard to live without.
			            </p>
			            <p class="smallText" >
			                All developers, at one time or another, have ran into a bug that you just can't seem to work out.
			                Unfortunately, when it comes to customers the age old "well it works on my machine" just won't cut it.
			                They need a fix...and they need it now.
			            </p>
				        <p class="smallText" >
				            With Metrix, you are able to sync up directly to a particular user (with their permission) and
				            <strong>see exactly what is on their screen</strong>. 
				            This allows you to address any and every bug, glitch or anomaly that your users encounter, 
				            no matter how obscure or rare.
				        </p>
				    </div>
				</li> 
				<li>
				     <div class="sliderText">
				         <img src="../../Images/iconMore32.png" /> &nbsp;
				        <img src="../../Images/headerMore.png" alt="More" /> 
				        <p class="smallText" style="margin-top: 10px">
			                This really is just the beginning. Our goal with Metrix is to create an ever evolving 
			                suite of tools that help developers maximize the potential of their apps. 
			                We have a lot in the works. Some are mentioned here; others will be announced when they
			                are closer to completion.
			            </p>
			            <p class="smallText" >
			                 Please feel free to
			                <a href= "/Signup">signup and download Metrix completely free</a> and see what you think.
			            </p>
			            <p class="smallText" >
			                If you have any questions, please don't hesitate to <a href="/Contact">contact us</a>, 
			                or you can also check out our 
			                <a href="http://forums.webosroundup.com/forumdisplay.php?23-Metrix">Metrix Forums</a> as well.
			            </p>
				     </div>
				</li> 
			</ul> 
		</div> 
    </div>
    <script type="text/javascript">
        $(document).ready(function() {
            $('a.zoom').fancyZoom({
                scaleImg: true,
                closeOnClick: true,
                directory: "../../Images/fancy"
            });
        
            $("#slider").easySlider({
                auto: false,
                continuous: false
            });
        });	
	</script> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>  
<script type="text/javascript" src="../../Scripts/easySlider1.7.js"></script>  
<script type="text/javascript" src="../../Scripts/fancyzoom.min.js"></script>

<link href="../../Content/screen.css" rel="stylesheet" type="text/css" media="screen" /> 
<link rel="stylesheet" href="../../Content/common.css" type="text/css" /> 
</asp:Content>
