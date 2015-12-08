var app = angular.module('codeWarriors', [
	'ngRoute',
    'ngCookies',
	'ngResource',
	'ui.bootstrap',
	'angularMoment',
    'angular-loading-bar',
    'SignalR',
    'toaster',
	'codeWarriors.directives.userValidation',
	'codeWarriors.controllers.user',
	'codeWarriors.controllers.post',
	'codeWarriors.controllers.signup',
	'codeWarriors.controllers.auth'
]);

app.factory('httpRequestInterceptor', [
    '$q',
    '$location',
    function ($q, $location) {
        return {
            'responseError': function (rejection) {
                return $q.reject(rejection);
            }
        };
    }
]);

app.factory('authInterceptor', [
    '$rootScope',
    '$q',
    '$window',
    '$location',
    function ($rootScope, $q, $window, $location) {
        return {
            request: function (config) {
                config.headers = config.headers || {};
                if ($window.sessionStorage.token) {
                    config.headers.Authorization = 'Bearer ' + $window.sessionStorage.token;
                } else {
                    if ($location.$$url == '/signup') {
                        $location.path('/signup');
                    }
                    else {
                        $location.path('/login');
                    }
                }
                return config;
            },
            response: function (response) {
                if (response.status === 401) {
                    // handle the case where the user is not authenticated
                }
                return response || $q.when(response);
            }
        };
    }
]);

//app.service('notification', ['$rootScope', 'Hub', function ($rootScope, Hub) {
//    var hub = new Hub('friend', {
//        'showFriendRequest': function (email) {
//            console.log('!!!!!!!!!!');
//            console.log(email);
//            if ($rootScope.userName == email) {
//                $rootScope.friendRequest++;
//            }
//            $rootScope.$apply();
//        }
//    }, ['showFriendRequest']);

//    this.sendRequest = function () {
//        hub.Send(); //Calling a server method
//    };
//}]);

//app.controller('SignalRAngularCtrl', function ($scope, notification, $rootScope) {
//    $scope.text = "";

//    $scope.greetAll = function () {
//        notification.sendRequest();
//    }
//});

app.config([
    '$routeProvider',
    '$interpolateProvider',
    '$httpProvider',
    function ($routeProvider, $interpolateProvider, $httpProvider) {
        $httpProvider.interceptors.push('httpRequestInterceptor');
        $httpProvider.interceptors.push('authInterceptor');

        $routeProvider
        .when('/posts', {
            templateUrl: 'Views/Post/list.html',
            controller: 'PostListCtrl'
        })
        .when('/users', {
            templateUrl: 'Views/User/list.html',
            controller: 'UserListCtrl'
        })
        .when('/user/:userId', {
            templateUrl: 'Views/Post/list.html',
            controller: 'PostListCtrl'
        })
        .when('/signup', {
            templateUrl: 'Views/signup.html',
            controller: 'SignUpCtrl'
        })
        .when('/friends', {
            templateUrl: 'Views/User/friendsList.html',
            controller: 'FriendListCtrl'
        })
        .when('/friend-requests', {
            templateUrl: 'Views/User/friendRequestsList.html',
            controller: 'FriendRequestListCtrl'
        })
        .when('/login', {
            templateUrl: 'Views/login.html',
            controller: 'LoginCtrl'
        })
        .when('/logout', {
            controller: 'LogoutCtrl'
        })
        .otherwise({
            redirectTo: '/users'
        });
    }
]);

app.run([
    '$rootScope',
    '$window',
    function ($rootScope, $window) {
        $rootScope.offset = 0;
        $rootScope.pageSize = 1;
        $rootScope.friendRequest = 0;

        if ($window.sessionStorage.token) {
            $rootScope.isAuthenticate = true;
            $rootScope.userName = $window.sessionStorage.userName;
        }
        else {
            $rootScope.isAuthenticate = false;
            delete $rootScope.userName;
        }
    }
]);