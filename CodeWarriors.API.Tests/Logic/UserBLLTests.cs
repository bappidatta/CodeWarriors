using System.Linq;
using CodeWarriors.BLL.Logic;
using Faker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace CodeWarriors.API.Tests.Logic
{
    [TestClass]
    public class UserBLLTests
    {
        [TestMethod]
        [Fact]
        public void GetAllUserTest()
        {
            var userBll = new UserBLL();

            var users = userBll.GetAllUser(NumberFaker.Number(1, 10), NumberFaker.Number(1, 3), ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));

            var test = users.Any();

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);
        }

        [TestMethod]
        [Fact]
        public void GetUserByUserNameTest()
        {
            var userBll = new UserBLL();

            var name = ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul");
            var foundUser = userBll.GetUserByUserName(name);

            if (TestingConfig.XUnit)
                Xunit.Assert.Null(foundUser);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(foundUser);

            var test = foundUser.FirstName.Equals(name);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

            /*
            var testSameId = foundUser.Id.Equals(user.Id);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(testSameId);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(testSameId);
            */

        }

        [TestMethod]
        [Fact]
        public void GetUserByIdTest()
        {
            var userBll = new UserBLL();

            var users = userBll.GetAllUser(NumberFaker.Number(1, 10), NumberFaker.Number(1, 3), ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));

            var user = users.First();

            var foundUser = userBll.GetUserById(user.Id);

            if (TestingConfig.XUnit)
                Xunit.Assert.Null(foundUser);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(foundUser);

            var testSameId = foundUser.Id.Equals(user.Id);

            if (TestingConfig.XUnit)
                Xunit.Assert.True(testSameId);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(testSameId);
        }

        [TestMethod]
        [Fact]
        public void GetUserByIdByIdListTest()
        {
            var userBll = new UserBLL();

            var users = userBll.GetAllUser(NumberFaker.Number(1, 10), NumberFaker.Number(1, 3), ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));

            var userIds = users.Select(r => r.Id).ToList();

            var foundUsers = userBll.GetUserById(userIds);

            if (TestingConfig.XUnit)
                Xunit.Assert.Null(foundUsers);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(foundUsers);

            var test = foundUsers.Any();


            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);

        }
    }
}
