using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web.WebSockets;
using CodeWarriors.API.Controllers;
using CodeWarriors.BLL.Interfaces;
using CodeWarriors.BLL.Logic;
using CodeWarriors.BLL.ViewModels;
using Faker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;


namespace CodeWarriors.API.Tests.Controllers
{
    [TestClass]
    public class PostControllerTests
    {
        private PostController getController()
        {

            IUserBLL userBll = new UserBLL();
            var userNames = ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul");
            var userVm = userBll.GetAllUser(10, 1, userNames).First();
            
            var mockUserBll = new Mock<IUserBLL>();
            mockUserBll.Setup(c => c.GetUserByUserName(userVm.Id)).Returns(userVm);
            mockUserBll.Setup(c => c.GetUserById(userVm.Id)).Returns(userVm);

            userBll = mockUserBll.Object;

            IFriendBLL friendBll = new FriendBLL(userBll);
            IPostBLL postBll = new PostBLL(userBll, friendBll);

            var identity = new GenericIdentity(userVm.Id);

            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            return new PostController(postBll, userBll);
        }

        [TestMethod]
        [Fact]
        public void GetAllPostsTest()
        {
            var controller = getController();

            IUserBLL userBll = new UserBLL();

            var userNames = ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul");

            var userId = userBll.GetAllUser(10, 1, userNames).First().Id;

            var data = controller.GetAllPosts(userId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(data);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(data);

            var test = data.Count() > 1;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

        }

        [TestMethod]
        [Fact]
        public void PostPostTest()
        {
            var controller = getController();

            var postVm = new PostViewModel()
                         {
                             PostDetails = TextFaker.Sentences(NumberFaker.Number(5, 25))
                         };

            var result = controller.PostPost(postVm);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(result);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
        }

        [TestMethod]
        [Fact]
        public void PostCommentTest()
        {

            var controller = getController();

            var post = controller.GetAllPosts().First();

            var commentVm = new CommentViewModel()
                            {
                                PostId = post.Id,
                                CommentDetails = TextFaker.Sentences(NumberFaker.Number(5, 25))
                            };

            var result = controller.PostComment(commentVm);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(result);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);

        }

        [TestMethod]
        [Fact]
        public void PutPostTest()
        {
            var controller = getController();

            var post = controller.GetAllPosts().First();

            var postVm = new PostViewModel()
            {
                Id = post.Id,
                PostDetails = TextFaker.Sentences(NumberFaker.Number(5, 25))
            };

            var result = controller.PutPost(postVm);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(result);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(result);

        }

        [TestMethod]
        [Fact]
        public void DeletePostTest()
        {
            var controller = getController();

            var post = controller.GetAllPosts().First();

            var result = controller.DeletePost(post.Id, post.UserId);

            if (TestingConfig.XUnit)
                Xunit.Assert.NotNull(result);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
        }
    }
}
