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
    [TestClass]
    public class UserControllerTests
    {
        private UserController getController()
        {
            IUserBLL userBll = new UserBLL();

            var userNames = ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul");

            var userName = userBll.GetAllUser(10, 1, userNames).First().Id;
            var identity = new GenericIdentity(userName);

            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            return new UserController(userBll);
        }

        [TestMethod]
        [Fact]
        public void GetAllUsersTest()
        {
            var controller = getController();

            var data = controller.GetAllUsers(10, 1,
                ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));

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
    }
}
