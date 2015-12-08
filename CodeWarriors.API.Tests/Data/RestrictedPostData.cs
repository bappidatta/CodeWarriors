using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using CodeWarriors.API.Controllers;
using CodeWarriors.BLL.Interfaces;
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
    public class RestrictedPostData
    {
        private IRepository<Post> postRepo = new Repository<Post>("Post");
        private IRepository<RestrictedPost> restrictedPostRepo = new Repository<RestrictedPost>("RestrictedPost");

        public void Pupolate()
        {
            for (var i = 0; i < 100; i++)
            {
                var post = postRepo.Get().Skip(NumberFaker.Number(1, 100)).First();
                var restrictedPost = new RestrictedPost()
                {
                    PostId = post.Id.ToString(),
                    UserId = post.UserId
                };
                restrictedPostRepo.Add(restrictedPost);
            }
        }

        private IPostBLL getBll()
        {
            IUserBLL userBll = new UserBLL();
            IFriendBLL friendBll = new FriendBLL(userBll);
            IPostBLL postBll = new PostBLL(userBll, friendBll);

            return postBll;
        }

        [TestMethod]
        [Fact]
        public void RunPupolate()
        {
            Pupolate();

            var repo = getBll();

            var controller = new UserController(new UserBLL());
            var userList = controller.GetAllUsers(100, 1, ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));
            var userId = userList.First().Id;

            var data = repo.GetAllPost(userId);

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
