/*
// JaxScript 0.1
// JavaScript API for XML Web Services
// http://www.jaxscript.com
//
// Copyright (c) 2010, Dan Steinman <dan@jaxcore.com>
// All rights reserved.

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:

	* Redistributions of source code must retain the above copyright notice, 
	  this list of conditions and the following disclaimer.
	* Redistributions in binary form must reproduce the above copyright notice, 
	  this list of conditions and the following disclaimer in the documentation 
	  and/or other materials provided with the distribution.
	* Neither the name of "JaxCore", nor "Dan Steinman" may be used to endorse 
	  or promote products derived from this software without specific prior 
	  written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
POSSIBILITY OF SUCH DAMAGE.
*/

function echo(o) {
var s = inspect(o);
var c = window.console;
if (c && c.log) c.log(s);
}
function hasProperty(o,p) {
if (isString(p)) {
if (p.indexOf('.')>0) {
var s = p.split('.');
for (var i=0;i<s.length;i++) {
o = o[s[i]];
if (!o) return false;
}
return true;
}
else return typeof o[p]!='undefined';
}
}
function exists(o,p) {
if (!!o && !!p) {
if (arguments.length>2) return existsArray(o,arguments,1);
if (isString(p)) return hasProperty(o,p);
if (isArray(p)) return existsArray(o,p);
}
return false;
}
function existsArray(o,a,start) {
if (!a.length) return false;
for (var i=start||0;i<a.length;i++)
if (!exists(o,a[i])) return false;
return true;
}
var type = {};
function enumerate(s) {
var o = {};
if (isString(s)) s = s.split(",");
if (isArray(s)) {
for (var i in s)
o[s[i]] = parseInt(i)+1;
return o;
}
}
function getFields(o) {
var a = [];
for (var i in o) {
a[a.length] = i;
}
return a;
}
function enumField(enm,field) {  
var i, c = 0;
for (i in enm) {
c = Math.max(c,enm[i]);
}
enm[field] = ++c;
return c;
}
function isType(t,o) {
var f = self['is'+t];
if (isFunction(f)) return f(o);
return false;
}
function areType(t,a) {
var f, i;
if (!!t && isArray(a)) {
for (i=0;i<a.length;i++) {
f = self['is'+t];
if (isFunction(f)) {
if (!f(a[i])) return false;
}
}
return true;
}
return false;
}
function defineType(t, desc, fn) {
if (typeof self['is'+t]=='undefined') {
var v = enumField(type,t);
self['is'+t] = fn;
}
else echo('addType() error: '+name+' is an invalid type');
}
defineType('Null','empty or undefined',function(o) {
return o==undefined || typeof o=='undefined' || o==null || o==='';
});
defineType('True','true',function(o) {
return o===true;
});
defineType('False','false',function(o) {
return o===false;
});
defineType('Boolean','true or false',function(o) {
return isTrue(o) || isFalse(o);
});
defineType('Integer','integer number',function(o) {
return parseInt(o)==o;
});
defineType('Float','integer or floating point number',function(o) {
return parseFloat(o)==Number(o);
});
defineType('String','character text',function(o) {
return typeof o==='string';
});
defineType('Object','JavaScript object of any type', function(o) {
return typeof o==='object';
});
defineType('Array','JavaScript Array object',function(o) {
return Object.prototype.toString.apply(o) === '[object Array]';
});
defineType('Function','JavaScript function or class',function(o) {
return typeof o=='function';
});
defineType('Literal','JavaScript object containing no functions or objects',function(o) {
if (isObject(o)) {
var c = 0, i, v;
for (i in o.prototype) c++;
if (c==0) {
for (i in o) {
v = o[i];
if (isObject(v) || isFunction(v)) return false;
}
return true;
}
}
return false;
});
defineType('Enum','JavaScript literal containing only integers',function(o) {
if (isLiteral(o)) {
for (var i in o) {
if (!isInteger(o[i])) return false;
}
return true;
}
return false;
});
defineType('XML','string containing xml tags',function(o) {
if (isString(o)) {
o = trim(o);
if (o.indexOf('<')>-1 && o.indexOf('>')>-1) return true; 
}
});
function arrayContains(a,o) {
return arrayIndexOf(a,o)>-1;
};
function arrayIndexOf(a,o,s) {
if (isFunction(a.indexOf)) return a.indexOf(o);
for (var i=(s||0); i<a.length; i++)
if (a[i] === o) return i;
echo('arrayIndexOf() : object '+inspect(o)+' is not a member of Array('+inspect(a)+')');
return -1;
};
function arrayRemove(a,o) {
if (isArray(a)) {
var i = arrayIndexOf(a,o);
if (i>-1) a.splice(i,1);
return a;
}
echo('arrayRemove() : argument 0 is not an array: '+inspect(a));
}
function keys(o) {
var k = [];
for (var i in o) k.push(i);
return k;
}
function isReserved(w) {
var r = 'abstract,as,boolean,break,byte,case,catch,char,class,continue,const,debugger,'+
'default,delete,do,double,else,enum,export,extends,false,final,finally,float,for,function,'+
'goto,if,implements,import,in,instanceof,int,interface,is,long,namespace,native,new,'+
'null,package,private,protected,public,return,short,static,super,switch,synchronized,'+
'this,throw,throws,transient,true,try,typeof,use,var,void,volatile,while,with';
return arrayContains(r.split(','),w);
}
function ucfirst(s) {
return s.charAt(0).toUpperCase()+s.substring(1);
}
function getSeconds(d) {
if (!d) d = new Date();
return d.getTime()/1000;
}
function parseNumber(t) {
if (isString(t)) {
t = t.replace(/[^0-9|\.|-]/g, '');
t = parseFloat(t);
if (isNaN(t)) return 0;
return t;
}
else if (isFloat(t)) return t;
else return 0;
}
function round(n,d) {
var p = !d? 1 : Math.pow(10,d);
return Math.round(parseFloat(n)*p)/p;
}
function trim(s) {
return isString(s)? s.replace(/^\s+|\s+$/g,'') : '';
};
function inspect(o,r) {
if (isString(o)) return o;
if (isFloat(o)) return o;
if (isObject(o)) {
var s = '{\n',t;
var b = (r==false)?'\t\t':'\t';
var v;
for (var i in o) {
v = o[i];
if (isString(v)) s += b+i+' : "'+v.replace('"','\"')+'"';
else if (isBoolean(v)) s += b+i+' : '+(v?'true':'false');
else if (isFloat(v)) s += b+i+' : '+o[i];
else if (isFunction(v)) s += b+i+' : "[Function]"';
else if (isArray(v))  s += b+i+' : "['+t+']"';
else if (isObject(v)) s += b+i+' : "[Object]"';
s += ',\n';
}
s = s.substring(0,s.length-2)+'\n';
if (r==false) s+= '\t';
s += '}';
return s;
}
};
function copy(t,s,p) {
if (!t) return alert('copy() target does not exist'); 
var i,j;
if (!!p) {
for (j in p) {
i = p[j];
t[i] = s[i];
}
}
else for (i in s) {
t[i] = s[i];
}
return t;
}
function print(s,d) {
if (jaxscript.isLoaded()) dom.append(s);
else document.write(s);
}
function println(s,d) {
if (!jaxscript.isBusy() && jaxscript.isLoaded()) {
var e = document.createElement('div');
e.className = "println";
e.innerHTML = s;
dom.append(e);
}
else document.write('<div class="println">'+s+'</div>');
}
var jaxscript = (function() {
var classes = {};
function getClasses() {
return getFields(classes);
}
function getClass(c) {
return isFunction(classes[c])? classes[c] : null;
}
function createClass(classId,proto,superId) {
function Constructor(a,b,d,e,f) {
if (isFunction(this[classId])) {
switch(arguments.length) {
case 0:
this[classId]();
break;
case 1:
this[classId](a);
break;
case 2:
this[classId](a,b);
break;
case 3:
this[classId](a,b,d);
break;
case 4:
this[classId](a,b,d,e);
break;
case 5:
this[classId](a,b,d,e,f);
break;
}
}
}
var c = classes[classId] = Constructor;
var s;
if (superId && isFunction(classes[superId])) {
s = classes[superId];
c.prototype = new s;
}
return defineClass(classId, c, proto, superId);
}
function defineClass(classId, c, proto, superId) {
var s;
if (superId) s = classes[superId];
if (isFunction(proto)) copy(c.prototype,proto());
copy(c.prototype,{
getClass : function() {
return c;
},
getSuper : function() {
return s;
},
callSuper : function(methodName) {
var a = new Array(arguments.length-1);
for (var i=1;i<arguments.length;i++) {
a[i-1] = arguments[i];
}
this.applySuper(methodName,a);
},
applySuper : function(methodName,a) {
this.getSuper().prototype[methodName].apply(this,a);
},
bindEvent : function(element, eventName, methodName) {
if (typeof this[methodName]=='function') {
var me = this;
var binding = {
element : element,
eventName : eventName,
methodName : methodName,
handler : function(e) {
me[methodName](e);
}
}
if (isNull(this.__bindings)) this.__bindings = [];
this.__bindings.push(binding);
dom.addEvent(element, eventName, binding.handler);
echo(classId+'.bindEvent(): binded "'+eventName+'" on '+element.nodeName+((element.id)?'#'+element.id:'')+' to [Object].'+methodName+'(e)');
return true;
}
else echo(classId+'.bindEvent() error: '+methodName+'(e) does not exist');
return false;
},
releaseEvent : function(element, eventName, methodName) {
if (isArray(this.__bindings)) {
for (var i=0;i<this.__bindings.length;i++) {
var binding = this.__bindings[i];
if (binding.element==element && binding.eventName==eventName && binding.methodName==methodName) {
dom.removeEvent(element, eventName, binding.handler);
arrayRemove(this.__bindings,binding);
echo(classId+'.releaseEvent(): released "'+eventName+'" on '+element.nodeName+((element.id)?'#'+element.id:'')+' from [Object].'+methodName+'(e)');
return true;
}
}
}
echo(classId+'.releaseEvent() error: could not find a "'+eventName+'" handler binded to "'+methodName+'()"');
return false;
}
});
copy(c,{
getClassId : function() {
return classId;
},
getSuperId : function() {
if (s) return superId;
},
extend : function(id, cons, p) {
echo('extending '+classId+' to '+id);
cons.prototype = new c; 
cons.prototype[classId] = c; 
defineClass(id, cons, p, classId);
},
implement : function(p,id) {
var omit = ['getClass','getSuper','callSuper','applySuper'];
for (var i in c.prototype) {
if (typeof p[i]=='undefined') {
p[i] = c.prototype[i];
}
}
return p;
},
isInstance : function(o) {
var v = false;
if (!!o && typeof o.getClass=='function') {
var cls = o.getClass();
if (cls.getClassId() == classId) return true;
else if (cls.getSuperId()) {
while (cls = classes[cls.getSuperId()]) {
if (cls.getClassId() == classId) return true;
}
}
}
else echo('o is not an instance of '+classId);
return v;
}
});
echo('jaxscript class '+classId+' defined');
return c;
}
var Class = createClass('Class');
var start = getSeconds();
var runs = [];
var busy = true;
var loaded = false;
var domloaded = false;
var supported = false;
var version = 0.1;
var feats = {};
var css = [];
var D = window.document;
function main() {
domloaded = true;
if (!jaxscript.allCSSLoaded()) {
setTimeout(main,1);
return;
}
for (var i in classes) {
if (isFunction(classes[i].main)) classes[i].main();
}
busy = false;
loaded = true;
for (var i=0;i<runs.length;i++) {
runs[i]();
}
echo('jaxscript types are '+getFields(type).join(', '));
echo('jaxscript client is '+getUserAgent());
echo('jaxscript server is '+location.host);
echo('jaxscript '+version+' loaded in '+round(getSeconds()-start,3)+' seconds');
}
function allCSSLoaded() {
var c = jaxscript.css;
if (c.length==0 || features('activex')) return true;
var loaded = 0;
var s = D.styleSheets;
for (var i=0; i<s.length; i++) {
if (features('gecko')) {
try {
if (!s[i].cssRules) return false;
}
catch(e) {
return false;
}
}
if (s[i].cssRules) {
for (var j=0; j<c.length; j++) {
if (s[i].href==c[j].href && !s[i].disabled) loaded++;
}
}
}
return loaded==c.length;
}
function getUserAgent() {
return (typeof navigator=='object')? navigator.userAgent : '';
}
function getPlugin(s) {
var a = navigator.plugins;
if (a.length>0)
for (i=0;i<a.length;i++)
if (a[i].name.indexOf(s) > -1)
return a[i];
}
function addFeature(id,name,test) {
feats[id] = {
name : name,
test : test
};
}
function features(id,v) {
if (feats[id]) return feats[id].test(v);
return false;
}
addFeature('activex','ActiveX',function() {
return typeof ActiveXObject=='object' || typeof ActiveXObject=='function';
});
addFeature('fixed','CSS-P Fixed Position extension',function(v) {
return !features('activex');
});
addFeature('cookies','Browser cookies',function(v) {
D.cookie = "1";
return D.cookie.indexOf("1")>-1;
});
addFeature('dom','Document Object Model',function(v) {
if (!v) v = 1;
var cv = 0;
if (
( typeof D.addEventListener=='function' &&
(typeof DOMParser=='function'||typeof DOMParser=='object') &&
(typeof XMLSerializer=='function'||typeof XMLSerializer=='object') &&
(typeof XSLTProcessor=='function'||typeof XSLTProcessor=='object') ) ||
(features('activex') && typeof self.attachEvent=='object' && typeof D.getElementById=='object')
) cv = 2;
if (typeof D.adoptNode=='function') cv = 3;
return cv >= v;
});
addFeature('ecma','ECMAScript',function(v) {
if (!v) v = 1;
var cv = 0;
if (typeof [].pop=='function' && typeof parseFloat=='function' && typeof decodeURIComponent=='function')
cv = 3;
return cv >= v;
});
addFeature('flash','Adobe Shockwave Flash',function(v) {
return !!getPlugin("Shockwave Flash");
});
addFeature('gecko','Gecko HTML Rendering Engine',function() {
return getUserAgent().toLowerCase().indexOf("gecko")>-1 && !features("webkit");  
});
addFeature('webkit','WebKit HTML Rendering Engine',function() {
return getUserAgent().toLowerCase().indexOf("webkit")>-1;
});
addFeature('safari','Safari version of WebKit',function() {
return features("webkit") && getUserAgent().toLowerCase().indexOf("safari")>-1 && !features("chrome");  
});
addFeature('chrome','Chrome version of WebKit',function() {
return features("webkit") && getUserAgent().indexOf("Chrome")>-1;
});
addFeature('iphone','Apple iPhone/iPod',function() {
return features("webkit") && getUserAgent().indexOf("iPhone")>-1 || getUserAgent().indexOf("iPod")>-1;
});
addFeature('java','Sun Microsystems Java',function() {
return typeof java=='object' && !!getPlugin("Java");
});
addFeature('js','JavaScript',function(v) {
if (!v) v = 1;
var cv = 1.5; 
if (typeof [].indexOf=='function' && typeof [].forEach=='function')
cv = 1.6;
return cv > v;
});
addFeature('xhr','XMLHTTPRequest communication',function() {
return features('activex') || typeof XMLHttpRequest=='function' || typeof XMLHttpRequest=='object';
});
if (typeof D.onreadystatechange=='object') { 
supported = true;
D.onreadystatechange = function() {
if (D.readyState == "complete") {
main();
}
};
}
else if (typeof D.addEventListener=='function') {
supported = true;
D.addEventListener("DOMContentLoaded", main, true);  
}
if (!isSupported()) unsupported();
function isBusy() {
return busy;
}
function isLoaded() {
return domloaded; 
}
function isSupported() {
return supported;
}
function unsupported() {
if (confirm('Sorry. Your web browser is unsupported.\n\nWould you like to upgrade?'))
top.location = 'http://www.jaxcore.com/upgrade/';
};
function run(f, first) {
if (domloaded) f();
else if (first) runs.unshift(f);
else runs.push(f);
};
return {
Class : Class,
css : css,
main : main,
allCSSLoaded : allCSSLoaded,
createClass : createClass,
defineClass : defineClass,
getClasses : getClasses,
getClass : getClass,
getPlugin : getPlugin,
getUserAgent : getUserAgent,
features : features,
addFeature : addFeature,
isBusy : isBusy,
isLoaded : isLoaded,
isSupported : isSupported,
unsupported : unsupported,
run : run
};
})();
jaxscript.inspect = inspect;
defineType('Node','DOM Node',function(n,t) {
var b = (isObject(n) && exists(n,['nodeType','nodeName']));
if (b && isString(t))
return n.nodeName.toUpperCase()==t.toUpperCase();
return b;
});
defineType('Window','browser window or frame element',function(o) {
return o==window || o.nodeName=='frame' || o.nodeName=='iframe';  
});
defineType('Document','DOM Document',function(o) {
return isNode(o) && o.nodeType==1||o.nodeType==9;
});
defineType('Element','DOM Node that is a child of document.body',function(o) {
return isNode(o) && isChild(o,document.body);
});
defineType('Event','DOM Event object',function(e) {
if (!!self.event) return e===self.event;
else return (isObject(e) && !!e.type);  
});
var dom = jaxscript.dom = (function() {
function id(s,d) {
if (isObject(s)) return s;
if (isString(s)) {
if (!d) d = document;
var o = d.getElementById(s);
if (!!o) return o;
}
};
function dimensions(o,frame) {
var x=y=w=h=0;
var f = !!frame? frame : window;
var d = f.document;
var b = d.body;
var de = d.documentElement;
if (o=='#document') {
if (exists(f,'innerWidth','innerHeight','scrollMaxY','scrollMaxY')) { 
w = f.innerWidth + f.scrollMaxX;
h = f.innerHeight + f.scrollMaxY;
}
else if (exists(b,'scrollWidth','offsetWidth','scrollHeight','offsetHeight')) { 
w = (b.scrollWidth > b.offsetWidth)? b.scrollWidth : b.offsetWidth;
h = (b.scrollHeight > b.offsetHeight)? b.scrollHeight : b.offsetHeight;
}
else if (exists(f,'innerHeight','innerHeight')) { 
w = f.innerWidth,
h = f.innerHeight
}
else if (exists(b,'offsetWidth','offsetHeight')) {
echo('dimensions(document) : falling back to offsetWidth/Height');
w = b.offsetWidth;
h = b.offsetHeight;
}
if (exists(b,'scrollLeft','scrollTop')) {
x = b.scrollLeft;
y = b.scrollTop;
}
if (x==0 && y==0 && exists(de,'scrollLeft','scrollTop')) {
x = de.scrollLeft;
y = de.scrollTop;
}
}
else if (o=='#window') {
if (exists(f,'innerWidth','innerHeight')) {
w = f.innerWidth;
h = f.innerHeight;
}
else if (exists(b,'clientWidth','clientHeight')) {
w = b.clientWidth;
h = b.clientHeight;
}
else if (exists(de,'clientWidth','clientHeight')) {
w = de.clientWidth;
h = de.clientHeight;
}
if (jaxscript.features('activex')) {
x = f.screenLeft;
y = f.screenTop;
}
else {
x = f.screenX;
y = f.screenY;
}
}
else if (o=='#screen') {
w = screen.width;
h = screen.height;
}
else {
o = id(o);
if (!!o) {
var sx = 0;
if (b && b.scrollLeft) sx = b.scrollLeft;
if (de && de.scrollLeft) sx = de.scrollLeft;
var sy = 0;
if (b && b.scrollTop) sy = b.scrollTop;
if (de && de.scrollTop) sy = de.scrollTop;
var r;
if (o.getBoundingClientRect) { 
r = o.getBoundingClientRect();
x = r.left + sx;
y = r.top + sy;
w = r.right - r.left;
h = r.bottom - r.top;
}
else if (d.getBoxObjectFor) { 
r = d.getBoxObjectFor(o);
x = r.x;
y = r.y;
w = r.width;
h = r.height;
}
else {  
return offsetDimensions(o);
}
}
}
return {x:round(x),y:round(y),w:round(w),h:round(h)};
};
function offsetDimensions(o) {
var x=y=0;
var w = o.offsetWidth;
var h = o.offsetHeight;
while (o.offsetParent) {
x += o.offsetLeft;
y += o.offsetTop;
o = o.offsetParent;
}
var d = document;
x += (d.body.scrollLeft || d.documentElement.scrollLeft || 0);
y += (d.body.scrollTop || d.documentElement.scrollTop || 0);
return {x:x,y:y,w:w,h:h};
};
function getScroll(n) {
n = id(n);
return {x:n.scrollLeft,y:n.scrollTop};
}
function addClass(n,c) { 
n = id(n);
if (!n) {
echo('dom.addClass() error: element not found '+n);
return;
}
if (n.nodeType==1) {
if (!hasClass(n,c)) {
if (isNull(n.className)) n.className = c;
else n.className += " "+c;
}
}
return n;
};
function removeClass(n,c) {
n = id(n);
if (hasClass(n,c)) n.className = trim(n.className.replace(c,'').replace('  ',''));
}
function replaceClass(n,className,newClassName) {
n = id(n);
if (hasClass(n,className)) n.className = n.className.replace(className,newClassName);
return n;
}
function findClass(tagAndOrClass,parentNode) {
var node = !!parentNode? id(parentNode) : document.body;
var r = [];
if (!node) return r;
var n;
var dot = tagAndOrClass.indexOf('.');
if (dot>=0) {  
var tagname = tagAndOrClass.substring(0,dot);
var className = tagAndOrClass.substring(dot+1);
var nodes = node.getElementsByTagName(tagname);
for (var i=0;i<nodes.length;i++) {
nd = nodes[i];
if (hasClass(nd,className)) r.push(nd);
}
}
else {  
walkDOM(node, function(nd) {
if (hasClass(nd,cname)) r.push(nd);
});
}
return r;
};
function hasClass(n,cname) {
n = id(n);
return !!n.className && (n.className==cname || arrayIndexOf(n.className.split(' '),cname)>-1);
};
function cssLoaded(link,handler) {
var loaded = false;
var s = document.styleSheets;
for (var i=0;i<s.length;i++) {
if (s[i].href==link.href) {
try {
if (!!s[i].cssRules || !!s[i].rules) {
echo('loaded stylesheet '+link.href);
handler();
return;
}
}
catch(e) {}
}
}
setTimeout(function() {
cssLoaded(link,handler);
},10);
}
function insertTag(o) {
var s = document.createElement(o.tagName);
for (var i in o.attributes) {
s.setAttribute(i,o.attributes[i]);
}
if (o.attributes.type && typeof o.handler=='function') {
if (o.attributes.type=='text/javascript') {
var h = function() {
echo('loaded script '+s.src);
o.handler();
}
if (typeof s.onreadystatechange=='Object') {
s.onreadystatechange = function() {
if (s.readyState=='loaded') h();
};
return;
}
else if (jaxscript.features('webkit')) {
var t = setInterval(function() {
if (document.readyState=='complete') {
clearInterval(t);
h();
}
},10);
}
else s.onload = h;
}
if (o.attributes.type=='text/css') {
cssLoaded(s,o.handler);
}
}
if (o.after) after(s, o.after);
else if (o.before) before(s, o.before);
else if (o.prepend) prepend(s, o.prepend);
else if (o.append) {
if (o.append=='#head') append(s, document.getElementsByTagName('head')[0]);
else append(s, o.append);
}
else append(s);
return s;
};
function append(n, p) {
if (jaxscript.isLoaded()) {
n = id(n);
if (!p && exists(document,'body')) p = document.body;
if (!!p && !!p.appendChild) p.appendChild(n);
return n;
}
else echo("dom.append() error: cannot be called before the DOM is initialized");
}
function prepend(n, p) {
if (jaxscript.isLoaded()) {
n = id(n);
if (!p && exists(document,'body')) p = document.body;
if (hasChildren(n)) before(n, p.childNodes[0]);
else append(n, p);
return n;
}
else echo("dom.prepend() error: cannot be called before the DOM is initialized");
}
function after(n, s) {
n = id(n);
s.parentNode.insertBefore(n, s.nextSibling);
};
function before(n, s) {
n = id(n);
if (!s) echo('dom.before() error: source object does not exist');
else s.parentNode.insertBefore(n, s);
}
function remove(n) {
n = id(n);
id(n).parentNode.removeChild(n);
}
function replace(a,b) {
dom.before(b,a);
dom.remove(a);
}
function children(n) {
n = id(n);
if (n && n.childNodes && n.childNodes.length>0) return n.childNodes;
else return [];
}
function findParent(n,tag) {
n = id(n);
while (n.parentNode) {
if (n.parentNode.tagName.toUpperCase()==tag.toUpperCase())
return n.parentNode;
n = n.parentNode;
}
};
function hasChildren(n) {
return isNode(n) && n.childNodes.length>0;
}
function isChild(n,p) {
return isParent(p,n);
};
function isParent(p,n) {
p = id(p);
n = id(n);
if (n==p) return false;
if (jaxscript.features('activex') && typeof p.contains == 'function' && n.nodeType == 1) {  
return n==p || p.contains(n);
}
while (n) {
if (n===p) return true;
if (!!n.parentNode) n = n.parentNode;
else return false;
}
return false;
};
function isSibling(n,s) { 
s = id(s);
var c = id(n).parentNode.childNodes;
for (var i=0;i<c.length;i++)
if (c[i]==s) return true;
return false;
};
function firstChild(n) {
n = id(n);
var c = n.childNodes;
for (var i=0;i<c.length;i++) {
if (c[i].nodeType!=3) return c[i];
}
}
function getScriptPath(file) {
file = file.replace('.','\.');
var re = new RegExp("(^|\/)"+file+"([?#].*)?$","i");
var s = document.getElementsByTagName('script');
for (var i=0;i<s.length;i++) {
if(s[i].src && re.test(s[i].src)) {
return s[i].src.replace(/[^\/]+$/,'').replace(/\/$/, '');
}
}
echo('getScriptPath() error: no script by name of '+file);
return '';
}
function loadCSS(css,fn) {
if (!jaxscript.isLoaded()) {
var i = jaxscript.css.length;
document.write('<link id="_sheet_'+i+'" rel="stylesheet" type="text/css" href="'+css+'" />');
jaxscript.css[i] = id('_sheet_'+i);
echo('wrote stylesheet '+css);
if (isFunction(fn)) {
jaxscript.run(function() {
fn();
});
}
return dom.id('_sheet_'+i);
}
else return insertTag({
tagName : 'link',
handler : fn,
attributes : {
href : css,
type : 'text/css',
rel : 'stylesheet'
},
append : '#head'
});
};
function loadJS(js,fn) {
if (!jaxscript.isLoaded()) {
document.write('<script type="text/javascript" src="'+js+'"><\/script>');
echo('wrote script '+js);
if (isFunction(fn)) {
jaxscript.run(function() {
fn();
});
}
}
else return insertTag({
tagName : 'script',
handler : fn,
attributes : {
src : js,
type : 'text/javascript'
},
append : '#head'
});
}
function loadXML(url) {
return request({
url : url,
async : false
}).responseXML;
}
function loadText(url) {
return request({
url : url,
async : false
}).responseText;
}
function loadJSON(url, fn) {
jaxscript.run(function() {
request({
url : url,
method : "get",
handler : function(r) {
fn(transform.string2json(r.responseText));
},
async : true
});
});
};
window.loadJSONPc = 0;
function loadJSONP(url, handler) {
var callback = 'jsonp'+(++window.loadJSONPc);
window[callback] = function(json) {
handler(json);
setTimeout(function() {
delete window[callback].json;
window[callback] = null;
},1);
};
dom.loadJS(url+callback);
}
function walk(n, fn) {
if (n.nodeName && n.childNodes) {
fn(n);
n = n.firstChild;
while (n) {
walk(n,f);
n = n.nextSibling;
}
}
else if (isArray(n)) { 
for (var i in n) {
fn(n[i]);
}
}
};
function outerHTML(n,deep) {
if (!isNode(n)) return '';
if (n.outerHTML) return n.outerHTML;
else {
var d = document.createElement('div');
var clone = n.cloneNode(deep);
dom.append(clone,d);
return d.innerHTML;
}
}
function addEvent(n,e,h,p) {
n = id(n);
if (n) {
if (n.addEventListener) n.addEventListener(e,h,(p==null)?false:p);
else if (n.attachEvent) n.attachEvent("on"+e,h);
}
else echo('addEvent node does not exist');
}
function removeEvent(n,e,h,p) {
n = id(n);
if (n) {
if (n.removeEventListener) n.removeEventListener(e,h,(p==null)?false:p);
else if (n.detachEvent) n.detachEvent("on"+e,h);
}
else echo('addEvent node does not exists');
}
function cancelEvent(e) {
e.cancelBubble = true;
e.returnValue = false;
if (e.stopPropagation) e.stopPropagation();
if (e.preventDefault) e.preventDefault();
return false;
};
function eventAbsPosition(e) {
if (window.event) {
e = window.event;
var elm = (document.documentElement)? document.documentElement : document.body;
return {
x : e.clientX + elm.scrollLeft,
y : e.clientY + elm.scrollTop
};
}
else if (exists(e,'pageX','pageY')) return {x:e.pageX,y:e.pageY};
else return null;
};
function eventPosition(e) {
var g = jaxscript.features('gecko');
return {
x : g? e.layerX : e.offsetX,
y : g? e.layerY : e.offsetY
}
}
function relatedTarget(e) {
var r = e.relatedTarget;
if (r) {
try {
r.nodeName;
}
catch (e) {
if (r.nodeType==3) return r.parentNode; 
echo('error: relatedTarget() had an invalid node');
return null;
}
return r;
}
if (window.event) {
if (e.type=="mouseover" && window.event.fromElement) return window.event.fromElement;
if (e.type=="mouseout" && window.event.toElement) return window.event.toElement;
}
}
function eventTarget(e) {
var t = (e && e.target)? e.target : window.event.srcElement;
return (t.nodeType == 3)? t.parentNode:t; 
};
function request(o) {
if (!jaxscript.features('xhr')) return unsupported();
o = o||{};
if (!o.method) o.method = "get";
if (o.async==null) o.async = true;
if (o.cache===false) url += url.indexOf('?')>0?'&':'?'+'nocache='+Math.random().toString().substring(2);
var r = (typeof XMLHttpRequest=='function' || typeof XMLHttpRequest=='object')? new XMLHttpRequest():new ActiveXObject("Microsoft.XMLHTTP");
r.open(o.method,o.url,o.async);
r.onreadystatechange = function(){
if (r.readyState==4) {
if (r.status == 200 && typeof o.handler=='function') return o.handler(r);
if (typeof o.errorHandler=='function') o.errorHandler(r);
}
};
r.send('');
return r;
};
return {
id : id,
dimensions : dimensions,
offsetDimensions : offsetDimensions,
getScroll : getScroll,
addClass : addClass,
removeClass : removeClass,
replaceClass : replaceClass,
findClass : findClass,
hasClass : hasClass,
insertTag : insertTag,
append : append,
prepend : prepend,
after : after,
before : before,
remove : remove,
replace : replace,
children : children,
findParent : findParent,
isChild : isChild,
isParent : isParent,
isSibling : isSibling,
firstChild : firstChild,
getScriptPath : getScriptPath,
loadCSS : loadCSS,
loadJS : loadJS,
loadXML : loadXML,
loadText : loadText,
loadJSON : loadJSON,
loadJSONP : loadJSONP,
walk : walk,
outerHTML : outerHTML,
addEvent : addEvent,
removeEvent : removeEvent,
cancelEvent : cancelEvent,
eventAbsPosition : eventAbsPosition,
eventPosition : eventPosition,
relatedTarget : relatedTarget,
eventTarget : eventTarget,
request : request
};
})();
defineType('CSSDelcaration','CSS declaration (the stuff inside CSS brackets)',function(o) {
if (isString(o)) return o.indexOf(':')>1;
});
defineType('CSSRule','CSS Rule',function(o) {
if (isString(o)) return o.indexOf(':')>1;
});
var style = jaxscript.style = (function() {
function clip(n,i) {
return set(n,{
clip : (i&&i.length==4)? 'rect('+i[0]+'px '+i[1]+'px '+i[2]+'px '+i[3]+'px)' : 'auto'
});
}
function display(n,b) {
var s = b? 'block' : 'none';
return set(n,{
display : s
});
}
function getClip(n) {
n = dom.id(n);
var c = n.style.clip;
if (c && c.indexOf('rect(') == 0) {
c = c.replace("rect(","");
c = c.replace(")","");
var v = c.split(" ");
for (var i in v) v[i] = parseInt(v[i]);
return v;
}
else return [0, n.offsetWidth, n.offsetHeight, 0];
}
function getOpacity(n) {
n = dom.id(n);
return isFloat(n.style.opacity)? parseFloat(n.style.opacity) : 1;
}
function get(n,s) {
n = dom.id(n);
if (isString(s)) {
if (s=="opacity") return isFloat(n.style.opacity)? n.style.opacity : 1;
if (!!document.defaultView) return document.defaultView.getComputedStyle(n,"").getPropertyValue(convert.camel2dash(s));
else if (n.currentStyle) return n.currentStyle[s];
else if (n.style[s]) return n.style[s];
else if (isNode(n)) echo('error: style.get() could not obtain style for node '+node.nodeName+'#'+n.id);
}
}
function getXY(n) {
n = dom.id(n);
return {
x : parseInt(get(n,'left')),
y : parseInt(get(n,'top'))
}
};
var _z = 5000; 
function maxZ(n) {
return set(n,{zIndex:++_z});
}
function getSize(n) {
return {
w : parseInt(get(n,'width')),
h : parseInt(get(n,'height'))
}
}
function mk(s) {
var p = [
{node : type.Node},
{nodeId : type.String}
];
p[0][s] = p[1][s] = type.Integer;
return function(n,x) {
var o = {};
o[s] = px(x);
return set(n,o);
}
}
var left = mk('left');
var right = mk('right');
var top = mk('top');
var bottom = mk('bottom');
var width = mk('width');
var height = mk('height');
function move(n,x,y) {
return set(n,{
left:px(x),
top:px(y)
});
}
function opacity(n,f) {
n = dom.id(n);
f = parseFloat(f);
if (f<0) f = 0;
if (f>1) f = 1;
n.style.opacity = f;
if (jaxscript.features('activex')) n.style.filter = 'alpha(opacity='+f*100+')';
return n;
}
function px(i) {
return round(parseNumber(i))+'px';
}
function size(n,w,h) {
return set(n,{
width:px(w),
height:px(h)
});
}
function set(o,s) {
var n = dom.id(o);
if (!!n) {
if (isArray(n)) {  
for (var i=0;i<n.length;i++)
copy(n[i].style,s);
}
else if (!!n.style) {
copy(n.style,s);
return n;
}
}
echo('style.set() error: element '+(isString(o)?o:'')+' does not exist, properties were '+inspect(s));
}
function swap(a,b) {
display(a,0);
display(b,1);
}
function show(o) {
return visible(o,true);
}
function hide(o) {
return visible(o,false);
}
function visible(n,b) {
return set(n,{visibility:b?'visible':'hidden'});
}
function getRule(selector, stylesheet, returnIndex) {
var sheets = (!!stylesheet)? [stylesheet] : document.styleSheets;
var s,r,i,j;
for (i=0; i<sheets.length; i++) {
s = sheets[i];
r = (!!s.cssRules)? s.cssRules : s.rules;
if (!r) return echo('stylesheet invalid');
for (j=0; j<r.length; j++) {
if (r[j].selectorText == selector) {
if (returnIndex) return j;
else return r[j];
}
}
}
if (returnIndex) return -1;
}
function createRule(selector,styles,stylesheet) {
if (!stylesheet) stylesheet = document.styleSheets[0];
if (typeof stylesheet.addRule=='object' || typeof stylesheet.addRule=='function') {
var r = stylesheet.addRule(selector, styles, 0);
}
else if (typeof stylesheet.insertRule=='function') {
var r = stylesheet.insertRule(selector+' {'+styles+'}', 0);
}
}
function deleteRule(selector, stylesheet) {
var r = getRule(selector, stylesheet);
var i = getRule(selector, r.parentStyleSheet, true);
if (i>-1 && !!r && !!r.parentStyleSheet) {
if (r.parentStyleSheet.deleteRule) {
r.parentStyleSheet.deleteRule(i);
}
else if (r.parentStyleSheet.deleteRule) {
r.parentStyleSheet.removeRule(i);
}
}
}
return {
clip : clip,
display : display,
getClip : getClip,
getOpacity : getOpacity,
getSize : getSize,
get : get,
getXY : getXY,
maxZ : maxZ,
move : move,
opacity : opacity,
size : size,
set : set,
visible : visible,
left : left,
top : top,
right : right,
bottom : bottom,
width : width,
height : height,
show : show,
hide : hide,
getRule : getRule,
createRule : createRule,
deleteRule : deleteRule
}
})();var fx = jaxscript.fx = (function() {
function fadeIn(n,fn) {
fade(n,1,fn);
}
function fadeOut(n,fn) {
fade(n,0,fn);
}
function fadeCancel(n) {
if (!!n._fadeTimer) clearTimeout(n._fadeTimer);
}
function fade(n,limit,fn,inc,s) {
n = dom.id(n);
style.opacity(n, style.getOpacity(n));  
if (inc==null) inc = 0.1;
else inc = Math.abs(inc);
if (s==null) s = 50;
if (limit==null || limit<0) limit = 0;
if (limit>1) limit = 1;
if (style.getOpacity(n)>limit) inc = -inc;
fadeCancel(n);
fadeStep(n,limit,fn,inc,s);
};
function fadeStep(n,limit,fn,inc,s) {
var x = style.getOpacity(n) + inc;
if (inc>0 && x>limit || inc<0 && x<limit) x = limit;
style.opacity(n,x);
if (x!=limit)
n._fadeTimer = setTimeout(function() {
fadeStep(n,limit,fn,inc,s);
},s);
else {
if (fn) fn();
}
};
return {
fade : fade,
fadeIn : fadeIn,
fadeOut : fadeOut,
fadeCancel : fadeCancel
}
})();
var convert = jaxscript.convert = (function() {
function dash2camel(s) {
var i;
while(s.indexOf('-')>-1) {
i = s.indexOf('-');
s = s.substring(0,i)+s.charAt(i+1).toUpperCase()+s.substring(i+2);
}
return s;
}
function camel2dash(s) {
var i;
while(s.search(/[A-Z]/)>-1) {
i = s.search(/[A-Z]/);
s = s.substring(0,i)+'-'+s.charAt(i).toLowerCase()+s.substring(i+1);
}
return s;
}
function style2css(s) {
var c = '';
for (var i in s) {
if (isFloat(s[i]) || isString(s[i])) {
c += dash2camel(i)+':'+s[i]+'; ';
}
}
return c;
}
function css2style(c) {
var s = {};
var p = c.split(';');
var x;
for (var i=0;i<p.length;i++) {
if (isString(p[i])) {
x = p[i].split(':');
if (x.length==2) s[dash2camel(trim(x[0]))] = x[1];
}
}
return s;
}
function param2obj(url) {
if (!url) url = window.location.search;
if (url.indexOf('?')>-1) url = url.substring(url.indexOf('?')+1);
var o = {};
var q = url.length>1?url.split("&"):[];
for (var i=0;i<q.length;i++)
o[q[i].match(/^[^=]+/)] = unescape(q[i].replace(/^[^=]+=?/, ""));
return o;
}
function obj2param(o) { 
var i, a = [];
for (i in o) {
a[a.length] = i + '=' + encodeURIComponent(o[i]);
}
return a.join('&');
}
function attr2json(n,json) {
var a;
for (var i=0;i<n.attributes.length;i++) {
a = n.attributes[i];
json[a.nodeName] = trim(a.nodeValue);
}
return json;
}
function node2json(n) {
n = id(n);
var json;
var l = n.childNodes.length;
var i;
if (l==0) { 
if (n.attributes && n.attributes.length>0) {
json = attr2json(n,{});
}
}
else {
var childnodes = [];
var child;
var isarray = true;  
for (i=0;i<n.childNodes.length;i++) {
child = n.childNodes[i];
if (child.nodeName=="#text") {
continue;
}
l = childnodes.length;
childnodes[l] = child;
if (isarray && l>0) {
if (childnodes[l-1].nodeName != child.nodeName) isarray = false;
}
}
if (childnodes.length==0) {
var v = trim(n.childNodes[0].nodeValue);
json = (v=="")?null:v;
}
else if (childnodes.length==1) {
json = attr2json(n,{});
json[childnodes[0].nodeName] = node2json(childnodes[0]);
}
else {
if (isarray) {
var arrayname = childnodes[0].nodeName;
json = {};
json[arrayname] = [];
for (i=0;i<childnodes.length;i++) {
json[arrayname][i] = node2json(childnodes[i]);
}
}
else {
json = {};
for (i=0;i<childnodes.length;i++) {
child = childnodes[i];
json[child.nodeName] = node2json(child);
}
}
}
}
return json;
}
function dom2json(d) {
if (d.childNodes.length>=1) {
var n = d.childNodes[0];
var j = {};
j[n.nodeName] = node2json(n);
return j;
}
return {};
}
function dom2xml(n) {
if (typeof XMLSerializer=='function') return (new XMLSerializer()).serializeToString(n);
else if (n.xml) return n.xml;
echo('dom2xml() error: could not serialize DOM to XML, not supported');
}
function json2dom(json,tag) {
return xml2dom(json2xml(json,tag));
}
function xmlstr(s) {
if (s.indexOf('\n')>-1 && s.indexOf('<![CDATA[')==-1) return '<![CDATA[' + s + ']]>';
else return trim(s.replace('&','&amp;').replace('<','&lt;').replace('>','&gt;').replace('"','&quot;').replace('\'','&apos;'));
}
function json2xml(o,tag,addDefinition) {
var xml = (addDefinition)? '<?xml version="1.0"?>\n':'';
if (isArray(o))
for (var i=0;i<o.length;i++)
xml += json2xml(o[i],tag);
else {
var start = (tag)?'<'+tag+'>':'';
var end = (tag)?'</'+tag+'>':'';
if (isInteger(o) || isFloat(o)) xml += start + o + end;
if (isBoolean(o)) xml += start + o + end;
else if (isString(o)) xml += start + xmlstr(o) + end;
else if (isObject(o)) {
xml += start;
for (var i in o)
xml += json2xml(o[i],i);
xml += end;
}
}
return xml;
}
function stripXML(xml) {
xml = xml.replace(/\r/g,'');
xml = xml.replace(/\n/g,'\uffff');
xml = xml.replace(/<\!(.*?)>/g,"");
xml = xml.replace(/<\?(.*?)\?>/g,"");
xml = xml.replace(/\uffff/g,'\n');
return trim(xml);
}
function xml2dom(xml) {
var d,p;
xml = stripXML(xml);
if (typeof DOMParser=='function' || typeof DOMParser=='object') {
try {
p = new DOMParser();
d = p.parseFromString(xml,"text/xml");
}
catch (e) {
echo('convert.xml2dom() error: '+e.message);
}
}
else if (jaxscript.features('activex')) {
try {
d = new ActiveXObject("Microsoft.XMLDOM");
d.async = false;
d.loadXML(xml);
}
catch (e) {
echo('convert.xml2dom() error: '+e.message);
}
}
return d;
}
function xml2json(xml) {
return dom2json(xml2dom(xml));
}
function obj2json(o, nullStrings, depth) {
if (!!o) {
var json = {};
for (var i in o) {
var v = o[i];
if (!!v) {
if (isArray(v)) {
json[i] = v;
}
else if (isObject(v)) {
var o_json = obj2json(v,nullStrings,(depth>1)?--depth:0);
if (o_json) json[i] = o_json;
}
else if (!isFunction(v)) json[i] = v;
}
else if (nullStrings) json[i] = '';
}
return json;
}
else if (nullStrings) return {};
}
function quote(s) {
var fn = function (a) {
var meta = {
'\b': '\\b',
'\t': '\\t',
'\n': '\\n',
'\f': '\\f',
'\r': '\\r',
'"' : '\\"',
'\\': '\\\\'
}
var c = meta[a];
return typeof c === "string" ? c : '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
};
var c = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g;
return c.test(s)? '"' + s.replace(c, fn) + '"' : '"' + s + '"';
}
function json2string(o,k) {
var s = '';
if (isArray(o)) {
var a = [];
for (var i=0; i<o.length; i++) {
a.push(json2string(o[i]));
}
s = '[' +a.join(',') + ']';
}
else if (isObject(o)) {
var a = [];
for (var i in o) {
if (typeof o[i]!='function') {
a.push(quote(i) + ':' + json2string(o[i]));
}
}
s = '{' + a.join(',') + '}';
}
else if (isString(o)) s = quote(o);
else if (isInteger(o) || isFloat(o)) {
if (isFinite(o)) s = String(o);
else echo('convert.json2string','value of '+o+' is not finite');
}
else if (isBoolean(o)) s = String(o);
else if (isNull(o)) s = '""';
else echo('convert.json2string','value '+inspect(o)+' could not be converted to a JSON string');
if (isString(k)) return '{' + quote(k) + ':' + s + '}';
else return s;
}
function string2json(s) {
var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g;
if (cx.test(s)) {
s = s.replace(cx, function(a) {
echo('string2json() : string contains control characters : \n'+s);
return '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
});
}
return eval('(' + s + ')');
}
return {
dash2camel : dash2camel,
camel2dash : camel2dash,
style2css : style2css,
css2style : css2style,
param2obj : param2obj,
obj2param : obj2param,
attr2json : attr2json,
node2json : node2json,
dom2json : dom2json,
dom2xml : dom2xml,
json2dom : json2dom,
xmlstr : xmlstr,
json2xml : json2xml,
stripXML : stripXML,
xml2dom : xml2dom,
xml2json : xml2json,
obj2json : obj2json,
json2string : json2string,
string2json : string2json
};
})();
jaxscript.param = convert.param2obj();var xsl = jaxscript.xsl = (function() {
function setParam(xsldom,paramName,paramValue) {
var nodes = xsldom.childNodes[xsldom.childNodes.length-1].childNodes;
for (var i=0;i<nodes.length;i++) {
if (nodes[i].nodeName=='xsl:param') {
if (nodes[i].getAttribute('name')==paramName) {
var svalue;
if (isString(paramValue)) svalue = "'"+paramValue+"'";
else svalue = paramValue;
echo('xsl.setParam(): '+paramName+' = '+svalue);
nodes[i].setAttribute('select',svalue);
return;
}
}
}
}
function transform(xmldom, xsldom, options) {
if (!options) options = {
params:{},  
output:'html'  
};
if (isString(xmldom)) xmldom = dom.loadXML(xmldom);
if (isString(xsldom)) xsldom = dom.loadXML(xsldom);
if (!!xsldom && !!xsldom.childNodes && !!xmldom && !!xmldom.childNodes) {
if ((typeof XSLTProcessor=='function' && typeof XMLSerializer=='function') || (typeof XSLTProcessor=='object' && typeof XMLSerializer=='object')) {
var p=new XSLTProcessor(),s=new XMLSerializer();
p.importStylesheet(xsldom);
if (options.output=="document") return p.transformToDocument(xmldom);
else {
var x = p.transformToFragment(xmldom,document);
if (x.childNodes && x.childNodes.length>0) {
if (options.output=="xml") return x;
else return s.serializeToString(x,"text/xml");
}
else echo('xsl.transform(): result had no nodes');
return '';
}
}
else if (typeof ActiveXObject=='function') {
var xml = xmldom.transformNode(xsldom);
if (options.output=="document" || options.output=="fragment") return xml2dom(xml);
else return xml;
}
else jaxscript.unsupported();
}
echo('xsl.transform() error: invalid xml document(s)');
return (returnType=="xml")?null:"";
}
return {
setParam : setParam,
transform : transform
}
})();
