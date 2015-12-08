var app = angular.module('codeWarriors.controllers.user', ['codeWarriors.services.user']);

app.controller('UserListCtrl', [
    '$scope',
    '$rootScope',
    'toaster',
    'UserService', function ($scope, $rootScope, toaster, UserService) {

        console.log($rootScope);

        $rootScope.user = {};
        UserService.searchUser($rootScope.searchByName).then(function (response) {
            $rootScope.user.list = response.data;
        });

        $scope.sendFriendRequest = function (userId, index) {
            UserService.friendRequest(userId).then(function (response) {
                if (response.data == "true") {
                    $rootScope.user.list.splice(index, 1);
                    toaster.pop('success', "Friend Request", "Your friend request is successfully done");
                } else {
                    toaster.pop('error', "Friend Request", "Your friend request is already done");
                }
            });
        }

        $scope.searchUser = function () {
            UserService.searchUser($scope.user.searchByName).then(function (response) {
                $scope.user.list = response.data;
            });
        }
    }
]);

app.controller('UserHomeCtrl', ['$scope', '$routeParams', function ($scope, $routeParams) {


}]);

app.controller('FriendListCtrl', [
    '$scope',
    'toaster',
    '$rootScope',
    'UserService', function ($scope, toaster, $rootScope, UserService) {
        
        $scope.friend = {};
        UserService.getFriends().then(function (response) {
            $scope.friend.list = response.data;
        });

        $scope.removeFriend = function (userId, friendId, index) {
            UserService.removeFriend(userId, friendId).then(function (response) {
                if (response.data == "true") {
                    toaster.pop('success', "Friend Remove", $scope.friend.list[index].FirstName+" is removed from your friend list");
                    $scope.friend.list.splice(index, 1);
                } else {
                    toaster.pop('error', "Friend Remove", "Please contact with bit book");
                }
            });
        }
    }
]);

app.controller('FriendRequestListCtrl', [
    '$scope',
    'toaster',
    '$rootScope',
    'UserService', function ($scope, toaster, $rootScope, UserService) {

        $rootScope.friendRequest = 0;
        $scope.friendRequest = {};
        UserService.getFriendRequests().then(function (response) {
            $scope.friendRequest.list = response.data;
        });

        $scope.acceptFriendRequest = function (userId, friendId, index) {
            console.log(index);
            UserService.acceptFriend(userId, friendId).then(function (response) {
                if (response.data == "true") {
                    toaster.pop('success', "Friend Accept", $scope.friendRequest.list[index].FirstName + " and you are now friends");
                    $scope.friendRequest.list.splice(index, 1);
                } else {
                    toaster.pop('error', "Friend Accept", "Please contact with bit book");
                }
            });
        }

        $scope.rejectFriendRequest = function (userId, friendId, index) {
            UserService.rejectFriend(userId, friendId).then(function (response) {
                if (response.data == "true") {
                    toaster.pop('success', "Friend Reject", "You reject "+$scope.friendRequest.list[index].FirstName);
                    $scope.friendRequest.list.splice(index, 1);
                } else {
                    toaster.pop('error', "Friend Reject", "Please contact with bit book");
                }
            });
        }
    }
]);

app.controller('UserSearchCtrl', [
    '$scope', '$rootScope', '$location', 'UserService', function ($scope, $rootScope, $location, UserService) {
        $scope.user = {};
        $scope.searchUser = function () {
            $rootScope.searchByName = $scope.searchByName;
            if ($location.$$url != '/users') {
                $location.path('/users').replace();
                $location.path('/users');
            } else {
                UserService.searchUser($rootScope.searchByName).then(function (response) {
                    $rootScope.user.list = response.data;
                });
            }
        }
    }
]);