/* Nohros framework 0.0.1 - Seven module
 *
 * Copyright (c) 2008 Nohros Systems, Inc.(www.nohros.com)
 * $Date: 2009-01-22 - GMT-03:00 $
 *
 * 2009-01-22 - nohros
 */
nohros.extend({
    sevens: {
        // process a AJAX response that comes from the Nohros framework library.
        process: function(res, container) {
            // if the error member does not exists a unknown error has been occured
            if(typeof res.error == undefined )
                return nohros.error("process", "Unknown");
                
            // let the user known the cause of the error
            if(res.error != null && res.error.length > 0)
                return nohros.error("process", res.error);
            
            if(res.length < 1)
                return;
            
            // start the processing
            var k, actions = res.actions;
            for(k=0; k<res.length; k++) {
                exec(res[k]);
            }
        },
    
        exec: function(action) {
            var unbusy = true;
            var b = false;
            switch(action.type)
            {
                case "setattr":
                    var expr = action.selector;
                    jQuery(expr).attr(action.attribute, action.value);
                    break;
                    
                case "setcss":
                    var expr = action.selector;
                    jQuery(expr).css(action.cssattr, action.value);
                    break;
                    
                case "settimeout":
                    var delay = parseInt(action.timeout);
                    if(isNaN(delay))
                        nh_showError(evalAction+"->settimeout", "Invalid argument args");
                    else {
                        setTimeout( action.functiontocall, delay );
                        unbusy = false;
                    }
                    break;
                    
                case "appendmkp":
                    var expr = action.selector;
                    jQuery(expr).append(action.markup);
                    break;
                    
                case "prependmkp":
                    var expr = action.selector;
                    jQuery(expr).prepend(action.markup);
                    break;
                    
                case "aftermkp":
                    var expr = action.selector;
                    jQuery(expr).after(action.markup);
                    break;
                    
                case "setval":
                    var expr = action.selector;
                    jQuery(expr).val(action.markup);
                    break;
                    
                case "settext":
                    var expr = action.selector;
                    jQuery(expr).text(action.markup);
                    break;
                    
                case "sethtml":
                    var expr = action.selector;
                    jQuery(expr).html(action.markup);
                    break;
                    
                case "showinfo":
                    nh_showMessage(action.title, action.message);
                    unbusy = false;
                    break;
                    
                case "replacemkp":
                    var expr = action.selector;
                    jQuery(expr).replaceWith(action.markup);
                    unbusy = false;
                    break;
                    
                case "filltable":
                    b = true;
                    
                case "fillintable":
                    var expr = action.selector;
                    var c = action.constant;
                    var dt = eval(action.markup);
                    var html = "";
                    var etd = "";
                    var i=0;
                    var j=0;
                    var k=0;
                    var l=0;
                    if(c.length > 0)
                    {
                        c = "<td>" + c + "</td>";
                    }
                    for(i=0, j=dt.length;i<j;i++)
                    {
                        var d = dt[i];
                        html += "<tr>";
                        for(k=0,l=d.length;k<l;k++)
                            html += "<td>"+ d[k] + "</td>";
                        html += c + "</tr>";
                    }
                    if(b)
                        jQuery(expr).html(html);
                    else
                        jQuery(expr).append(html);
                    
                    if(nhi_showprg)
                        nh_unshowProgress();
                    break;
                    
                case "resetfrm":
                    var expr = action.selector;
                    var frm = jQuery(expr).get(0);
                    if(frm.tagName == "FORM");
                        frm.reset();
                    break;
                    
                case "focus":
                    var expr = action.selector;
                    var $elm = jQuery(expr);

                    if(action.set == "true")
                        $elm.focus();
                    else
                        $elm.blur();
                    break;
                    
                case "trigger":
                    var expr = action.selector;
                    var evt = action.evt;
                    jQuery(expr).trigger(evt);
                    break;
                    
                case "debugger":
                    debugger;
                    break;
                    
                case "redirect":
                    var expr = action.selector;
                    var url = action.url;
                    document.location.href=url;
                    break;
            }

            return unbusy;
        }
    }
});

// process any ajax-non-ajax generated response
jQuery(document).ready(function() {
    if(typeof(s_event) != 'undefined')
        nohros.sevens.process(s_event);
});