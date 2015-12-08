var app = angular.module('codeWarriors.controllers.post', [
	'codeWarriors.services.post',
    'codeWarriors.services.signalr',
	'textAngular',
	'tagger'
]);

app.controller('PostListCtrl', [
    '$scope',
    '$rootScope',
    '$routeParams',
    'PostService',
    'SignalRService',
    'toaster',
    '$window',
    '$location',
    function ($scope, $rootScope, $routeParams, PostService, SignalRService, toaster, $window, $location) {

        SignalRService.initialize();
        
        $rootScope.post = {};
        $rootScope.post.list = [];
        $scope.pageList = [];
        var pagingQuery = {};
        $rootScope.likedUserList = "";

        PostService.getAllPosts().then(function (response) {
            $rootScope.post.list = response.data;
            for (i in $rootScope.post.list) {
                for (k in $rootScope.post.list[i].LikedUserName) {
                    $scope.likedUserList += $rootScope.post.list[i].LikedUserName[k].FirstName + " " + $rootScope.post.list[i].LikedUserName[k].LastName + ";";
                }
            }
        });

        $scope.postModel = {};

        $scope.postSubmit = function () {
            $scope.postModel.CreatedTime = new Date().getTime();

            PostService.insertPost($scope.postModel).then(function (response) {
                //console.log(response);
                if (response.status === 201) {
                    $scope.postSubmitForm.$setPristine();
                    $scope.postModel.PostDetails = "";
                    $rootScope.post.list.unshift(response.data);
                }
            });
        };

        $scope.commentSubmit = function () {
            $scope.comment.CreatedTime = new Date().getTime();
            
        }

        $scope.removePost = function (userId, postId, index) {
            PostService.removePost(postId, userId).then(function (response) {
                if (response.status == 200) {

                    $rootScope.post.list.splice(index, 1);
                    toaster.pop('success', "Status Remove", "Your status is successfully deleted");
                }
            });
        }

        $scope.hidePost = function (postId, index) {
            PostService.hidePost(postId).then(function (response) {
                if (response.status == 200) {

                    $rootScope.post.list.splice(index, 1);
                    toaster.pop('success', "Status Hide", "Your status is successfully hiden");
                }
            });
        }

        $scope.likePost = function (userId, postId, index) {
            PostService.likePost(postId).then(function (response) {
                if (response.status == 200) {
                    toaster.pop('success', "Like Post", "You like the post");
                }
            });
        }

        $scope.unLikePost = function (userId, postId, index) {
            PostService.unLikePost(postId).then(function (response) {
                if (response.status == 200) {
                    toaster.pop('success', "Un Like Post", "You un like the post");
                }
            });
        }
    }
]);

app.controller('CommentSubmitCtrl', [
    '$scope',
    '$rootScope',
    '$location',
    'PostService',
    '$window',
    function ($scope, $rootScope, $location, PostService, $window) {
        
        $scope.commentSubmit = function (postId, index) {
            
            $scope.commentModel.PostId = postId;
            $scope.commentModel.CreatedTime = new Date().getTime();

            PostService.insertComment($scope.commentModel).then(function (response) {
                //console.log(response);
                if (response.status === 201) {

                    $scope.commentModel.CommentDetails = "";
                    console.log($rootScope.post.list[index].Comments);
                    if ($rootScope.post.list[index].Comments) {
                        $rootScope.post.list[index].Comments.push(response.data);
                    }
                    
                    //$scope.postModel.PostDetails = "";
                    //$scope.post.list.push(response.data);
                }
            });
        };
    }
]);

app.controller("PostAnswerCtrl", [
    '$scope',
    function ($scope) {
        $scope.postAnswer = function () {
            console.log($scope.questionModel);
        };
    }
]);