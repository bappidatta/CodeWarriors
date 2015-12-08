using CodeWarriors.BLL.Interfaces;
using CodeWarriors.BLL.ViewModels;
using CodeWarriors.DAL.Interfaces;
using CodeWarriors.DAL.Model;
using CodeWarriors.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CodeWarriors.BLL.Logic
{
    public class PostBLL : IPostBLL
    {
        private IRepository<Post> postRepo = new Repository<Post>("Post");
        private IRepository<RestrictedPost> restrictedPostRepo = new Repository<RestrictedPost>("RestrictedPost");
        private IRepository<User> userRepo = new Repository<User>("AspNetUsers");

        private IUserBLL userBLL;
        private IFriendBLL friendBLL;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userBLL"></param>
        /// <param name="friendBLL"></param>
        public PostBLL(IUserBLL userBLL, IFriendBLL friendBLL)
        {
            this.userBLL = userBLL;
            this.friendBLL = friendBLL;
        }

        /// <summary>
        /// Create new Post
        /// </summary>
        /// <param name="postVM">PostViewModel</param>
        /// <returns></returns>
        public string CreatePost(PostViewModel postVM)
        {
            Post post = new Post
            {
                UserId = postVM.UserId,
                PostDetails = postVM.PostDetails,
                CreatedTime = postVM.CreatedTime,
            };

            postRepo.Add(post);

            return post.Id.ToString();
        }

        /// <summary>
        /// Update a Post
        /// </summary>
        /// <param name="postVM">PostViewModel</param>
        /// <returns></returns>
        public bool UpatePost(PostViewModel postVM)
        {
            Post post = new Post
            {
                Id = new ObjectId(postVM.Id),
                UserId = postVM.UserId,
                PostDetails = postVM.PostDetails,
                LikedUserIds = postVM.LikedUserIds,
                CreatedTime = postVM.CreatedTime,
            };

            postRepo.Update(post);

            return true;
        }

        /// <summary>
        /// Delete a Post
        /// </summary>
        /// <param name="postId">Id of a Post</param>
        /// <returns></returns>
        public bool DeletePost(string postId)
        {
            postRepo.Delete(new ObjectId(postId));

            return true;
        }

        /// <summary>
        /// Hide a Post
        /// </summary>
        /// <param name="postId">Id of a Post</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean HidePost(string postId, string userId) 
        {
            RestrictedPost restrictedPost = new RestrictedPost
            {
                PostId = postId,
                UserId = userId
            };

            restrictedPostRepo.Add(restrictedPost);

            return true;
        }

        /// <summary>
        /// Add Comments to a Post
        /// </summary>
        /// <param name="commentVM">CommentViewModel</param>
        /// <returns></returns>
        public bool AddComments(CommentViewModel commentVM)
        {
            Comment comments = new Comment()
                           {
                               UserId = commentVM.UserId,
                               CommentDetails = commentVM.CommentDetails,
                               CreatedTime = commentVM.CreatedTime
                           };

            var post = postRepo.Get().Where(x => x.Id == new ObjectId(commentVM.PostId)).SingleOrDefault();
            post.Comments.Add(comments);

            postRepo.Update(post);

            return true;
        }

        /// <summary>
        /// Like a Post
        /// </summary>
        /// <param name="userId">Id of a User</param>
        /// <param name="postId">Id of a Post</param>
        /// <returns></returns>
        public bool AddLike(string userId, string postId)
        {
            var post = postRepo.Get().Where(x => x.Id == new ObjectId(postId)).SingleOrDefault();
            post.LikedUserIds.Add(userId);

            postRepo.Update(post);

            return true;
        }

        /// <summary>
        /// UnLike a Liked Post
        /// </summary>
        /// <param name="userId">Id of a User</param>
        /// <param name="postId">Id of a Post</param>
        /// <returns></returns>
        public bool RemoveLike(string userId, string postId)
        {
            var post = postRepo.Get().Where(x => x.Id == new ObjectId(postId)).SingleOrDefault();
            post.LikedUserIds.Remove(userId);

            postRepo.Update(post);

            return true;
        }

        /// <summary>
        /// Get All Post of a User including post of friends and excluding hidden post
        /// </summary>
        /// <param name="userId">Id of a User</param>
        /// <returns></returns>
        public IEnumerable<PostViewModel> GetAllPost(string userId)
        {

            IEnumerable<PostViewModel> postList;

            // Get Post of friend
            postList = GetAllPostByUser(userId);

            // Get All friend List
            var friendList = friendBLL.GetAllFriendsByUser(userId);

            string friendId = string.Empty;

            // Get Post of All friend
            foreach (var friend in friendList)
            {
                if (friend.FriendId != userId)
                    friendId = friend.FriendId;
                else
                    friendId = friend.UserId;

                var friendPostList = GetAllPostByUser(friendId);
                postList = postList.Union(friendPostList);
            }

            // Get restricted PostId
            var restrictedPostIds = (from s in restrictedPostRepo.Get()
                                      where s.UserId == userId
                                      select s.PostId).AsEnumerable();


            var result = from s in postList
                         where !restrictedPostIds.Contains(s.Id)
                         orderby s.CreatedTime descending
                         select s;

            return result;
        }
         
        /// <summary>
        /// Get all post of a user
        /// </summary>
        /// <param name="userId">Id of a User</param>
        /// <returns></returns>
        public IEnumerable<PostViewModel> GetAllPostByUser(string userId)
        {
            var postListVM = new List<PostViewModel>();

            var postList = (from s in postRepo.Get()
                            where s.UserId == userId
                            orderby s.CreatedTime descending
                            select s).AsEnumerable();

            foreach (var post in postList)
            {
                var postVM = GetPost(post);

                // Add single Post information into PostList
                postListVM.Add(postVM);
            }

            return postListVM;
        }

        /// <summary>
        /// Convert Post Entity to PostViewModel after incluing UserName ofassociate UserID
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        private PostViewModel GetPost(Post post)
        {
            // Get User Information of Post owner
            var postUser = userBLL.GetUserById(post.UserId);

            // Get Post Information
            var postVM = new PostViewModel
                         {
                             CreatedTime = post.CreatedTime,
                             Id = post.Id.ToString(),
                             LikedUserIds = post.LikedUserIds,
                             NoOfLike = post.LikedUserIds.Count,
                             PostDetails = post.PostDetails,
                             UserId = post.UserId,
                             FirstName = postUser.FirstName,
                             LastName = postUser.LastName,
                             Email = postUser.Email,
                             IsLike = false
                         };

            if (post.Comments != null && post.Comments.Count()>0)
            {

                // Get UserIds of Commenters for a single Post
                var commentUserIds = post.Comments.Select(s => s.UserId).ToList();

                // Get User Name & Id of Commenters
                var commentUsers = userBLL.GetUserById(commentUserIds);

                // Get Question Information including Commenter Name
                var commentsVM = (from s in post.Comments
                    join c in commentUsers on s.UserId equals c.Id
                    select new CommentViewModel
                           {
                               CommentDetails = s.CommentDetails,
                               CreatedTime = s.CreatedTime,
                               FirstName = c.FirstName,
                               LastName = c.LastName,
                               UserId = s.UserId
                           }).ToList();

                postVM.Comments = commentsVM;
            }
            // Add Liked User Name
            if (postVM.LikedUserIds == null)
                return postVM;

            List<UserViewModel> likedUserNames = new List<UserViewModel>();

            foreach (var item in post.LikedUserIds)
            {
                likedUserNames.Add(userBLL.GetUserById(item));
            }

            postVM.LikedUserName = likedUserNames;

            return postVM;
        }

        public PostViewModel GetPostByID(string id)
        {
            var post = (from s in postRepo.Get()
                        where s.Id == new ObjectId(id)
                        select s).SingleOrDefault();

            if (post != null)
                return GetPost(post);

            return new PostViewModel();
        }
    }
}
