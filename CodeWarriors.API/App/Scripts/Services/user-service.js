var app = angular.module('codeWarriors.services.user', []);

app.service('UserService', [
    '$http', function ($http) {
        this.searchUser = function (data) {
            return $http({
                url: '/api/user',
                method: "GET",
                params: { pageSize: 50, offset: 0, userName: data  }
            });
        };

        this.friendRequest = function (id) {
            return $http({
                url: '/api/friend/SendFriendRequest',
                method: "POST",
                params: { friendId: id }
            });
        }

        this.getFriends = function () {
            return $http({
                url: '/api/Friend/GetAllFriend',
                method: "GET"
            });
        }

        this.getFriendRequests = function () {
            return $http({
                url: '/api/Friend/GetAllFriendRequest',
                method: "GET"
            });
        }

        this.removeFriend = function (userId, friendId) {
            return $http({
                url: '/api/Friend/RemoveFriend',
                method: "POST",
                params: { userId: userId, friendId: friendId }
            });
        }

        this.acceptFriend = function (userId, friendId) {
            return $http({
                url: '/api/Friend/AcceptFriendRequest',
                method: "POST",
                params: { userId: userId, friendId: friendId }
            });
        }

        this.rejectFriend = function (userId, friendId) {
            return $http({
                url: '/api/Friend/RejectFriendRequest',
                method: "POST",
                params: { userId: userId, friendId: friendId }
            });
        }
    }
]);

app.service('SignupService', [
    '$http', function ($http) {
        this.insertUser = function (userModel) {
            return $http.post('/api/account/register', userModel);
        };
    }
]);
