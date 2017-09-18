(function (window, angular) {
    'use strict';

    //hub
    var hub;

    //app run
    var appRun = function (Hub) {

        var hub = new Hub('BatchProxyHub',
            {
                listners: {
                    'refresh': function () { }
                },
                errorHandler: function () { },
                rootPath: ''
            });
    };

    //controllers
    var homeCtrl = function ($scope, batchProxyService) {

        $scope.model = {
            source: {
                input: undefined,
                directories: [],
                files: [],
                valid: false
            },
            destination: {
                input: undefined,
                directories: [],
                files: [],
                valid: false
            }
        };

        $scope.controller = {
            addBatch: function () { },
            addBatchEnabled: function() {
                return $scope.model.source.valid && $scope.model.destination.valid; 
            }
        };

        $scope.$watch('model.source.input', function (newval, oldval) {
            if (newval) {
                $scope.model.source.valid = false;

                if (newval.indexOf(':\\') > 0 &&
                    newval.length >= 3) {

                    batchProxyService.getDirectoryInfo(newval).then(function (response) {
                        if (response.data !== '') {
                            $scope.model.source.valid = true;
                            $scope.model.source.directories = response.directories;
                            $scope.model.source.files = response.files;
                        }
                    });
                }
            }
        });

        $scope.$watch('model.destination.input', function (newval, oldval) {
            if (newval) {

                $scope.model.destination.valid = false;

                if (newval.indexOf(':\\') > 0 &&
                    newval.length >= 3) {

                    batchProxyService.getDirectoryInfo(newval).then(function (response) {
                        if (response.data !== '') {
                            $scope.model.destination.valid = true;
                            $scope.model.destination.directories = response.directories;
                            $scope.model.destination.files = response.files;
                        }
                    });
                }
            }
        });
    };

    //factories
    var batchProxyService = function ($http, $window) {
        var rootUrl = $window.location.origin;
        return {
            getDirectoryInfo: function (path) {
                return $http({
                    url: rootUrl + '/Service/GetDirectoryInfo',
                    method: 'get',
                    params: { path: path }
                }).then(function (response) {
                    var data = response.data;
                    return {
                        directories: data.directories,
                        files: data.files
                    };
                });
            }
        }
    }

    //directives
    var autoComplete = function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attributes, ctrl) {
                if (!ctrl) return;

                debugger;

                console.log('scope = ' + { scope: scope });
                console.log('element = ' + element);
                console.log('attributes = ' + attributes);
                console.log('ctrl = ' + ctrl);
                var elem = element[0];
                elem.autocomplete({
                    source: scope[attributes.items],
                    select: function () {
                        $timeout(function () {
                            element.trigger('input');
                        });
                    }
                });
            }
        }
    }

    //angular app
    angular.module('BatchProxy', ['ngRoute', 'SignalR'])
        //.config([])
        //.run(['Hub', appRun])
        .controller('HomeCtrl', ['$scope', 'batchProxyService', homeCtrl])
        .factory('batchProxyService', ['$http', '$window', batchProxyService])
        //.directive('autoComplete', ['$timeout', autoComplete])
        ;


})(window, window.angular);

