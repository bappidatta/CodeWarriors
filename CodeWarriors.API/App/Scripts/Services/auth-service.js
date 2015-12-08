var app = angular.module('codeWarriors.services.auth', []);

app.service('AuthService', [
    '$http', function ($http) {
        this.login = function (userModel) {
            var xsrf = "grant_type=password&username=" + userModel.Email + "&password=" + userModel.Password;
            return $http({
                method: 'POST',
                url: '/token',
                data: xsrf,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            });
        }

        this.logout = function () {
            return $http.post('/api/account/logout');
        }
    }
]);