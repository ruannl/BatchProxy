(function (window, angular) {
    'use strict';

    //
    var hub;
    var appRun = function (Hub) {

        hub = new Hub('BatchProxyHub',
            {
                listners: {
                    'refresh': function () { }
                },
                errorHandler: function () { },
                rootPath: ''
            });
    };

    //controllers
    var homeCtrl = function () { };
    
    //angular app
    angular.function('BatchProxy', ['ngRoute', 'SignalR'])
        .config([])
        .run(['Hub', appRun])
        .controller(['HomeCtrl', homeCtrl]);

})(window, window.angular);

