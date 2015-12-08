var app = angular.module('codeWarriors.services.signalr', ['codeWarriors.services.post']);
app.service('SignalRService', function ($, $rootScope, toaster, PostService) {
    var proxy = null;

    var initialize = function () {
        //Getting the connection object
        connection = $.hubConnection();

        //Creating proxy
        this.proxy = connection.createHubProxy('friend');

        //Publishing an event when server pushes a greeting message
        this.proxy.on('showFriendRequest', function (email) {
            console.log(email);
            if ($rootScope.userName == email) {
                toaster.pop('success', "Notification", "You have " + ++$rootScope.friendRequest + "Friend Request. <a href='/friend-request'>Take a look</a>");
            }
            //console.log($rootScope.friendRequest);
            $rootScope.$emit("showFriendRequest", email);
        });

        this.proxy.on('showUpdatedPost', function () {
            
            PostService.getAllPosts().then(function (response) {
                $rootScope.post.list = response.data;
                for (i in $rootScope.post.list) {
                    for (k in $rootScope.post.list[i].LikedUserName) {
                        $rootScope.likedUserList += $rootScope.post.list[i].LikedUserName[k].FirstName + " " + $rootScope.post.list[i].LikedUserName[k].LastName + ";";
                    }
                }
            });
        });

        //Starting connection
        connection.start();
    };

    var sendRequest = function () {
        //Invoking greetAll method defined in hub
        this.proxy.invoke('Send');
    };

    return {
        initialize: initialize,
        sendRequest: sendRequest
    };
});