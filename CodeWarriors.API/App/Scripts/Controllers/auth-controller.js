var app = angular.module('codeWarriors.controllers.auth', ['codeWarriors.services.auth']);

app.controller('LoginCtrl', [
    '$scope',
    '$rootScope',
    'AuthService',
    '$window',
    '$location', function ($scope, $rootScope, AuthService, $window, $location) {
        /*if ($window.sessionStorage.token) {
            $location.path('/posts');
        }*/

        $scope.status = {
            isFirstOpen: true
        };
        $scope.userModel = {};
        $scope.alerts = [];
        $scope.isDisabled = false;

        $scope.login = function () {
            $scope.isDisabled = true;
            AuthService.login($scope.userModel).then(function (response) {
                if (response.status == 200) {
                    $window.sessionStorage.token = response.data.access_token;
                    $window.sessionStorage.userName = response.data.userName;
                    $rootScope.userName = response.data.userName;
                    $rootScope.isAuthenticate = true;

                    $location.path('/posts').replace();
                    $location.path('/posts');
                }
                $scope.isDisabled = false;
            }, function (reason) {
                delete $window.sessionStorage.token;
                delete $window.sessionStorage.userName;
                delete $rootScope.userName;

                $rootScope.isAuthenticate = false;

                $scope.alerts.push({
                    type: 'danger',
                    msg: 'Username and password is incorrect'
                });
                $scope.isDisabled = false;
            });
        }

        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };

        $scope.logout = function () {
            AuthService.logout().then(function (response) {
                delete $window.sessionStorage.token;
                delete $rootScope.userName;
                $rootScope.isAuthenticate = false;

                $location.path('/login').replace();
                $location.path('/login');
            });
        }
    }
]);