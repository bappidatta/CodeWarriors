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
    /// This class is responsible for send friend request, accept friend friend request and all friend related activities
    /// </summary>
    [System.Web.Http.Authorize]
    public class FriendController : ApiController
    {
        // Declare Business Logic Class
        private IFriendBLL friendBLL;
        private IUserBLL userBLL;

        // Declare HubContext
        protected readonly Lazy<IHubContext> friendHub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<FriendHub>());

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="friendBLL">Business Logic Class of Friend</param>
        /// <param name="userBLL">Business Logic Class of User</param>
        public FriendController(IFriendBLL friendBLL, IUserBLL userBLL)
        {
            this.friendBLL = friendBLL;
            this.userBLL = userBLL;
        }

        /// <summary>
        /// Get all friend of Logged User
        /// </summary>
        /// <returns>All friends</returns>
        [Route("api/Friend/GetAllFriend")]
        public IEnumerable<FriendViewModel> GetAllFriend()
        {
            var user = userBLL.GetUserByUserName(User.Identity.Name);

            var friendList = friendBLL.GetAllFriendsByUser(user.Id);

            return friendList;
        }

        /// <summary>
        /// Get all friend request of logged user
        /// </summary>
        /// <returns>All Requests</returns>
        [Route("api/Friend/GetAllFriendRequest")]
        public IEnumerable<FriendViewModel> GetAllFriendRequest()
        {
            var user = userBLL.GetUserByUserName(User.Identity.Name);

            var friendList = friendBLL.GetAllFriendRequestsByUser(user.Id);

            return friendList;
        }

        /// <summary>
        /// Send friend request to a user
        /// </summary>
        /// <param name="friendId">Id of User who will receive friend request</param>
        /// <returns></returns>
        [Route("api/Friend/SendFriendRequest")]
        public Boolean SendFriendRequest(string friendId)
        {
            var user = userBLL.GetUserByUserName(User.Identity.Name);

            bool flag = false;

            if (user.Id != friendId)
            {
                if (!friendBLL.IsExistsFriend(user.Id, friendId))
                {
                    flag = friendBLL.SendFriendRequest(user.Id, friendId);

                    var friend = userBLL.GetUserById(friendId);

                    // Send Notification using SignalR
                    friendHub.Value.Clients.All.showFriendRequest(friend.Email);
                }
            }
            return flag;
        }

        /// <summary>
        /// Accept friend request
        /// </summary>
        /// <param name="userId">UserId of Friend</param>
        /// <param name="friendId">User Id of logged User</param>
        /// <returns></returns>
        [Route("api/Friend/AcceptFriendRequest")]
        public Boolean AcceptFriendRequest(string userId, string friendId)
        {
            var flag = friendBLL.AcceptFriendRequest(userId, friendId);

            return flag;
        }

        /// <summary>
        /// Reject frined request
        /// </summary>
        /// <param name="userId">UserId of Friend</param>
        /// <param name="friendId">User Id of logged User</param>
        /// <returns></returns>
        [Route("api/Friend/RejectFriendRequest")]
        public Boolean RejectFriendRequest(string userId, string friendId)
        {
            var flag = friendBLL.RejectFriendRequest(userId, friendId);

            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">User Id of a User</param>
        /// <param name="friendId">User Id of a User</param>
        /// <returns></returns>
        [Route("api/Friend/RemoveFriend")]
        public Boolean RemoveFriend(string userId, string friendId)
        {
            var flag = friendBLL.RemoveFriend(userId, friendId);

            return flag;
        }

        /// <summary>
        /// Get Mutual Friend of Two friends
        /// </summary>
        /// <param name="userId">User Id of User one</param>
        /// <param name="friendId">User Id of User two</param>
        /// <returns></returns>
        [Route("api/Friend/GetMutualFriends")]
        public IEnumerable<FriendViewModel> GetMutualFriends(string userId, string friendId)
        {
            var friendList = friendBLL.GetMutualFriend(userId, friendId);

            return friendList;
        }

        //[Route("api/Friend/GetFriendSuggestion")]
        //public IEnumerable<FriendViewModel> GetFriendSuggestion(string userId)
        //{
        //    var friendList = friendBLL.GetFriendSuggestion(userId);

        //    return friendList;
        //}

    }
}
