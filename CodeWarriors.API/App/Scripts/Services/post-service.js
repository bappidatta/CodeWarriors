var app = angular.module('codeWarriors.services.post', []);

app.service('PostService', [
    '$http', '$routeParams', function ($http, $routeParams) {
        this.getAllPosts = function () {
            if ($routeParams.userId) {
                return $http({
                    url: '/api/post',
                    method: "GET",
                    params: { userId: $routeParams.userId }
                });
            } else {
                return $http({
                    url: '/api/post',
                    method: "GET",
                    //params: { pageSize: pagingQuery.pageSize, offset: pagingQuery.offset }
                });
            }
            
        };

        /*this.getQuestionAnswer = function (questionId) {
            return $http({
                url: '/api/question',
                method: "GET",
                params: { Id: questionId }
            });
        };*/

        this.insertPost = function (questionModel) {
            return $http.post('/api/post', questionModel);
        };

        this.insertComment = function (commentModel) {
            return $http.post('/api/post/comment', commentModel);
        };

        this.removePost = function (postId, userId) {
            return $http({
                url: '/api/post',
                method: "DELETE",
                params: { postId: postId, userId: userId }
            });
        }

        this.hidePost = function (postId) {
            return $http({
                url: '/api/post/hide',
                method: "POST",
                params: { postId: postId }
            });
        }

        this.likePost = function (postId) {
            return $http({
                url: '/api/post/like',
                method: "POST",
                params: { postId: postId }
            });
        }

        this.unLikePost = function (postId) {
            return $http({
                url: '/api/post/unlike',
                method: "POST",
                params: { postId: postId }
            });
        }
    }
]);