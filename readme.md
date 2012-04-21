Metrix - A Analytics Suite for HP webOS
=======================================

Metrix was/is an analytics suite for the HP webOS mobile platform.  It was originally created for personal use since HP (Palm at that time) did not offer real-time or even daily download stats.
After showing a few fellow developers, it was realized that this tool could be utilized by all.  The folks over at [webOSroundup.com (WOR)](http://www.webosroundup.com)
offered to assist in developing and maintaining the infrastructure.  Through the course of time some really neat tools were intertwined with Metrix such as the App Wall, App Spotlight, etc and thus became very intertwined with the WOR website.  We even had
some pretty big amibtions on what we could grow Metrix into.  Sadly through a series of unfortunate events, the current version of webOS as we know and love it has been put to rest and we can no longer justify and afford to host the tool used by so many (averaged about 6 million hits a month).  We understand that 
many developers are still actively supporting the legacy version of webOS ([Syntactix](http://www.gosyntactix.com) included) and would like to still use the tool. So here it is, well sort of :/.
See since the tool became so intertwined with the WOR website it has made it difficult to isolate into an easy to deploy package.  We have done are best and will continue to try and make it as easy as possible
for you to deploy on your own server, but fair warning _things may not work right out of the gate_.

The server was built using .NET MVC and thus needs to be hosted on a Windows server.  The graphs are done using the free legacy flash version of [AM Charts](http://www.amcharts.com).  The SQL data tables are outlined in an image in the root directory called "SQL Tables.jpg".
At this time the LINQ dbml is compiled as a binary DLL and therefore is not extensible at this time.

**ADDITIONAL INFO AND UPDATED README FORTHCOMING...**

Copyright (c) 2010 Syntactix, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.