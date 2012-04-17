/*
*  JaxCore Button Component v1.0
*
*   A free product by JaxCore - visit us at http://www.jaxcore.com
*   Released under the CreativeCommons 2.5 license (http://creativecommons.org/licenses/by/2.5/)
*   You are free to share, change, or redistribute this work so long as this notice remains
*/

if (typeof Button=='undefined') {

    var Button = (function() {

        defineType('Button', 'JaxCore Button Component', function(o) {
            return o.nodeName == 'A' && (dom.hasClass(o, 'button_large') || dom.hasClass(o, 'button') || dom.hasClass(o, 'button_small') || dom.hasClass(o, 'button_bullet') || dom.hasClass(o, 'button_compact'));
        });

        var path = dom.getScriptPath('jaxcore-button.js');
        if (path) dom.loadCSS(path + '/jaxcore-button.css');

        return {
            defaultColor: '#E2E4E8',
            defaultTextColor: '#000000',
            disable: function(n) {
                n = dom.id(n);
                if (!dom.hasClass(n, 'button_disabled')) dom.addClass(n, 'button_disabled');
            },
            enable: function(n) {
                n = dom.id(n);
                if (dom.hasClass(n, 'button_disabled')) dom.removeClass(n, 'button_disabled');
            },
            isEnabled: function(n) {
                n = dom.id(n);
                return !this.isDisabled(n);
            },
            isDisabled: function(n) {
                n = dom.id(n);
                return dom.hasClass(n, 'button_disabled');
            },
            mousedownHandler: function(e) {
                var src = dom.eventTarget(e);
                if (src.nodeName != 'A') src = dom.findParent(src, 'A');
                echo('Button mousedown ' + src.nodeName + '.' + src.className + ((src.id) ? '#' + src.id : ''));

                if (!dom.hasClass(src, 'button_disabled')) {
                    dom.replaceClass(src, 'button', 'button_down');
                    dom.addEvent(src, 'mouseup', Button.mouseupHandler, false);
                    dom.addEvent(src, 'mouseout', Button.mouseupHandler, false);
                }
                src.focus();
                e.cancelBubble = true;
                e.returnValue = false;
                if (e.preventDefault) e.preventDefault();
                return false;
            },
            mouseupHandler: function(e) {
                var src = dom.eventTarget(e);
                if (src.nodeName != 'A') src = dom.findParent(src, 'A');
                echo('Button mouseup ' + src.nodeName + '.' + src.className + ((src.id) ? '#' + src.id : ''));

                dom.replaceClass(src, 'button_down', 'button');

                dom.removeEvent(src, 'mouseup', Button.mouseupHandler, false);
                dom.removeEvent(src, 'mouseout', Button.mouseupHandler, false);

                return false;
            },
            rollover: function(n, bgColor, textColor) {
                n._btn_bgcolor = n.style.backgroundColor;
                n._btn_color = style.get(n, 'color');
                if (bgColor) n.style.backgroundColor = bgColor;
                if (textColor) n.style.color = textColor;
            },
            rollout: function(n) {
                n.style.backgroundColor = '#eeeeee';
                if (n._btn_color) n.style.color = n._btn_color;
            },
            init: function(elm) {
                if (!elm) elm = document.body;

                var b, i, nodes;
                if (isButton(elm)) nodes = [elm];
                else nodes = elm.getElementsByTagName('a');

                for (i = 0; i < nodes.length; i++) {
                    b = nodes[i];
                    if (isButton(b)) {
                        dom.addEvent(b, 'mousedown', Button.mousedownHandler, false);
                    }
                }
            }
        };
    })();
	
	jaxscript.run(function() {
		Button.init();
	});

}