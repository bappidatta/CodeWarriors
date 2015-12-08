using System.Collections.Generic;
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
using Xunit;

namespace CodeWarriors.API.Tests.Data
{
    [TestClass]
    public class PostData
    {
        private IRepository<Post> postRepo = new Repository<Post>("Post");

        public void Pupolate()
        {
            var controller = new UserController(new UserBLL());
            var userList = controller.GetAllUsers(100, 1, ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));
            var userIds = userList.Select(r => r.Id).ToArray();
            for (var i = 0; i < 1000; i++)
            {
                var post = new Post()
                           {
                               UserId = ArrayFaker.SelectFrom(userIds),
                               PostDetails = TextFaker.Sentences(NumberFaker.Number(1, 25)),
                               CreatedTime = DateTimeFaker.DateTime().ToString(CultureInfo.InvariantCulture),
                               LikedUserIds = ArrayFaker.SelectFrom(NumberFaker.Number(0, 5), userIds).ToList(),
                               Comments = new List<Comment>()
                                          {
                                              new Comment()
                                              {
                                                  CommentDetails = TextFaker.Sentences(NumberFaker.Number(1, 25)),
                                                  CreatedTime =
                                                      DateTimeFaker.DateTime().ToString(CultureInfo.InvariantCulture),
                                                  UserId = ArrayFaker.SelectFrom(userIds),
                                              },
                                              new Comment()
                                              {
                                                  CommentDetails = TextFaker.Sentences(NumberFaker.Number(1, 25)),
                                                  CreatedTime =
                                                      DateTimeFaker.DateTime().ToString(CultureInfo.InvariantCulture),
                                                  UserId = ArrayFaker.SelectFrom(userIds),
                                              }
                                          }
                           };
                postRepo.Add(post);
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
