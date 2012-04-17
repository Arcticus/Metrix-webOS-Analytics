<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%  string pid = (string)ViewData["App"]; %>
 <!-- amline script-->
   <script type="text/javascript" src="../../amcolumn/swfobject.js"></script>
   <script type="text/javascript" src="../../amstock/swfobject.js"></script>
   
   <div id="chartDownloads">
      <strong>You need to upgrade your Flash Player</strong>
   </div>
   
   <div id="chartActivity" style="float: left; margin-right: 20px;">
      <strong>You need to upgrade your Flash Player</strong>
   </div>
   <div id="chartDevice" style="float: left;">
      <strong>You need to upgrade your Flash Player</strong>
   </div>
   
   
   <div id="chartCarrier" style="float: left; margin-right: 20px;">
      <strong>You need to upgrade your Flash Player</strong>
   </div>
   <div id="chartResolution" style="float: left;">
      <strong>You need to upgrade your Flash Player</strong>
   </div>
   
      
   <div id="chartLocale" style="float: left;">
      <strong>You need to upgrade your Flash Player</strong>
   </div>
   
   
   <div id="chartAppVersion" style="float: left; margin-right: 20px;">
      <strong>You need to upgrade your Flash Player</strong>
   </div>
   <div id="chartWebOSBuildDist" style="float: left; ">
      <strong>You need to upgrade your Flash Player</strong>
   </div>
   
              
   
   <div id="chartWebOSVersion" style="float: left; margin-right: 20px;">
      <strong>You need to upgrade your Flash Player</strong>
   </div>
      
   <br style="clear: both" />
       
   <script type="text/javascript">

       // <![CDATA[
       var settings = "<settings><margins>12</margins><font>Georgia</font><add_time_stamp>1</add_time_stamp><data_reloading><reset_period>1</reset_period></data_reloading><number_format><thousand_separator>,</thousand_separator><decimal_separator>.</decimal_separator><letters/></number_format><data_sets><data_set><title>App Download Timeline</title><short>ADT</short><color>FFFFFF</color><file_name>../../../MetrixInterface.asmx/GetAppHeuristicData?packageId=<%=pid %></file_name><csv><reverse>1</reverse><separator>,</separator><date_format>MM/DD/YYYY</date_format><skip_first_rows>2</skip_first_rows><skip_last_rows>1</skip_last_rows><columns><column>date</column><column>close</column></columns></csv></data_set></data_sets><charts><chart><height>60</height><bg_alpha>100</bg_alpha><column_width>100</column_width><grid><x><color>FFFFFF</color></x><y_left><color>CCCCCC</color><alpha>50</alpha><dashed>1</dashed><fill_color>CCCCCC</fill_color><fill_alpha>5</fill_alpha></y_left><y_right><color>FFFFFF</color></y_right></grid><values><y_left><bg_color>000000</bg_color><unit_position>left</unit_position></y_left></values><legend><show_date>1</show_date></legend><comparing><recalculate_from_start>0</recalculate_from_start></comparing><events/><graphs><graph><color>FF7F2A</color><fill_alpha>10</fill_alpha><bullet>round_outline</bullet><data_sources><close>close</close></data_sources><legend><date title='0' key='0'><![CDATA[Downloads: <b>{close}</b>]]></date><period title='0' key='0'><![CDATA[Total:<b>{sum}</b> Daily Average:<b>{average}</b>]]></period></legend></graph></graphs></chart><chart><height>30</height><border_color>999999</border_color><border_alpha>0</border_alpha><column_width>100</column_width><grid><x><color>CCCCCC</color><alpha>50</alpha></x><y_left><color>999999</color><alpha>50</alpha><dashed>1</dashed><approx_count>3</approx_count><fill_color>CCCCCC</fill_color><fill_alpha>5</fill_alpha></y_left><y_right><color>999999</color><alpha>0</alpha></y_right></grid><values><x><enabled>0</enabled><text_color>000000</text_color></x><y_left><text_color>000000</text_color></y_left></values><legend><enabled>0</enabled></legend><comparing><recalculate_from_start>0</recalculate_from_start></comparing><events><color>000000</color></events><graphs><graph><type>step</type><connect>0</connect><color>FF7F2A</color><fill_alpha>20</fill_alpha><cursor_color>FF7F2A</cursor_color><bullet>round</bullet><bullet_size>0</bullet_size><bullet_alpha>86</bullet_alpha><bullet_color>000000</bullet_color><data_sources><close>close</close></data_sources><legend><date title='0' key='0'><![CDATA[Downloads: <b>{close}</b>]]></date><period title='0' key='0'><![CDATA[Total Downloads: <b>{sum}</b> Average Daily Downloads: <b>{average}</b>]]></period></legend></graph></graphs></chart></charts><data_set_selector><enabled>0</enabled><drop_down><scroller_color>C7C7C7</scroller_color></drop_down></data_set_selector><period_selector><periods_title>Zoom:</periods_title><custom_period_title>Custom period:</custom_period_title><button><bg_color_hover>EEEEEE</bg_color_hover><bg_color_selected>EEEEEE</bg_color_selected><border_color>FFFFFF</border_color><border_color_hover>FF7F2A</border_color_hover><border_color_selected>FF7F2A</border_color_selected></button><input><bg_color>EEEEEE</bg_color><border_color>FF7F2A</border_color></input><periods><period pid='0' type='DD' count='7'>1W</period><period pid='1' type='MM' count='1' selected='1'>1M</period><period pid='2' type='MM' count='3'>3M</period><period pid='3' type='YYYY' count='1'>1Y</period><period pid='4' type='YYYY' count='3'>3Y</period><period pid='5' type='YTD' count='0'>YTD</period><period pid='6' type='MAX' count='0'>MAX</period></periods></period_selector><header><enabled>0</enabled></header><balloon><border_color>B81D1B</border_color></balloon><background><alpha>100</alpha></background><plot_area><border_color>709FC5</border_color></plot_area><scroller><graph_data_source>close</graph_data_source><resize_button_style>dragger</resize_button_style><resize_button_color>FF7F2A</resize_button_color><connect>0</connect><graph_selected_color>FF7F2A</graph_selected_color><graph_selected_fill_color>FF7F2A</graph_selected_fill_color><graph_selected_alpha>75</graph_selected_alpha><bg_color>FFFFFF</bg_color><graph_alpha>100</graph_alpha><grid><color>999999</color><dashed>1</dashed></grid><playback><enabled>1</enabled><color>FF7F2A</color><color_hover>FF7F2A</color_hover><speed>3</speed><speed_indicator><color>FF7F2A</color><bg_color>FF9933</bg_color></speed_indicator></playback></scroller><context_menu><default_items><print>0</print></default_items></context_menu></settings>";
       var soTest = new SWFObject("../../amstock/amstock.swf", "amstock", "920", "400", "8", "#ffffff");
       soTest.addVariable("chart_id", "appChart");
       soTest.addVariable("path", "amstock/");
       soTest.addVariable("chart_settings", encodeURIComponent(settings));
       soTest.addVariable("preloader_color", "#FFFFFF");
       soTest.write("chartDownloads");
       
       var so = new SWFObject("../../amcolumn/amcolumn.swf", "amline", "445", "400", "8", "#FFFFFF");
       so.addVariable("path", "amcolumn/");
       so.addVariable("settings_file", escape("../../amcolumn/version_settings.xml"));  // you can set two or more different settings files here (separated by commas)
       so.addVariable("data_file", escape("../../MetrixInterface.asmx/GetVersionBreakDown?packageId=<%=pid %>"));
       so.addVariable("preloader_color", "#999999");
       so.write("chartAppVersion");

       var so7 = new SWFObject("../../amcolumn/amcolumn.swf", "amline", "910", "400", "8", "#FFFFFF");
       so7.addVariable("path", "amcolumn/");
       so7.addVariable("settings_file", escape("../../amcolumn/locale_settings.xml"));  // you can set two or more different settings files here (separated by commas)
       so7.addVariable("data_file", escape("../../MetrixInterface.asmx/GetLocaleBreakDown?packageId=<%=pid %>"));
       so7.addVariable("preloader_color", "#999999");
       so7.write("chartLocale");
       
       var so2 = new SWFObject("../../amcolumn/amcolumn.swf", "amline", "910", "400", "8", "#FFFFFF");
       so2.addVariable("path", "amcolumn/");
       so2.addVariable("settings_file", escape("../../amcolumn/webOSversion_settings.xml"));  // you can set two or more different settings files here (separated by commas)
       so2.addVariable("data_file", escape("../../MetrixInterface.asmx/GetWebOsVersionBreakDown?packageId=<%=pid %>"));
       so2.addVariable("preloader_color", "#999999");
       so2.write("chartWebOSVersion");

       var so3 = new SWFObject("../../amcolumn/amcolumn.swf", "amline", "445", "400", "8", "#FFFFFF");
       so3.addVariable("path", "amcolumn/");
       so3.addVariable("settings_file", escape("../../amcolumn/carrier_settings.xml"));  // you can set two or more different settings files here (separated by commas)
       so3.addVariable("data_file", escape("../../MetrixInterface.asmx/GetCarrierBreakDown?packageId=<%=pid %>"));
       so3.addVariable("preloader_color", "#999999");
       so3.write("chartCarrier");

       var so4 = new SWFObject("../../amcolumn/amcolumn.swf", "amline", "290", "400", "8", "#FFFFFF");
       so4.addVariable("path", "amcolumn/");
       so4.addVariable("settings_file", escape("../../amcolumn/deviceName_settings.xml"));  // you can set two or more different settings files here (separated by commas)
       so4.addVariable("data_file", escape("../../MetrixInterface.asmx/GetDeviceNameBreakDown?packageId=<%=pid %>"));
       so4.addVariable("preloader_color", "#999999");
       so4.write("chartDevice");

       var so5 = new SWFObject("../../amcolumn/amcolumn.swf", "amline", "445", "400", "8", "#FFFFFF");
       so5.addVariable("path", "amcolumn/");
       so5.addVariable("settings_file", escape("../../amcolumn/screenRes_settings.xml"));  // you can set two or more different settings files here (separated by commas)
       so5.addVariable("data_file", escape("../../MetrixInterface.asmx/GetScreenResolutionBreakDown?packageId=<%=pid %>"));
       so5.addVariable("preloader_color", "#999999");
       so5.write("chartResolution");

       var so6 = new SWFObject("../../amcolumn/amcolumn.swf", "amline", "445", "400", "8", "#FFFFFF");
       so6.addVariable("path", "amcolumn/");
       so6.addVariable("settings_file", escape("../../amcolumn/webOSbuild_settings.xml"));  // you can set two or more different settings files here (separated by commas)
       so6.addVariable("data_file", escape("../../MetrixInterface.asmx/GetWebOsBuildNumberBreakDown?packageId=<%=pid %>"));
       so6.addVariable("preloader_color", "#999999");
       so6.write("chartWebOSBuildDist");
       
       var soPie = new SWFObject("../../ampie/ampie.swf", "ampie", "600", "400", "8", "#FFFFFF");
       soPie.addVariable("path", "ampie/");
       soPie.addVariable("settings_file", escape("../../ampie/ampie_settings.xml"));  // you can set two or more different settings files here (separated by commas)
       soPie.addVariable("data_file", escape("../../MetrixInterface.asmx/GetCurrentActivityBreakDown?packageId=<%=pid %>"));
       soPie.addVariable("preloader_color", "#999999");
       soPie.write("chartActivity");
        
   </script>
<!-- end of amline script -->