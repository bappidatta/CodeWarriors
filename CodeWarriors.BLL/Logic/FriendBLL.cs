using CodeWarriors.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeWarriors.DAL.Model;
using CodeWarriors.DAL.Interfaces;
using CodeWarriors.DAL.Repositories;
using CodeWarriors.BLL.ViewModels;
using MongoDB.Driver.Linq;
using MongoDB.Bson;

namespace CodeWarriors.BLL.Logic
{
    public class FriendBLL : IFriendBLL
    {
        private IRepository<Friend> friendRepo = new Repository<Friend>("Friend");

        private IUserBLL userBLL;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userBLL">UserBLL</param>
        public FriendBLL(IUserBLL userBLL)
        {
            this.userBLL = userBLL;
        }

        /// <summary>
        /// Get All Friend By User
        /// </summary>
        /// <param name="id">Id of a User</param>
        /// <returns></returns>
        public List<FriendViewModel> GetAllFriendsByUser(string id)
        {
            var friendList = from s in friendRepo.Get()
                             where (s.UserId == id || s.FriendId == id ) && s.IsAccepted == true
                             select s;

            var friendUserIds = (from s in friendList
                                 where s.UserId != id
                                 select s.UserId.ToString()).ToList();

            friendUserIds = friendUserIds.Union(from s in friendList
                                                where s.FriendId != id
                                                select s.FriendId.ToString()).ToList();

            var friendUserNames = userBLL.GetUserById(friendUserIds);

            var friendListVM = (from s in friendList.AsEnumerable()
                                from c in friendUserNames
                                where (s.UserId == c.Id) || (s.FriendId == c.Id)
                                select new FriendViewModel
                                {
                                    UserId = s.UserId,
                                    FriendId = s.FriendId,
                                    FirstName = c.FirstName,
                                    LastName = c.LastName,
                                    IsAccepted = s.IsAccepted,
                                }).ToList();

            return friendListVM;
        }

        /// <summary>
        /// Get All Friend Requests By User
        /// </summary>
        /// <param name="id">Id of a User</param>
        /// <returns></returns>
        public List<FriendViewModel> GetAllFriendRequestsByUser(string id)
        {
            var friendList = from s in friendRepo.Get()
                             where s.FriendId == id && s.IsAccepted == false
                             select s;

            var friendUserIds = (from s in friendList
                                 select s.UserId).ToList();

            var friendUserNames = userBLL.GetUserById(friendUserIds);

            var friendListVM = (from s in friendList.AsEnumerable()
                                join c in friendUserNames on s.UserId equals c.Id
                                select new FriendViewModel
                                {
                                    UserId = s.UserId,
                                    FriendId = s.FriendId,
                                    FirstName = c.FirstName,
                                    LastName = c.LastName,
                                    IsAccepted = s.IsAccepted,
                                }).ToList();

            return friendListVM;
        }

        /// <summary>
        /// Get Mutual Friend
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public List<FriendViewModel> GetMutualFriend(string userId, string friendId)
        {
            var usersFriendList = (from s in friendRepo.Get()
                where (s.UserId == userId || s.FriendId == userId) && s.IsAccepted == true
                select (s.UserId == userId ? s.FriendId : s.UserId).ToString()).ToList();

            var friendsFriendList = (from s in friendRepo.Get()
                where (s.UserId == friendId || s.FriendId == friendId) && s.IsAccepted == true
                select (s.UserId == friendId ? s.FriendId : s.UserId).ToString()).ToList();

            var friendUserIds = usersFriendList.Intersect(friendsFriendList).ToList();

            var friendUserNames = userBLL.GetUserById(friendUserIds);

            var friendListVM = from c in friendUserNames
                                select new FriendViewModel
                                {
                                    UserId = c.Id,
                                    FriendId = userId,
                                    FirstName = c.FirstName,
                                    LastName = c.LastName,
                                    IsAccepted = true,
                                };

            return friendListVM.ToList();
        }

        /// <summary>
        /// Check that a user is alredy friend or a friend request have alresdy sent.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public bool IsExistsFriend(string userId, string friendId)
        {
            var friend = (from s in friendRepo.Get()
                         where (s.UserId == userId && s.FriendId == friendId) 
                         || (s.UserId == friendId && s.FriendId == userId)
                         select s).FirstOrDefault();

            if (friend != null)
                return true;

            return false;
        }

        /// <summary>
        /// Send Friend Request
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public bool SendFriendRequest(string userId, string friendId)
        {
            Friend friend = new Friend
            {
                UserId = userId,
                FriendId = friendId,
                IsAccepted = false
            };

            friendRepo.Add(friend);

            return true;
        }

        /// <summary>
        /// Accept Friend Request
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public bool AcceptFriendRequest(string userId, string friendId)
        {
            var friendRequests = (from s in friendRepo.Get()
                                 where (s.UserId == userId && s.FriendId == friendId)
                                 || (s.FriendId == userId && s.UserId == friendId)
                                 select new Friend()
                                        {
                                           Id = s.Id,
                                           FriendId = s.FriendId,
                                           UserId = s.UserId,
                                           IsAccepted = true
                                        });

            friendRepo.UpdateAll(friendRequests.ToList<Friend>());

            return true;
        }

        /// <summary>
        /// Reject Friend Request
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public bool RejectFriendRequest(string userId, string friendId)
        {
            var frinedRequest = (from s in friendRepo.Get()
                      where (s.UserId == userId && s.FriendId == friendId)
                      || (s.UserId == friendId && s.FriendId == userId)
                      select s).SingleOrDefault();

            friendRepo.Delete(new ObjectId(frinedRequest.Id.ToString()));

            return true;
        }

        /// <summary>
        /// Remove Friend
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public bool RemoveFriend(string userId, string friendId)
        {
            var frinedRequest = (from s in friendRepo.Get()
                                 where (s.UserId == userId && s.FriendId == friendId)
                                 || (s.UserId == friendId && s.FriendId == userId)
                                 select s.Id).ToList<ObjectId>();

            friendRepo.DeleteAll(frinedRequest);

            return true;
        }

        public List<FriendViewModel> GetFriendSuggestion(string userId)
        {
            var usersFriendList = (from s in friendRepo.Get()
                                   where (s.UserId == userId || s.FriendId == userId) && s.IsAccepted == true
                                   select (s.UserId == userId ? s.FriendId : s.UserId).ToString()).Take(5).ToList();

            var friendsFriendList = (from s in friendRepo.Get()
                                      where (s.UserId.In(usersFriendList) || s.FriendId.In(usersFriendList)) && s.IsAccepted == true
                                      select (s.UserId.In(usersFriendList) ? s.FriendId : s.UserId).ToString()).ToList();

            friendsFriendList.RemoveAll(r => r.Equals(userId));

            var friendUserIds = usersFriendList.Except(friendsFriendList).ToList();

            var friendUserNames = userBLL.GetUserById(friendUserIds);

            var friendListVM = from c in friendUserNames
                               select new FriendViewModel
                               {
                                   UserId = c.Id,
                                   FriendId = userId,
                                   FirstName = c.FirstName,
                                   LastName = c.LastName,
                                   IsAccepted = true,
                               };

            return friendListVM.ToList();
        }

    }
}
