/* Nohros Framework 0.0.1 - Javascript module
 *
 * Copyright (c) 2008 Nohros Systems, Inc.(www.nohros.com)
 * $Date: 2009-01-22 - GMT-03:00 $
 *
 * 2009-01-22 - nohros
 */
(function() {

var
    // will speed up references to window, and allows munging its name
    window = this,
    // will speed up references to undefined, and munging its name.
    undefined,
    
    nohros = function() {
        // the nohros object is actually just the init constructor 'enhanced'
        return new nohros.fn.init();
    };

nohros.fn = nohros.prototype = {
    init: function() {
    },
       
    error: function(msg) {
        alert(msg);
    },
    
    info: function(msg) {
        alert(msg);
    },
    
    // the current version of nohros beign used
    version: "0.0.1"
};

nohros.extend = nohros.fn.extend = function() {
    // copy reference to target object
    var i = 0, length = arguments.length, options;
    
    // extend the nohros object
    for( ; i < length; i++) {
        // only deal with non-null/undefined values
        if( (options = arguments[i]) != null ) {
            // extend the base object
            for( var name in options ) {
                var src = this[ name ], copy = options[ name ];
                
                if( copy !== undefined )
                    this[ name ] = copy;
            }
        }
    }
    
    return this;
};

// give the init function the nohros prototype for later instantiation
nohros.fn.init.prototype = nohros.fn;

// creates an instance of the nohros class
window.nohros = nohros();

})();