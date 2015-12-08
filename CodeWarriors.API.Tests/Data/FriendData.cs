using System.Configuration;
using System.Linq;
using CodeWarriors.API.Controllers;
using CodeWarriors.BLL.Logic;
using CodeWarriors.DAL.Interfaces;
using CodeWarriors.DAL.Model;
using CodeWarriors.DAL.Repositories;
using Faker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Xunit;

namespace CodeWarriors.API.Tests.Data
{
    [TestClass]
    public class FriendData
    {
        private IRepository<User> userRepo = new Repository<User>("AspNetUsers");

        private IRepository<Friend> friendRepo = new Repository<Friend>("Friend");

        public void Pupolate()
        {
            var controller = new UserController(new UserBLL());
            var userList = controller.GetAllUsers(100, 1,NameFaker.FirstName());

            var userIds = userList.Select(r => r.Id).ToArray();

            for (var i = 0; i < 1000; i++)
            {
                var friend = new Friend()
                {
                    FriendId = ArrayFaker.SelectFrom(1, userIds).First(),
                    UserId = ArrayFaker.SelectFrom(1, userIds).First(),
                    IsAccepted = BooleanFaker.Boolean()
                };
                friendRepo.Add(friend);
            }

             userList = controller.GetAllUsers(10, 1,
                ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));

             userIds = userList.Select(r => r.Id).ToArray();

            for (var i = 0; i < 10; i++)
            {
                var friend = new Friend()
                             {
                                 FriendId = ArrayFaker.SelectFrom(1, userIds).First(),
                                 UserId = ArrayFaker.SelectFrom(1, userIds).First(),
                                 IsAccepted = BooleanFaker.Boolean()
                             };
                friendRepo.Add(friend);
            }
        }

        [TestMethod]
        [Fact]
        public void RunPupolate()
        {
            Pupolate();

            var repo = new FriendBLL(new UserBLL());

            var controller = new UserController(new UserBLL());
            var userList = controller.GetAllUsers(100, 1,
                ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));

            var userId = userList.Select(r => r.Id).First();

            var data = repo.GetAllFriendsByUser(userId);

            if (TestingConfig.XUnit)
                Xunit.Assert.Null(data);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(data);

            var test = data.Count() > 10;

            if (TestingConfig.XUnit)
                Xunit.Assert.True(test);
            else
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(test);
        }
    }
}
