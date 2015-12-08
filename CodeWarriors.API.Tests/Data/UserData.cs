using System.Configuration;
using System.Linq;
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
    public class UserData
    {
        private IRepository<User> userRepo = new Repository<User>("AspNetUsers");

        public void Pupolate()
        {
            var user = new User()
            {
                UserName = StringFaker.AlphaNumeric(12),
                FirstName = "Jahirul",
                LastName = "Islam"
            };
            userRepo.Add(user);
            user = new User()
           {
               UserName = StringFaker.AlphaNumeric(12),
               FirstName = "Monirul",
               LastName = "Islam"
           };
            userRepo.Add(user);
            user = new User()
           {
               UserName = StringFaker.AlphaNumeric(12),
               FirstName = "Aminul",
               LastName = "Islam"
           };
            userRepo.Add(user);
            user = new User()
           {
               UserName = StringFaker.AlphaNumeric(12),
               FirstName = "Fukrul",
               LastName = "Islam"
           };
            userRepo.Add(user);

            user = new User()
           {
               UserName = StringFaker.AlphaNumeric(12),
               FirstName = "Najrul",
               LastName = "Islam"
           };
            userRepo.Add(user);

            user = new User()
           {
               UserName = StringFaker.AlphaNumeric(12),
               FirstName = "Kamrul",
               LastName = "Islam"
           };
            userRepo.Add(user);

            for (var i = 0; i < 1000; i++)
            {
                user = new User()
                          {
                              UserName = StringFaker.AlphaNumeric(12),
                              FirstName = NameFaker.FirstName(),
                              LastName = NameFaker.LastName()
                          };
                userRepo.Add(user);
            }
            for (var i = 0; i < 10; i++)
            {
                user = new User()
               {
                   UserName = StringFaker.AlphaNumeric(12),
                   FirstName = ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"),
                   LastName = ArrayFaker.SelectFrom("Islam", "Bhuiyan", "Khan", "")
               };
                userRepo.Add(user);
            }
        }

        [TestMethod]
        [Fact]
        public void RunPupolate()
        {
            Pupolate();

            var repo = new UserBLL();

            var data = repo.GetAllUser(100, 1, ArrayFaker.SelectFrom("Jahirul", "Monirul", "Aminul", "Fukrul", "Najrul", "Kamrul"));

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
