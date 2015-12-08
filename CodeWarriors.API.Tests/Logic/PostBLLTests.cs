using System.Globalization;
using System.Linq;
using CodeWarriors.API.Controllers;
using CodeWarriors.BLL.Interfaces;
using CodeWarriors.BLL.Logic;
using CodeWarriors.BLL.ViewModels;
using Faker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace CodeWarriors.API.Tests.Logic
{
    [TestClass]
    public class PostBLLTests
    {
        private IPostBLL getBll()
        {
            IUserBLL userBll = new UserBLL();
            IFriendBLL friendBll = new FriendBLL(userBll);
            IPostBLL postBll = new PostBLL(userBll, friendBll);

            return postBll;
        }


        [TestMethod]
        [Fact]
        public void CreatePostTest()
        {
            var postBll = getBll();

            var controller = new UserController(new UserBLL());
            var userList = controller.GetAllUsers(100, 1, ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));
            var userId = userList.First().Id;

            var postId = postBll.CreatePost(new PostViewModel()
                                            {
                                                UserId = userId,
                                                PostDetails = TextFaker.Sentences(NumberFaker.Number(1, 25)),
                                                CreatedTime =
                                                    DateTimeFaker.DateTime().ToString(CultureInfo.InvariantCulture)
                                            });

            if (TestingConfig.XUnit)
                Xunit.Assert.NotEmpty(postId);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(postId);

        }

        [TestMethod]
        [Fact]
        public void UpatePostTest()
        {
            var postBll = getBll();

            var controller = new UserController(new UserBLL());
            var userList = controller.GetAllUsers(100, 1, ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));

            var userId = userList.First().Id;

            var userIds = userList.Select(r => r.Id).ToList();

            postBll.CreatePost(new PostViewModel()
                               {
                                   UserId = userId,
                                   PostDetails = TextFaker.Sentences(5),
                                   CreatedTime = DateTimeFaker.DateTime().ToString(CultureInfo.InvariantCulture)
                               });

            var posts = postBll.GetAllPost(userId);

            var post = posts.First();

            var test = postBll.UpatePost(new PostViewModel()
                              {
                                  Id = post.Id,
                                  UserId = post.UserId,
                                  PostDetails = TextFaker.Sentences(NumberFaker.Number(5, 30)),
                                  LikedUserIds = userIds,
                                  CreatedTime = DateTimeFaker.DateTime().ToString(CultureInfo.InvariantCulture)
                              });


            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            var updatedPost = postBll.GetPostByID(post.Id);

            var testNoOfUserLike = updatedPost.LikedUserIds.Count() == userId.Count();

            if (TestingConfig.XUnit)
                Xunit.Assert.True(testNoOfUserLike);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(testNoOfUserLike);

        }

        [TestMethod]
        [Fact]
        public void DeletePostTest()
        {
            var postBll = getBll();

            var controller = new UserController(new UserBLL());
            var userList = controller.GetAllUsers(100, 1, ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));
            var userId = userList.First().Id;

            var posts = postBll.GetAllPost(userId);

            var post = posts.First();

            var test = postBll.DeletePost(post.Id);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            var testNotExist = postBll.DeletePost(post.Id);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(testNotExist);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(testNotExist);

        }

        [TestMethod]
        [Fact]
        public void AddCommentsTest()
        {
            var postBll = getBll();

            var controller = new UserController(new UserBLL());
            var userList = controller.GetAllUsers(100, 1, ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));
            var userId = userList.First().Id;

            var posts = postBll.GetAllPost(userId);

            var test = posts.Any();

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            var post = posts.First();

            post = postBll.GetPostByID(post.Id);

            var userIds = userList.Select(r => r.Id).ToArray();

            var comment = new CommentViewModel()
            {
                PostId = post.Id,
                CommentDetails = TextFaker.Sentences(NumberFaker.Number(1, 25)),
                CreatedTime =
                    DateTimeFaker.DateTime().ToString(CultureInfo.InvariantCulture),
                UserId = ArrayFaker.SelectFrom(userIds)
            };

            postBll.AddComments(comment);

            var postWithComment = postBll.GetPostByID(post.Id);

            var testCommentInsert = post.Comments.Count() == postWithComment.Comments.Count() - 1;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(testCommentInsert);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(testCommentInsert);
        }

        [TestMethod]
        [Fact]
        public void GetAllPostTest()
        {
            var postBll = getBll();

            var controller = new UserController(new UserBLL());
            var userList = controller.GetAllUsers(100, 1, ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));
            var userId = userList.First().Id;

            postBll.CreatePost(new PostViewModel()
                               {
                                   UserId = userId,
                                   PostDetails = TextFaker.Sentences(5),
                                   CreatedTime = DateTimeFaker.DateTime().ToString(CultureInfo.InvariantCulture)
                               });

            var posts = postBll.GetAllPost(userId);

            var test = posts.Any();

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

        }

        [TestMethod]
        [Fact]
        public void GetAllPostByUserTest()
        {
            var postBll = getBll();

            var controller = new UserController(new UserBLL());
            var userList = controller.GetAllUsers(100, 1, ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));
            var userId = userList.First().Id;

            var post = postBll.GetPostByID(userId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(post);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(post);

        }

        [TestMethod]
        [Fact]
        public void GetPostByIDTest()
        {
            var postBll = getBll();

            var controller = new UserController(new UserBLL());
            var userList = controller.GetAllUsers(100, 1, ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));
            var userId = userList.First().Id;

            postBll.CreatePost(new PostViewModel()
                               {
                                   UserId = userId,
                                   PostDetails = TextFaker.Sentences(5),
                                   CreatedTime = DateTimeFaker.DateTime().ToString(CultureInfo.InvariantCulture)
                               });

            var posts = postBll.GetAllPost(userId);

            var post = posts.First();

            var foundPost = postBll.GetPostByID(post.Id);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(foundPost);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(foundPost);

            var test = foundPost.Id.Equals(post.Id);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

        }
    }
}
