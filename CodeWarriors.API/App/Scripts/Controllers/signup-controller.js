var app = angular.module('codeWarriors.controllers.signup', ['codeWarriors.services.user']);

app.controller('SignUpCtrl', [
    '$scope',
    function ($scope) {
        $scope.status = {
            isFirstOpen: true
        };
    }
]);

app.controller('SignUpPostCtrl', [
    '$scope',
    'SignupService',
    '$window', function ($scope, SignupService, $window) {

        if ($window.sessionStorage.token) {
            $location.path('/posts');
        }

        $scope.userModel = {};
        $scope.alerts = [];
        $scope.isDisabled = false;

        $scope.signup = function () {
            $scope.isDisabled = true;
            SignupService.insertUser($scope.userModel).then(function (response) {
                //console.log(response);
                if (response.status === 200) {
                    $scope.isDisabled = false;
                    $scope.signupForm.$setPristine();
                    $scope.userModel = {};
                    $scope.alerts.push({
                        type: 'success',
                        msg: 'Sign up for Code Warriors is successfully done'
                    });
                    //$location.path("/login");
                }
            }, function (reason) {
                if (reason.status === 400) {
                    $scope.isDisabled = false;
                    $scope.alerts.push({
                        type: 'danger',
                        msg: 'Email ' + $scope.userModel.Email + ' is already Taken' //reason.data.ModelState[""][0]
                    });
                }
            });
        };

        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };
    }
]);