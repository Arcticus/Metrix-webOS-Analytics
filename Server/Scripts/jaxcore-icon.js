/*
*  JaxCore Icon Component v1.0
*
*   A free product by JaxCore - visit us at http://www.jaxcore.com
*   Released under the CreativeCommons 2.5 license (http://creativecommons.org/licenses/by/2.5/)
*   You are free to share, change, or redistribute this work so long as this notice remains
*/

if (typeof Icon=='undefined') {
	
	var Icon = (function() {
	
		defineType('Icon','JaxCore Icon Component',function(o) {
			return o.nodeName=='SPAN' && (dom.hasClass(o,'icon') || dom.hasClass(o,'iconr') || dom.hasClass(o,'iconb') || dom.hasClass(o,'iconbr') || dom.hasClass(o,'icononly')) && o.className.indexOf('-')>-1;
		});
		
		var path = dom.getScriptPath('jaxcore-icon.js');
		if (path) {
			var css = dom.loadCSS(path+'/jaxcore-icon.css');
			var stylesheet = (css.sheet)? css.sheet : document.styleSheets[document.styleSheets.length-1];
		}
		
		return {
			path : path,
			stylesheet : stylesheet,
			iconsets : {
				silk : {
					ext : 'png',
					icon : 'silk-application_view_icons'
				},
				app : {
					ext : 'png',
					icon : 'silk-application_xp'
				},
				bullet : {
					ext : 'png',
					icon : 'bullet-toggle_minus'
				},
				file : {
					ext : 'png',
					icon : 'silk-page_white_copy'
				},
				flag : {
					ext : 'gif',
					icon : 'silk-flag_blue'
				},
				website : {
					ext : 'gif',
					icon : 'silk-world'
				}
			},
			add : function(iconname) {
				
				var n = iconname.indexOf('-');
				if (n==-1) return false;
				
				var iconset = iconname.substring(0,n);
				var iconfile = iconname.substring(n+1);
				
				var ics = this.iconsets[iconset];
				if (!ics) return false;
			
				if (!ics.icons) ics.icons = {};
				if (ics.icons[iconfile]) return true;
				
				if (jaxscript.isLoaded()) {
					var url = Icon.path+'/iconsets/'+iconset+'/'+iconfile+'.'+ics.ext;
					style.createRule('SPAN.'+iconset+'-'+iconfile,'background-image:url('+url+')',this.stylesheet);
					ics.icons[iconfile] = url;
					//echo('Icon '+iconname+' loaded');
					return true;
				}
				else jaxscript.run(function() {
					Icon.add(iconname);
				});
				return false;
			},
			getURL : function(iconname) {
				if (this.add(iconname)) {
					var n = iconname.indexOf('-');
					var iconset = iconname.substring(0,n);
					var iconfile = iconname.substring(n+1);
					return this.iconsets[iconset].icons[iconfile];
				}
				return '';
			},
			createFor : function(elm) {
				var cs = elm.className.split(' ');
				for (i=0;i<cs.length;i++) {
					Icon.add(cs[i]);
				}
			},
			init : function(elm) {
				if (!elm) elm = document.body;
				if (isIcon(elm)) this.createFor(elm);
				else {
					var nodes = elm.getElementsByTagName('span');
					for (var i=0;i<nodes.length;i++) {
						if (isIcon(nodes[i])) this.createFor(nodes[i]);
					}
				}
			}
		};
	})();
	
	jaxscript.run(function() {
		Icon.init();
	});

}