using CodeWarriors.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarriors.BLL.Interfaces
{
    public interface IFriendBLL
    {
        List<FriendViewModel> GetAllFriendsByUser(string id);

        List<FriendViewModel> GetAllFriendRequestsByUser(string id);

        List<FriendViewModel> GetMutualFriend(string userId, string friendId);

        Boolean IsExistsFriend(string userId, string friendId);

        Boolean SendFriendRequest(string userId, string friendId);

        Boolean AcceptFriendRequest(string userId, string friendId);

        Boolean RejectFriendRequest(string userId, string friendId);

        Boolean RemoveFriend(string userId, string friendId); 
    }
}
