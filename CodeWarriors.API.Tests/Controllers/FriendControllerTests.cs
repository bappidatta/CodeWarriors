using System.Linq;
using System.Security.Principal;
using System.Threading;
using CodeWarriors.API.Controllers;
using CodeWarriors.BLL.Interfaces;
using CodeWarriors.BLL.Logic;
using Faker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace CodeWarriors.API.Tests.Controllers
{
    [TestClass()]
    public class FriendControllerTests
    {

        private FriendController getController()
        {
            IUserBLL userBll = new UserBLL();
            IFriendBLL friendBll = new FriendBLL(userBll);

            var userNames = ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul");

            var userName = userBll.GetAllUser(10, 1, userNames).First().Id;
            var identity = new GenericIdentity(userName);

            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            return new FriendController(friendBll, userBll);
        }

        [TestMethod]
        [Fact]
        public void GetAllFriendTest()
        {
            var controller = getController();

            var data = controller.GetAllFriend();

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
        public void GetAllFriendRequestTest()
        {
            var controller = getController();

            var data = controller.GetAllFriendRequest();

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
        public void SendFriendRequestTest()
        {
            var controller = getController();

            var friendId = "";

            var result = controller.SendFriendRequest(friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(result);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(result);

        }

        [TestMethod]
        [Fact]
        public void AcceptFriendRequestTest()
        {
            var controller = getController();

            var data = controller.GetAllFriendRequest().First();

            var userId = data.UserId;
            var friendId = data.FriendId;

            var result = controller.AcceptFriendRequest(userId,friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(result);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(result);
        }

        [TestMethod]
        [Fact]
        public void RejectFriendRequestTest()
        {
            var controller = getController();

            var data = controller.GetAllFriendRequest().First();

            var userId = data.UserId;
            var friendId = data.FriendId;

            var result = controller.RejectFriendRequest(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(result);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(result);
            
        }

        [TestMethod]
        [Fact]
        public void RemoveFriendTest()
        {
            var controller = getController();

            var data = controller.GetAllFriend().First();

            var userId = data.UserId;
            var friendId = data.FriendId;

            var result = controller.RejectFriendRequest(userId, friendId);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(result);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(result);
        }
    }
}
