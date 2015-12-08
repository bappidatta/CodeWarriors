using CodeWarriors.API.Hubs;
using CodeWarriors.BLL.Interfaces;
using CodeWarriors.BLL.ViewModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CodeWarriors.API.Controllers
{
    /// <summary>
    /// This Clas is responsible for Post Related activities such as create post, add comment, delete post, hide post etc.
    /// </summary>
    [System.Web.Http.Authorize]
    public class PostController : ApiController
    {
        // Declare Business Logic Class
        private IPostBLL postBLL;
        private IUserBLL userBLL;

        // Declare HubContext
        protected readonly Lazy<IHubContext> friendHub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<FriendHub>());

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="postBLL"></param>
        /// <param name="userBLL"></param>
        public PostController(IPostBLL postBLL, IUserBLL userBLL)
        {
            this.postBLL = postBLL;
            this.userBLL = userBLL;
        }

        /// <summary>
        /// Get All Post of a User icluding Post of Friends and excluding Hidden Post
        /// </summary>
        /// <param name="userId">Id of a User</param>
        /// <returns></returns>
        public IEnumerable<PostViewModel> GetAllPosts(string userId = null)
        {
            if(userId == null)
            {
                var user = userBLL.GetUserByUserName(User.Identity.Name);
                userId = user.Id;
            }

            var postList = postBLL.GetAllPost(userId);

            return postList;
        }

        /// <summary>
        /// Create New Post
        /// </summary>
        /// <param name="postVM">PostViewModel</param>
        /// <returns></returns>
        public HttpResponseMessage PostPost(PostViewModel postVM)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;

                var user = userBLL.GetUserByUserName(userName);

                postVM.UserId = user.Id;

                string postId = postBLL.CreatePost(postVM);

                postVM = postBLL.GetPostByID(postId);

                var response = Request.CreateResponse(HttpStatusCode.Created, postVM);
                string uri = Url.Link("DefaultApi", new { id = postVM.Id });
                response.Headers.Location = new Uri(uri);

                // Send Notification using SignalR
                friendHub.Value.Clients.All.showUpdatedPost();

                return response;
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                string uri = Url.Link("DefaultApi", new { id = postVM.Id });
                response.Headers.Location = new Uri(uri);

                return response;
            }
        }

        /// <summary>
        /// Create New Comments to a Post
        /// </summary>
        /// <param name="commentVM">CommentViewModel</param>
        /// <returns></returns>
        [Route("api/Post/Comment")]
        public HttpResponseMessage PostComment(CommentViewModel commentVM)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;

                var user = userBLL.GetUserByUserName(userName);

                // Set Properties
                commentVM.UserId = user.Id;
                commentVM.FirstName = user.FirstName;
                commentVM.LastName = user.LastName;

                postBLL.AddComments(commentVM);

                // Send Notification using SignalR
                friendHub.Value.Clients.All.showUpdatedPost();

                return Request.CreateResponse(HttpStatusCode.Created, commentVM);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        /// <summary>
        /// Like a Post
        /// </summary>
        /// <param name="userId">Id of User</param>
        /// <param name="postId">Id of Post</param>
        /// <returns></returns>
        [Route("api/Post/Like")]
        public HttpResponseMessage PostLike(string postId)
        {
            if (postId == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var user = userBLL.GetUserByUserName(User.Identity.Name);

            postBLL.AddLike(user.Id, postId);

            // Send Notification using SignalR
            friendHub.Value.Clients.All.showUpdatedPost();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        /// <summary>
        /// UnLike a Post
        /// </summary>
        /// <param name="userId">Id of User</param>
        /// <param name="postId">Id of Post</param>
        /// <returns></returns>
        [Route("api/Post/UnLike")]
        public HttpResponseMessage PostUnLike(string userId, string postId)
        {
            if (userId == null || postId == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            postBLL.RemoveLike(userId, postId);

            // Send Notification using SignalR
            friendHub.Value.Clients.All.showUpdatedPost();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        /// <summary>
        /// Update a Post
        /// </summary>
        /// <param name="postVM">PostViewModel</param>
        /// <returns></returns>
        public bool PutPost(PostViewModel postVM)
        {
            if (ModelState.IsValid)
            {
                return postBLL.UpatePost(postVM);
            }

            return false;
        }

        /// <summary>
        /// Delete Post
        /// </summary>
        /// <param name="postId">Id of Post</param>
        /// <param name="userId">Id of User</param>
        /// <returns></returns>
        public HttpResponseMessage DeletePost(string postId, string userId)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;

                var user = userBLL.GetUserByUserName(userName);

                if (user.Id == userId)
                {
                    postBLL.DeletePost(postId);

                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    string uri = Url.Link("DefaultApi", new { id = postId });
                    response.Headers.Location = new Uri(uri);

                    return response;
                }
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
                    string uri = Url.Link("DefaultApi", new { id = postId });
                    response.Headers.Location = new Uri(uri);

                    return response;
                }
            }

            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Hide Post
        /// </summary>
        /// <param name="postId">Id of Post</param>
        /// <returns></returns>
        [Route("api/Post/Hide")]
        public bool HidePost(string postId)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;

                var user = userBLL.GetUserByUserName(userName);

                return postBLL.HidePost(postId, user.Id);
            }

            return false;
        }
    }
}
