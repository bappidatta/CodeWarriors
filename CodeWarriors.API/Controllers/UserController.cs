using CodeWarriors.API.App_Start;
using CodeWarriors.BLL.Interfaces;
using CodeWarriors.BLL.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CodeWarriors.API.Controllers
{
    /// <summary>
    /// This class is responsible for user related activities
    /// </summary>
    [System.Web.Http.Authorize]
    public class UserController : ApiController
    {
        private IUserBLL userBLL;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userBLL">UserBLL</param>
        public UserController(IUserBLL userBLL)
        {
            this.userBLL = userBLL;
        }

        /// <summary>
        /// Get all filterd User by search criteria
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="offset"></param>
        /// <param name="userName"></param>
        /// <returns>List of User</returns>
        public IEnumerable<UserViewModel> GetAllUsers(int pageSize, int offset, string userName)
        {
            var users = userBLL.GetAllUser(pageSize, offset, userName);

            return users;
        }
    }
}
