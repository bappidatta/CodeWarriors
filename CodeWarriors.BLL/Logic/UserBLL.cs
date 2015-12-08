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
    public class UserBLL : IUserBLL
    {
        private IRepository<User> userRepo = new Repository<User>("AspNetUsers");

        public IEnumerable<UserViewModel> GetAllUser(int pageSize, int offset, string userName)
        {
            var users = (from s in userRepo.Get()
                         where s.FirstName.ToLower().Contains(userName.ToLower()) ||
                         s.LastName.ToLower().Contains(userName.ToLower())
                         select s);

            List<UserViewModel> userList = (from s in users
                                            select new UserViewModel
                                             {
                                                 Id = s.Id.ToString(),
                                                 Email = s.UserName,
                                                 FirstName = s.FirstName,
                                                 LastName = s.LastName
                                             }).ToList();

            return userList;
        }

        public UserViewModel GetUserByUserName(string userName)
        {
            var user = (from s in userRepo.Get().Where(x => x.UserName.Contains(userName))
                select new UserViewModel
                       {
                           Id = s.Id.ToString(),
                           FirstName = s.FirstName,
                           LastName = s.LastName,
                           Email = s.UserName
                       }).SingleOrDefault();

            return user;
        }

        public UserViewModel GetUserById(string Id)
        {
            var user = (from s in userRepo.Get().Where(x => x.Id == Id)
                        select new UserViewModel
                        {
                            Id = s.Id.ToString(),
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            Email = s.UserName
                        }).SingleOrDefault();

            return user;
        }

        public List<UserViewModel> GetUserById(List<string> userIdList)
        {
            var user = (from s in userRepo.Get()
                        where userIdList.Contains(s.Id)
                        select new UserViewModel
                        {
                            Id = s.Id.ToString(),
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            Email = s.UserName
                        }).ToList();

            return user;
        }
    }
}
