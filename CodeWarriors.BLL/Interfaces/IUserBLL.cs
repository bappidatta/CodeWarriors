using CodeWarriors.BLL.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarriors.BLL.Interfaces
{
    public interface IUserBLL
    {
        IEnumerable<UserViewModel> GetAllUser(int pageSize, int offset, string userName);

        UserViewModel GetUserByUserName(string userName);

        UserViewModel GetUserById(string Id);

        List<UserViewModel> GetUserById(List<string> userIdList);
    }
}
