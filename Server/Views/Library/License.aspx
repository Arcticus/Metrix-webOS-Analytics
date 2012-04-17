<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Download
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="LeftContent">
        <p class="bigText">
            Software License...please sign right here.
        </p>
        <p class="mediumText">
            I know what you are thinkin'...Didn't I just do this? What the heck do you want now? And yes, you did just sign something. That was the NDA. This
            right here is the license for the software.
        </p>
        <p class="smallText">
            What the small forest of legalese below you is saying that Syntactix owns the software you are about to download and while you are welcome to
            use it, you can't give it away to someone else, or tinker with it. Furthermore, the blob says that Syntactix isn't responsible if your phone blows up 
            (don't worry, we have been testing it for a while and so far so good). That's about it...Cool?
        </p>
        <p class="smallText">
            Feel free to read it all, and when you are ready to move on, just click I agree and we will ship you the zip file with all the goodies. Enjoy!
        </p>
        <br /><br />
        <p>This license governs use of the accompanying software ("Software"), and your use of the Software constitutes acceptance of this license.</p>
        <p>Subject to the restrictions below, you may use the Software for any commercial or noncommercial purpose, including distributing derivative works.</p>
        <p><strong>SECTION 1: DEFINITIONS</strong></p>
        <p>A. "Syntactix LLC" refers to Syntactix, LLC, a limited liability corporation organized and operating under the laws of the state of Florida.</p>
        <p>B. "Metrix" or "Metrix Library" refers to the Metrix Library WebOS Framework, which is a Syntactix LLC software product.</p>
        <p>C. "SOFTWARE" refers to the source code, compiled binaries, installation files documentation and any other materials provided by Syntactix LLC.</p>
        <p><strong>SECTION 2: LICENSE</strong></p>
        <p>You agree that:</p>
        <p>A. Subject to the terms of this license, the Licensor grants you a non-transferable, non-exclusive, worldwide, royalty-free copyright license to reproduce and redistribute unmodified the SOFTWARE for use within your Palm WebOS application provided that the following conditions
are met:<br /><br />
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(i)   All copyright notices are retained.<br />
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(ii)  A copy of this license is retained in the header of each source file of the software.</p>
        <p>B. You may NOT decompile, disassemble, reverse engineer or otherwise attempt to extract, generate or retrieve source code from any compiled binary provided in the SOFTWARE.</p>
        <p>C. You will (a) NOT use Syntactix's name, logo, or trademarks in association with distribution of the SOFTWARE or derivative works unless otherwise permitted in writing; and (b) you WILL indemnify, hold harmless, and defend Syntactix from and against any claims or lawsuits, including attorneys fees, that arise or result from the use or distribution of your modifications to the SOFTWARE and any additional software you distribute along with the SOFTWARE.</p>
        <p>D. The SOFTWARE comes "as is", with no warranties. None whatsoever. This means no express, implied or statutory warranty, including without limitation, warranties of merchantability or fitness for a particular purpose or any warranty of title or non-infringement.</p>
        <p>E. Neither Syntactix LLC nor its suppliers will be liable for any of those types of damages known as indirect, special, consequential, or incidental related to the SOFTWARE or this license, to the maximum extent the law permits, no matter what legal theory its based on. Also, you must pass this limitation of liability on whenever you distribute the SOFTWARE or derivative works.</p>
        <p>F. If you sue anyone over patents that you think may apply to the SOFTWARE for a person's use of the SOFTWARE, your license to the SOFTWARE ends automatically.</p>
        <p>G. The patent rights, if any, granted in this license only apply to the SOFTWARE, not to any derivative works you make.</p>
        <p>H. The SOFTWARE is subject to U.S. export jurisdiction at the time it is licensed to you, and it may be subject to additional export or import laws in other places.  You agree to comply with all such laws and regulations that may apply to the SOFTWARE after delivery of the SOFTWARE to you.</p>
        <p>I. If you are an agency of the U.S. Government, (i) the SOFTWARE is provided pursuant to a solicitation issued on or after December 1, 1995, is provided with the commercial license rights set forth in this license, and (ii) the SOFTWARE is provided pursuant to a solicitation issued prior to December 1, 1995, is provided with Restricted Rights as set forth in FAR, 48 C.F.R. 52.227-14 (June 1987) or DFAR, 48 C.F.R. 252.227-7013 (Oct 1988), as applicable.</p>
        <p>J. Your rights under this license end automatically if you breach it in any way.</p>
        <p>K. This license contains the only rights associated with the SOFTWARE and Syntactix LLC reserves all rights not expressly granted to you in this license.</p>
        <p>© 2010 Syntactix, LLC. All rights reserved.</p>
        <%Html.BeginForm("License", "Library", FormMethod.Post, new { style = "float:right" }); %>
        <%=Html.Hidden("hdnHiThere") %>
        <button class="bigButton" type="submit">I Agree!!</button>
        <%Html.EndForm(); %>
        <p class="mediumText" style="color:red"><%=ViewData["Status"] %>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
